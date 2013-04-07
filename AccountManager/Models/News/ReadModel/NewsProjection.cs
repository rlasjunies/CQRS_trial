using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccountManager.Models.News.Events;
using AccountManager.Models.News.Domain;

namespace AccountManager.Models.News.ReadModel
{
    public class NewsProjection
    {
        public List<NewsModel> News = new List<NewsModel>();
 
        public void Handle(NewsCreatedEvent @event)
        {
            if (News.All(news => news.Id != @event.NewsId))
                News.Add(new NewsModel(@event.NewsId, @event.Title, @event.Summary, @event.Content));
        }

        public void Handle(NewsDeletedEvent @event)
        {
            News.Remove(News.Single(news => news.Id == @event.NewsId));
        }

        public void Handle(NewsUpdatedEvent @event)
        {

            var tmpNews = News.FirstOrDefault(news => news.Id == @event.NewsId);

            if (tmpNews != null)
                tmpNews.Title = @event.Title;
                tmpNews.Summary = @event.Summary;
                tmpNews.Content = @event.Content;
        }

        public void Handle(NewsPublishedEvent @event)
        {
            var tmpNews = News.FirstOrDefault(news => news.Id == @event.NewsId);

            if (tmpNews != null)
                tmpNews.Status = "Published";

        }
        public void Handle(NewsUnpublishedEvent @event)
        {
            var tmpNews = News.FirstOrDefault(news => news.Id == @event.NewsId);

            if (tmpNews != null)
                tmpNews.Status = "Created";

        }
    }
}