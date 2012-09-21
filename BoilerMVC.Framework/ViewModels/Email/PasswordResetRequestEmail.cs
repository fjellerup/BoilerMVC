using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Postal;

namespace BoilerMVC.Framework.ViewModels
{
    public class PasswordResetRequestEmail : BaseEmail
    {
        public string PasswordResetUrl { get; set; }
    }
}