using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Models.Common
{
    /// <summary>
    /// Модель для popup наименования фильтра грида.
    /// </summary>
    public class FilterNamePopupModel
    {
        /// <summary>
        /// Задает или получает токен безопасности.
        /// </summary>
        public SecurityToken Token { get; set; }

        /// <summary>
        /// Задает или получает наименование Popup.
        /// </summary>
        public string PopupName { get; set; }

        /// <summary>
        /// Задает или получает имя контроллера.
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Задает или получает имя грида.
        /// </summary>
        public string GridName { get; set; }

        /// <summary>
        /// Задает или получает название фильтра.
        /// </summary>
        [Required]
        [DisplayName("Название")]
        public string FilterName { get; set; }
    }
}