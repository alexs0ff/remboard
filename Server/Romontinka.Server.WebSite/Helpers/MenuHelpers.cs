using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Romontinka.Server.WebSite.Helpers
{
    /// <summary>
    /// Хелперы для меню.
    /// </summary>
    public static class MenuHelpers
    {
        /// <summary>
        /// Осуществяет выбор для меню в зависимости от текущего контроллера и действия.
        /// </summary>
        /// <param name="html">Контекст.</param>
        /// <param name="controllerName">Имя необходимого контроллера.</param>
        /// <param name="actionName">Имя необходимого действия.</param>
        /// <param name="activeClass">Имя возвращаемого класса в случае совпадения по контроллеру и методу.</param>
        /// <param name="unactiveClass">Имя возвращаемого класса в случае отсутствия совпадения по контроллеру и методу</param>
        /// <returns></returns>
        public static string GetCurrentClass(this HtmlHelper html,string controllerName, string actionName,string activeClass,string unactiveClass)
        {
            var currentAction = "index";
            var currentController = string.Empty;

            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            if (routeValues != null)
            {
                if (routeValues.ContainsKey("action"))
                {
                    currentAction = routeValues["action"].ToString();
                }
                if (routeValues.ContainsKey("controller"))
                {
                    currentController = routeValues["controller"].ToString();
                }
            }

            if (StringComparer.OrdinalIgnoreCase.Equals(controllerName,currentController)&& StringComparer.OrdinalIgnoreCase.Equals(currentAction,actionName))
            {
                return activeClass;
            }

            return unactiveClass;
        }
    }
}