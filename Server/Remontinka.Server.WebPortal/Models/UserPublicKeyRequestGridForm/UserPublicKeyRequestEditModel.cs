using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.UserPublicKeyRequestGridForm
{
    public class UserPublicKeyRequestEditModel: GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код запроса пользователя.
        /// </summary>
        public Guid? UserPublicKeyRequestID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return UserPublicKeyRequestID;
        }

        /// <summary>
        /// Задает или получает ip клиента.
        /// </summary>
        [DisplayName("IP клиента")]
        [Required]
        public string ClientIdentifier { get; set; }

        /// <summary>
        /// Задает или получает логин пользователя.
        /// </summary>
        [DisplayName("Пользователь")]
        [Required]
        public string UserLogin { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        [DisplayName("Заметки ключа")]
        public string KeyNotes { get; set; }

        /// <summary>
        /// Задает или получает признак активации.
        /// </summary>
        [DisplayName("Активировать")]
        public bool IsActivated { get; set; }
    }
}