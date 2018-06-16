using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Модель базового класса для форм поиска.
    /// </summary>
    public abstract class JLookupItemBaseModel
    {
        /// <summary>
        /// Задает или получает строковое представление кода идентификатора пункта.
        /// </summary>
        public string ItemID { get; set; }
    }
}