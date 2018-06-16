using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.UserPublicKeyRequestForm
{
    /// <summary>
    /// Модель для пункта грида.
    /// </summary>
    public class UserPublicKeyRequestGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает логин запросившего пользователя.
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// Задает или получает идентификатор пользователя.
        /// </summary>
        public string ClientIdentifier { get; set; }

        /// <summary>
        /// Задает или получает заметки к ключу.
        /// </summary>
        public string KeyNotes { get; set; }

        /// <summary>
        /// Задает или получает дату запроса.
        /// </summary>
        public string EventDate { get; set; }
    }
}