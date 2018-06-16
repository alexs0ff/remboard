using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.ReportInputs
{
    /// <summary>
    /// Форма ввода для отчета по движениям на складе.
    /// </summary>
    public class WarehouseFlowReportInput : JReportParametersBaseModel
    {
        /// <summary>
        /// Задает или получает код склада.
        /// </summary>
        [DisplayName("Склад")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("report-edit")]
        [LabelHtmlClass("report-label")]
        [Required]
        [AjaxComboBox("AjaxWarehouseComboBox")]
        public Guid? WarehouseID { get; set; }

        /// <summary>
        /// Задает или получает начальную дату задач.
        /// </summary>
        [DisplayName("Начало периода")]
        [EditorHtmlClass("report-edit")]
        [LabelHtmlClass("report-label")]
        [Required]
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Задает или получает окончательную дату задач.
        /// </summary>
        [DisplayName("Окончание периода")]
        [EditorHtmlClass("report-edit")]
        [LabelHtmlClass("report-label")]
        [Required]
        public DateTime EndDate { get; set; }
    }
}