using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.WorkItemGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида по выполненным работам.
    /// </summary>
    [ExtendedAuthorize]
    public class WorkItemGridController : GridControllerBase<Guid, WorkItemGridModel, WorkItemCreateModel, WorkItemCreateModel>
    {
        public WorkItemGridController() : base(new WorkItemGridDataAdapter())
        {

        }

        public const string ControllerName = "WorkItemGrid";

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
            return "WorkItemsGrid";
        }
    }
}