using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.DataGrid
{
    /// <summary>
    /// Описатель колонки с детализацией.
    /// </summary>
    public class ShowDetailsButtonColumn
    {
        /// <summary>
        /// Задает или получает подсказку к кнопке.
        /// </summary>
        public string ToolTip { get; set; }

        /// <summary>
        /// Задает или получает имя функции вызова в которую первым параметром передается .
        /// </summary>
        public string CallFunctionName { get; set; }
    }
}