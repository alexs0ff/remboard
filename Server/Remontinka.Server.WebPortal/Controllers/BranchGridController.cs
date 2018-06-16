using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.BranchForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида подразделений.
    /// </summary>
    [ExtendedAuthorize]
    public class BranchGridController : GridControllerBase<Guid, BranchGridModel,BranchCreateModel,BranchEditModel>
    {
        public BranchGridController() : base(new BranchGridDataAdapter())
        {
        }

        public const string ControllerName = "BranchGrid";

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
            return "BranchesGrid";
        }
    }
}