using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.WarehouseFlowReport
{
    /// <summary>
    /// Параметры отчета по приходу и расходу на складе.
    /// </summary>
    public class WarehouseFlowReportParameters : ReportParametersModelBase
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

        /// <summary>
        /// Задает или получает код склада.
        /// </summary>
        [DisplayName("Склад")]
        [Required]
        [ReportParameter]
        public Guid? WarehouseID { get; set; }

        /// <summary>
        /// Задает или получает список складов.
        /// </summary>
        public IQueryable<Warehouse> Warehouses { get; set; }
    }
}