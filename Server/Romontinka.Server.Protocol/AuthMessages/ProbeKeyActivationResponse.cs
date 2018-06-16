using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.AuthMessages
{
    /// <summary>
    /// Ответ на запрос проверки активации ключа.
    /// </summary>
    public class ProbeKeyActivationResponse:MessageBase
    {
        public ProbeKeyActivationResponse()
        {
            Kind = MessageKind.ProbeKeyActivationResponse;
        }

        /// <summary>
        /// Задает или получает признак существования ключа.
        /// </summary>
        public bool IsExists { get; set; }

        /// <summary>
        /// Задает или получает признак отзыва ключа.
        /// </summary>
        public bool IsRevoked { get; set; }

        /// <summary>
        /// Задает или получает признак окончания срока действия ключа.
        /// </summary>
        public bool IsExpired { get; set; }

        /// <summary>
        /// Признак того, что ключ еще не принят.
        /// </summary>
        public bool IsNotAccepted { get; set; }

        /// <summary>
        /// Задает или получает код пользователя.
        /// </summary>
        public Guid? UserID { get; set; }
    }
}
