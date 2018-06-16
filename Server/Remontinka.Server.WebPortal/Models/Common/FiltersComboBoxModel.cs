using System.Collections.Generic;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.Common
{
    /// <summary>
    /// Модель для комбобокса.
    /// </summary>
    public class FiltersComboBoxModel
    {
        /// <summary>
        /// Задает или получает токен безопасности.
        /// </summary>
        public SecurityToken Token { get; set; }

        /// <summary>
        /// Задает или получает список фильтров пользователя.
        /// </summary>
        public List<UserGridFilter> UserGridFilters { get; set; }

        /// <summary>
        /// Задает или получает имя грида.
        /// </summary>
        public string GridName { get; set; }

        /// <summary>
        /// Задает или получает имя контроллера.
        /// </summary>
        public string ControllerName { get; set; }
    }
}