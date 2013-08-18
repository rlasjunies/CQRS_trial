using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace AccountManager
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "newsHTML",
                url: "newshtml/{action}/{id}",
                defaults: new { controller = "NewsHTML", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "biographie",
                url: "biographie/{action}/{id}",
                defaults: new { controller = "biographie", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "login",
                url: "login/{action}/{id}",
                defaults: new { controller = "login", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "newsJSON",
                url: "newsjson/{action}/{id}",
                defaults: new { controller = "NewsJSON", action = "sayHello", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "account",
                url: "account/{action}/{id}",
                defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}