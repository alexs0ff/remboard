using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Models.WorkItemForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер для грида пунктов выполненных работ.
    /// </summary>
    [Authorize]
    public class WorkItemController : JGridControllerBase<Guid, WorkItemGridItemModel, WorkItemCreateModel, WorkItemCreateModel, WorkItemSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "WorkItem";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public WorkItemController(JGridDataAdapterBase<Guid, WorkItemGridItemModel, WorkItemCreateModel, WorkItemCreateModel, WorkItemSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }
    }
}