using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.DataGrid;
using Romontinka.Server.WebSite.Models.UserPublicKeyForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер управления публичными ключами.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class UserPublicKeyController : JGridControllerBase<Guid, UserPublicKeyGridItemModel, UserPublicKeyCreateModel, UserPublicKeyCreateModel, UserPublicKeySearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "UserPublicKey";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public UserPublicKeyController(JGridDataAdapterBase<Guid, UserPublicKeyGridItemModel, UserPublicKeyCreateModel, UserPublicKeyCreateModel, UserPublicKeySearchModel> adapter) : base(adapter)
        {
            EditItemViewName = CreateItemViewNameDefault;
        }

        /// <summary>
        /// Получает содержимое страницы.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new UserPublicKeyViewModel();
            model.UserPublicKeysGrid = new DataGridDescriptor();
            model.UserPublicKeysGrid.Name = ControllerName;
            model.UserPublicKeysGrid.Controller = ControllerName;
            model.UserPublicKeysGrid.SearchInputs.Add(new TextSearchInput { Id = "Name", Value = string.Empty, Name = "Название" });

            model.UserPublicKeysGrid.Columns.Add(new TextGridColumn { Name = "Логин", Id = "LoginName" });
            model.UserPublicKeysGrid.Columns.Add(new TextGridColumn { Name = "ФИО", Id = "UserFullName" });
            model.UserPublicKeysGrid.Columns.Add(new TextGridColumn { Name = "Дата", Id = "EventDate" });
            model.UserPublicKeysGrid.Columns.Add(new TextGridColumn { Name = "Статус", Id = "Status" });
            model.UserPublicKeysGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить запрос от ", DataId = "LoginName" };

            model.UserPublicKeysGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 350, Width = 500 };
            model.UserPublicKeysGrid.CreateButtonGrid = null;

            model.UserPublicKeysGrid.AutoLoad = true;

            model.UserPublicKeysGrid.HasTableBorderedClass = true;
            model.UserPublicKeysGrid.HasTableStripedClass = true;


            return View(model);
        }
    }
}