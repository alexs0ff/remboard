using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.FinancialGroupForm
{
    /// <summary>
    /// Модель создания и редактирования финансовых групп филиалов.
    /// </summary>
    public class FinancialGroupCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает название финансовой группы.
        /// </summary>
        [DisplayName("Название")]
        [Required]
        [EditorHtmlClass("financial-group-edit")]
        [LabelHtmlClass("financial-group-label")]
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает юр название фирмы.
        /// </summary>
        [DisplayName("Юр название")]
        [Required]
        [EditorHtmlClass("financial-group-edit")]
        [LabelHtmlClass("financial-group-label")]
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает торговую марку фирмы.
        /// </summary>
        [DisplayName("Торговая марка")]
        [Required]
        [EditorHtmlClass("financial-group-edit")]
        [LabelHtmlClass("financial-group-label")]
        public string Trademark { get; set; }

        /// <summary>
        /// Задает или получает список филиалов.
        /// </summary>
        [UIHint("AjaxCheckBoxList")]
        [DisplayName("Филиалы")]
        [AjaxCheckBoxList("BranchesCheckBoxList")]
        [EditorHtmlClass("financial-group-edit")]
        [LabelHtmlClass("financial-group-label")]
        [Required]
        public Guid?[] BranchIds { get; set; }

        /// <summary>
        /// Задает или получает список складов.
        /// </summary>
        [UIHint("AjaxCheckBoxList")]
        [DisplayName("Склады")]
        [AjaxCheckBoxList("WarehousesCheckBoxList")]
        [EditorHtmlClass("financial-group-edit")]
        [LabelHtmlClass("financial-group-label")]
        public Guid?[] WarehouseIds { get; set; }
    }
}