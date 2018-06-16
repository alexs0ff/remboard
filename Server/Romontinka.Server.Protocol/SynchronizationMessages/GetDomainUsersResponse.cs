using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Ответ с пользователями домена.
    /// </summary>
    public class GetDomainUsersResponse:MessageBase
    {
        public GetDomainUsersResponse()
        {
            Users = new List<DomainUserDTO>();
            Kind = MessageKind.GetDomainUsersResponse;
        }

        /// <summary>
        /// Получает список пользователей.
        /// </summary>
        public IList<DomainUserDTO> Users { get; private set; }
    }
}
