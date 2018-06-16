using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.UserForm
{
    /// <summary>
    /// Пункт списка пользователей.
    /// </summary>
    public class UserListItem
    {
        /// <summary>
        /// Задает или получает код пользователя.
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        /// Задает или получает роль в проекте.
        /// </summary>
        public byte? ProjectRoleID { get; set; }

        /// <summary>
        /// Задает или получает название роли пользователя.
        /// </summary>
        public byte? ProjectRoleTitle { get; set; }

        /// <summary>
        /// Задает или получает логин пользователя.
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// Задает или получает имя пользователя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Задает или получает Фамилию пользователя.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Задает или получает отчетство пользователя.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Задает или получает телефон пользователя.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Задает или получает Email пользователя.
        /// </summary>
        public string Email { get; set; }
    }
}