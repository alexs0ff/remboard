using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.OrderKindGridForm
{
    /// <summary>
    /// Модель грида для типов заказа.
    /// </summary>
    public class OrderKindGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "OrderKindID"; } }
    }
}