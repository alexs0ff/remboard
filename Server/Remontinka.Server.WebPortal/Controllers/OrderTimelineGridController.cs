using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.OrderTimelineGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида истории по заказам.
    /// </summary>
    [ExtendedAuthorize]
    public class OrderTimelineGridController : GridControllerBase<Guid, OrderTimelineGridModel, OrderTimelineCreateModel, OrderTimelineCreateModel>
    {
        public OrderTimelineGridController() : base(new OrderTimelineGridDataAdapter())
        {

        }

        public const string ControllerName = "OrderTimelineGrid";

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
            return "OrderTimelinesGrid";
        }
    }
}