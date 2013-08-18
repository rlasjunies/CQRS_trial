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
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            var infos = Configuration.Instance().NewsReadModel.News;
            Configuration.Context().NewsModels = infos;
            
            return View("Login", Configuration.Context());
        }

        public ActionResult Logout()
        {
            var infos = Configuration.Instance().NewsReadModel.News;
            Configuration.Context().NewsModels = infos;
            return View("Logout", Configuration.Context());
        }


        [HttpPost]
        public ActionResult LoginCmd(LoginCommand command)
        {
            if (!Configuration.Context().logged)
            {
                Configuration.Context().logged = true;
            }
            return Redirect("/newshtml/index");
            

        }

        [HttpPost]
        public ActionResult LogoutCmd(LoginCommand command)
        {
            if (Configuration.Context().logged)
            {
                Configuration.Context().logged = false;
            }

            return Redirect("/newshtml/index");
        }

    }
}
