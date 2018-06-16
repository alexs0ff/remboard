using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Представляет сущность запроса на добавление публичного ключа пользователю.
    /// </summary>
    public class UserPublicKeyRequest : EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код запроса пользователя.
        /// </summary>
        public Guid? UserPublicKeyRequestID { get; set; }

        /// <summary>
        /// Задает или получает код пользователя.
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        /// Задает или получает дату запроса.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает данные по ключу запроса.
        /// </summary>
        public string PublicKeyData { get; set; }

        /// <summary>
        /// Задает или получает номер ключа.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Задает или получает заметки по ключу.
        /// </summary>
        public string KeyNotes { get; set; }

        /// <summary>
        /// Задает или получает идентификатор клиента.
        /// </summary>
        public string ClientIdentifier { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (UserPublicKeyRequest) entityBase;
            entity.ClientIdentifier = ClientIdentifier;
            entity.EventDate = EventDate;
            entity.KeyNotes = KeyNotes;
            entity.Number = Number;
            entity.PublicKeyData = PublicKeyData;
            entity.UserID = UserID;
            entity.UserPublicKeyRequestID = UserPublicKeyRequestID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return UserPublicKeyRequestID;
        }
    }
}
