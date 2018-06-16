using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.FinancialItemForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер статей бюджета.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class FinancialItemController :JGridControllerBase<Guid, FinancialItemGridItemModel, FinancialItemCreateModel, FinancialItemCreateModel,FinancialItemSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "FinancialItem";

        public FinancialItemController(JGridDataAdapterBase<Guid, FinancialItemGridItemModel, FinancialItemCreateModel, FinancialItemCreateModel, FinancialItemSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }
    }
}