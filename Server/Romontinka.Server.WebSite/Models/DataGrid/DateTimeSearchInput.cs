using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.DataGrid
{
    /// <summary>
    /// Форма ввода даты.
    /// </summary>
    public class DateTimeSearchInput : SearchInputBase
    {
        /// <summary>
        /// Задает или получает значение для отображения.
        /// </summary>
        public DateTime DateTimeValue { get; set; }

    }
}