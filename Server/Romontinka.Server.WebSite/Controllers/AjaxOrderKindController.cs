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
    /// Ajax котнроллер для типов заказа.
    /// </summary>
     [Authorize]
    public class AjaxOrderKindController : AjaxComboBoxBaseController<Guid>
    {
        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedId"> Выбранный пункт.</param>
        /// <param name="parentValue">Значение родительского контрола.</param>
        public override void GetInitializedItems(SecurityToken token, List<JSelectListItem<Guid>> list, Guid? selectedId, string parentValue)
        {
            foreach (var orderKind in RemontinkaServer.Instance.EntitiesFacade.GetOrderKinds(token))
            {
                list.Add(new JSelectListItem<Guid>
                         {
                             Text = orderKind.Title,
                             Value = orderKind.OrderKindID,
                             Selected = selectedId == orderKind.OrderKindID
                         });
            } //foreach
        }
    }
}