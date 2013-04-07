using System;
using AccountManager.Models.News.Domain;

namespace AccountManager.Models.News.Events
{

    public class NewsCreatedEvent
    {
        public Guid NewsId { get; private set; }
        public string Title { get; private set; }
        public string Summary { get; private set; }
        public string Content { get; private set; }
        public enumNewsStatus Status { get; private set; }
 
        public NewsCreatedEvent(Guid @newsId, string @title, string @summary, string @content)
        {
            NewsId = @newsId;
            Title = @title;
            Summary = @summary;
            Content = @content;
            Status = enumNewsStatus.Created;
        }

        public override string ToString()
        {
            return string.Format("News {0} created.", NewsId);
        }
    }
}