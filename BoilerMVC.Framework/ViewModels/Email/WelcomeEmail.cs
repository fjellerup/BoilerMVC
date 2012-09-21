using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoilerMVC.Framework.ViewModels
{
    public class WelcomeEmail : BaseEmail
    {
        public string Password { get; set; }

        public string LoginUrl { get; set; }

        public string WebsiteName { get; set; }
    }
}