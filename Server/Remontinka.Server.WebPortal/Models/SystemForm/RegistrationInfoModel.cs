using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Models.SystemForm
{
    /// <summary>
    /// Модель отображения данных регистрации
    /// </summary>
    public class RegistrationInfoModel
    {
        /// <summary>
        /// Задает или получает главный логин в системе.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Задает или получает email.
        /// </summary>
        [DisplayName("Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Задает или получает юрназвание фирмы.
        /// </summary>
        [DisplayName("Юридическое наименование")]
        [Required]
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает торговое наименование фирмы.
        /// </summary>
        [DisplayName("Торговое наименование")]
        [Required]
        public string Trademark { get; set; }

        /// <summary>
        /// Задает или получает адрес фирмы.
        /// </summary>
        [DisplayName("Адрес")]
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Задает или получает токен безопасности.
        /// </summary>
        public SecurityToken Token { get; set; }
    }
}