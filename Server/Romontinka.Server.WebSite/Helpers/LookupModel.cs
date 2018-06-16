using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Helpers
{
    /// <summary>
    /// Модель для lookup представления.
    /// </summary>
    public class LookupModel
    {
        /// <summary>
        /// Задает или получает контроллер для получения данных.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Задает или получает метод для получения отфильтрованных данных.
        /// </summary>
        public string GetItemsAction { get; set; }

        /// <summary>
        /// Получение определенного пункта по его id.
        /// </summary>
        public string GetItemAction { get; set; }

        /// <summary>
        /// Задает или получает название свойства данных.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Задает или получает значение для свойства данных.
        /// </summary>
        public string PropertyValue { get; set; }

        /// <summary>
        /// Задает или получает код "родительского" поля.
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// Задает или получает минимальное количество символов при начале получений данных.
        /// </summary>
        public int MinLenght { get; set; }

        /// <summary>
        /// Задает или получает классы для применения на контроле ввода.
        /// </summary>
        public string EditorClasses { get; set; }

        /// <summary>
        /// Задает или получает значение которое указывает на необходимость генерации сообщения для валидации.
        /// </summary>
        public bool CreateValidation { get; set; }

        /// <summary>
        /// Задает или получает атрибуты html.
        /// </summary>
        public IDictionary<string,object> HtmlAttributes { get; set; }
    }
}