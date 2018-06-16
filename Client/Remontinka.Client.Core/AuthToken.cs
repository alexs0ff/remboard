using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Текущие токен безопасности.
    /// </summary>
    public sealed class AuthToken
    {
        /// <summary>
        /// Задает или получает наименование логина текущего пользователя.
        /// </summary>
        public string LoginName { get; internal set; }

        /// <summary>
        /// Задает или получает код текущего пользователя.
        /// </summary>
        public Guid? UserID { get; internal set; }

        /// <summary>
        /// Задает или получает код домена пользователя.
        /// </summary>
        public Guid? UserDomainID { get; internal set; }

        /// <summary>
        /// Задает или получает данные ключа.
        /// </summary>
        public string PrivateKeyData { get; internal set; }

        /// <summary>
        /// Задает или получает номер ключа.
        /// </summary>
        public string KeyNumber { get; internal set; }

        /// <summary>
        /// Задает или получает текущий пароль.
        /// </summary>
        public SecureString Password { get; internal set; }
    }
}
