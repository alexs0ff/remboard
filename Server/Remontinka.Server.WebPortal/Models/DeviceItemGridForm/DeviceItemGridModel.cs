using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.DeviceItemGridForm
{
    /// <summary>
    /// Модель грида пункта выполненных работ.
    /// </summary>
    public class DeviceItemGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "DeviceItemID"; } }

        /// <summary>
        /// Задает или получает инжинеров.
        /// </summary>
        public List<SelectListItem<Guid>> Engineers { get; set; }
    }
}