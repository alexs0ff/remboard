using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Metadata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ReportParameterAttribute:Attribute
    {
        /// <summary>
        /// Задает или получает признак скрытого параметра.
        /// </summary>
        public bool IsHidden { get; set; }
    }
}