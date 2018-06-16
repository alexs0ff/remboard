using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Remontinka.Server.WebPortal.Models.Menu
{
    /// <summary>
    /// Базовый класс для всех пунктов главного меню.
    /// </summary>
    public abstract class MainMenuItemBase
    {
        /// <summary>
        /// Задает или получает название пункта меню.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает название контроллера.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Задает или получает название действия.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Задает или получает список ролей доступных для меню.
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// Создает url для контроллера и действия.
        /// </summary>
        /// <param name="context">Контекст запроса.</param>
        /// <returns>Готовый урл.</returns>
        public string CreateUrl(RequestContext context)
        {
            if (string.IsNullOrWhiteSpace(Action) && string.IsNullOrWhiteSpace(Controller))
            {
                return "#";
            }

            var action = Action;
            if (action==null)
            {
                action = string.Empty;
            }

            UrlHelper u = new UrlHelper(context);
            return u.Action(action, Controller, null);
        }
    }
}