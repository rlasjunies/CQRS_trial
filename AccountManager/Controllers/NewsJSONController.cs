using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MTG.Core;
using AccountManager.Models.News.Commands;
using AccountManager.Models.News.Domain;
using AccountManager.Models.News.ReadModel;
using System.Web.Services;
using System.Web.Script.Services;

namespace AccountManager.Controllers
{
    public class NewsJSONController : Controller
    {
        
        public ActionResult Index()
        {
            var infos = Configuration.Instance().NewsReadModel.News;
            return View(infos);
        }

        public ActionResult Edit( Guid @id)
        {
            var news = Configuration.Instance().NewsReadModel.News.First(x => x.Id == @id);

            return View("Edit", news);
        }
        
        [HttpGet]
        public ActionResult CreateNews()
        {
            var command = new CreateNewsCommand { Id = Guid.NewGuid() };
            return View(command);
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
            var news = Configuration.Instance().NewsReadModel.News;
            return View("Index",news);
        }

    }
}
