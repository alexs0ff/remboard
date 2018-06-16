using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.TransferDocItemForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер управления пунктами документа перемещения между складами.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class TransferDocItemController : JGridControllerBase<Guid, TransferDocItemGridItemModel, TransferDocItemCreateModel, TransferDocItemCreateModel, TransferDocItemSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "TransferDocItem";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public TransferDocItemController(JGridDataAdapterBase<Guid, TransferDocItemGridItemModel, TransferDocItemCreateModel, TransferDocItemCreateModel, TransferDocItemSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }
    }
}