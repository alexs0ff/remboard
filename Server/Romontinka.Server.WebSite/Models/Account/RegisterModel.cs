using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.Account
{
    /// <summary>
    /// Модель регистрации.
    /// </summary>
    public class RegisterModel
    {
        [DisplayName("Логин")]
        [Required]
        [EditorHtmlClass("register-user-edit")]
        [LabelHtmlClass("register-user-label")]
        public string Login { get; set; }

        /// <summary>
        /// Задает или получает пароль.
        /// </summary>
        [UIHint("Password")]
        [DisplayName("Пароль")]
        [Required]
        [EditorHtmlClass("register-user-edit")]
        [LabelHtmlClass("register-user-label")]
        public string Password { get; set; }

        /// <summary>
        /// Задает или получает копию пароля.
        /// </summary>
        [UIHint("Password")]
        [DisplayName("Пароль еще раз")]
        [Required]
        [EditorHtmlClass("register-user-edit")]
        [LabelHtmlClass("register-user-label")]
        [Compare("Password", ErrorMessage = "Пароли должны совпадать")]
        public string PasswordCopy { get; set; }

        /// <summary>
        /// Задает или получает имя.
        /// </summary>
        [DisplayName("Имя")]
        [Required]
        [EditorHtmlClass("register-user-edit")]
        [LabelHtmlClass("register-user-label")]
        public string FirstName { get; set; }

        /// <summary>
        /// Задает или получает фамилию.
        /// </summary>
        [DisplayName("Фамилия")]
        [Required]
        [EditorHtmlClass("register-user-edit")]
        [LabelHtmlClass("register-user-label")]
        public string LastName { get; set; }

        /// <summary>
        /// Задает или получает email.
        /// </summary>
        [DisplayName("Email")]
        [EditorHtmlClass("register-user-edit")]
        [LabelHtmlClass("register-user-label")]
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail ошибочного формата")]
        public string Email { get; set; }

        /// <summary>
        /// Задает или получает юрназвание фирмы.
        /// </summary>
        [DisplayName("Юридическое наименование")]
        [Required]
        [EditorHtmlClass("register-user-edit")]
        [LabelHtmlClass("register-user-label")]
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает торговое наименование фирмы.
        /// </summary>
        [DisplayName("Торговое наименование")]
        [Required]
        [EditorHtmlClass("register-user-edit")]
        [LabelHtmlClass("register-user-label")]
        public string Trademark { get; set; }

        /// <summary>
        /// Задает или получает адрес фирмы.
        /// </summary>
        [DisplayName("Адрес")]
        [Required]
        [UIHint("MultilineString")]
        [MultilineString(3, 5)]
        [EditorHtmlClass("register-user-edit")]
        [LabelHtmlClass("register-user-label")]
        public string Address { get; set; }
    }
}