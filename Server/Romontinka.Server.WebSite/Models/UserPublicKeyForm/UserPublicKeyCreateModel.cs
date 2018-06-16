using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.UserPublicKeyForm
{
    /// <summary>
    /// Модель для редактирования публичных ключей.
    /// </summary>
    public class UserPublicKeyCreateModel : JGridDataModelBase<Guid>
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
        /// Задает или получает описание.
        /// </summary>
        [DisplayName("Заметки ключа")]
        [UIHint("MultilineString")]
        [MultilineString(3, 5)]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string KeyNotes { get; set; }

        /// <summary>
        /// Задает или получает публичный ключ.
        /// </summary>
        [DisplayName("Публичный ключ")]
        [UIHint("MultilineString")]
        [MultilineString(3, 5)]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string PublicKeyData { get; set; }

        /// <summary>
        /// Задает или получает признак отозванного ключа.
        /// </summary>
        [UIHint("Boolean")]
        [DisplayName("Отозванный")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public bool IsRevoked { get; set; }
    }
}