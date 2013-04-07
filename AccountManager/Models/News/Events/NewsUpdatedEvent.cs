using System;

namespace AccountManager.Models.News.Events
{

    public class NewsUpdatedEvent
    {
        public Guid NewsId { get; private set; }
        public string Title { get; private set; }
        public string Summary { get; private set; }
        public string Content { get; private set; }
 

        public NewsUpdatedEvent(Guid @newsId, string @title, string @summary, string @content)
        {
            NewsId = @newsId;
            Title = @title;
            Summary = @Summary;
            Content = @content;
        }

        public override string ToString()
        {
            return string.Format("News {0} Updated.", NewsId);
        }
    }
}