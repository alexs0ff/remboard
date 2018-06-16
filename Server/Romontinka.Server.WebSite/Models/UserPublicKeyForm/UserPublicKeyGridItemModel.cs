using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.UserPublicKeyForm
{
    public class UserPublicKeyGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает логин пользователя.
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// Задает или получает ФИО пользователя.
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Задает или получает дату ключа.
        /// </summary>
        public string EventDate { get; set; }

        /// <summary>
        /// Задает или получает признак отозванного ключа.
        /// </summary>
        public string Status { get; set; }
    }
}