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
    /// Модель для полей ввода отчета по выполненной работе.
    /// </summary>
    public class EngineerWorkReportInput : JReportParametersBaseModel
    {
        /// <summary>
        /// Задает или получает код инженера.
        /// </summary>
        [DisplayName("Инженер")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("report-edit")]
        [LabelHtmlClass("report-label")]
        [AjaxComboBox("AjaxEngineerComboBox")]
        public Guid? UserID { get; set; }

        /// <summary>
        /// Задает или получает начальную дату задач.
        /// </summary>
        [DisplayName("Дата начала выполнения")]
        [EditorHtmlClass("report-edit")]
        [LabelHtmlClass("report-label")]
        [Required]
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Задает или получает окончательную дату задач.
        /// </summary>
        [DisplayName("Дата окончания выполнения")]
        [EditorHtmlClass("report-edit")]
        [LabelHtmlClass("report-label")]
        [Required]
        public DateTime EndDate { get; set; }
    }
}