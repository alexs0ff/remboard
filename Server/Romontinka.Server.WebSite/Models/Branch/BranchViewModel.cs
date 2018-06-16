using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.Branch
{
    /// <summary>
    /// Модель отображения для списка филиалов.
    /// </summary>
    public class BranchViewModel
    {
        /// <summary>
        /// Задает или получает описатель грида.
        /// </summary>
        public DataGridDescriptor BranchesGrid { get; set; }
    }
}