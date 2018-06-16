using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.ItemCategoryForm
{
    /// <summary>
    /// Модель для поиска категорий в гриде.
    /// </summary>
    public class ItemCategorySearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поика по имени.
        /// </summary>
        public string ItemCategoryName { get; set; }
    }
}