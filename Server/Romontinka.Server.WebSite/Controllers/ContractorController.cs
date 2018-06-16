using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.ContractorForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер управления контрагентами.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class ContractorController : JGridControllerBase<Guid, ContractorGridItemModel, ContractorCreateModel, ContractorCreateModel, ContractorSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "Contractor";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public ContractorController(JGridDataAdapterBase<Guid, ContractorGridItemModel, ContractorCreateModel, ContractorCreateModel, ContractorSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }
    }
}