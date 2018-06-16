using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.User
{
    /// <summary>
    /// Модель поиска для лукапа. 
    /// </summary>
    public class JLookupUserSearchModel : JLookupSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поиска в гриде.
        /// </summary>
        [DisplayName("Наименование")]
        public string Name { get; set; }
    }
}