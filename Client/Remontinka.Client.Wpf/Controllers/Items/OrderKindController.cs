using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core;
using Remontinka.Client.Wpf.Model.Controls;

namespace Remontinka.Client.Wpf.Controllers.Items
{
    /// <summary>
    /// Контроллер для типов заказа.
    /// </summary>
    public class OrderKindController: ComboBoxItemControllerBase<Guid>
    {
        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Текущий токен безопасности. </param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedId"> Выбранный пункт.</param>
        protected override void GetInitializedItems(SecurityToken token, List<SelectListItem<Guid>> list, Guid? selectedId)
        {
            foreach (var orderKind in ClientCore.Instance.DataStore.GetOrderKinds())
            {
                list.Add(new SelectListItem<Guid>
                {
                    Selected = orderKind.OrderKindIDGuid == selectedId,
                    Value = orderKind.OrderKindIDGuid,
                    Text = orderKind.Title
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
