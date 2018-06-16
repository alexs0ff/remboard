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
    /// Контроллер списка типов документов.
    /// </summary>
    [Authorize]
    public class AjaxDocumentKindComboBoxController : AjaxComboBoxBaseController<byte>
    {
        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedId"> Выбранный пункт.</param>
        /// <param name="parentValue">Значение родительского контрола.</param>
        public override void GetInitializedItems(SecurityToken token,List<JSelectListItem<byte>> list, byte? selectedId, string parentValue)
        {
            foreach (var documentKind in DocumentKindSet.Kinds)
            {
                list.Add(new JSelectListItem<byte>
                             {
                                 Text = documentKind.Title,
                                 Value = documentKind.DocumentKindID,
                                 Selected = documentKind.DocumentKindID == selectedId
                             });
            }
        }
    }
}