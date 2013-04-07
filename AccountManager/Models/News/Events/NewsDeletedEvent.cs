using System;
using AccountManager.Models.News.Domain;

namespace AccountManager.Models.News.Events
{

    public class NewsDeletedEvent
    {
        public Guid NewsId { get; private set; } 
        //public enumNewsStatus Status { get; private set; } 

        public NewsDeletedEvent(Guid @newsId)
        {
            NewsId = @newsId;
        }

        public override string ToString()
        {
            return string.Format("News {0} deleted.", NewsId);
        }
    }
}