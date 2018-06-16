using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.DataGrid
{
    /// <summary>
    /// Базовый класс для всех колонок в гриде.
    /// </summary>
    public abstract class GridColumnBase
    {
        /// <summary>
        /// Задает или получает наименование колонки.
        /// </summary>
        public string Name { get; set; }
    }
}