using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [DisplayName("Логин")]
        [Required]
        public string Login { get; set; }

        /// <summary>
        /// Задает или получает пароль.
        /// </summary>
        [DisplayName("Пароль")]
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Задает или получает копию пароля.
        /// </summary>
        
        [DisplayName("Пароль еще раз")]
        [Required]
        [Compare("Password", ErrorMessage = "Пароли должны совпадать")]
        public string PasswordCopy { get; set; }

        /// <summary>
        /// Задает или получает имя.
        /// </summary>
        [DisplayName("Имя")]
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Задает или получает фамилию.
        /// </summary>
        [DisplayName("Фамилия")]
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Задает или получает email.
        /// </summary>
        [DisplayName("Email")]
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail ошибочного формата")]
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
    }

    /// <summary>
    /// Модель результатов активации зарегистрированного пользователя.
    /// </summary>
    public class ActivateResultModel
    {
        /// <summary>
        /// Задает или получает успех активации.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Задает или получает активированный логин.
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// Задает или получает описание ошибки.
        /// </summary>
        public string ErrorDescription { get; set; }
    }

    /// <summary>
    /// Модель восстановления пароля.
    /// </summary>
    public class RecoveryLoginModel
    {
        [DisplayName("Логин для восстановления")]
        [Required]
        public string Login { get; set; }
    }

    /// <summary>
    /// Модель для формы смены пароля.
    /// </summary>
    public class ChangePasswordModel
    {
        public SecurityToken Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Пароль {0} должен быть не меншье  {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [Compare("NewPassword", ErrorMessage = "Пароли должны быть равнозначны")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
