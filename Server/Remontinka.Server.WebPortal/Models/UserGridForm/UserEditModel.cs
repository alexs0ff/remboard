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
    /// Модель для редактирования пользователя.
    /// </summary>
    public class UserEditModel : GridDataModelBase<Guid>
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
        /// Задает или получает имя.
        /// </summary>
        [DisplayName("Имя:")]
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Задает или получает фамилию.
        /// </summary>
        [DisplayName("Фамилия:")]
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Задает или получает отчество.
        /// </summary>
        [DisplayName("Отчество:")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Задает или получает телефон.
        /// </summary>
        [DisplayName("Телефон:")]
        public string Phone { get; set; }

        /// <summary>
        /// Задает или получает email.
        /// </summary>
        [DisplayName("Email:")]
        public string Email { get; set; }

        /// <summary>
        /// Задает или получает код проектной роли.
        /// </summary>
        [DisplayName("Роль в проекте:")]
        [Required]
        public byte? ProjectRoleID { get; set; }
       
        [DisplayName("Филиалы")]
        public Guid?[] BranchIds { get; set; }

        /// <summary>
        /// Содержит название свойства для выбора филиалов.
        /// </summary>
        public const string BranchIdsCheckListPropertyName = "BranchIdsUnbound";

    }
}