using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.AutocompleteItemForm;
using Romontinka.Server.WebSite.Models.Controls;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер управления пунктами автозаполнения.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class AutocompleteItemController : JGridControllerBase<Guid, AutocompleteItemGridItemModel, AutocompleteItemCreateModel, AutocompleteItemCreateModel, AutocompleteItemSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "AutocompleteItem";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public AutocompleteItemController(JGridDataAdapterBase<Guid, AutocompleteItemGridItemModel, AutocompleteItemCreateModel, AutocompleteItemCreateModel, AutocompleteItemSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }

        /// <summary>
        /// Получает содержимое страницы.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new AutocompleteItemViewModel();
            model.AutocompleteItemsGrid = new DataGridDescriptor();
            model.AutocompleteItemsGrid.Name = ControllerName;
            model.AutocompleteItemsGrid.Controller = ControllerName;
            model.AutocompleteItemsGrid.SearchInputs.Add(new TextSearchInput { Id = "AutocompleteItemSearchTitle", Value = string.Empty, Name = "Название" });
            model.AutocompleteItemsGrid.SearchInputs.Add(new ComboBoxSearchInput { Name = "Тип", ComboBoxModel = new AjaxComboBoxModel { Property = "AutocompleteItemSearchKindID", Controller = "AjaxAutocompleteKindComboBox", FirstIsNull = true } });

            model.AutocompleteItemsGrid.Columns.Add(new TextGridColumn { Name = "Название", Id = "Title" });
            model.AutocompleteItemsGrid.Columns.Add(new TextGridColumn { Name = "Тип", Id = "AutocompleteKindTitle" });
            model.AutocompleteItemsGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить пункт автозавершения ", DataId = "Title" };

            model.AutocompleteItemsGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 300, Width = 500 };
            model.AutocompleteItemsGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание автозавершения", Height = 300, Width = 500 };

            model.AutocompleteItemsGrid.AutoLoad = true;

            model.AutocompleteItemsGrid.HasTableBorderedClass = true;
            model.AutocompleteItemsGrid.HasTableStripedClass = true;


            return View(model);
        }
    }
}