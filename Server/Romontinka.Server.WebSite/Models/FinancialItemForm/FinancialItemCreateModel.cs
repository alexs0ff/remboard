using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.FinancialItemForm
{
    /// <summary>
    /// Модель для финансовых статей.
    /// </summary>
    public class FinancialItemCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает название.
        /// </summary>
        [DisplayName("Название")]
        [Required]
        [EditorHtmlClass("financial-item-edit")]
        [LabelHtmlClass("financial-item-label")]
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает описание статьи.
        /// </summary>
        [DisplayName("Описание")]
        [Required]
        [UIHint("MultilineString")]
        [MultilineString(3, 5)]
        [EditorHtmlClass("financial-item-edit")]
        [LabelHtmlClass("financial-item-label")]
        public string Description { get; set; }
        
        /// <summary>
        /// Задает или получает тип статьи бюджета.
        /// </summary>
        [DisplayName("Тип статьи")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("financial-item-edit")]
        [LabelHtmlClass("financial-item-label")]
        [AjaxComboBox("AjaxFinancialItemKindComboBox")]
        [Required]
        public int? FinancialItemKindID { get; set; }

        /// <summary>
        /// Задает или получает код типа операции.
        /// </summary>
        [DisplayName("Доход/расход")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("financial-item-edit")]
        [LabelHtmlClass("financial-item-label")]
        [AjaxComboBox("AjaxTransactionKindComboBox")]
        [Required]
        public byte? TransactionKindID { get; set; }

        /// <summary>
        /// Задает или получает дату введения статьи.
        /// </summary>
        [DisplayName("Дата")]
        [EditorHtmlClass("financial-item-edit")]
        [LabelHtmlClass("financial-item-label")]
        [Required]
        public DateTime EventDate { get; set; }
    }
}