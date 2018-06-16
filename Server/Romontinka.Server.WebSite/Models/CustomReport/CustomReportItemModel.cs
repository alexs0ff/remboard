using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.CustomReport
{
    /// <summary>
    /// Модель для создания и редактирования сущностей документов.
    /// </summary>
    public class CustomReportItemModel : JCrudModelBase
    {
        /// <summary>
        /// Задает или получает код документа.
        /// </summary>
        public Guid? CustomReportItemID { get; set; }

        /// <summary>
        /// Задает или получает название документа.
        /// </summary>
        [DisplayName("Название")]
        [Required]
        [EditorHtmlClass("customreport-edit")]
        [LabelHtmlClass("customreport-label")]
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает тип настраеваемого документа.
        /// </summary>
        [DisplayName("Тип")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("customreport-edit")]
        [LabelHtmlClass("customreport-label")]
        [Required]
        [AjaxComboBox("AjaxDocumentKindComboBox")]
        public byte? DocumentKindID { get; set; }
    }
}