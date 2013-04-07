using System;
using System.Web.Script.Services;
using System.Web.Services;
using AccountManager.Models.News.ReadModel;
using System.Collections.Generic;

namespace AccountManager.WebServices
{
    public partial class test : System.Web.UI.Page
    {
        [WebMethod(EnableSession = false)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string HelloWorld()
        {
            return "Hello: " + DateTime.Now.Millisecond;
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<NewsModel> sayHello()
        {
            NewsModel n;
            List<NewsModel> newsS = new List<NewsModel>();

            n = new NewsModel(Guid.NewGuid(), "titre", "summary", "conten content");
            newsS.Add(n);
            n = new NewsModel(Guid.NewGuid(), "titre2", "summary2", "conten content 2");
            newsS.Add(n);

            return newsS;
            //return "hello ";
        }
    }
}