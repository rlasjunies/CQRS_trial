using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using Newtonsoft.Json;
using System.Data.SQLite;
using System.ComponentModel.DataAnnotations;

namespace MTG.Core
{
      
    //CREATE TABLE [dbo].[Streams](
    //[StreamID] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
    //[CurrentSequence] [bigint] NOT NULL,
    //CONSTRAINT [PK_Streams] PRIMARY KEY CLUSTERED 
    //(
    //[StreamID] ASC
    //)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
    //)

    // Table sqlite
    //CREATE TABLE STREAMS(
    // StreamID nvarcher(50) not null,
    // CurrentSequence bigint not null);

    //public class Streams
    //{
    //    [MaxLength(50)]
    //    public string StreamID { get; set; }

    //    public int CurrentSequence { get; set; }
    //}

//    CREATE TABLE [dbo].[EventWrappers](
//    [EventId] [uniqueidentifier] NOT NULL,
//    [StreamId] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
//    [Sequence] [bigint] NOT NULL,
//    [TimeStamp] [datetime] NOT NULL,
//    [EventType] [nvarchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
//    [Body] [nvarchar](max) COLLATE Latin1_General_CI_AS NOT NULL,
// CONSTRAINT [PK_EventWrappers] PRIMARY KEY CLUSTERED 
//(
//    [EventId] ASC
//)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
//)
//END
//GO
    //CREATE TABLE EventWrappers(
    //    EventID guid NOT NULL,
    //    StreamID nvarchar(50) NOT NULL,
    //    Sequence bigint NOT NULL,
    //    TimeStamp datetime NOT NULL,
    //    EventType nvarchar(100) NOT NULL,
    //    Body text NOT NULL);


    public class SqlLiteEventStore : IEventStore
    {
        readonly MessageBus _bus;
        private SQLiteConnection _db;
        const string ConnectionStringName = "SqliteEventStore";

        public SqlLiteEventStore(MessageBus bus)
        {
            _bus = bus;
            _db = new SQLiteConnection(ConnectionStringName);

        }

        public void StoreEvents(object streamId, IEnumerable<object> events, long expectedInitialVersion)
        {
            events = events.ToArray();

            var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
            var serializedEvents = events.Select(x => new Tuple<string, string>(x.GetType().FullName, JsonConvert.SerializeObject(x)));

            using (var con = new SQLiteConnection(connectionString))
            {
                con.Open();

                //const string commandText = "Select Top 1 CurrentSequence from Streams where StreamId = @StreamId;";
                const string commandText = "Select CurrentSequence from Streams where StreamId = @StreamId limit 1;";
                long? existingSequence;
                using (var command = new SQLiteCommand(commandText, con))
                {
                    command.Parameters.AddWithValue("StreamId", streamId.ToString());
                    var current = command.ExecuteScalar();
                    existingSequence = current == null ? (long?)null : (long)current;

                    if (existingSequence != null && ((long)existingSequence) > expectedInitialVersion)
                        throw new ConcurrencyException();
                }

                using (var t = new TransactionScope())
                {
                    var nextVersion = insertEventsAndReturnLastVersion(streamId, con, expectedInitialVersion, serializedEvents);

                    if (existingSequence == null)
                        startNewSequence(streamId, nextVersion, con);
                    else
                        updateSequence(streamId, expectedInitialVersion, nextVersion, con);

                    t.Complete();
                }
            }

            _bus.Publish(events);
        }

        static void updateSequence(object streamId, long expectedInitialVersion, long nextVersion, SQLiteConnection con)
        {
            const string cmdText =
                "Update Streams SET CurrentSequence = @CurrentSequence WHERE StreamID = @StreamID AND CurrentSequence = @OriginalSequence;";
            using (var cmd = new SQLiteCommand(cmdText, con))
            {
                cmd.Parameters.AddWithValue("StreamID", streamId.ToString());
                cmd.Parameters.AddWithValue("CurrentSequence", nextVersion);
                cmd.Parameters.AddWithValue("OriginalSequence", expectedInitialVersion);

                var rows = cmd.ExecuteNonQuery();
                if (rows != 1)
                    throw new ConcurrencyException();
            }
        }

        static void startNewSequence(object streamId, long nextVersion, SQLiteConnection con)
        {
            const string cmdText = "Insert into Streams (StreamId, CurrentSequence) values (@StreamId, @CurrentSequence);";
            using (var cmd = new SQLiteCommand(cmdText, con))
            {
                cmd.Parameters.AddWithValue("StreamId", streamId.ToString());
                cmd.Parameters.AddWithValue("CurrentSequence", nextVersion);

                int rows = cmd.ExecuteNonQuery();
                if (rows != 1)
                {
                    throw new ConcurrencyException();
                }
            }
        }

        static long insertEventsAndReturnLastVersion(object streamId, SQLiteConnection con, long nextVersion, IEnumerable<Tuple<string, string>> serializedEvents)
        {
            foreach (var e in serializedEvents)
            {
                const string insertText =
                    "Insert into EventWrappers (EventId, StreamId, Sequence, TimeStamp, EventType, Body) values (@EventId, @StreamId, @Sequence, @TimeStamp, @EventType, @Body);";
                using (var command = new SQLiteCommand(insertText, con))
                {
                    command.Parameters.AddWithValue("EventId", Guid.NewGuid());
                    command.Parameters.AddWithValue("StreamId", streamId.ToString());
                    command.Parameters.AddWithValue("Sequence", ++nextVersion);
                    command.Parameters.AddWithValue("TimeStamp", DateTime.UtcNow);
                    command.Parameters.AddWithValue("EventType", e.Item1);
                    command.Parameters.AddWithValue("Body", e.Item2);

                    command.ExecuteNonQuery();
                }
            }

            return nextVersion;
        }

        public IEnumerable<object> LoadEvents(object id, long version = 0)
        {
            const string cmdText = "SELECT EventType, BODY from EventWrappers WHERE StreamId = @StreamId AND Sequence >= @Sequence ORDER BY TimeStamp";
            var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
            using (var con = new SQLiteConnection(connectionString))
            using (var cmd = new SQLiteCommand(cmdText, con))
            {
                cmd.Parameters.AddWithValue("StreamId", id.ToString());
                cmd.Parameters.AddWithValue("Sequence", version);

                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var eventTypeString = reader["EventType"].ToString();
                    var eventType = Type.GetType(eventTypeString);
                    var serializedBody = reader["Body"].ToString();
                    yield return JsonConvert.DeserializeObject(serializedBody, eventType);
                }
            }
        }

        public IEnumerable<object> GetAllEventsEver()
        {
            const string cmdText = "SELECT EventType, BODY from EventWrappers ORDER BY TimeStamp";
            //const string cmdText = "SELECT EventType, BODY from EventWrappers";

            var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
            using (var con = new SQLiteConnection(connectionString))
            using (var cmd = new SQLiteCommand(cmdText, con))
            {
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var eventTypeString = reader["EventType"].ToString();
                    var eventType = Type.GetType(eventTypeString);
                    var serializedBody = reader["Body"].ToString();
                    yield return JsonConvert.DeserializeObject(serializedBody, eventType);
                }
            }
        }
    }
}