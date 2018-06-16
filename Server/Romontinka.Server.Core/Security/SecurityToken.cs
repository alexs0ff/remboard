using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romontinka.Server.DataLayer.Entities;

namespace Romontinka.Server.Core.Security
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
    }
}
