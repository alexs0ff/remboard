using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.FinancialGroupItemGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// КОнтроллер грида финансовых групп филиалов.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class FinancialGroupItemGridController : GridControllerBase<Guid, FinancialGroupItemGridModel, FinancialGroupItemCreateModel, FinancialGroupItemCreateModel>
    {
        public FinancialGroupItemGridController() : base(new FinancialGroupItemGridDataAdapter())
        {
        }

        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "FinancialGroupItemGrid";

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
            return "FinancialGroupItemsGrid";
        }
    }
}