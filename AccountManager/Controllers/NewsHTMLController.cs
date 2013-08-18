using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MTG.Core;
using AccountManager.Models.News.Commands;
using AccountManager.Models.News.Domain;
using AccountManager.Models.News.ReadModel;
using AccountManager.Models.UI.Commands;

namespace AccountManager.Controllers
{
    public class NewsHTMLController : Controller
    {
        public ActionResult Index()
        {
            var infos = Configuration.Instance().NewsReadModel.News;
            Configuration.Context().NewsModels = infos;
            return View("Index", Configuration.Context());
        }

        public ActionResult Login()
        {
            //var infos = Configuration.Instance().NewsReadModel.News;
            //Configuration.Context().NewsModels = infos;
            return View("Login", Configuration.Context());
        }

        public ActionResult Edit( Guid @id)
        {
            var news = Configuration.Instance().NewsReadModel.News.First(x => x.Id == @id);
            Configuration.Context().NewsModel = news;
            return View("Edit", Configuration.Context());
        }

        public ActionResult deleteConfirmation(Guid @id)
        {
            var news = Configuration.Instance().NewsReadModel.News.First(x => x.Id == @id);
            Configuration.Context().NewsModel = news;
            return View("DeleteConfirmation", Configuration.Context());
        }

        public ActionResult Delete(Guid @id)
        {
            var command = new DeleteNewsCommand { Id = @id };
            Configuration.Instance().Bus.Handle(command);

            return this.Index();
        }

        public ActionResult Biographie()
        {
            return View("Biographie", Configuration.Context());
        }

        public ActionResult Actions(Guid @id)
        {
            var news = Configuration.Instance().NewsReadModel.News.First(x => x.Id == @id);
            Configuration.Context().NewsModel = news;
            return View("Actions", Configuration.Context());
        }

        public ActionResult CreateNews()
        {
            //var command = new CreateNewsCommand { Id = Guid.NewGuid() };
            Configuration.Context().NewsModel = new NewsModel( Guid.NewGuid(),"","","");
            return View("CreateNews",Configuration.Context());
        }

        //[HttpGet]
        //public ActionResult CreateNews()
        //{
        //    var command = new CreateNewsCommand { Id = Guid.NewGuid() };
        //    return View("CreateNews", command);
        //}
        
        [HttpPost]
        public ActionResult CreateNews(CreateNewsCommand command)
        {
            Configuration.Instance().Bus.Handle(command);
            //var news = Configuration.Instance().NewsReadModel.News;
            //Configuration.Context().NewsModels = news;
            //return View("Index", Configuration.Context());
            return this.Index();
        }

        [HttpPost]
        public ActionResult LoginCmd(LoginCommand command)
        {
            //Configuration.Instance().Bus.Handle(command);
            //var news = Configuration.Instance().NewsReadModel.News;
            if (Configuration.Context().logged)
            {
                Configuration.Context().logged = false;
            }
            else
            {
                Configuration.Context().logged = true;
            }
            //return View("Index", Configuration.Context());
            return this.Index();
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
