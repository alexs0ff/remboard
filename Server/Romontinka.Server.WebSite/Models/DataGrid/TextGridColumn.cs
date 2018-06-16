using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.DataGrid
{
    /// <summary>
    /// Задает или получает текстовую колонку.
    /// </summary>
    public class TextGridColumn : GridColumnBase
    {
        /// <summary>
        /// Задает или получает идентификатор колонки.
        /// </summary>
        public string Id { get; set; }
    }
}