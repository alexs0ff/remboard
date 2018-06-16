using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.DataGrid;
using Romontinka.Server.WebSite.Models.OrderStatus;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер управления статусами заказа.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class OrderStatusController : JGridControllerBase<Guid, OrderStatusGridItemModel, OrderStatusCreateModel, OrderStatusCreateModel, OrderStatusSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "OrderStatus";

        public OrderStatusController(JGridDataAdapterBase<Guid, OrderStatusGridItemModel, OrderStatusCreateModel, OrderStatusCreateModel, OrderStatusSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }

        /// <summary>
        /// Получает содержимое страницы.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new OrderStatusViewModel();
            model.OrderStatusesGrid = new DataGridDescriptor();
            model.OrderStatusesGrid.Name = ControllerName;
            model.OrderStatusesGrid.Controller = ControllerName;
            model.OrderStatusesGrid.SearchInputs.Add(new TextSearchInput { Id = "Name", Value = string.Empty, Name = "Название" });

            model.OrderStatusesGrid.Columns.Add(new TextGridColumn { Name = "Название", Id = "Title" });
            model.OrderStatusesGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить тип ", DataId = "Title" };

            model.OrderStatusesGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 300, Width = 300 };
            model.OrderStatusesGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание типа", Height = 300, Width = 300 };

            model.OrderStatusesGrid.AutoLoad = true;

            model.OrderStatusesGrid.HasTableBorderedClass = true;
            model.OrderStatusesGrid.HasTableStripedClass = false;

            return View(model);
        }
    }
}