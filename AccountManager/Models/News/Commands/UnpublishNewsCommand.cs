using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountManager.Models.News.Commands
{
    public class UnpublishNewsCommand
    {
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        public override string ToString()
        {
            return string.Format("Request is made to unpublish the news {0}", Id);
        }
    }
}
