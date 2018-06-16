using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.TransferDocGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида документов перемещения.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class TransferDocGridController : GridControllerBase<Guid, TransferDocGridModel, TransferDocCreateModel, TransferDocCreateModel>
    {
        public TransferDocGridController() : base(new TransferDocGridDataAdapter())
        {
        }

        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "TransferDocGrid";

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
        public const string GridName = "TransferDocsGrid";

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
        public ActionResult MasterDetailDetailPartial(Guid? transferDocID)
        {
            var model = new TransferDocGridDetailModel();

            model.TransferDocID = transferDocID;

            return PartialView("details", model);
        }
    }
}