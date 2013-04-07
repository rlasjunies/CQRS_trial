using System;
using AccountManager.Models.News.Domain;

namespace AccountManager.Models.News.Events
{

    public class NewsUnpublishedEvent
    {
        public Guid NewsId { get; private set; } 
        public enumNewsStatus Status { get; private set; } 

        public NewsUnpublishedEvent(Guid @newsId)
        {
            NewsId = @newsId;
            Status = enumNewsStatus.Created;
        }

        public override string ToString()
        {
            return string.Format("News {0} Unpublished.", NewsId);
        }
    }
}