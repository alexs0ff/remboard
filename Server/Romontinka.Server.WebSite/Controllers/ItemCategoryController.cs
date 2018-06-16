using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.ItemCategoryForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер категории товаров.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class ItemCategoryController : JGridControllerBase<Guid, ItemCategoryGridItemModel, ItemCategoryCreateModel, ItemCategoryCreateModel, ItemCategorySearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "ItemCategory";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public ItemCategoryController(JGridDataAdapterBase<Guid, ItemCategoryGridItemModel, ItemCategoryCreateModel, ItemCategoryCreateModel, ItemCategorySearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }
    }
}