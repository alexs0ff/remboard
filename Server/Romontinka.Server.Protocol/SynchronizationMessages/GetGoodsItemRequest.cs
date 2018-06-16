using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Запрос на получение информации о номенклатуре товаров, а также их категориях.
    /// </summary>
    public class GetGoodsItemRequest : SignedRequestBase
    {
        public GetGoodsItemRequest()
        {
            Kind = MessageKind.GetGoodsItemRequest;
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
