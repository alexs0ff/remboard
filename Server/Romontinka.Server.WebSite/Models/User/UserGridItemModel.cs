using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.User
{
    /// <summary>
    /// Модель строки грида.
    /// </summary>
    public class UserGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает логин пользователя.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Задает или получает ФИО.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Задает или получает контакты пользователя.
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// Задает или получает название проектной роли пользователя.
        /// </summary>
        public string ProjectRoleTitle { get; set; }
    }
}