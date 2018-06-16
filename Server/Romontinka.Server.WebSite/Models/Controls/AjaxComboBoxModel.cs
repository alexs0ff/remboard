using System.Collections.Generic;

namespace Romontinka.Server.WebSite.Models.Controls
{
    /// <summary>
    /// Представляет модель для комбобокса.
    /// </summary>
    public class AjaxComboBoxModel
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
        /// Задает или получает значение для комбобокса.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Задает или получает флаг указывающий, что первый из списка должен быть null. т.е. к списку первоначально добавляется null элемент.
        /// </summary>
        public bool FirstIsNull { get; set; }

        /// <summary>
        /// Задает или получает флаг указывающий, что необходимо выбрать значение отличное от пустого.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Задает или получает классы для применения на контроле ввода.
        /// </summary>
        public string EditorClasses { get; set; }

        /// <summary>
        /// Задает или получает атрибуты html.
        /// </summary>
        public IDictionary<string, object> HtmlAttributes { get; set; }
    }
}