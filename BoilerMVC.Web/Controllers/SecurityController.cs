using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using BoilerMVC.Data;
using BoilerMVC.Framework;
using BoilerMVC.Framework.ViewModels;
using BoilerMVC.Services;

namespace BoilerMVC.Web.Controllers
{
    public class SecurityController : BaseController
    {
        private SecurityService _securityService;
        private UserService _userService;

        public SecurityController(
            SecurityService securityService,
            UserService userService)
        {
            _securityService = securityService;
            _userService = userService;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool valid = _securityService.ValidateLogin(model.Email, model.Password, Request.UserHostAddress);

                if (valid)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
                    SetSuccess("You have been logged in.");
                    return RedirectToRoute(new { controller = "Home", action = "Index" });
                }
                else
                    ModelState.AddModelErrorFor<LoginViewModel>(m => m.Email, "Invalid login information.");
            }

            PersistModelState();

            return RedirectToSelf();
        }

        public ActionResult Logout()
        {
            if (Request.IsAuthenticated)
            {
                SetSuccess("You have been logged out.");
                FormsAuthentication.SignOut();
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (_userService.EmailInUse(model.Email))
                ModelState.AddModelErrorFor<RegisterViewModel>(m => m.Email, "Email is already in use by another user.");

            if (IsModelValidAndPersistErrors())
            {
                User user = _userService.Register(model.Email, model.Password, Request.UserHostAddress);
                SetSuccess("Account successfully created.");
                WelcomeEmail welcomeEmail = Emailer.Welcome(user.Email, model.Password, Url.Action("Login", "Security"));
                welcomeEmail.SendAsync();
                return RedirectToAction("Login");
            }

            return RedirectToSelf();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            PasswordResetRequest passwordResetRequest = null;
            try
            {
                passwordResetRequest = _securityService.IssuePasswordResetRequest(model.Email);
            }
            catch
            {
                ModelState.AddModelErrorFor<ResetPasswordViewModel>(m => m.Email, "Email was not found.");
            }

            if (IsModelValidAndPersistErrors())
            {
                string code = passwordResetRequest.Code.ToString();
                string resetUrl = Url.RouteUrl(new { action = "DoResetPassword", controller = "Security", email = model.Email, code = code });
                var email = Emailer.PasswordResetRequest(model.Email, resetUrl);
                email.SendAsync();
                SetSuccess("A password reset link was sent to the email provided. Please check your email and your spam folders for the link.");
            }

            return RedirectToSelf();
        }

        public ActionResult DoResetPassword(string email, Guid code)
        {
            if (_securityService.ValidatePasswordResetRequest(email, code))
            {
                string newPassword = _securityService.ResetPassword(email);
                string resetUrl = Url.Action("Login", "Security");
                var emailMessage = Emailer.PasswordReset(email, newPassword, resetUrl);
                emailMessage.SendAsync();
                SetSuccess("An email has been sent to your email containing your new password.");
                return RedirectToAction("Login");
            }

            SetError("This password reset link is invalid. Please ensure you used the proper link or request another password reset.");
            return RedirectToSelf();
        }

        [ChildActionOnly]
        public ActionResult LoginMenu()
        {
            return View();
        }
    }
}