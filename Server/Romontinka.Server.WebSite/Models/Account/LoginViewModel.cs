using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Romontinka.Server.WebSite.Models.Account
{
    public class LoginViewModel
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        [Required]
        [DisplayName("Логин")]
        public string UserName { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Пароль")]
        public string Password { get; set; }

        /// <summary>
        /// Запомнить авторизацию.
        /// </summary>
        [Required]
        [DisplayName("Запомнить")]
        public bool RememberMe { get; set; }

    }
}