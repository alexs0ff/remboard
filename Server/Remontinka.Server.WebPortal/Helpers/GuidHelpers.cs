using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Helpers
{
    /// <summary>
    /// Хелпер для работы с Guid.
    /// </summary>
    public static class GuidHelpers
    {
        /// <summary>
        /// Убирает из Guid все незначащие символы.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns></returns>
        public static string EscapeForHtml(this Guid? value)
        {
            if (!value.HasValue)
            {
                return string.Empty;
            }
            return value.Value.EscapeForHtml();
        }

        /// <summary>
        /// Убирает из Guid все незначащие символы.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns></returns>
        public static string EscapeForHtml(this Guid value)
        {
            return value.ToString().Replace("-", string.Empty);
        }
    }
}