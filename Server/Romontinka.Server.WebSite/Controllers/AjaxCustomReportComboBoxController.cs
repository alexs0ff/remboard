using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Комбобокс с документами.
    /// </summary>
    [Authorize]
    public class AjaxCustomReportComboBoxController: AjaxComboBoxBaseController<Guid>
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
            foreach (var customReportItem in RemontinkaServer.Instance.EntitiesFacade.GetCustomReportItems(token))
            {
                list.Add(new JSelectListItem<Guid>
                             {
                                 Text = customReportItem.Title,
                                 Value = customReportItem.CustomReportID,
                                 Selected = selectedId == customReportItem.CustomReportID
                             });
            }
        }
    }
}