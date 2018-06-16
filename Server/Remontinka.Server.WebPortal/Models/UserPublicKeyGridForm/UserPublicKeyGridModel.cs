using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.UserPublicKeyGridForm
{

    /// <summary>
    /// Модель для грида управления публичными ключами.
    /// </summary>
    public class UserPublicKeyGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "UserPublicKeyID"; } }
    }
}