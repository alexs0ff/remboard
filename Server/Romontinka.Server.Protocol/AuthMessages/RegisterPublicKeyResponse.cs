using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.AuthMessages
{
    /// <summary>
    /// Ответ на запрос регистрации ключей.
    /// </summary>
    public class RegisterPublicKeyResponse : MessageBase
    {
        public RegisterPublicKeyResponse()
        {
            Kind = MessageKind.RegisterPublicKeyResponse;
        }

        /// <summary>
        /// Задает или получает номер ответа авторизации.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Задает или получает код домена пользователя.
        /// </summary>
        public Guid? UserDomainID { get; set; }
    }
}
