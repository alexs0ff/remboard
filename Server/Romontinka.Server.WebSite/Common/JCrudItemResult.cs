using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Результат выполнения содержащий элемент данных.
    /// </summary>
    /// <typeparam name="T">Тип данных для элемента данных.</typeparam>
    public class JCrudItemResult<T> : JCrudResult
    {
        /// <summary>
        /// Задает или получает элемент для передачи.
        /// </summary>
        public T Item { get; set; }
    }
}