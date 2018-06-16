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
    /// Хелпер для комбобокса, получающего данные через Ajax запросы.
    /// </summary>
    public static class AjaxComboBoxHelpers
    {
        private const string GetItemsActionDefault = "GetItems";

        public static MvcHtmlString AjaxComboBox(this HtmlHelper html, string propertyId, bool firstIsNull,bool isRequired,string value, string controller, string getItemsAction, string editorClasses)
        {
            return AjaxComboBox(html, new AjaxComboBoxModel
                                      {
                                          Controller = controller,
                                          FirstIsNull = firstIsNull,
                                          GetItemsAction = getItemsAction,
                                          Property = propertyId,
                                          Value = value,
                                          EditorClasses = editorClasses,
                                          IsRequired = isRequired,
                                      });
        }

        public static MvcHtmlString AjaxComboBox(this HtmlHelper html, AjaxComboBoxModel model)
        {
            if (string.IsNullOrWhiteSpace(model.GetItemsAction))
            {
                model.GetItemsAction = GetItemsActionDefault;
            } //if

            if (string.IsNullOrWhiteSpace(model.Controller))
            {
                model.Controller = model.Property;
            } //if
            return html.Partial("AjaxComboBox", model);
        }
    }
}