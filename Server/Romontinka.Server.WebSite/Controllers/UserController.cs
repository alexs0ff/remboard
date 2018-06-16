using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models;
using Romontinka.Server.WebSite.Models.Branch;
using Romontinka.Server.WebSite.Models.DataGrid;
using Romontinka.Server.WebSite.Models.User;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер управления пользователями.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class UserController : JGridControllerBase<Guid, UserGridItemModel, UserCreateModel, UserEditModel, UserSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "User";

        public UserController(JGridDataAdapterBase<Guid, UserGridItemModel, UserCreateModel, UserEditModel, UserSearchModel> adapter) : base(adapter)
        {

        }

        /// <summary>
        /// Получает содержимое страницы.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new UserViewModel();
            model.UsersGrid = new DataGridDescriptor();
            model.UsersGrid.Name = ControllerName;
            model.UsersGrid.Controller = ControllerName;
            model.UsersGrid.SearchInputs.Add(new TextSearchInput { Id = "Name", Value = string.Empty, Name = "Название" });

            model.UsersGrid.Columns.Add(new TextGridColumn { Name = "Логин", Id = "Login" });
            model.UsersGrid.Columns.Add(new TextGridColumn { Name = "ФИО", Id = "FullName" });
            model.UsersGrid.Columns.Add(new TextGridColumn { Name = "Роль", Id = "ProjectRoleTitle" });
            model.UsersGrid.Columns.Add(new TextGridColumn { Name = "Контакты", Id = "Contacts" });
            model.UsersGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить пользователя ", DataId = "FullName" };

            model.UsersGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 500, Width = 550 };
            model.UsersGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание пользователя", Height = 650, Width = 550 };

            model.UsersGrid.AutoLoad = true;

            model.UsersGrid.HasTableBorderedClass = true;
            model.UsersGrid.HasTableStripedClass = true;

            return View(model);
        }


    }
}