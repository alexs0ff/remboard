using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Romontinka.Server.WebSite.Models.Controls;

namespace Romontinka.Server.WebSite.Helpers
{
    /// <summary>
    /// Хелпер для списка checkboxов, получающие данные через Ajax запросы.
    /// </summary>
    public static class AjaxCheckBoxListHelpers
    {
        private const string GetItemsActionDefault = "GetItems";

        public static MvcHtmlString AjaxCheckBoxList(this HtmlHelper html, string propertyId, bool isRequired, string[] values, string parentId, string controller, string getItemsAction, string editorClasses)
        {
            return AjaxCheckBoxList(html, new AjaxCheckBoxListModel
            {
                Controller = controller,
                ParentId = parentId,
                GetItemsAction = getItemsAction,
                Property = propertyId,
                Values = values,
                EditorClasses = editorClasses,
                IsRequired = isRequired
            });
        }

        public static MvcHtmlString AjaxCheckBoxList(this HtmlHelper html, AjaxCheckBoxListModel model)
        {
            if (string.IsNullOrWhiteSpace(model.GetItemsAction))
            {
                model.GetItemsAction = GetItemsActionDefault;
            } //if

            if (string.IsNullOrWhiteSpace(model.Controller))
            {
                model.Controller = model.Property;
            } //if
            return html.Partial("AjaxCheckBoxList", model);
        }
    }
}