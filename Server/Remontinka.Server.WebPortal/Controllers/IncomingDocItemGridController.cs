using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.IncomingDocItemGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида пунктов приходной накладной.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class IncomingDocItemGridController : GridControllerBase<Guid, IncomingDocItemGridModel, IncomingDocItemCreateModel, IncomingDocItemCreateModel>
    {
        public IncomingDocItemGridController() : base(new IncomingDocItemGridDataAdapter())
        {
        }

        public const string ControllerName = "IncomingDocItemGrid";

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
            return "IncomingDocItemsGrid";
        }
    }
}