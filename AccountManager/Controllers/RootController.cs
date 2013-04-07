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
    public class RootController : Controller
    {
        public ActionResult Index()
        {
            var infos = Configuration.Instance().NewsReadModel.News;
            return View(infos);
        }
        
    }
}
