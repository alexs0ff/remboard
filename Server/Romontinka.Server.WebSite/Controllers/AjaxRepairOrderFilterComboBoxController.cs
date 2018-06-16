using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core.Security;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Models.RepairOrderForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Фильтр для заказов.
    /// </summary>
    public class AjaxRepairOrderFilterComboBoxController: AjaxComboBoxBaseController<int>
    {
        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedId"> Выбранный пункт.</param>
        /// <param name="parentValue">Значение родительского контрола.</param>
        public override void GetInitializedItems(SecurityToken token,List<JSelectListItem<int>> list, int? selectedId, string parentValue)
        {
            list.Add(new JSelectListItem<int>{Value = OrderSearchSet.All.Key, Text = OrderSearchSet.All.Title, Selected = true});
            list.Add(new JSelectListItem<int> { Value = OrderSearchSet.CurrentUser.Key, Text = OrderSearchSet.CurrentUser.Title, Selected = false });
            list.Add(new JSelectListItem<int> { Value = OrderSearchSet.SpecificUser.Key, Text = OrderSearchSet.SpecificUser.Title, Selected = false });
            list.Add(new JSelectListItem<int> { Value = OrderSearchSet.OnlyUrgents.Key, Text = OrderSearchSet.OnlyUrgents.Title, Selected = false });
        }
    }
}