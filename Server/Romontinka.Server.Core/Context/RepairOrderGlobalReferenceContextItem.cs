using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romontinka.Server.DataLayer.Entities;

namespace Romontinka.Server.Core.Context
{
    /// <summary>
    /// Глобальная ссылка на заказ.
    /// </summary>
    public class RepairOrderGlobalReferenceContextItem: ContextItemBase
    {
        /// <summary>
        /// Инициализирует контекст для глобальной ссылки на заказ.
        /// </summary>
        /// <param name="order">Заказ.</param>
        /// <param name="domain">Домен.</param>
        public RepairOrderGlobalReferenceContextItem(RepairOrder order,UserDomain domain)
        {
            _values[ContextConstants.DomainRepairOrderNumber] = RepairOrderGlobalReferenceHelper.MakeGlobalReference(order.Number,domain.Number);
            _values[ContextConstants.DomainRepairOrderPassword] = order.AccessPassword;
        }
    }
}
