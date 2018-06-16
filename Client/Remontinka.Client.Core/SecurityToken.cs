using System;
using Remontinka.Client.DataLayer.Entities;

namespace Remontinka.Client.Core
{
    /// <summary>
    ///   Токен безопасности текущего контекста.
    /// </summary>
    public class SecurityToken
    {
        /// <summary>
        ///   Получает или задает имя логина текущего пользователя.
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// Получает или задает Email для пользователя.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Получает или задает ассоциированного пользователя.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Получает код пользователя.
        /// </summary>
        public Guid? UserID
        {
            get { return User.UserIDGuid; }
        }
    }
}
