using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Запрос на получение информации по остаткам на складах.
    /// </summary>
    public class GetWarehouseItemsRequest : SignedRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public GetWarehouseItemsRequest()
        {
            Kind = MessageKind.GetWarehouseItemsRequest;
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
