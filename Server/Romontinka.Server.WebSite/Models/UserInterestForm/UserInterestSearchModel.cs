using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.UserInterestForm
{
    /// <summary>
    /// Модель поиска пунктов вознаграждений пользователей.
    /// </summary>
    public class UserInterestSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает название для поиска.
        /// </summary>
        public string UserInterestSearchTitle { get; set; }
    }
}