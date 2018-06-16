using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Helpers;

namespace Romontinka.Server.WebSite.Models.DataGrid
{
    /// <summary>
    /// Лукап для строки поиска.
    /// </summary>
    public class LookupSearchInput : SearchInputBase
    {
        /// <summary>
        /// Задает или получает модель лукапа для отображения контрола.
        /// </summary>
        public LookupModel LookupModel { get; set; }
    }
}