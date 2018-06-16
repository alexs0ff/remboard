using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core;
using Remontinka.Client.DataLayer.Entities;
using Remontinka.Client.Wpf.Model.Controls;

namespace Remontinka.Client.Wpf.Controllers.Items
{
    /// <summary>
    /// Контроллер филиалов.
    /// </summary>
    public class BranchComboBoxController: ComboBoxItemControllerBase<Guid>
    {
        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Текущий токен безопасности. </param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedId"> Выбранный пункт.</param>
        protected override void GetInitializedItems(SecurityToken token, List<SelectListItem<Guid>> list, Guid? selectedId)
        {
            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                foreach (var branch in ClientCore.Instance.DataStore.GetBranches())
                {
                    list.Add(new SelectListItem<Guid>
                    {
                        Selected = branch.BranchIDGuid == selectedId,
                        Text = branch.Title,
                        Value = branch.BranchIDGuid
                    });
                }
            }
            else
            {
                foreach (var branch in ClientCore.Instance.DataStore.GetUserBranchMapByItemsByUser(token.User.UserIDGuid))
                {
                    list.Add(new SelectListItem<Guid>
                    {
                        Selected = branch.BranchIDGuid == selectedId,
                        Text = branch.BranchTitle,
                        Value = branch.BranchIDGuid
                    });
                }
            }

            if (list.Count == 1 && selectedId == null)
            {
                list.First().Selected = true;
            } //if
        }

        /// <summary>
        /// Производит установку модели.
        /// </summary>
        /// <param name="model">Настраеваемая модель.</param>
        protected override void SetUpModel(ComboBoxControlModel model)
        {
            
        }
    }
}
