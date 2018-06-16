using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.DeviceItemForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер пунктов грида установленных запчастей.
    /// </summary>
    [ExtendedAuthorize]
    public class DeviceItemController : JGridControllerBase<Guid, DeviceItemGridItemModel, DeviceItemCreateModel, DeviceItemCreateModel, DeviceItemSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "DeviceItem";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public DeviceItemController(JGridDataAdapterBase<Guid, DeviceItemGridItemModel, DeviceItemCreateModel, DeviceItemCreateModel, DeviceItemSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }
    }
}