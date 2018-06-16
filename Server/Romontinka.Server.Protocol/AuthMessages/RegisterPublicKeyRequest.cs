using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.AuthMessages
{
    /// <summary>
    /// Запрос на регистрацию публичного ключа.
    /// </summary>
    public class RegisterPublicKeyRequest : MessageBase
    {
        public RegisterPublicKeyRequest()
        {
            Kind = MessageKind.RegisterPublicKeyRequest;
        }

        /// <summary>
        /// Задает или получает клиентский код домена (из зарегистрированных).
        /// Если будет здесь null, значит зарегистрированных еще нет.
        /// </summary>
        public Guid? ClientUserDomainID { get; set; }

        /// <summary>
        /// Задает или получает логин пользователя.
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        /// Задает или получает данные ключа.
        /// </summary>
        public string PublicKeyData { get; set; }

        /// <summary>
        /// Задает или получает заметки по ключам.
        /// </summary>
        public string KeyNotes { get; set; }

        /// <summary>
        /// Задает или получает дату запроса.
        /// </summary>
        public DateTime EventDate { get; set; }
    }
}
