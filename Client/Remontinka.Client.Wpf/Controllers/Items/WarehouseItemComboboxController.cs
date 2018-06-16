using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core;
using Remontinka.Client.Wpf.Model.Controls;

namespace Remontinka.Client.Wpf.Controllers.Items
{
    /// <summary>
    /// Контроллер пунктов склада.
    /// </summary>
    public class WarehouseItemComboboxController : ComboBoxItemControllerBase<Guid>
    {
        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Текущий токен безопасности. </param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedId"> Выбранный пункт.</param>
        protected override void GetInitializedItems(SecurityToken token, List<SelectListItem<Guid>> list, Guid? selectedId)
        {
            foreach (var warehouseItem in ClientCore.Instance.DataStore.GetWarehouseItems())
            {
                list.Add(new SelectListItem<Guid>
                         {
                             Selected = warehouseItem.GoodsItemIDGuid==selectedId,
                             Text = warehouseItem.GoodsItemTitle,
                         });
            } //foreach
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
