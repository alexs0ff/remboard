using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.GoodsItemForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер управления номенклатурой.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class GoodsItemController : JGridControllerBase<Guid, GoodsItemGridItemModel, GoodsItemCreateModel, GoodsItemCreateModel, GoodsItemSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "GoodsItem";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public GoodsItemController(JGridDataAdapterBase<Guid, GoodsItemGridItemModel, GoodsItemCreateModel, GoodsItemCreateModel, GoodsItemSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }
    }
}