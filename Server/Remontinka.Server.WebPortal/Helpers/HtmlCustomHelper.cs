using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Remontinka.Server.WebPortal.Helpers
{
    /// <summary>
    /// Хэлпер для работы с html.
    /// </summary>
    public static class HtmlCustomHelper
    {
        /// <summary>
        /// Создает настроенный grid для DevExpress.
        /// </summary>
        /// <param name="html">Текущий контекст.</param>
        /// <param name="gridControllerName">Имя контроллера грида.</param>
        /// <param name="parentId">Код связанной сущности.</param>
        /// <returns>Результат.</returns>
        public static MvcHtmlString DevexpressGrid(this HtmlHelper html, string gridControllerName,string parentId)
        {
            return html.Action("GetGrid", gridControllerName, new {parentId});
        }

        /// <summary>
        /// Создает настроенный grid для DevExpress.
        /// </summary>
        /// <param name="html">Текущий контекст.</param>
        /// <param name="gridControllerName">Имя контроллера грида.</param>
        /// <returns>Результат.</returns>
        public static MvcHtmlString DevexpressGrid(this HtmlHelper html, string gridControllerName)
        {
            return html.DevexpressGrid(gridControllerName, null);
        }

        /// <summary>
        /// Создает настроенный report viewer для DevExpress.
        /// </summary>
        /// <param name="html">Текущий контекст.</param>
        /// <param name="reportControllerName">Имя контроллера отчета.</param>
        /// <returns>Результат.</returns>
        public static MvcHtmlString DevexpressReport(this HtmlHelper html, string reportControllerName)
        {
            return html.Action("ReportView", reportControllerName);
        }

        /// <summary>
        /// Создает настроенный комбобокс выбора фильтров для грида.
        /// </summary>
        /// <param name="html">Текущий контекст.</param>
        /// <param name="gridControllerName">Имя контроллера грида.</param>
        /// <returns>Результат.</returns>
        public static MvcHtmlString DevexpressGridFilterComboBox(this HtmlHelper html, string gridControllerName)
        {
            return MvcHtmlString.Create(html.Action("GetFilterNamePopup", gridControllerName).ToString()+
            html.Action("GetFilters", gridControllerName).ToString());
        }

        public static MvcHtmlString LiActionLink(this HtmlHelper html, string text, string action, string controller)
        {
            var context = html.ViewContext;
            if (context.Controller.ControllerContext.IsChildAction)
                context = html.ViewContext.ParentActionViewContext;
            var routeValues = context.RouteData.Values;
            var currentAction = routeValues["action"].ToString();
            var currentController = routeValues["controller"].ToString();

            var str = String.Format("<li{0}>{1}</li>",
                currentAction.Equals(action, StringComparison.InvariantCulture) &&
                currentController.Equals(controller, StringComparison.InvariantCulture) ?
                " class=\"current\"" :
                String.Empty, html.ActionLink(text, action, controller).ToHtmlString()
            );
            return new MvcHtmlString(str);
        }
    }
}