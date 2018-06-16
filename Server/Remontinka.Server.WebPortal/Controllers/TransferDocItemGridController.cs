using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.TransferDocItemGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида пунктов документа перемещения.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class TransferDocItemGridController : GridControllerBase<Guid, TransferDocItemGridModel, TransferDocItemCreateModel, TransferDocItemCreateModel>
    {
        public TransferDocItemGridController() : base(new TransferDocItemGridDataAdapter())
        {
        }

        public const string ControllerName = "TransferDocItemGrid";

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
            return "TransferDocItemsGrid";
        }
    }
}