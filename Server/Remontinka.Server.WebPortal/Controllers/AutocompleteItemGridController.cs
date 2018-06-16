using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.AutocompleteItemGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида управления автодополнениями.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class AutocompleteItemGridController : GridControllerBase<Guid, AutocompleteItemGridModel, AutocompleteItemCreateModel, AutocompleteItemCreateModel>
    {
        public AutocompleteItemGridController() : base(new AutocompleteItemGridDataAdapter())
        {
        }

        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "AutocompleteItemGrid";

        /// <summary>
        /// Получает название контроллера.
        /// </summary>
        /// <returns>Название контроллера.</returns>
        protected override string GetControllerName()
        {
            return ControllerName;
        }

        /// <summary>
        /// Получает название грида.
        /// </summary>
        /// <returns>Название грида.</returns>
        protected override string GetGridName()
        {
            return "AutocompleteItemsGrid";
        }
    }
}