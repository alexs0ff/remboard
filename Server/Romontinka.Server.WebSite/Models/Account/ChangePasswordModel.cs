using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Romontinka.Server.WebSite.Models.Account
{
    /// <summary>
    /// Модель для формы смены пароля.
    /// </summary>
    public class ChangePasswordModel
    {
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
}