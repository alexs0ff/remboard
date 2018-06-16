using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Результат с данными для Грида.
    /// </summary>
    public class JGridDataResult<T> : JCrudResult
    {
        /// <summary>
        /// Задает или получает элементы для передачи.
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Задает или получает общее количество элементов.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Задает или получает количество элементов на странице.
        /// </summary>
        public int PerPage { get; set; }

        /// <summary>
        /// Задает или получает текущую страницу.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Задает или получает максимальное количество страниц в пагинаторе.
        /// </summary>
        public int PaginatorMaxItems { get; set; }
    }
}