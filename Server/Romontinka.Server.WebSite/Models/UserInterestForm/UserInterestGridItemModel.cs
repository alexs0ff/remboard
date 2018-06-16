using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.UserInterestForm
{
    /// <summary>
    /// Модель пункто грида для списка вознаграждений пользователей.
    /// </summary>
    public class UserInterestGridItemModel:  JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает ФИО пользователя.
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Задает или получает значения.
        /// </summary>
        public string Values { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Задает или получает дату.
        /// </summary>
        public string EventDate { get; set; }
    }
}