using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Запрос на получение заказов.
    /// </summary>
    public class GetRepairOrdersRequest : SignedRequestBase
    {
        public GetRepairOrdersRequest()
        {
            Kind = MessageKind.GetRepairOrdersRequest;
            RepairOrderIds= new List<Guid?>();
        }

        /// <summary>
        /// Заносит поля в сообщении для подписи.
        /// </summary>
        /// <param name="signVisitor">Реализация подписывателя.</param>
        protected override void ProcessFields(MessageSignVisitorBase signVisitor)
        {
            signVisitor.AddValue(Kind.ToString());
            signVisitor.AddValue(Utils.GuidToString(UserID));

            var res = new StringBuilder();

            foreach (var repairOrderId in RepairOrderIds)
            {
                res.Append(repairOrderId);
            } //foreach
            signVisitor.AddValue(res.ToString());
        }

        /// <summary>
        /// Получает список кодов заказов.
        /// </summary>
        public List<Guid?> RepairOrderIds { get; private set; }
    }
}
