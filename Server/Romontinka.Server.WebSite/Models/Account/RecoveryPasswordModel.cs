using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.Account
{
    /// <summary>
    /// Модель восстановления пароля.
    /// </summary>
    public class RecoveryPasswordModel
    {
        /// <summary>
        /// Задает или получает новый пароль.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Пароль {0} должен быть не меншье  {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        [EditorHtmlClass("register-user-edit")]
        [LabelHtmlClass("register-user-label")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [Compare("NewPassword", ErrorMessage = "Пароли должны быть равнозначны")]
        [EditorHtmlClass("register-user-edit")]
        [LabelHtmlClass("register-user-label")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Номер восстановления.
        /// </summary>
        public string RecoveryNumber { get; set; }
    }
}