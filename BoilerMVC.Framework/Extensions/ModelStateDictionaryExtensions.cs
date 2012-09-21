using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace BoilerMVC.Web
{
    public static class ModelStateDictionaryExtensions
    {
        public static void AddModelErrorFor<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, object>> expression, string errorMessage) where TModel : class
        {
            var inputName = ExpressionHelper.GetExpressionText(expression.Body.ToString());
            inputName = inputName.Replace(expression.Parameters[0].Name + ".", "")
                                 .Replace("Convert(", "").TrimEnd(new char[] { ')' });

            modelState.AddModelError(inputName, errorMessage);
        }
    }
}