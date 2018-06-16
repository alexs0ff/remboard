using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Helpers
{
    /// <summary>
    /// Хелпер для настроек безопасности видимости полей заказа для опеределенного пользователя.
    /// </summary>
    public static class RepairOrderSecurityHelper
    {
        private static readonly string[] AllowFields =  { "RepairOrderStatusID", "Recommendation" };

        /// <summary>
        /// Устанавливает настройки безопасности для редактора.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <param name="token">Токен безорпасности.</param>
        public static void SetSecuritySettings(this EditorSettings settings, SecurityToken token)
        {
            if (token.User.ProjectRoleID == ProjectRoleSet.Engineer.ProjectRoleID)
            {
                if (AllowFields.Any(i => StringComparer.OrdinalIgnoreCase.Equals(settings.Name, i)))
                {
                    settings.Enabled = true;
                }
                else
                {
                    settings.Enabled = false;
                }
            }
        }
        
    }
}