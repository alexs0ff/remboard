using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.DataGrid;
using Romontinka.Server.WebSite.Models.OrderKind;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер управления типами заказа.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class OrderKindController : JGridControllerBase<Guid, OrderKindGridItemModel, OrderKindCreateModel, OrderKindCreateModel, OrderKindSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "OrderKind";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public OrderKindController(JGridDataAdapterBase<Guid, OrderKindGridItemModel, OrderKindCreateModel, OrderKindCreateModel, OrderKindSearchModel> adapter)
            : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }

        /// <summary>
        /// Получает содержимое страницы.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new OrderKindViewModel();
            model.OrderKindGrid = new DataGridDescriptor();
            model.OrderKindGrid.Name = ControllerName;
            model.OrderKindGrid.Controller = ControllerName;
            model.OrderKindGrid.SearchInputs.Add(new TextSearchInput { Id = "Name", Value = string.Empty, Name = "Название" });

            model.OrderKindGrid.Columns.Add(new TextGridColumn { Name = "Название", Id = "Title" });
            model.OrderKindGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить тип заказа ", DataId = "Title" };

            model.OrderKindGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 300, Width = 500 };
            model.OrderKindGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание статуса", Height = 300, Width = 500 };

            model.OrderKindGrid.AutoLoad = true;

            model.OrderKindGrid.HasTableBorderedClass = true;
            model.OrderKindGrid.HasTableStripedClass = true;

            return View(model);
        }
    }
}