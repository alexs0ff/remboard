using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.SystemForm
{
    /// <summary>
    /// Модель редактирования данных регистрации.
    /// </summary>
    public class RegistrationInfoEditModel : JCrudModelBase
    {
        /// <summary>
        /// Задает или получает email.
        /// </summary>
        [DisplayName("Email")]
        [EditorHtmlClass("registration-info-edit")]
        [LabelHtmlClass("registration-info-label")]
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail ошибочного формата")]
        public string Email { get; set; }

        /// <summary>
        /// Задает или получает юрназвание фирмы.
        /// </summary>
        [DisplayName("Юридическое наименование")]
        [Required]
        [EditorHtmlClass("registration-info-edit")]
        [LabelHtmlClass("registration-info-label")]
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает торговое наименование фирмы.
        /// </summary>
        [DisplayName("Торговое наименование")]
        [Required]
        [EditorHtmlClass("registration-info-edit")]
        [LabelHtmlClass("registration-info-label")]
        public string Trademark { get; set; }

        /// <summary>
        /// Задает или получает адрес фирмы.
        /// </summary>
        [DisplayName("Адрес")]
        [Required]
        [UIHint("MultilineString")]
        [MultilineString(3, 5)]
        [EditorHtmlClass("registration-info-edit")]
        [LabelHtmlClass("registration-info-label")]
        public string Address { get; set; }
    }
}