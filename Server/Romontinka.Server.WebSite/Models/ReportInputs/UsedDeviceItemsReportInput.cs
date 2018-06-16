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
    /// Модель ввода для отчета по использованным запчастям.
    /// </summary>
    public class UsedDeviceItemsReportInput : JReportParametersBaseModel
    {
        /// <summary>
        /// Задает или получает код финансовой группы.
        /// </summary>
        [DisplayName("Фингруппа")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("report-edit")]
        [LabelHtmlClass("report-label")]
        [AjaxComboBox("AjaxFinancialGroupComboBox")]
        public Guid? FinancialGroupID { get; set; }

        /// <summary>
        /// Задает или получает код филиала.
        /// </summary>
        [DisplayName("Филиал")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("report-edit")]
        [LabelHtmlClass("report-label")]
        [AjaxComboBox("AjaxBranchComboBox")]
        public Guid? BranchID { get; set; }

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