using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Helpers
{
    /// <summary>
    /// Методы для работы с гридом.
    /// </summary>
    public static class GridHelpers
    {
        /// <summary>
        /// Создает  для определенного поля по модели.
        /// </summary>
        public static MvcHtmlString DataGrid(this HtmlHelper html, DataGridDescriptor model)
        {
            if (string.IsNullOrWhiteSpace(model.Controller))
            {
                model.Controller = model.Name;
            }

            if (string.IsNullOrWhiteSpace(model.CreateItemAction))
            {
                model.CreateItemAction = "GetNewItemForm";
            }

            if (string.IsNullOrWhiteSpace(model.GetItemsAction))
            {
                model.GetItemsAction = "GetItems";
            }
            if (string.IsNullOrWhiteSpace(model.DeleteItemAction))
            {
                model.DeleteItemAction = "DeleteItem";
            }

            if (string.IsNullOrWhiteSpace(model.EditItemAction))
            {
                model.EditItemAction = "GetItemForm";
            }

             if (string.IsNullOrWhiteSpace(model.SaveEditedItemAction))
            {
                model.SaveEditedItemAction = "SaveEditedItem";
            }

             if (string.IsNullOrWhiteSpace(model.SaveEditedItemAction))
             {
                 model.SaveEditedItemAction = "SaveEditedItem";
             }

             if (string.IsNullOrWhiteSpace(model.SaveCreatedItemAction))
             {
                 model.SaveCreatedItemAction = "SaveCreatedItem";
             }
             if (string.IsNullOrWhiteSpace(model.RowClassId))
             {
                 model.RowClassId = "RowClass";
             }
             
            return html.Partial("DataGrid", model);
        }

        /// <summary>
        /// Создает идентификаторы для элементов с определенным именем.
        /// </summary>
        public static string CreateId(this DataGridDescriptor descriptor, string name)
        {
            return string.Concat(descriptor.Name, name);
        }
    }
}