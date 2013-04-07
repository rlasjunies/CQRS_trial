using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MTG.Core;
using AccountManager.Models.News.Events;

namespace AccountManager.Models.News.Domain
{
    public enum enumNewsStatus
    {
        Created=1,
        Published,
        Deleted
    };

    public class News : Aggregate
    {
        private string _title;
        private string _summary;
        private string _content;
        private enumNewsStatus _status;

        public News(Guid @id, string @title, string @summary, string @content)
            : base(id)
        {
            Apply(new NewsCreatedEvent(@id, @title, @summary, @content));
        }

        private News()
        {
        }

        public void Update(string @title, string @summary, string @content)
        {
            if (_status == enumNewsStatus.Published)
                throw new NewsPublishedException((Guid)Id);

            Apply(new NewsUpdatedEvent((Guid)Id, @title, @summary, @content));

        }

        public void Publish()
        {
            if (_status == enumNewsStatus.Created)
                Apply(new NewsPublishedEvent((Guid)Id));
        }

        public void Delete()
        {
            if (_status == enumNewsStatus.Published)
                throw new NewsPublishedException((Guid)Id);


            if (_status == enumNewsStatus.Created)
                Apply(new NewsDeletedEvent((Guid)Id));
        }

        public void Unpublish()
        {
            if (_status == enumNewsStatus.Created)
                Apply(new NewsUnpublishedEvent((Guid)Id));
        }

        private void UpdateFrom(NewsCreatedEvent @event)
        {
            Id = @event.NewsId;
            _title = @event.Title;
            _summary = @event.Summary;
            _content = @event.Content;
            _status = enumNewsStatus.Created;
        }

        private void UpdateFrom(NewsUpdatedEvent @event)
        {
            _title = @event.Title;
            _summary = @event.Summary;
            _content = @event.Content;
        }

        private void UpdateFrom(NewsPublishedEvent @event)
        {
            _status = enumNewsStatus.Published;
        }

        private void UpdateFrom(NewsDeletedEvent @event)
        {
            _status = enumNewsStatus.Deleted;
        }

        public void updateFrom(NewsUnpublishedEvent @event)
        {
            _status = enumNewsStatus.Created;
        }
    }

    public class NewsPublishedException : Exception
    {
        public NewsPublishedException(Guid @id)
            : base(string.Format("News {0} is 'published' and cannot be updated.", id))
        {
        }
    }
}