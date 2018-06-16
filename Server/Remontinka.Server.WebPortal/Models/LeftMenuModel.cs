using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Widgets;

namespace Remontinka.Server.WebPortal.Models
{
    /// <summary>
    /// Модель отображения левого меню.
    /// </summary>
    public class LeftMenuModel
    {
        /// <summary>
        /// Получает доступные виджеты.
        /// </summary>
        public IEnumerable<WidgetItem> Widgets { get { return WidgetSet.Widgets; } }

        /// <summary>
        /// Задат или получает имя контроллера.
        /// </summary>
        public string ControllerName { get; set; }
    }
}