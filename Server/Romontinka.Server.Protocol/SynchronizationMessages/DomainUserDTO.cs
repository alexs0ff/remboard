using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объект пользователя домена.
    /// </summary>
    public class DomainUserDTO
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

        /// <summary>
        /// Задает или получает логин.
        /// </summary>
        public string LoginName { get; set; }
    }
}
