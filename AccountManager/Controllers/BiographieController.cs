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
    public class BiographieController : Controller
    {
        public ActionResult Index()
        {
            return View("Index", Configuration.Context());
        }
    }
}
