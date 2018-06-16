using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.CancellationDocItemGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида отмен.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class CancellationDocItemGridController : GridControllerBase<Guid, CancellationDocItemGridModel, CancellationDocItemCreateModel, CancellationDocItemCreateModel>
    {
        public CancellationDocItemGridController() : base(new CancellationDocItemGridDataAdapter())
        {
        }

        public const string ControllerName = "CancellationDocItemGrid";

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
            return "CancellationDocItemsGrid";
        }
    }
}