using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.EngineerWorkReport
{
    /// <summary>
    /// Параметры для отчета выполненных работ.
    /// </summary>
    public class EngineerWorkReportParameters: ReportParametersModelBase
    {
        /// <summary>
        /// Задает или получает код инженера.
        /// </summary>
        [DisplayName("Инженер")]
        [ReportParameter]
        public Guid? EngineerWorkReportUserID { get; set; }

        /// <summary>
        /// Задает или получает инжинеров.
        /// </summary>
        public List<SelectListItem<Guid>> Engineers { get; set; }

        /// <summary>
        /// Задает или получает начальную дату задач.
        /// </summary>
        [DisplayName("Дата начала выполнения")]
        [Required]
        [ReportParameter]
        public DateTime? EngineerWorkReportBeginDate { get; set; }

        /// <summary>
        /// Задает или получает окончательную дату задач.
        /// </summary>
        [DisplayName("Дата окончания выполнения")]
        [Required]
        [ReportParameter]
        public DateTime? EngineerWorkReportEndDate { get; set; }
    }
}