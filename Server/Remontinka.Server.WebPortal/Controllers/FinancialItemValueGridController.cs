using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.FinancialItemValueGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида текущих расходов и доходов.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class FinancialItemValueGridController : GridControllerBase<Guid, FinancialItemValueGridModel, FinancialItemValueCreateModel, FinancialItemValueCreateModel>
    {
        public FinancialItemValueGridController() : base(new FinancialItemValueGridDataAdapter())
        {
        }

        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "FinancialItemValueGrid";

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
            return "FinancialItemValuesGrid";
        }
    }
}