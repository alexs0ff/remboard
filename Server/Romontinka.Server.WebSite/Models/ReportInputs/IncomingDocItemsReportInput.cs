using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.ReportInputs
{
    /// <summary>
    /// Форма ввода для отчета о входных накладных.
    /// </summary>
    public class IncomingDocItemsReportInput : JReportParametersBaseModel
    {
        /// <summary>
        /// Задает или получает код приходной накладной.
        /// </summary>
        public Guid? IncomingDocID { get; set; }
    }
}