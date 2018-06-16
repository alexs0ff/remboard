using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Запрос на получение пользовательских отчетов.
    /// </summary>
    public class GetCustomReportItemRequest : SignedRequestBase
    {
        public GetCustomReportItemRequest()
        {
            Kind = MessageKind.GetCustomReportItemRequest;
        }

        /// <summary>
        /// Заносит поля в сообщении для подписи.
        /// </summary>
        /// <param name="signVisitor">Реализация подписывателя.</param>
        protected override void ProcessFields(MessageSignVisitorBase signVisitor)
        {
            signVisitor.AddValue(Kind.ToString());
            signVisitor.AddValue(Utils.GuidToString(UserID));
        }
    }
}
