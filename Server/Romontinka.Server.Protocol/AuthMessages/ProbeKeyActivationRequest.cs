using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.AuthMessages
{
    /// <summary>
    /// Запрос на проверку активации пользовательского ключа.
    /// </summary>
    public class ProbeKeyActivationRequest : MessageBase
    {
        public ProbeKeyActivationRequest()
        {
            Kind = MessageKind.ProbeKeyActivationRequest;
        }

        /// <summary>
        /// Задает или получает номер ключа, который необходимо проверить.
        /// </summary>
        public string KeyNumber { get; set; }

        /// <summary>
        /// Задает или получает код домена в котором необходимо производить поиск.
        /// </summary>
        public Guid? UserDomainID { get; set; }
    }
}
