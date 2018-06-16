using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.WarehouseItemGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида остатков на складе.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class WarehouseItemGridController : GridControllerBase<Guid, WarehouseItemGridModel, WarehouseItemEditModel, WarehouseItemEditModel>
    {
        public WarehouseItemGridController() : base(new WarehouseItemGridDataAdapter())
        {
        }

        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "WarehouseItemGrid";

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
            return "WarehouseItemsGrid";
        }
    }
}