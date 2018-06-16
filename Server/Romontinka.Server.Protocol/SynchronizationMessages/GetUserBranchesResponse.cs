using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Ответ на запрос о получении филиалов и их связей с пользователями.
    /// </summary>
    public class GetUserBranchesResponse : MessageBase
    {
        public GetUserBranchesResponse()
        {
            Kind = MessageKind.GetUserBranchesResponse;
            Branches = new List<BranchDTO>();
            UserBranchMapItems = new List<UserBranchMapItemDTO>();
        }

        /// <summary>
        /// Получает список существующих филиалов.
        /// </summary>
        public IList<BranchDTO> Branches { get; private set; }

        /// <summary>
        /// Получает список существующих связей между филиалами и пользователями.
        /// </summary>
        public IList<UserBranchMapItemDTO> UserBranchMapItems { get; private set; }
    }
}
