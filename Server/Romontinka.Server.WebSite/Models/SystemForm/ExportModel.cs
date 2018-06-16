using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.SystemForm
{
    /// <summary>
    /// Модель для настройки экспорта.
    /// </summary>
    public class ExportModel
    {
        /// <summary>
        /// Задает или получает дату начала экспорта.
        /// </summary>
        [DisplayName("Дата начала")]
        [Required]
        [EditorHtmlClass("registration-info-edit")]
        [LabelHtmlClass("registration-info-label")]
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Задает или получает дату окончания экспорта.
        /// </summary>
        [DisplayName("Дата окончания")]
        [Required]
        [EditorHtmlClass("registration-info-edit")]
        [LabelHtmlClass("registration-info-label")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Тип экспорта.
        /// </summary>
        [DisplayName("Тип")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("registration-info-edit")]
        [LabelHtmlClass("registration-info-label")]
        [Required]
        [AjaxComboBox("AjaxExportKindComboBox")]
        public int? Kind { get; set; }
    }
}