using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.UserPublicKeyRequestGridForm
{
    /// <summary>
    /// Модель грида для активации ключей.
    /// </summary>
    public class UserPublicKeyRequestGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "UserPublicKeyRequestID"; } }

    }
}