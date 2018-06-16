using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.UserGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// КОнтроллер грида пользователей.
    /// </summary>
    [ExtendedAuthorize]
    public class UserGridController : GridControllerBase<Guid, UserGridModel, UserCreateModel, UserEditModel>
    {
        public UserGridController() : base(new UserGridDataAdapter())
        {
        }

        public const string ControllerName = "UserGrid";

        /// <summary>
        /// Получает название контроллера.
        /// </summary>
        /// <returns>Название контроллера.</returns>
        protected override string GetControllerName()
        {
            return ControllerName;
        }

        /// <summary>
        /// Получает название грида.
        /// </summary>
        /// <returns>Название грида.</returns>
        protected override string GetGridName()
        {
            return "UsersGrid";
        }
    }
}