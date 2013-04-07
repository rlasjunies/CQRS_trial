using AccountManager.Models.News.Domain;
using System;

namespace AccountManager.Models.News.ReadModel
{
    public class NewsModel

    {
        public Guid Id { get; private set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }

        public NewsModel(Guid @id, string @title, string @summary, string @content)
        {
            Id = @id;
            Title = @title;
            Summary = @summary;
            Content = @content;
            Status = "Created";
        }

    }
}