using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountManager.Models
{
    public class RazorContext
    {
        public bool logged = false;
        public List<AccountManager.Models.News.ReadModel.NewsModel> NewsModels;
        public AccountManager.Models.News.ReadModel.NewsModel NewsModel;
    }
}