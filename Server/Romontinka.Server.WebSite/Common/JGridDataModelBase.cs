using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Базовый класс для моделей грида.
    /// </summary>
    public abstract class JGridDataModelBase<T> where T:struct
    {
        /// <summary>
        /// Задает или получает идентификатор.
        /// </summary>
        public T? Id { get; set; }
    }
}