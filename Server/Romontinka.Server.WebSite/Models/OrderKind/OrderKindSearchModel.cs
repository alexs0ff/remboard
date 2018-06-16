using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.OrderKind
{
    /// <summary>
    /// Модель для поиска типов заказа.
    /// </summary>
    public class OrderKindSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поика по имени типа.
        /// </summary>
        public string Name { get; set; }
    }
}