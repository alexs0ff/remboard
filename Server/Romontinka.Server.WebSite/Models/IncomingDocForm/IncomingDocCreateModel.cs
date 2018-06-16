using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.IncomingDocForm
{
    /// <summary>
    /// Модель создания и редактирования прикладных накладных.
    /// </summary>
    public class IncomingDocCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код склада.
        /// </summary>
        [DisplayName("Склад")]
        [UIHint("AjaxComboBox")]
        [AjaxComboBox("AjaxWarehouseComboBox")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public Guid? WarehouseID { get; set; }
        
        /// <summary>
        /// Задает или получает код контрагента.
        /// </summary>
        [DisplayName("Контрагент")]
        [UIHint("AjaxComboBox")]
        [AjaxComboBox("AjaxContractorComboBox")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public Guid? ContractorID { get; set; }

        /// <summary>
        /// Задает или получает дату накладной.
        /// </summary>
        [DisplayName("Дата накладной")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        [Required]
        public DateTime DocDate { get; set; }

        /// <summary>
        /// Задает или получает номер накладной.
        /// </summary>
        [DisplayName("Номер документа")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string DocNumber { get; set; }

        /// <summary>
        /// Задает или получает описание документа.
        /// </summary>
        [DisplayName("Описание накладной")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        [UIHint("MultilineString")]
        [MultilineString(3, 5)]
        public string DocDescription { get; set; }
    }
}