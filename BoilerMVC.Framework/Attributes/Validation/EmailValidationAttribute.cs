using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace BoilerMVC.Framework
{
    public class EmailValidationAttribute : ValidationAttribute
    {
        private string _emailRegularExpression = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        public override bool IsValid(object value)
        {
            string email = value as string;

            if (string.IsNullOrWhiteSpace(email))
                return false;

            return Regex.IsMatch(email, _emailRegularExpression);

            // Although the code below is the "correct" way of validating an email address,
            // the standard regular expression doesn't catch common typos like
            // forgetting the '.' in the domain name or adding an extra '.'
            /*
            try
            {
                email = new MailAddress(email).Address;
            }
            catch (FormatException)
            {
                return false;
            }
            */
        }
    }
}