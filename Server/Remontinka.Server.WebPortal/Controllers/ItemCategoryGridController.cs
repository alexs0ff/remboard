using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.ItemCategoryGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида категорий товаров.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class ItemCategoryGridController : GridControllerBase<Guid, ItemCategoryGridModel, ItemCategoryCreateModel, ItemCategoryCreateModel>
    {
        public ItemCategoryGridController() : base(new ItemCategoryGridDataAdapter())
        {
        }

        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "ItemCategoryGrid";

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
            return "ItemCategoriesGrid";
        }
    }
}