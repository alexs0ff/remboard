using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Представляет модель сущностей грида.
    /// </summary>
    /// <typeparam name="T">Тип идентификатора.</typeparam>
    public class JGridItemModel<T>
        where T:struct
    {
        /// <summary>
        /// Задает или получает идентификатор сущности.
        /// </summary>
        public T? Id { get; set; }

        /// <summary>
        /// Задает или получает текущий класс строки.
        /// </summary>
        public string RowClass { get; set; }
    }
}