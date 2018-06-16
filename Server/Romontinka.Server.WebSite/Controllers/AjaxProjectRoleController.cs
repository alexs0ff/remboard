using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Ajax контроллер для проектных ролей.
    /// </summary>
     [Authorize]
    public class AjaxProjectRoleController : AjaxComboBoxBaseController<byte>
    {
        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Токен безопсаности.</param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedId"> Выбранный пункт.</param>
        /// <param name="parentValue">Значение родительского контрола.</param>
        public override void GetInitializedItems(SecurityToken token, List<JSelectListItem<byte>> list, byte? selectedId, string parentValue)
        {
            foreach (var role in ProjectRoleSet.Roles)
            {
                list.Add(new JSelectListItem<byte>
                         {Selected = role.ProjectRoleID == selectedId, Text = role.Title, Value = role.ProjectRoleID});
            }
        }
    }
}