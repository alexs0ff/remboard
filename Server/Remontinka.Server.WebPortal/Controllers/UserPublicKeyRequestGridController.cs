using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.UserPublicKeyRequestGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида для ключей на активацию.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class UserPublicKeyRequestGridController : GridControllerBase<Guid, UserPublicKeyRequestGridModel, UserPublicKeyRequestEditModel, UserPublicKeyRequestEditModel>
    {
        public UserPublicKeyRequestGridController() : base(new UserPublicKeyRequestGridDataAdapter())
        {
        }

        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "UserPublicKeyRequestGrid";

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
            return "UserPublicKeyRequestsGrid";
        }
    }
}