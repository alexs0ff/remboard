using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.User
{
    /// <summary>
    /// Модель пункта выбора пользователя.
    /// </summary>
    public class JLookupUserItemModel : JLookupItemBaseModel
    {
        /// <summary>
        /// Задает или получает полное имя пользователя.
        /// </summary>
        public string FullName { get; set; }
    }
}