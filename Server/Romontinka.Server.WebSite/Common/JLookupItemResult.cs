using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Результат выдачи конкретного элемента для lookup.
    /// </summary>
    public class JLookupItemResult : JCrudResult
    {
        /// <summary>
        /// Задает или получает строковое значение, которое отображается в поле лукапа.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Задает или получает код элемента.
        /// </summary>
        public string Id { get; set; }
    }
}