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
    /// Ajax контроллер для списка типов пунктов автозаполнения.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class AjaxAutocompleteKindComboBoxController : AjaxComboBoxBaseController<byte>
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
            foreach (var autocompleteKind in AutocompleteKindSet.Kinds)
            {
                list.Add(new JSelectListItem<byte>
                {
                    Text = autocompleteKind.Title,
                    Value = autocompleteKind.AutocompleteKindID,
                    Selected = autocompleteKind.AutocompleteKindID == selectedId
                });
            }
        }
    }
}