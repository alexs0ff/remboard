using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.Account
{
    /// <summary>
    /// Модель результатов активации зарегистрированного пользователя.
    /// </summary>
    public class ActivateResultModel
    {
        /// <summary>
        /// Задает или получает успех активации.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Задает или получает активированный логин.
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// Задает или получает описание ошибки.
        /// </summary>
        public string ErrorDescription { get; set; }
    }
}