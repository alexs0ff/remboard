using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.RevenueAndExpenditureReport
{
    /// <summary>
    /// Модель параметров по доходам и расходам.
    /// </summary>
    public class RevenueAndExpenditureReportParameters : ReportParametersModelBase
    {
        /// <summary>
        /// Задает или получает связанные финансовые группы.
        /// </summary>
        public IQueryable<FinancialGroupItem> FinancialGroups { get; set; }

        /// <summary>
        /// Задает или получает начальную дату задач.
        /// </summary>
        [DisplayName("Начало периода")]
        [Required]
        [ReportParameter]
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Задает или получает окончательную дату задач.
        /// </summary>
        [DisplayName("Окончание периода")]
        [Required]
        [ReportParameter]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Задает или получает код финансовой группы.
        /// </summary>
        [DisplayName("Фингруппа")]
        [ReportParameter]
        public Guid? FinancialGroupID { get; set; }
    }
}