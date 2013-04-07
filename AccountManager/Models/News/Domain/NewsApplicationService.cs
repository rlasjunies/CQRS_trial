using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MTG.Core;
using AccountManager.Models.News.Commands;

namespace AccountManager.Models.News.Domain
{
    public class NewsApplicationService
    {
        private readonly IRepository _repository;

        public NewsApplicationService(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(CreateNewsCommand command)
        {
            var news = new News(command.Id, command.Title, command.Summary, command.Content);
            _repository.Save(news);
        }

        public void Handle(UpdateNewsCommand command)
        {
            var news = _repository.GetById<News>(command.Id);
            news.Update(command.Title, command.Summary, command.Content);
            _repository.Save(news);
        }

        public void Handle(PublishNewsCommand command)
        {
            var news = _repository.GetById<News>(command.Id);
            news.Publish();
            _repository.Save(news);
        }

        public void Handle(DeleteNewsCommand command)
        {
            var news = _repository.GetById<News>(command.Id);
            news.Delete();
            _repository.Save(news);
        }

        public void Handle(UnpublishNewsCommand command)
        {
            var news = _repository.GetById<News>(command.Id);
            news.Unpublish();
            _repository.Save(news);
        }


        //public void Handle(DebitAccountCommand command)
        //{
        //    var account = _repository.GetById<News>(command.AccountId);
        //    account.Debit(command.Amount);
        //    _repository.Save(account);
        //}

        //public void Handle(UnlockAccountCommand command)
        //{
        //    var account = _repository.GetById<News>(command.AccountId);
        //    account.UnlockAccount();
        //    _repository.Save(account);
        //}
    }
}