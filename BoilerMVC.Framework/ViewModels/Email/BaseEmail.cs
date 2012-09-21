using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Postal;

namespace BoilerMVC.Framework.ViewModels
{
    public class BaseEmail : Email
    {
        public string To { get; set; }

        public string From { get; set; }

        public string Subject { get; set; }
    }
}