using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.DeviceItemGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер по установленым запчастям.
    /// </summary>
    [ExtendedAuthorize]
    public class DeviceItemGridController : GridControllerBase<Guid, DeviceItemGridModel, DeviceItemCreateModel, DeviceItemCreateModel>
    {
        public DeviceItemGridController() : base(new DeviceItemGridDataAdapter())
        {
        }

        public const string ControllerName = "DeviceItemGrid";

        /// <summary>
        /// Получает название контроллера.
        /// </summary>
        /// <returns>Название контроллера.</returns>
        protected override string GetControllerName()
        {
            return ControllerName;
        }

        /// <summary>
        /// Получает название грида.
        /// </summary>
        /// <returns>Название грида.</returns>
        protected override string GetGridName()
        {
            return "DeviceItemsGrid";
        }
    }
}