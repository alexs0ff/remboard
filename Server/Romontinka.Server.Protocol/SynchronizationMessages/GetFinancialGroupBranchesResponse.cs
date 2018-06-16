using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Ответ на запрос на получение финансовых групп и связей с филиалами.
    /// </summary>
    public class GetFinancialGroupBranchesResponse : MessageBase
    {
        public GetFinancialGroupBranchesResponse()
        {
            Kind = MessageKind.GetFinancialGroupBranchesResponse;
            FinancialGroupItems = new List<FinancialGroupItemDTO>();
            FinancialGroupBranchMapItems = new List<FinancialGroupBranchMapItemDTO>();
        }

        /// <summary>
        /// Получает список финансовых групп.
        /// </summary>
        public IList<FinancialGroupItemDTO> FinancialGroupItems { get; private set; }

        /// <summary>
        /// Получает список связей фингрупп и филиалов.
        /// </summary>
        public IList<FinancialGroupBranchMapItemDTO> FinancialGroupBranchMapItems { get; private set; }
    }
}
