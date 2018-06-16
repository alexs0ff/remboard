using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.CancellationDocItemForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер управления пунктами документа по списанию со склада.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class CancellationDocItemController : JGridControllerBase<Guid, CancellationDocItemGridItemModel, CancellationDocItemCreateModel, CancellationDocItemCreateModel, CancellationDocItemSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "CancellationDocItem";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public CancellationDocItemController(JGridDataAdapterBase<Guid, CancellationDocItemGridItemModel, CancellationDocItemCreateModel, CancellationDocItemCreateModel, CancellationDocItemSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }
    }
}