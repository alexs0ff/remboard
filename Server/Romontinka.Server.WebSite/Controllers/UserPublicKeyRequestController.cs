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
using Romontinka.Server.WebSite.Models.UserPublicKeyRequestForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер управления запросами на регистрацию публичных ключей.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class UserPublicKeyRequestController : JGridControllerBase<Guid, UserPublicKeyRequestGridItemModel, UserPublicKeyRequestCreateModel, UserPublicKeyRequestCreateModel, UserPublicKeyRequestSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "UserPublicKeyRequest";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public UserPublicKeyRequestController(JGridDataAdapterBase<Guid, UserPublicKeyRequestGridItemModel, UserPublicKeyRequestCreateModel, UserPublicKeyRequestCreateModel, UserPublicKeyRequestSearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }

        /// <summary>
        /// Получает содержимое страницы.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new UserPublicKeyRequestViewModel();
            model.UserPublicKeyRequestsGrid = new DataGridDescriptor();
            model.UserPublicKeyRequestsGrid.Name = ControllerName;
            model.UserPublicKeyRequestsGrid.Controller = ControllerName;
            model.UserPublicKeyRequestsGrid.SearchInputs.Add(new TextSearchInput { Id = "Name", Value = string.Empty, Name = "Название" });

            model.UserPublicKeyRequestsGrid.Columns.Add(new TextGridColumn { Name = "Логин", Id = "LoginName" });
            model.UserPublicKeyRequestsGrid.Columns.Add(new TextGridColumn { Name = "IP", Id = "ClientIdentifier" });
            model.UserPublicKeyRequestsGrid.Columns.Add(new TextGridColumn { Name = "Дата", Id = "EventDate" });
            model.UserPublicKeyRequestsGrid.Columns.Add(new TextGridColumn { Name = "Заметки", Id = "KeyNotes" });
            model.UserPublicKeyRequestsGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить запрос от ", DataId = "LoginName" };

            model.UserPublicKeyRequestsGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 320, Width = 500 };
            model.UserPublicKeyRequestsGrid.CreateButtonGrid = null;

            model.UserPublicKeyRequestsGrid.AutoLoad = true;

            model.UserPublicKeyRequestsGrid.HasTableBorderedClass = true;
            model.UserPublicKeyRequestsGrid.HasTableStripedClass = true;


            return View(model);
        }
    }
}