using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Результат выполнения отчета.
    /// </summary>
    public class JReportDataResult : JCrudResult
    {
        /// <summary>
        /// Задает или получает данные для отчета.
        /// </summary>
        public string[] ReportData { get; set; }
    }
}