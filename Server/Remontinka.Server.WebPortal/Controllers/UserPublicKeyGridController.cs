using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.UserPublicKeyGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида управления публичными ключами.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class UserPublicKeyGridController : GridControllerBase<Guid, UserPublicKeyGridModel, UserPublicKeyEditModel, UserPublicKeyEditModel>
    {
        public UserPublicKeyGridController() : base(new UserPublicKeyGridDataAdapter())
        {
        }

        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "UserPublicKeyGrid";

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
            return "UserPublicKeysGrid";
        }
    }
}