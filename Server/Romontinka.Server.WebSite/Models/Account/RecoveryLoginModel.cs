using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.Account
{
    /// <summary>
    /// Модель восстановления пароля.
    /// </summary>
    public class RecoveryLoginModel
    {
        [DisplayName("Логин для восстановления")]
        [Required]
        [EditorHtmlClass("register-user-edit")]
        [LabelHtmlClass("register-user-label")]
        public string Login { get; set; }
    }
}