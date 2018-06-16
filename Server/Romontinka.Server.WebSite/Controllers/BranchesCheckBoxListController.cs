using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер множественного выбора филиалов.
    /// </summary>
    [ExtendedAuthorize]
    public class BranchesCheckBoxListController : CheckBoxListBaseController<Guid>
    {
        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedIds"> Выбранныйые пункты.</param>
        /// <param name="parentValue">Значение родительского контрола.</param>
        public override void GetInitializedItems(SecurityToken token, List<JSelectListItem<Guid>> list, Guid[] selectedIds, string parentValue)
        {
            foreach (var branch in RemontinkaServer.Instance.EntitiesFacade.GetBranches(token))
            {
                list.Add(new JSelectListItem<Guid>
                             {
                                 Text = branch.Title,
                                 Value = branch.BranchID,
                                 Selected = selectedIds.Any(i => i == branch.BranchID)
                             });
            }
        }
    }
}