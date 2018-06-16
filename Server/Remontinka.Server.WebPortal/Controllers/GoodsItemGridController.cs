using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.GoodsItemGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида номенклатуры.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class GoodsItemGridController : GridControllerBase<Guid, GoodsItemGridModel, GoodsItemCreateModel, GoodsItemCreateModel>
    {
        public GoodsItemGridController() : base(new GoodsItemGridDataAdapter())
        {
        }

        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "GoodsItemGrid";

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
            return "GoodsItemsGrid";
        }
    }
}