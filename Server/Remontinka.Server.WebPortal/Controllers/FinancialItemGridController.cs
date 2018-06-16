using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.FinancialItemGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// КОнтроллер грида статей бюджета.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class FinancialItemGridController : GridControllerBase<Guid, FinancialItemGridModel, FinancialItemCreateModel, FinancialItemCreateModel>
    {
        public FinancialItemGridController() : base(new FinancialItemGridDataAdapter())
        {
        }

        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "FinancialItemGrid";

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
            return "FinancialItemsGrid";
        }
    }
}