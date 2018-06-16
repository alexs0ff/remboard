using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.Branch
{
    /// <summary>
    /// Модель поиска для филиала.
    /// </summary>
    public class BranchSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поика по имени филиала.
        /// </summary>
        public string Name { get; set; }
    }
}