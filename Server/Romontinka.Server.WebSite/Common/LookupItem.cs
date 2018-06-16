using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Представляет собой пункт для лукапов.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LookupItem<T>
        where T:struct 
    {
        /// <summary>
        /// Задает или получает код лукапа.
        /// </summary>
        public T Id { get; set; }

        /// <summary>
        /// Задает или получает название пункта.
        /// </summary>
        public string Value { get; set; }
    }
}