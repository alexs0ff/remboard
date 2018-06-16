using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.Controls
{
    /// <summary>
    /// Модель для lookup предствления с открывющимся окном.
    /// </summary>
    public class SingleLookupModel
    {
        /// <summary>
        /// Задает или получает флаг указывающий, что необходимо создать кнопку "очистить значение".
        /// </summary>
        public bool ClearButton { get; set; }

        /// <summary>
        /// Задает или получает флаг указывающий, что окно нужно открывать на полный экран.
        /// </summary>
        public bool FullScreen { get; set; }

        /// <summary>
        /// Задает или получает ширину окна.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Задает или получает высоту окна.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Задает или получает модель поиска данных, может быть null.
        /// </summary>
        public JLookupSearchBaseModel SearchModel { get; set; }

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
        /// Задает или получает название свойства.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Задает или получает значение для свойства данных.
        /// </summary>
        public string PropertyValue { get; set; }

        /// <summary>
        /// Задает или получает код "родительского" поля.
        /// </summary>
        public string ParentID { get; set; }

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