using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Models.User;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Комбобокс с менеджерами.
    /// </summary>
    [Authorize]
    public class AjaxManagerComboBoxController : AjaxComboBoxBaseController<Guid>
    {
        /// <summary>
        /// Переопределяется для получения переопределенного списка с пунктами.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="list">Список который необходимо инициализировать.</param>
        /// <param name="selectedId"> Выбранный пункт.</param>
        /// <param name="parentValue">Значение родительского контрола.</param>
        public override void GetInitializedItems(SecurityToken token,List<JSelectListItem<Guid>> list, Guid? selectedId, string parentValue)
        {
            var securityToken = GetToken();
            UserHelper.PopulateUserList(list, selectedId, securityToken, ProjectRoleSet.Manager.ProjectRoleID);
        }
    }
}