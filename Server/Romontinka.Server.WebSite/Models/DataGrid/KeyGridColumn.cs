using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.DataGrid
{
    /// <summary>
    /// Задает или получает ключевую колонку.
    /// Она не отображается.
    /// </summary>
    public class KeyGridColumn
    {
        /// <summary>
        /// Задает или получает идентификатор в данных.
        /// </summary>
        public string Id { get; set; }
    }
}