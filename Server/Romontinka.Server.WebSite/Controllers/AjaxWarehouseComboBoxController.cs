using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Комбобокс контроллер выбора складов.
    /// </summary>
    [ExtendedAuthorize]
    public class AjaxWarehouseComboBoxController: AjaxComboBoxBaseController<Guid>
    {
        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Текущий токен безопасности. </param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedId"> Выбранный пункт.</param>
        /// <param name="parentValue">Значение родительского контрола.</param>
        public override void GetInitializedItems(SecurityToken token, List<JSelectListItem<Guid>> list, Guid? selectedId, string parentValue)
        {
            foreach (var warehouse in RemontinkaServer.Instance.EntitiesFacade.GetWarehouses(token))
            {
                list.Add(new JSelectListItem<Guid>
                {
                    Text = warehouse.Title,
                    Value = warehouse.WarehouseID,
                    Selected = warehouse.WarehouseID == selectedId
                });
            } //foreach

            //Если присутствует только один склад, тогда выбираем его по умолчанию
            if (list.Count==1)
            {
                list[0].Selected = true;
            } //if
        }
    }
}