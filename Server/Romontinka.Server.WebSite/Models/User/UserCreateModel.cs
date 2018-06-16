using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.User
{
    /// <summary>
    /// Модель создания пользователя.
    /// </summary>
    public class UserCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает название филиала.
        /// </summary>
        [DisplayName("Логин")]
        [Required]
        [EditorHtmlClass("user-edit")]
        [LabelHtmlClass("user-label")]
        public string Login { get; set; }

        /// <summary>
        /// Задает или получает пароль.
        /// </summary>
        [UIHint("Password")]
        [DisplayName("Пароль")]
        [Required]
        [EditorHtmlClass("user-edit")]
        [LabelHtmlClass("user-label")]
        public string Password { get; set; }

        /// <summary>
        /// Задает или получает копию пароля.
        /// </summary>
        [UIHint("Password")]
        [DisplayName("Пароль еще раз")]
        [Required]
        [EditorHtmlClass("user-edit")]
        [LabelHtmlClass("user-label")]
        [Compare("Password", ErrorMessage = "Пароли должны совпадать")]
        public string PasswordCopy { get; set; }

        /// <summary>
        /// Задает или получает имя.
        /// </summary>
        [DisplayName("Имя")]
        [Required]
        [EditorHtmlClass("user-edit")]
        [LabelHtmlClass("user-label")]
        public string FirstName { get; set; }

        /// <summary>
        /// Задает или получает фамилию.
        /// </summary>
        [DisplayName("Фамилия")]
        [Required]
        [EditorHtmlClass("user-edit")]
        [LabelHtmlClass("user-label")]
        public string LastName { get; set; }

        /// <summary>
        /// Задает или получает отчество.
        /// </summary>
        [DisplayName("Отчество")]
        [EditorHtmlClass("user-edit")]
        [LabelHtmlClass("user-label")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Задает или получает телефон.
        /// </summary>
        [DisplayName("Телефон")]
        [EditorHtmlClass("user-edit")]
        [LabelHtmlClass("user-label")]
        public string Phone { get; set; }

        /// <summary>
        /// Задает или получает email.
        /// </summary>
        [DisplayName("Email")]
        [EditorHtmlClass("user-edit")]
        [LabelHtmlClass("user-label")]
        public string Email { get; set; }

        /// <summary>
        /// Задает или получает код проектной роли.
        /// </summary>
        [DisplayName("Роль в проекте")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("user-edit")]
        [LabelHtmlClass("user-label")]
        [Required]
        [AjaxComboBox("AjaxProjectRole")]
        public byte? ProjectRoleID { get; set; }

        [UIHint("AjaxCheckBoxList")]
        [DisplayName("Филиалы")]
        [AjaxCheckBoxList("BranchesCheckBoxList")]
        [EditorHtmlClass("user-edit")]
        [LabelHtmlClass("user-label")]
        [Required]
        public Guid?[] BranchIds { get; set; }
    }
}