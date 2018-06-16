using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.TransferDocForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер документов перемещения со склада на склад.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class TransferDocController : JGridControllerBase<Guid, TransferDocGridItemModel, TransferDocCreateModel, TransferDocCreateModel, TransferDocSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "TransferDoc";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public TransferDocController(JGridDataAdapterBase<Guid, TransferDocGridItemModel, TransferDocCreateModel, TransferDocCreateModel, TransferDocSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }
    }
}