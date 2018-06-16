using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.AuthMessages
{
    /// <summary>
    /// Ошибка авторизации.
    /// </summary>
    public class AuthErrorResponse : MessageBase
    {
        public AuthErrorResponse()
        {
            Kind = MessageKind.AuthErrorResponse;
        }

        /// <summary>
        /// Задает или получает сообщение ошибки авторизации.
        /// </summary>
        public string Message { get; set; }
    }
}
