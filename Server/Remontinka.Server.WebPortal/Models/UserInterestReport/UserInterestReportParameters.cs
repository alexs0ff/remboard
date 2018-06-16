using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.UserInterestReport
{
    /// <summary>
    /// Параметры отчета по вознаграждениям пользователей.
    /// </summary>
    public class UserInterestReportParameters : ReportParametersModelBase
    {
        /// <summary>
        /// Задает или получает начальную дату задач.
        /// </summary>
        [DisplayName("Дата начала выполнения")]
        [Required]
        [ReportParameter]
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Задает или получает окончательную дату задач.
        /// </summary>
        [DisplayName("Дата окончания выполнения")]
        [Required]
        [ReportParameter]
        public DateTime EndDate { get; set; }
    }
}