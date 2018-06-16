using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.OrderStatusGridForm
{
    /// <summary>
    /// Модель для грида статусов заказа.
    /// </summary>
    public class OrderStatusGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "OrderStatusID"; } }

        /// <summary>
        /// Типы заказов.
        /// </summary>
        public IEnumerable<StatusKind> StatusKinds { get; set; }
    }
}