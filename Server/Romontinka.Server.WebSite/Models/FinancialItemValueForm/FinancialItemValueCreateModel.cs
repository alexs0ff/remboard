using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.FinancialItemSingleLookupForm;
using Romontinka.Server.WebSite.Models.User;

namespace Romontinka.Server.WebSite.Models.FinancialItemValueForm
{
    /// <summary>
    /// Модель создания и редактирования значения статьи бюджета.
    /// </summary>
    public class FinancialItemValueCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код статьи бюджета.
        /// </summary>
        [UIHint("SingleLookup")]
        [Required]
        [SingleLookup("FinancialItemSingleLookup", typeof(JLookupFinancialItemSearchModel), null,false , true)]
        [DisplayName("Статья бюджета")]
        [EditorHtmlClass("financial-item-value-edit")]
        [LabelHtmlClass("financial-item-value-label")]
        public Guid? FinancialItemID { get; set; }

        /// <summary>
        /// Задает или получает код финансовой группы.
        /// </summary>
        [DisplayName("Фингруппа")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("financial-item-value-edit")]
        [LabelHtmlClass("financial-item-value-label")]
        [AjaxComboBox("AjaxFinancialGroupComboBox")]
        [Required]
        public Guid? FinancialGroupID { get; set; }
        
        /// <summary>
        /// Задает или получает дату добавления значения.
        /// </summary>
        [DisplayName("Дата")]
        [Required]
        [EditorHtmlClass("financial-item-value-edit")]
        [LabelHtmlClass("financial-item-value-label")]
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает само значение.
        /// </summary>
        [UIHint("Decimal")]
        [DisplayName("Сумма")]
        [Required]
        [EditorHtmlClass("financial-item-value-edit")]
        [LabelHtmlClass("financial-item-value-label")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Задает или получает себестоимость.
        /// </summary>
        [UIHint("Decimal")]
        [DisplayName("Себестоимость")]
        [EditorHtmlClass("financial-item-value-edit")]
        [LabelHtmlClass("financial-item-value-label")]
        public decimal? CostAmount { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        [DisplayName("Примечание")]
        [UIHint("MultilineString")]
        [MultilineString(3, 5)]
        [EditorHtmlClass("financial-item-value-edit")]
        [LabelHtmlClass("financial-item-value-label")]
        public string Description { get; set; }
    }
}