using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Запрос на получение серверных хэшей заказов.
    /// </summary>
    public class GetServerRepairOrderHashesRequest : SignedRequestBase
    {
        public GetServerRepairOrderHashesRequest()
        {
            Kind = MessageKind.GetServerRepairOrderHashesRequest;
        }

        /// <summary>
        /// Задает или получает код заказа, начиная с которого необходимо сделать выборку других хэшей.
        /// </summary>
        public Guid? LastRepairOrderID { get; set; }

        /// <summary>
        /// Заносит поля в сообщении для подписи.
        /// </summary>
        /// <param name="signVisitor">Реализация подписывателя.</param>
        protected override void ProcessFields(MessageSignVisitorBase signVisitor)
        {
            signVisitor.AddValue(Kind.ToString());
            signVisitor.AddValue(Utils.GuidToString(UserID));
            signVisitor.AddValue(Utils.GuidToString(LastRepairOrderID));
        }
    }
}
