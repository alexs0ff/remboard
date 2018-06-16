using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Ответ с отчетами пользователей.
    /// </summary>
    public class GetCustomReportItemResponse : MessageBase
    {
        public GetCustomReportItemResponse()
        {
            Kind = MessageKind.GetCustomReportItemResponse;
            CustomReportItems = new List<CustomReportItemDTO>();
        }

        /// <summary>
        /// Получает список пользовательских отчетов.
        /// </summary>
        public IList<CustomReportItemDTO> CustomReportItems { get; private set; }
    }
}
