using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.IncomingDocGridForm;
using Remontinka.Server.WebPortal.Models.IncomingDocItemGridForm;
using Remontinka.Server.WebPortal.Models.RepairOrderGridForm;
using Romontinka.Server.Core;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида приходных накладных.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class IncomingDocGridController : GridControllerBase<Guid, IncomingDocGridModel, IncomingDocCreateModel, IncomingDocCreateModel>
    {
        public IncomingDocGridController() : base(new IncomingDocGridDataAdapter())
        {
        }

        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "IncomingDocGrid";

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
        public const string GridName = "IncomingDocsGrid";

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
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <returns>Результат.</returns>
        [ChildActionOnly]
        public ActionResult MasterDetailDetailPartial(Guid? incomingDocID)
        {
            var model = new IncomingDocGridDetailModel();
           
            model.IncomingDocID = incomingDocID;
           
            return PartialView("details", model);
        }
    }
}