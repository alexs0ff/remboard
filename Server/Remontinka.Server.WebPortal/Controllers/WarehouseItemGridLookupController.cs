using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models.WarehouseItemGridLookupForm;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Лукап контроллер для остатков на складе.
    /// </summary>
    [ExtendedAuthorize]
    public class WarehouseItemGridLookupController: GridLookupControllerBase<WarehouseItemGridLookupModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "WarehouseItemGridLookup";

        /// <summary>
        /// Возвращает имя контроллера.
        /// </summary>
        /// <returns>Имя контроллера.</returns>
        protected override string GetControllerName()
        {
            return ControllerName;
        }

        /// <summary>
        /// Переопределяется, чтобы инициализировать модель.
        /// </summary>
        /// <param name="model">Модель для инициализации</param>
        /// <param name="token">Токен безопасности.</param>
        protected override void IntitializeModel(SecurityToken token, WarehouseItemGridLookupModel model)
        {
            model.FieldName = "WarehouseItemID";
            model.KeyField = "WarehouseItemID";
            model.Warehouses = RemontinkaServer.Instance.EntitiesFacade.GetWarehouses(token);
        }

        /// <summary>
        /// Вохвращает данные для грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns></returns>
        protected override IQueryable GetData(SecurityToken token)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetWarehouseItems(token);
        }
    }
}