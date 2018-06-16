using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.IncomingDocItemsReport
{
    public class IncomingDocItemsReportParameters : ReportParametersModelBase
    {
        /// <summary>
        /// Задает или получает код приходной накладной.
        /// </summary>
        [ReportParameter(IsHidden = true)]
        public Guid? IncomingDocID { get; set; }
    }
}