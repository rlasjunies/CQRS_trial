using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MTG.Core;
using AccountManager.Models.News.Commands;
using AccountManager.Models.News.Domain;
using AccountManager.Models.News.ReadModel;

namespace AccountManager.Controllers
{
    public class NewsHTMLController : Controller
    {
        public ActionResult Index()
        {
            var infos = Configuration.Instance().NewsReadModel.News;
            return View("Index",infos);
        }

        public ActionResult Edit( Guid @id)
        {
            var news = Configuration.Instance().NewsReadModel.News.First(x => x.Id == @id);

            return View("Edit", news);
        }

        public ActionResult deleteConfirmation(Guid @id)
        {
            var news = Configuration.Instance().NewsReadModel.News.First(x => x.Id == @id);

            return View("DeleteConfirmation", news);
        }

        public ActionResult Delete(Guid @id)
        {
            var command = new DeleteNewsCommand { Id = @id };
            Configuration.Instance().Bus.Handle(command);

            return this.Index();
        }

        public ActionResult Actions(Guid @id)
        {
            var news = Configuration.Instance().NewsReadModel.News.First(x => x.Id == @id);

            return View("Actions", news);
        }

        [HttpGet]
        public ActionResult CreateNews()
        {
            var command = new CreateNewsCommand { Id = Guid.NewGuid() };
            return View("CreateNews", command);
        }
        
        [HttpPost]
        public ActionResult CreateNews(CreateNewsCommand command)
        {
            Configuration.Instance().Bus.Handle(command);
            var news = Configuration.Instance().NewsReadModel.News;
            return View("Index", news);
        }

        [HttpPost]
        public ActionResult Update(UpdateNewsCommand command)
        {
            Configuration.Instance().Bus.Handle(command);

            //var news = Configuration.Instance().NewsReadModel.News;
            //return View("Index",news);
            return this.Index();
        }

    }
}
