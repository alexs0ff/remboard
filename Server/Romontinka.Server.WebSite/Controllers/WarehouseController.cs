using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.Branch;
using Romontinka.Server.WebSite.Models.DataGrid;
using Romontinka.Server.WebSite.Models.WarehouseForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер управления складами.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class WarehouseController : JGridControllerBase<Guid, WarehouseGridItemModel, WarehouseCreateModel, WarehouseCreateModel, WarehouseSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "Warehouse";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public WarehouseController(JGridDataAdapterBase<Guid, WarehouseGridItemModel, WarehouseCreateModel, WarehouseCreateModel, WarehouseSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }

        /// <summary>
        /// Получает содержимое страницы.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new WarehouseViewModel();
            model.WarehousesGrid = new DataGridDescriptor();
            model.WarehousesGrid.Name = ControllerName;
            model.WarehousesGrid.Controller = ControllerName;
            model.WarehousesGrid.SearchInputs.Add(new TextSearchInput { Id = "WarehouseName", Value = string.Empty, Name = "Название" });

            model.WarehousesGrid.Columns.Add(new TextGridColumn { Name = "Название", Id = "Title" });
            model.WarehousesGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить склад ", DataId = "Title" };

            model.WarehousesGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 200, Width = 500 };
            model.WarehousesGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание склада", Height = 200, Width = 500 };

            model.WarehousesGrid.AutoLoad = true;

            model.WarehousesGrid.HasTableBorderedClass = true;
            model.WarehousesGrid.HasTableStripedClass = true;

            /*Категории товара*/

            model.ItemCategoryGrid = new DataGridDescriptor();
            model.ItemCategoryGrid.Name = ItemCategoryController.ControllerName;
            model.ItemCategoryGrid.Controller = ItemCategoryController.ControllerName;
            model.ItemCategoryGrid.SearchInputs.Add(new TextSearchInput { Id = "ItemCategoryName", Value = string.Empty, Name = "Название" });

            model.ItemCategoryGrid.Columns.Add(new TextGridColumn { Name = "Название", Id = "Title" });
            model.ItemCategoryGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить категорию ", DataId = "Title" };

            model.ItemCategoryGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 200, Width = 500 };
            model.ItemCategoryGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание категории", Height = 200, Width = 500 };

            model.ItemCategoryGrid.AutoLoad = true;

            model.ItemCategoryGrid.HasTableBorderedClass = true;
            model.ItemCategoryGrid.HasTableStripedClass = true;

            /*Контрагенты*/

            model.ContractorGrid = new DataGridDescriptor();
            model.ContractorGrid.Name = ContractorController.ControllerName;
            model.ContractorGrid.Controller = ContractorController.ControllerName;
            model.ContractorGrid.SearchInputs.Add(new TextSearchInput { Id = "ContractorName", Value = string.Empty, Name = "Название" });

            model.ContractorGrid.Columns.Add(new TextGridColumn { Name = "Юр название", Id = "LegalName" });
            model.ContractorGrid.Columns.Add(new TextGridColumn { Name = "Адрес", Id = "Address" });
            model.ContractorGrid.Columns.Add(new TextGridColumn { Name = "Телефон", Id = "Phone" });
            model.ContractorGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить контрагента ", DataId = "Title" };

            model.ContractorGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 400, Width = 500 };
            model.ContractorGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание контрагента", Height = 400, Width = 500 };

            model.ContractorGrid.AutoLoad = true;

            model.ContractorGrid.HasTableBorderedClass = true;
            model.ContractorGrid.HasTableStripedClass = true;

            /*Номенклатура*/

            model.GoodsItemGrid = new DataGridDescriptor();
            model.GoodsItemGrid.Name = GoodsItemController.ControllerName;
            model.GoodsItemGrid.Controller = GoodsItemController.ControllerName;
            model.GoodsItemGrid.SearchInputs.Add(new TextSearchInput { Id = "GoodsItemName", Value = string.Empty, Name = "Название" });

            model.GoodsItemGrid.Columns.Add(new TextGridColumn { Name = "Название", Id = "Title" });
            model.GoodsItemGrid.Columns.Add(new TextGridColumn { Name = "Категория", Id = "ItemCategoryTitle" });
            model.GoodsItemGrid.Columns.Add(new TextGridColumn { Name = "Артикул", Id = "Particular" });
            model.GoodsItemGrid.Columns.Add(new TextGridColumn { Name = "Код", Id = "UserCode" });
            model.GoodsItemGrid.Columns.Add(new TextGridColumn { Name = "Описание", Id = "Description" });
            
            model.GoodsItemGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить номенклатуру ", DataId = "Title" };

            model.GoodsItemGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 400, Width = 500 };
            model.GoodsItemGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание номенклатуры", Height = 400, Width = 500 };

            model.GoodsItemGrid.AutoLoad = true;

            model.GoodsItemGrid.HasTableBorderedClass = true;
            model.GoodsItemGrid.HasTableStripedClass = true;

            return View(model);
        }
    }
}