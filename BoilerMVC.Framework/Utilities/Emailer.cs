using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoilerMVC.Framework.ViewModels;

namespace BoilerMVC.Framework
{
    public static class Emailer
    {
        public static readonly string FromEmail = "somewhere@test.com";
        public static readonly string BaseUrl = "http://localhost:60097/";
        public static readonly Uri BaseUri = new Uri(BaseUrl);

        public static string CombineUrl(string url)
        {
            Uri uri = new Uri(BaseUri, url);
            return uri.ToString();
        }

        public static T CreateEmail<T>(string to, string subject) where T : BaseEmail, new()
        {
            T email = new T();
            email.From = FromEmail;
            email.To = to;
            email.Subject = subject;
            return email;
        }

        public static WelcomeEmail Welcome(string to, string password, string loginUrl)
        {
            WelcomeEmail email = CreateEmail<WelcomeEmail>(to, "Welcome");
            email.LoginUrl = CombineUrl(loginUrl);
            email.Password = password;
            return email;
        }

        public static PasswordResetRequestEmail PasswordResetRequest(string to, string passwordResetUrl)
        {
            PasswordResetRequestEmail email = CreateEmail<PasswordResetRequestEmail>(to, "Password reset request");
            email.PasswordResetUrl = CombineUrl(passwordResetUrl);
            return email;
        }

        public static PasswordResetEmail PasswordReset(string to, string password, string loginUrl)
        {
            PasswordResetEmail email = CreateEmail<PasswordResetEmail>(to, "Password reset");
            email.Password = password;
            email.LoginUrl = CombineUrl(loginUrl);
            return email;
        }
    }
}