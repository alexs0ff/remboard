using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.Controls
{
    /// <summary>
    /// Модель данных для автодополнения с локальным источником.
    /// </summary>
    public class AutocompleteLocalSourceModel
    {
        /// <summary>
        /// Задает или получает имя свойства.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Задает или получает значение для текстбокса.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Задает или получает название локального источника.
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// Задает или получает признак того, что контрл должен поддерживать множество дополнений через запятую.
        /// </summary>
        public bool IsMultiple { get; set; }

        /// <summary>
        /// Задает или получает классы для применения на контроле ввода.
        /// </summary>
        public string EditorClasses { get; set; }
    }
}