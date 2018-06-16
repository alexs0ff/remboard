using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер комбобокса с категориям товаров.
    /// </summary>
    public class AjaxItemCategoryComboBoxController : AjaxComboBoxBaseController<Guid>
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
            foreach (var category in RemontinkaServer.Instance.EntitiesFacade.GetItemCategories(token))
            {
                list.Add(new JSelectListItem<Guid>
                {
                    Text = category.Title,
                    Value = category.ItemCategoryID,
                    Selected = category.ItemCategoryID == selectedId
                });
            } //foreach
        }
    }
}