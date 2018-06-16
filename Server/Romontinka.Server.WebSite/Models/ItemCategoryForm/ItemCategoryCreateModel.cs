using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.ItemCategoryForm
{
    /// <summary>
    /// Модель категорий товаров.
    /// </summary>
    public class ItemCategoryCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или название категории.
        /// </summary>
        [DisplayName("Название")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string Title { get; set; }
    }
}