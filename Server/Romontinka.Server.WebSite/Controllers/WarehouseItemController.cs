using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.Core;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.DataLayer.EntityFramework;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.Controls;
using Romontinka.Server.WebSite.Models.DataGrid;
using Romontinka.Server.WebSite.Models.WarehouseItemForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер остатков на складе.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class WarehouseItemController : JGridControllerBase<Guid, WarehouseItemGridItemModel, WarehouseItemCreateModel, WarehouseItemEditModel, WarehouseItemSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "WarehouseItem";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public WarehouseItemController(JGridDataAdapterBase<Guid, WarehouseItemGridItemModel, WarehouseItemCreateModel, WarehouseItemEditModel, WarehouseItemSearchModel> adapter) : base(adapter)
        {
            ItemsPerPage = 25;
        }

        /// <summary>
        /// Получает содержимое страницы.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var warehouses = RemontinkaServer.Instance.EntitiesFacade.GetWarehouses(GetToken()).ToList();

            var warehouseID = Guid.Empty;

            if (warehouses.Count==1)
            {
                warehouseID = warehouses.First().WarehouseID??Guid.Empty;
            }

            var model = new WarehouseItemViewModel();
            model.WarehouseItemsGrid = new DataGridDescriptor();
            model.WarehouseItemsGrid.Name = ControllerName;
            model.WarehouseItemsGrid.Controller = ControllerName;
            model.WarehouseItemsGrid.SearchInputs.Add(new ComboBoxSearchInput { Name = "Склад", ComboBoxModel = new AjaxComboBoxModel { Property = "WarehouseItemWarehouseID", Controller = "AjaxWarehouseComboBox", FirstIsNull = true, Value = warehouseID.ToString() } });
            model.WarehouseItemsGrid.SearchInputs.Add(new TextSearchInput { Id = "WarehouseItemName", Value = string.Empty, Name = "Наименование" });

            model.WarehouseItemsGrid.Columns.Add(new TextGridColumn { Name = "Наименование", Id = "GoodsItemTitle" });
            model.WarehouseItemsGrid.Columns.Add(new TextGridColumn { Name = "Количество", Id = "Total" });
            model.WarehouseItemsGrid.Columns.Add(new TextGridColumn { Name = "Нулевая цена", Id = "StartPrice" });
            model.WarehouseItemsGrid.Columns.Add(new TextGridColumn { Name = "Ремонтная цена", Id = "RepairPrice" });
            model.WarehouseItemsGrid.Columns.Add(new TextGridColumn { Name = "Цена продажи", Id = "SalePrice" });
            
            model.WarehouseItemsGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 300, Width = 500 };

            model.WarehouseItemsGrid.AutoLoad = true;

            model.WarehouseItemsGrid.HasTableBorderedClass = true;
            model.WarehouseItemsGrid.HasTableStripedClass = false;

            return View(model);
        }
    }
}