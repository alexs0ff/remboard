using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.Common
{
    /// <summary>
    /// Базовый класс для моделей грида.
    /// </summary>
    public abstract class GridDataModelBase<T> where T : struct
    {
        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public abstract T? GetId();
    }
}