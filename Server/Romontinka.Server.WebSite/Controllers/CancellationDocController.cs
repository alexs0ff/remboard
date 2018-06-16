using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.CancellationDocForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер для документов списания.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class CancellationDocController : JGridControllerBase<Guid, CancellationDocGridItemModel, CancellationDocCreateModel, CancellationDocCreateModel, CancellationDocSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "CancellationDoc";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public CancellationDocController(JGridDataAdapterBase<Guid, CancellationDocGridItemModel, CancellationDocCreateModel, CancellationDocCreateModel, CancellationDocSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }
    }
}