using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace Romontinka.Server.WebSite.Common.RdlcReport
{
    /// <summary>
    /// Результат выполнения регистрации данных для контрооллера отчетов.
    /// </summary>
    internal class JReportResult
    {
        /// <summary>
        /// Задает или получает отчет.
        /// </summary>
        public LocalReport Report { get; set; }

        /// <summary>
        /// Задает или получает название файла для отчета.
        /// </summary>
        public string OutputFileName { get; set; }
    }
}