using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.Widgets
{
    /// <summary>
    /// Описатель виджета.
    /// </summary>
    public class WidgetItem
    {
        /// <summary>
        /// Задает или получает код виджета.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Задает или получает название виджета.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает имя связанного представления для виджета.
        /// </summary>
        public string PartialName { get; set; }

        /// <summary>
        /// Задает или получает название связанной картинки.
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// Задает или получает признак того, что необходимо показывать виджет при старте страницы.
        /// </summary>
        public bool ShowOnLoad { get; set; }
    }
}