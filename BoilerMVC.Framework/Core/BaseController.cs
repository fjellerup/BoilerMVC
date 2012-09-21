using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Postal;

namespace BoilerMVC.Framework
{
    public class BaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (TempData[ViewDataKeys.ModelState] != null && !ModelState.Equals(TempData[ViewDataKeys.ModelState]))
                ModelState.Merge((ModelStateDictionary)TempData[ViewDataKeys.ModelState]);
        }

        public void SetError(string message)
        {
            ViewData[ViewDataKeys.ErrorMessage] = message;
            TempData[ViewDataKeys.ErrorMessage] = message;
        }

        public void SetError(string message, params object[] args)
        {
            string formattedMessage = string.Format(message, args);
            SetError(formattedMessage);
        }

        public void SetSuccess(string message)
        {
            TempData[ViewDataKeys.SuccessMessage] = message;
            ViewData[ViewDataKeys.SuccessMessage] = message;
        }

        public void SetNotice(string message)
        {
            TempData[ViewDataKeys.NoticeMessage] = message;
            ViewData[ViewDataKeys.NoticeMessage] = message;
        }

        protected void PersistModelState()
        {
            TempData[ViewDataKeys.ModelState] = ModelState;
            TempData[ViewDataKeys.ModelStatePersisted] = true;
        }

        protected bool IsModelValidAndPersistErrors()
        {
            PersistModelState();
            return ModelState.IsValid;
        }

        protected RedirectToRouteResult RedirectToSelf()
        {
            return RedirectToSelf(null);
        }

        protected RedirectToRouteResult RedirectToSelf(object routeValues)
        {
            RouteValueDictionary routeData = new RouteValueDictionary(routeValues);
            string controller = (string)this.ControllerContext.RouteData.Values["controller"];
            string action = (string)this.ControllerContext.RouteData.Values["action"];
            routeData.Add("controller", controller);
            routeData.Add("action", action);

            return RedirectToRoute(routeData);
        }
    }
}