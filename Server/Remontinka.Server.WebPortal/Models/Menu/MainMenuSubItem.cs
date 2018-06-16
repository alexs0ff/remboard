using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.Menu
{
    /// <summary>
    /// Задает или получает подменю.
    /// </summary>
    public class MainMenuSubItem: MainMenuItemBase
    {
        /// <summary>
        /// Задает или получает признак группы.
        /// </summary>
        public bool BeginGroup { get; set; }

        /// <summary>
        /// Производит копирование подпункта меню.
        /// </summary>
        /// <returns>Скопированный подпункта.</returns>
        public MainMenuSubItem CreateCopy()
        {
            var result = new MainMenuSubItem();

            result.Roles = Roles;
            result.Title = Title;
            result.Action = Action;
            result.BeginGroup = BeginGroup;
            result.Controller = Controller;

            return result;
        }
    }
}