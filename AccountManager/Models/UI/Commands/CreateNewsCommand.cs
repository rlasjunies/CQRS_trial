using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountManager.Models.UI.Commands
{
    public class LoginCommand
    {
        public string User { get; set; }
        public string Password { get; set; }

        //public override string ToString()
        //{
        //    return string.Format("Request is made to create a news {0} - {1}", Id, Title);
        //}
    }
}
