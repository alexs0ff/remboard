using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.CancellationDocGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида документов списания.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class CancellationDocGridController : GridControllerBase<Guid, CancellationDocGridModel, CancellationDocCreateModel, CancellationDocCreateModel>
    {
        public CancellationDocGridController() : base(new CancellationDocGridDataAdapter())
        {

        }

        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "CancellationDocGrid";

        /// <summary>
        /// Получает название контроллера.
        /// </summary>
        /// <returns>Название контроллера.</returns>
        protected override string GetControllerName()
        {
            return ControllerName;
        }

        /// <summary>
        /// Содержит имя грида.
        /// </summary>
        public const string GridName = "CancellationDocsGrid";

        /// <summary>
        /// Получает название грида.
        /// </summary>
        /// <returns>Название грида.</returns>
        protected override string GetGridName()
        {
            return GridName;
        }

        /// <summary>
        /// Получает контент для детализации.
        /// </summary>
        /// <param name="cancellationDocID">Код документа списания.</param>
        /// <returns>Результат.</returns>
        [ChildActionOnly]
        public ActionResult MasterDetailDetailPartial(Guid? cancellationDocID)
        {
            var model = new CancellationDocGridDetailModel();

            model.CancellationDocID = cancellationDocID;

            return PartialView("details", model);
        }
    }
}