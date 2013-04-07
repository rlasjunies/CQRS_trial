using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTG.Core;
using AccountManager.Models.News.Commands;
using AccountManager.Models.News.Domain;
using AccountManager.Models.News.Events;

namespace AccountManager.Tests
{
    public class NewsCannotBeModifiedWhenPublished : TestBase
    {
        readonly Guid _newsId = Guid.NewGuid();

        public override Dictionary<object, List<object>> GivenTheseEvents()
        {
            return new Dictionary<object, List<object>>
            {
                {_newsId, new List<object>
                    {
                        new NewsCreatedEvent(_newsId, "title", "news summary", "news content"),
                        new NewsPublishedEvent(_newsId),
                    }
                }
            };
        }

        public override object WhenThisHappens()
        {
            return new UpdateNewsCommand {Id = _newsId,Title= "title modified" };
        }

        //public override IEnumerable<object> TheseEventsShouldOccur()
        //{
        //    yield return new NewsPublishedException(_newsId);
        //    //yield return new AccountLockedEvent(_accountId);
        //}

        public override Exception ThisExceptionShouldOccur()
        //public override IEnumerable<object> ThisExceptionShouldOccur()
        {
            return new NewsPublishedException((Guid)_newsId);
        }
        public override void RegisterHandler(MessageBus bus, IRepository repo)
        {
            var svc = new NewsApplicationService(repo);
            bus.RegisterHandler<UpdateNewsCommand>(svc.Handle);
            bus.RegisterHandler<PublishNewsCommand>(svc.Handle);
        }
    }
}
