using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.Controls;
using Romontinka.Server.WebSite.Models.DataGrid;
using Romontinka.Server.WebSite.Models.FinancialItemValueForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер внесения значений в статьи бюджета финансовых групп филиалов.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class FinancialItemValueController : JGridControllerBase<Guid, FinancialItemValueGridItemModel, FinancialItemValueCreateModel, FinancialItemValueCreateModel, FinancialItemValueSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "FinancialItemValue";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public FinancialItemValueController(JGridDataAdapterBase<Guid, FinancialItemValueGridItemModel, FinancialItemValueCreateModel, FinancialItemValueCreateModel, FinancialItemValueSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
            ItemsPerPage = 25;
        }

        /// <summary>
        /// Получает содержимое страницы.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new FinancialItemValueViewModel();
            model.FinancialItemValuesGrid = new DataGridDescriptor();
            model.FinancialItemValuesGrid.Name = ControllerName;
            model.FinancialItemValuesGrid.Controller = ControllerName;
            model.FinancialItemValuesGrid.SearchInputs.Add(new DateTimeSearchInput {DateTimeValue = Utils.GetFirstDayOfMonth(DateTime.Today), Id = "FinancialItemValueSearchBeginDate", Value = string.Empty, Name = "Начало" });
            model.FinancialItemValuesGrid.SearchInputs.Add(new DateTimeSearchInput {DateTimeValue =Utils.GetLastDayOfMonth(DateTime.Today), Id = "FinancialItemValueSearchEndDate", Value = string.Empty, Name = "Окончание" });
            model.FinancialItemValuesGrid.SearchInputs.Add(new NewLineSearchInput());
            model.FinancialItemValuesGrid.SearchInputs.Add(new ComboBoxSearchInput { Name = "Финансовая группа", ComboBoxModel = new AjaxComboBoxModel { Property = "FinancialItemValueSearchFinancialGroupID", Controller = "AjaxFinancialGroupComboBox", FirstIsNull = true } });
            model.FinancialItemValuesGrid.SearchInputs.Add(new TextSearchInput { Id = "FinancialItemSearchName", Value = string.Empty, Name = "Название" });


            model.FinancialItemValuesGrid.Columns.Add(new TextGridColumn { Name = "Дата", Id = "EventDate" });
            model.FinancialItemValuesGrid.Columns.Add(new TextGridColumn { Name = "Статья", Id = "FinancialItemTitle" });
            model.FinancialItemValuesGrid.Columns.Add(new TextGridColumn { Name = "Значение", Id = "Amount" });
            model.FinancialItemValuesGrid.Columns.Add(new TextGridColumn { Name = "Описание", Id = "Description" });
            model.FinancialItemValuesGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить запись ", DataId = "FinancialItemTitle" };

            model.FinancialItemValuesGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 420, Width = 500 };
            model.FinancialItemValuesGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание записи", Height = 420, Width = 500 };

            model.FinancialItemValuesGrid.AutoLoad = true;

            model.FinancialItemValuesGrid.HasTableBorderedClass = true;
            model.FinancialItemValuesGrid.HasTableStripedClass = true;

            return View(model);
        }
    }
}