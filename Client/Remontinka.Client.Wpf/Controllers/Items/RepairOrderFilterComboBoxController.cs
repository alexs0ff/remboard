using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core;
using Remontinka.Client.Wpf.Model.Controls;
using Remontinka.Client.Wpf.Model.Items;

namespace Remontinka.Client.Wpf.Controllers.Items
{
    /// <summary>
    /// Фильтр для заказов.
    /// </summary>
    public class RepairOrderFilterComboBoxController : ComboBoxItemControllerBase<int>
    {
        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Текущий токен безопасности. </param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedId"> Выбранный пункт.</param>
        protected override void GetInitializedItems(SecurityToken token, List<SelectListItem<int>> list, int? selectedId)
        {
            list.Add(new SelectListItem<int> { Value = OrderSearchSet.All.Key, Text = OrderSearchSet.All.Title, Selected = true });
            list.Add(new SelectListItem<int> { Value = OrderSearchSet.CurrentUser.Key, Text = OrderSearchSet.CurrentUser.Title, Selected = false });
            list.Add(new SelectListItem<int> { Value = OrderSearchSet.SpecificUser.Key, Text = OrderSearchSet.SpecificUser.Title, Selected = false });
            list.Add(new SelectListItem<int> { Value = OrderSearchSet.OnlyUrgents.Key, Text = OrderSearchSet.OnlyUrgents.Title, Selected = false });
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
