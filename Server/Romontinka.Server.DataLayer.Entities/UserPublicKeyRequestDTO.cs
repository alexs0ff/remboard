using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// DTO объект для запроса публичных ключей.
    /// </summary>
    public class UserPublicKeyRequestDTO : UserPublicKeyRequest
    {
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
        /// Получает ФИО пользователя.
        /// </summary>
        public string FullName { get { return string.Concat(LastName, " ", FirstName, " ", MiddleName); } }

        /// <summary>
        /// Задает или получает логин пользователя.
        /// </summary>
        public string LoginName { get; set; }
    }
}
