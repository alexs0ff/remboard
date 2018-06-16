using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.Core.ServiceEntities;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер комбобокса с параметрами экспорта.
    /// </summary>
    public class AjaxExportKindComboBoxController : AjaxComboBoxBaseController<int>
    {
        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Текущий токен безопасности. </param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedId"> Выбранный пункт.</param>
        /// <param name="parentValue">Значение родительского контрола.</param>
        public override void GetInitializedItems(SecurityToken token, List<JSelectListItem<int>> list, int? selectedId, string parentValue)
        {
            foreach (var exportKind in ExportKindSet.Kinds)
            {
                list.Add(new JSelectListItem<int>
                {
                    Text = exportKind.Title,
                    Value = exportKind.KindID,
                    Selected = exportKind.KindID == selectedId
                });
            } //foreach
        }
    }
}