using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Модель для поиска в lookup.
    /// </summary>
    public class JLookupSearchBaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public JLookupSearchBaseModel()
        {
            Page = 1;
        }

        /// <summary>
        /// Задает или получает страницу с которой необходимо делать выборку данных.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Задает или получает родительское значение контрола.
        /// </summary>
        public string Parent { get; set; }
    }
}