using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Представляет собой пункт выбора.
    /// </summary>
    /// <typeparam name="T">Тип идентификатора.</typeparam>
    public class JSelectListItem<T> where T:struct 
    {
        /// <summary>
        /// Задает или получает текст для надписи.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Задает или получает значение.
        /// </summary>
        public T? Value { get; set; }

        /// <summary>
        /// Задает или получает признак выбора.
        /// </summary>
        public bool Selected { get; set; }
    }
}