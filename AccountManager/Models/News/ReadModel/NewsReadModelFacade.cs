using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using AccountManager.Models.News.ReadModel;

namespace AccountManager.Models.News.ReadModel
{
    public class NewsReadModelFacade
    {
        public List<NewsModel> News { get; private set; }

        public NewsReadModelFacade(NewsProjection @news)
        {
            News = news.News;
        }
    }
}