using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.UserGridForm
{
    /// <summary>
    /// Модель создания пользователя.
    /// </summary>
    public class UserCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код пользователя.
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return UserID;
        }

        /// <summary>
        /// Задает или получает название филиала.
        /// </summary>
        [DisplayName("Логин")]
        [Required]
        public string UserLoginName { get; set; }

        /// <summary>
        /// Задает или получает пароль.
        /// </summary>
        [DisplayName("Пароль")]
        [Required]
        public string LoginPassword { get; set; }

        /// <summary>
        /// Задает или получает копию пароля.
        /// </summary>
        [DisplayName("Пароль еще раз")]
        [Required]
        [Compare("LoginPassword", ErrorMessage = "Пароли должны совпадать")]
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
        /// Задает или получает отчество.
        /// </summary>
        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Задает или получает телефон.
        /// </summary>
        [DisplayName("Телефон")]
        [Mask("+0(000) 000-0000", IncludeLiterals = MaskIncludeLiteralsMode.None, ErrorMessage = "Ошибочный телефонный номер")]
        public string Phone { get; set; }

        /// <summary>
        /// Задает или получает email.
        /// </summary>
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Ошибочный email")]
        public string Email { get; set; }

        /// <summary>
        /// Задает или получает код проектной роли.
        /// </summary>
        [DisplayName("Роль в проекте")]
        [Required]
        public byte? ProjectRoleID { get; set; }

       
        [DisplayName("Филиалы")]
        public Guid?[] BranchIds { get; set; }
    }
}