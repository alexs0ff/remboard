using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.DataGrid;
using Romontinka.Server.WebSite.Models.MyTasksForm;
using Romontinka.Server.WebSite.Models.RepairOrderForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер текущих задач.
    /// </summary>
    [ExtendedAuthorize]
    public class MyTasksController : JGridControllerBase<Guid, RepairOrderGridItemModel, RepairOrderCreateModel, RepairOrderEditModel, MyTasksSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "MyTasks";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public MyTasksController(JGridDataAdapterBase<Guid, RepairOrderGridItemModel, RepairOrderCreateModel, RepairOrderEditModel, MyTasksSearchModel> adapter) : base(adapter)
        {
            CreateItemViewName = "~/Views/RepairOrder/create.cshtml";
            EditItemViewName = "~/Views/RepairOrder/edit.cshtml";
        }

        /// <summary>
        /// Получает содержимое страницы.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = RepairOrderController.CreateRepairOrderViewModel(GetToken());
            model.OrderGrid.Controller = ControllerName;
            model.OrderGrid.SearchInputs.Clear();
            model.OrderGrid.SearchInputs.Add(new TextSearchInput {Id = "Name", Value = string.Empty, Name = "Имя"});
            model.OrderGrid.SearchInputs.Add(new HiddenSearchInput { Id = "CopyFromRepairOrderID" });

            ViewBag.Title = "Моя работа";
            return View("RepairOrderGrid", model);
        }
    }
}