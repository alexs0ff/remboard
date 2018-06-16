using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.ItemCategoryForm
{
    /// <summary>
    /// Модель пункта грида по категориям номенклатуры.
    /// </summary>
    public class ItemCategoryGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает название категории.
        /// </summary>
        public string Title { get; set; }
    }
}