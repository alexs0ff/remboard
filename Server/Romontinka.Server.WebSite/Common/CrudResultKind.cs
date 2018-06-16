using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Результат выполнения действий в функции.
    /// </summary>
    public enum CrudResultKind
    {
        /// <summary>
        /// Успешный результат выполнения.
        /// </summary>
        Success,

        /// <summary>
        /// Признак произошедшей ошибки.
        /// </summary>
        Error,

        /// <summary>
        /// Признак необходимости авторизоваться.
        /// </summary>
        Authorize,
    }
}