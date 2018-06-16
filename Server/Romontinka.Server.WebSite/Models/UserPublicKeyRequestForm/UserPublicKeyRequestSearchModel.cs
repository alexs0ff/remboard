using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.UserPublicKeyRequestForm
{
    /// <summary>
    /// Модель для поиска запросов публичных ключей.
    /// </summary>
    public class UserPublicKeyRequestSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поика по запросам публичных ключей.
        /// </summary>
        public string UserPublicKeyRequestName { get; set; }
    }
}