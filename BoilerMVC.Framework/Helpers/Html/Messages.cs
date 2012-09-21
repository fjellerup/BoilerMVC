using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BoilerMVC.Framework.Helpers
{
    public static class Messages
    {
        public static IHtmlString RenderMessages(this HtmlHelper html)
        {
            string output = GenerateMessage(ViewDataKeys.SuccessMessage, html);
            output += GenerateMessage(ViewDataKeys.NoticeMessage, html);
            output += GenerateMessage(ViewDataKeys.ErrorMessage, html);
            return new HtmlString(output);
        }

        private static string GenerateMessage(string messageType, HtmlHelper html)
        {
            string message = (string)html.ViewContext.TempData[messageType] ?? (string)html.ViewData[messageType] ?? string.Empty;

            if (string.IsNullOrWhiteSpace(message))
                return string.Empty;

            string cssClass = string.Empty;

            if (messageType == ViewDataKeys.ErrorMessage)
                cssClass = "error";
            else if (messageType == ViewDataKeys.NoticeMessage)
                cssClass = "notice";
            else if (messageType == ViewDataKeys.SuccessMessage)
                cssClass = "success";

            TagBuilder tag = new TagBuilder("div");
            tag.AddCssClass(cssClass);
            tag.AddCssClass("global_message");
            tag.InnerHtml = message;

            return tag.ToString();
        }
    }
}