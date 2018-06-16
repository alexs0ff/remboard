using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Romontinka.Server.WebSite.Models.Controls;

namespace Romontinka.Server.WebSite.Helpers
{
    /// <summary>
    /// Хелпер для автокомплита с локальным источником.
    /// </summary>
    public static class AutocompleteLocalSourceHelpers
    {
        public static MvcHtmlString AutocompleteLocalSourceTextBox(this HtmlHelper html, string propertyId,string value, string source, bool isMultiple,string editorClasses)
        {
            return AutocompleteLocalSourceTextBox(html, new AutocompleteLocalSourceModel
            {
                EditorClasses = editorClasses,
                SourceName = source,
                IsMultiple = isMultiple,
                Property = propertyId,
                Value = value
            });
        }

        public static MvcHtmlString AutocompleteLocalSourceTextBox(this HtmlHelper html, AutocompleteLocalSourceModel model)
        {

            return html.Partial("AutocompleteLocalSourceTextBox", model);
        }

        public static MvcHtmlString AutocompleteLocalSourceCreate(this HtmlHelper html, string name,IEnumerable<string> source)
        {
            var result = new StringBuilder();

            result.AppendLine("var "+name+" = [");
            bool first = true;
            foreach (var str in source)
            {
                if (!first)
                {
                    result.Append(",");
                } //if
                result.AppendLine("'" + str + "'");
                first = false;
            } //foreach
            result.AppendLine(" ];");
            return new MvcHtmlString(result.ToString());
        }
    }
}