using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Результат выполнения содержащий элементы данных.
    /// </summary>
    /// <typeparam name="T">Тип данных для элементов данных.</typeparam>
    public class JCrudItemsResult<T> : JCrudResult
    {
        /// <summary>
        /// Задает или получает элементы для передачи.
        /// </summary>
        public IEnumerable<T> Items { get; set; }
    }
}