using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.SystemForm
{
    /// <summary>
    /// Пункт комбобокса для выбора темы.
    /// </summary>
    public class ThemeComboBoxItem
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public ThemeComboBoxItem()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public ThemeComboBoxItem(string id, string title)
        {
            Id = id;
            Title = title;
        }

        /// <summary>
        /// Задает или получает код темы.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Задает или получает название темы.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает URL темы
        /// </summary>
        public string Url { get { return "/Content/images/theme/" + Id + ".png"; } }
    }
}