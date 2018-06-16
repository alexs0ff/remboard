using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Ajax контроллер для типа финансового документа.
    /// </summary>
    public class AjaxFinancialItemKindComboBoxController : AjaxComboBoxBaseController<int>
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
            foreach (var financialItemKind in FinancialItemKindSet.Kinds)
            {
                list.Add(new JSelectListItem<int>
                {
                    Text = financialItemKind.Title,
                    Value = financialItemKind.FinancialItemKindID,
                    Selected = financialItemKind.FinancialItemKindID == selectedId
                });
            }
        }
    }
}