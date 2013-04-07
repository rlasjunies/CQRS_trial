using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountManager.Models.News.Commands
{
    public class CreateNewsCommand
    {
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            return string.Format("Request is made to create a news {0} - {1}", Id, Title);
        }
    }
}
