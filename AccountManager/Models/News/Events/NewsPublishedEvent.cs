using System;
using AccountManager.Models.News.Domain;

namespace AccountManager.Models.News.Events
{

    public class NewsPublishedEvent
    {
        public Guid NewsId { get; private set; } 
        //public enumNewsStatus Status { get; private set; } 

        public NewsPublishedEvent(Guid @newsId)
        {
            NewsId = @newsId;
            //Status = enumNewsStatus.Published;
        }

        public override string ToString()
        {
            return string.Format("News {0} published.", NewsId);
        }
    }
}