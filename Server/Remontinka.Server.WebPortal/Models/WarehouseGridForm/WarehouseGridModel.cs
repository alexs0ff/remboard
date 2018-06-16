using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.WarehouseGridForm
{
    /// <summary>
    /// Модель грида складов.
    /// </summary>
    public class WarehouseGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "WarehouseID"; } }
    }
}