using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.Menu
{
    /// <summary>
    /// Главный пункт меню.
    /// </summary>
    public class MainMenuItem: MainMenuItemBase
    {
        /// <summary>
        /// Получает пункты подменю.
        /// </summary>
        public List<MainMenuSubItem> SubItems { get; set; }

        /// <summary>
        /// Производит копирование пункта меню.
        /// </summary>
        /// <returns>Скопированный пункт.</returns>
        public MainMenuItem CreateCopy()
        {
            var result = new MainMenuItem();

            result.Roles = Roles;
            result.Title = Title;
            result.Action = Action;
            result.Controller = Controller;

            return result;
        }
    }
}