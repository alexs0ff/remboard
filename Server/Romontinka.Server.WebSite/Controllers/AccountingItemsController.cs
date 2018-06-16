using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.AccountingItemsModel;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер управления бухгалтерскими сущностями.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class AccountingItemsController : BaseController
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "AccountingItems";

        /// <summary>
        /// Получает содержимое страницы.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new AccountingItemsViewModel();
            model.FinancialGroupGrid = new DataGridDescriptor();
            model.FinancialGroupGrid.Name = FinancialGroupController.ControllerName;
            model.FinancialGroupGrid.Controller = FinancialGroupController.ControllerName;
            model.FinancialGroupGrid.SearchInputs.Add(new TextSearchInput { Id = "FinancialGroupName", Value = string.Empty, Name = "Название" });

            model.FinancialGroupGrid.Columns.Add(new TextGridColumn { Name = "Название", Id = "Title" });
            model.FinancialGroupGrid.Columns.Add(new TextGridColumn { Name = "Юр название", Id = "LegalName" });
            model.FinancialGroupGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить группу ", DataId = "Title" };

            model.FinancialGroupGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 400, Width = 500 };
            model.FinancialGroupGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание фингруппы", Height = 400, Width = 500 };

            model.FinancialGroupGrid.AutoLoad = true;

            model.FinancialGroupGrid.HasTableBorderedClass = true;
            model.FinancialGroupGrid.HasTableStripedClass = true;

            model.FinancialItemsGrid = new DataGridDescriptor();
            model.FinancialItemsGrid.Name = FinancialItemController.ControllerName;
            model.FinancialItemsGrid.Controller = FinancialItemController.ControllerName;
            model.FinancialItemsGrid.SearchInputs.Add(new TextSearchInput { Id = "FinancialItemName", Value = string.Empty, Name = "Название" });

            model.FinancialItemsGrid.Columns.Add(new TextGridColumn { Name = "Название", Id = "Title" });
            model.FinancialItemsGrid.Columns.Add(new TextGridColumn { Name = "Тип", Id = "TransactionKindTitle" });
            model.FinancialItemsGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить статью ", DataId = "Title" };

            model.FinancialItemsGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 380, Width = 540 };
            model.FinancialItemsGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание статьи бюджета", Height = 380, Width = 540 };

            model.FinancialItemsGrid.AutoLoad = true;

            model.FinancialItemsGrid.HasTableBorderedClass = true;
            model.FinancialItemsGrid.HasTableStripedClass = false;

            return View(model);
        }
    }
}