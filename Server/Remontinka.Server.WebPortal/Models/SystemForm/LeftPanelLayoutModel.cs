using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Remontinka.Server.WebPortal.Models.Widgets;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Models.SystemForm
{
    /// <summary>
    /// Модель лайаута левой панели.
    /// </summary>
    public class LeftPanelLayoutModel
    {
        /// <summary>
        /// Задает или получает токен безопасности.
        /// </summary>
        public SecurityToken Token { get; set; }

        /// <summary>
        /// Получает доступные виджеты.
        /// </summary>
        public IEnumerable<WidgetItem> Widgets { get { return WidgetSet.Widgets; } }
    }
}