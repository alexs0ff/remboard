using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.IncomingDocForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер приходных накладных.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class IncomingDocController : JGridControllerBase<Guid, IncomingDocGridItemModel, IncomingDocCreateModel, IncomingDocCreateModel, IncomingDocSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "IncomingDoc";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public IncomingDocController(JGridDataAdapterBase<Guid, IncomingDocGridItemModel, IncomingDocCreateModel, IncomingDocCreateModel, IncomingDocSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }
    }
}