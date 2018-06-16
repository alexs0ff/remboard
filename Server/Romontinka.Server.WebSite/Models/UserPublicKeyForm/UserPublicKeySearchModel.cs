using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.UserPublicKeyForm
{
    /// <summary>
    /// Модель поиска публичных ключей
    /// </summary>
    public class UserPublicKeySearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поика по публичным ключам.
        /// </summary>
        public string UserPublicKeyName { get; set; }
    }
}