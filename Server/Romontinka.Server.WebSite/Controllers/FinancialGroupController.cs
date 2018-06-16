using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.FinancialGroupForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер финансовых групп филиалов.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class FinancialGroupController :
        JGridControllerBase
            <Guid, FinancialGroupGridItemModel, FinancialGroupCreateModel, FinancialGroupCreateModel,
            FinancialGroupSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "FinancialGroup";

        public FinancialGroupController(
            JGridDataAdapterBase
                <Guid, FinancialGroupGridItemModel, FinancialGroupCreateModel, FinancialGroupCreateModel,
                FinancialGroupSearchModel> adapter)
            : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }
    }
}