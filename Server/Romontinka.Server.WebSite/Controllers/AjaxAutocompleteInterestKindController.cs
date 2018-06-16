using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Ajax контроллер для списка типов вознаграждения.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class AjaxAutocompleteInterestKindController : AjaxComboBoxBaseController<byte>
    {
        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Текущий токен безопасности. </param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedId"> Выбранный пункт.</param>
        /// <param name="parentValue">Значение родительского контрола.</param>
        public override void GetInitializedItems(SecurityToken token, List<JSelectListItem<byte>> list, byte? selectedId, string parentValue)
        {
            foreach (var interestKind in InterestKindSet.Kinds)
            {
                list.Add(new JSelectListItem<byte>
                {
                    Text = interestKind.Title,
                    Value = interestKind.InterestKindID,
                    Selected = interestKind.InterestKindID == selectedId
                });
            }
        }
    }
}