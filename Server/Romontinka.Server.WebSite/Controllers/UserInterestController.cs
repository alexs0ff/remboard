using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.DataGrid;
using Romontinka.Server.WebSite.Models.UserInterestForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер управления вознаграждениями пользователей.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class UserInterestController : JGridControllerBase<Guid, UserInterestGridItemModel, UserInterestCreateModel, UserInterestCreateModel, UserInterestSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "UserInterest";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public UserInterestController(JGridDataAdapterBase<Guid, UserInterestGridItemModel, UserInterestCreateModel, UserInterestCreateModel, UserInterestSearchModel> adapter)
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
            var model = new UserInterestViewModel();
            model.UserInterestsGrid = new DataGridDescriptor();
            model.UserInterestsGrid.Name = ControllerName;
            model.UserInterestsGrid.Controller = ControllerName;
            model.UserInterestsGrid.SearchInputs.Add(new TextSearchInput { Id = "UserInterestSearchTitle", Value = string.Empty, Name = "Название" });

            model.UserInterestsGrid.Columns.Add(new TextGridColumn { Name = "Дата", Id = "EventDate" });
            model.UserInterestsGrid.Columns.Add(new TextGridColumn { Name = "Пользователь", Id = "UserFullName" });
            model.UserInterestsGrid.Columns.Add(new TextGridColumn { Name = "Значения", Id = "Values" });
            model.UserInterestsGrid.Columns.Add(new TextGridColumn { Name = "Описание", Id = "Description" });
            model.UserInterestsGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить пункт вознаграждения для ", DataId = "UserFullName" };

            model.UserInterestsGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 500, Width = 600 };
            model.UserInterestsGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание вознаграждения", Height = 500, Width = 600 };

            model.UserInterestsGrid.AutoLoad = true;

            model.UserInterestsGrid.HasTableBorderedClass = true;
            model.UserInterestsGrid.HasTableStripedClass = true;


            return View(model);
        }
    }
}