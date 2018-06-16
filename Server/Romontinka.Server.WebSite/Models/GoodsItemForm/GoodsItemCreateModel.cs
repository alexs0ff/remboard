using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.GoodsItemForm
{
    /// <summary>
    /// Модель для создания и редактирования номенклатуры.
    /// </summary>
    public class GoodsItemCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код измерения.
        /// </summary>
        [DisplayName("Измерение")]
        [UIHint("AjaxComboBox")]
        [AjaxComboBox("AjaxDimensionKindComboBox")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public byte? DimensionKindID { get; set; }

        /// <summary>
        /// Задает или получает код категории.
        /// </summary>
        [DisplayName("Категория")]
        [UIHint("AjaxComboBox")]
        [AjaxComboBox("AjaxItemCategoryComboBox")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public Guid? ItemCategoryID { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        [DisplayName("Название")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        [DisplayName("Описание")]
        [UIHint("MultilineString")]
        [MultilineString(3, 5)]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string Description { get; set; }

        /// <summary>
        /// Задает или получает код товара.
        /// </summary>
        [DisplayName("Код товара")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string UserCode { get; set; }

        /// <summary>
        /// Задает или получает Артикул.
        /// </summary>
        [DisplayName("Артикул")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string Particular { get; set; }
    }
}