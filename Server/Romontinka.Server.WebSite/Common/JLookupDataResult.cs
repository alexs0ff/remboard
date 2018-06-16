using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Результат работы данными для грида.
    /// </summary>
    public class JLookupDataResult : JCrudResult
    {
        /// <summary>
        /// Задает или получает отрендеренные данные с элементами.
        /// </summary>
        public string ItemsData { get; set; }

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