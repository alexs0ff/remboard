using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Common
{
    /// <summary>
    /// Базовый класс для класса поиска данных в гриде.
    /// </summary>
    public class JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает страницу с которой необходимо делать выборку данных.
        /// </summary>
        public int Page { get; set; }
    }
}