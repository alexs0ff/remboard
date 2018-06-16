using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Комбобокс с филиалами.
    /// </summary>
     [Authorize]
    public class AjaxBranchComboBoxController : AjaxComboBoxBaseController<Guid>
    {
        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Токен безопасности. </param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedId"> Выбранный пункт.</param>
        /// <param name="parentValue">Значение родительского контрола.</param>
        public override void GetInitializedItems(SecurityToken token,List<JSelectListItem<Guid>> list, Guid? selectedId, string parentValue)
        {

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                foreach (var branch in RemontinkaServer.Instance.EntitiesFacade.GetBranches(token))
                {
                    list.Add(new JSelectListItem<Guid>
                                 {
                                     Selected = branch.BranchID == selectedId,
                                     Text = branch.Title,
                                     Value = branch.BranchID
                                 });
                }
            }
            else
            {
                foreach (var branch in RemontinkaServer.Instance.DataStore.GetUserBranchMapByItemsByUser(token.User.UserID))
                {
                    list.Add(new JSelectListItem<Guid>
                    {
                        Selected = branch.BranchID == selectedId,
                        Text = branch.BranchTitle,
                        Value = branch.BranchID
                    });
                }
            }

            if (list.Count==1 && selectedId==null)
            {
                list.First().Selected = true;
            } //if
        }
    }
}