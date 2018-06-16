using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.Common
{
    /// <summary>
    /// Описатель параметров отчета.
    /// </summary>
    public class ReportParameterDescriptor
    {
        /// <summary>
        /// Задает или получает название параметра.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Задает или получает тип параметра.
        /// </summary>
        public ReportParameterKind Kind { get; set; }
    }
}