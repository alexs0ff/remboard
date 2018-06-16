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
    /// Модель фильтров отчетов для вознаграждений пользователей.
    /// </summary>
    public class UserInterestReportInput : JReportParametersBaseModel
    {
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