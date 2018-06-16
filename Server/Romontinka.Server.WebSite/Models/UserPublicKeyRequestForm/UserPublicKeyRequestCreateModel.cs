using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.UserPublicKeyRequestForm
{
    /// <summary>
    /// Модель для редактиврования запросов регитрации публичных ключей.
    /// </summary>
    public class UserPublicKeyRequestCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает ip клиента.
        /// </summary>
        [DisplayName("IP клиента")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string ClientIdentifier { get; set; }

        /// <summary>
        /// Задает или получает логин пользователя.
        /// </summary>
        [DisplayName("Пользователь")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string UserLogin { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        [DisplayName("Заметки ключа")]
        [UIHint("MultilineString")]
        [MultilineString(3, 5)]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string KeyNotes { get; set; }

        /// <summary>
        /// Задает или получает признак активации.
        /// </summary>
        [UIHint("Boolean")]
        [DisplayName("Активировать")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public bool IsActivated { get; set; }
    }
}