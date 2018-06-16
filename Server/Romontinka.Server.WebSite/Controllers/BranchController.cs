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

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер для управления филиалами.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class BranchController : JGridControllerBase<Guid, BranchGridItemModel, BranchCreateModel, BranchCreateModel, BranchSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "Branch";

        public BranchController(JGridDataAdapterBase<Guid, BranchGridItemModel, BranchCreateModel, BranchCreateModel, BranchSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }

        /// <summary>
        /// Получает содержимое страницы.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new BranchViewModel();
            model.BranchesGrid = new DataGridDescriptor();
            model.BranchesGrid.Name = ControllerName;
            model.BranchesGrid.Controller = ControllerName;
            model.BranchesGrid.SearchInputs.Add(new TextSearchInput { Id = "Name", Value = string.Empty, Name = "Название" });

            model.BranchesGrid.Columns.Add(new TextGridColumn { Name = "Название", Id = "Title" });
            model.BranchesGrid.Columns.Add(new TextGridColumn { Name = "Юр название", Id = "LegalName" });
            model.BranchesGrid.Columns.Add(new TextGridColumn { Name = "Адрес", Id = "Address" });
            model.BranchesGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить филиал ", DataId = "Title" };

            model.BranchesGrid.EditButtonGridColumn = new EditButtonGridColumn{Height = 300,Width = 500};
            model.BranchesGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание филиала", Height = 300, Width = 500 };

            model.BranchesGrid.AutoLoad = true;

            model.BranchesGrid.HasTableBorderedClass = true;
            model.BranchesGrid.HasTableStripedClass = true;

            
            return View(model);
        }

    }
}
