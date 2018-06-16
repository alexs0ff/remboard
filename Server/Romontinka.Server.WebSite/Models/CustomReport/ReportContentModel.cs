using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.CustomReport
{
    /// <summary>
    /// Модель для контента отчета.
    /// </summary>
    public class ReportContentModel : JCrudModelBase
    {
        /// <summary>
        /// Задает или получает код отчета.
        /// </summary>
        public Guid? CustomReportID { get; set; }

        /// <summary>
        /// Задает или получает данные по отчету.
        /// </summary>
        public string HtmlContent { get; set; }
    }
}