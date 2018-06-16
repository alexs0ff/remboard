using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.Controls
{
    /// <summary>
    /// Модель для списка с множественным выбором.
    /// </summary>
    public class AjaxCheckBoxListModel
    {
        /// <summary>
        /// Задает или получает имя связанного контроллера.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Задает или получает имя связанного действия.
        /// </summary>
        public string GetItemsAction { get; set; }

        /// <summary>
        /// Задает или получает имя свойства.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Задает или получает имя родительского свойства.
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Задает или получает значения для пунктов.
        /// </summary>
        public string[] Values { get; set; }
        
        /// <summary>
        /// Задает или получает флаг указывающий, что необходимо выбрать значение отличное от пустого.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Задает или получает классы для применения на контроле ввода.
        /// Не реализовано.
        /// </summary>
        public string EditorClasses { get; set; }

        /// <summary>
        /// Задает или получает атрибуты html.
        /// </summary>
        public IDictionary<string, object> HtmlAttributes { get; set; }
    }
}