using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoilerMVC.Common;
using BoilerMVC.Data;
using BoilerMVC.Framework;
using BoilerMVC.Framework.ViewModels;
using BoilerMVC.Services;

namespace BoilerMVC.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}