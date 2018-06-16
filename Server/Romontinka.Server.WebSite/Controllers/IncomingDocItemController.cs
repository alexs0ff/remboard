using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.IncomingDocItemForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер пунктов приходных накладных.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class IncomingDocItemController : JGridControllerBase<Guid, IncomingDocItemGridItemModel, IncomingDocItemCreateModel, IncomingDocItemCreateModel, IncomingDocItemSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "IncomingDocItem";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public IncomingDocItemController(JGridDataAdapterBase<Guid, IncomingDocItemGridItemModel, IncomingDocItemCreateModel, IncomingDocItemCreateModel, IncomingDocItemSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }
    }
}