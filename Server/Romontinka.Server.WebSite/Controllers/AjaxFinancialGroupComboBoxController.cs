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
    /// Ajax контроллер выбора финансовой группы.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class AjaxFinancialGroupComboBoxController : AjaxComboBoxBaseController<Guid>
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
            foreach (var financialGroupItem in RemontinkaServer.Instance.EntitiesFacade.GetFinancialGroupItems(token))
            {
                list.Add(new JSelectListItem<Guid>
                         {
                             Text = financialGroupItem.Title,
                             Value = financialGroupItem.FinancialGroupID,
                             Selected = financialGroupItem.FinancialGroupID==selectedId
                         });
            } //foreach
        }
    }
}