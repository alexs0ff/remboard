using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.UserPublicKeyGridForm
{
    public class UserPublicKeyEditModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код ключа пользователя.
        /// </summary>
        public Guid? UserPublicKeyID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return UserPublicKeyID;
        }

        /// <summary>
        /// Задает или получает ip клиента.
        /// </summary>
        [DisplayName("IP клиента")]
        [Required]
        public string ClientIdentifier { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        [DisplayName("Заметки ключа")]
        public string KeyNotes { get; set; }

        /// <summary>
        /// Задает или получает публичный ключ.
        /// </summary>
        [DisplayName("Публичный ключ")]
        public string PublicKeyData { get; set; }

        /// <summary>
        /// Задает или получает признак отозванного ключа.
        /// </summary>
        [DisplayName("Отозванный")]
        public bool IsRevoked { get; set; }
    }
}