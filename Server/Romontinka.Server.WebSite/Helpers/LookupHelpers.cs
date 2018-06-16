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
    /// Хелпер для создания контроля поиска.
    /// </summary>
    public static class LookupHelpers
    {
        private const string GetItemsActionDefault = "GetItems";

        private const string GetItemActionDefault = "GetItem";

        /// <summary>
        /// Создает lookup для определенного поля.
        /// </summary>
        public static MvcHtmlString Lookup(this HtmlHelper html, string propertyName, string propertyValue, string parentID, int minLenght, string editorClasses,bool createValidation)
        {
            return Lookup(html, null, null, null, propertyName, propertyValue, parentID, minLenght, editorClasses,createValidation);
        }

        /// <summary>
        /// Создает lookup для определенного поля.
        /// </summary>
        public static MvcHtmlString Lookup(this HtmlHelper html, string controller, string getItemsAction, string getItemAction, string propertyName, string propertyValue, string parentID, int minLenght, string editorClasses, bool createValidation)
        {
            var model = new LookupModel
                        {
                            GetItemsAction = getItemsAction,
                            GetItemAction = getItemAction,
                            Controller = controller,
                            ParentID = parentID,
                            Property = propertyName,
                            PropertyValue = propertyValue,
                            MinLenght = minLenght,
                            EditorClasses = editorClasses,
                            CreateValidation = createValidation
                        };
            return Lookup(html, model);
        }

        /// <summary>
        /// Создает lookup для определенного поля по модели.
        /// </summary>
        public static MvcHtmlString Lookup(this HtmlHelper html, LookupModel model)
        {
            if (string.IsNullOrWhiteSpace(model.GetItemAction))
            {
                model.GetItemAction = GetItemActionDefault;
            } //if

            if (string.IsNullOrWhiteSpace(model.GetItemsAction))
            {
                model.GetItemsAction = GetItemsActionDefault;
            } //if

            if (string.IsNullOrWhiteSpace(model.Controller))
            {
                model.Controller = model.Property;
            } //if
            return html.Partial("Lookup", model);
        }

        /// <summary>
        /// Создает lookup для определенного поля.
        /// </summary>
        public static MvcHtmlString SingleLookup(this HtmlHelper html, string controller, string getItemsAction, string getItemAction, string propertyName, string propertyValue, string parentID, bool clieanButton, string editorClasses, bool createValidation)
        {
            var model = new SingleLookupModel
            {
                GetItemsAction = getItemsAction,
                GetItemAction = getItemAction,
                Controller = controller,
                ParentID = parentID,
                Property = propertyName,
                PropertyValue = propertyValue,
                ClearButton = clieanButton,
                EditorClasses = editorClasses
            };
            return SingleLookup(html, model);
        }

        private const int SingleLookupMinWidth = 600;

        private const int SingleLookupMinHeight = 480;

        /// <summary>
        /// Создает название свойства отображения вложенных свойств в лукапах.
        /// </summary>
        /// <param name="property">Код основного свойства.</param>
        /// <returns>ID свойства для отображения.</returns>
        public static string CreateLookupDisplayProperty(string property)
        {
            return string.Format("d{0}", property);
        }

        /// <summary>
        /// Создает lookup для определенного поля по модели.
        /// </summary>
        public static MvcHtmlString SingleLookup(this HtmlHelper html, SingleLookupModel model)
        {
            if (string.IsNullOrWhiteSpace(model.GetItemAction))
            {
                model.GetItemAction = GetItemActionDefault;
            } //if

            if (string.IsNullOrWhiteSpace(model.GetItemsAction))
            {
                model.GetItemsAction = GetItemsActionDefault;
            } //if

            if (string.IsNullOrWhiteSpace(model.Controller))
            {
                model.Controller = model.Property;
            } //if

            if (model.Width < SingleLookupMinWidth)
            {
                model.Width = SingleLookupMinWidth;
            } //if

            if (model.Height < SingleLookupMinHeight)
            {
                model.Height = SingleLookupMinHeight;
            } //if

            return html.Partial("SingleLookup", model);
        }
    }
}