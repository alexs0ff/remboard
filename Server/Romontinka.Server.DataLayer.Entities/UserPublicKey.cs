using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Представляет публичный ключ пользователя.
    /// </summary>
    public class UserPublicKey : EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код ключа пользователя.
        /// </summary>
        public Guid? UserPublicKeyID { get; set; }

        /// <summary>
        /// Задает или получает код пользователя.
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        /// Задает или получает номер ключа.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Задает или получает дату ключа.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает данные публичного ключа.
        /// </summary>
        public string PublicKeyData { get; set; }

        /// <summary>
        /// Задает или получает идентификатор клиента.
        /// </summary>
        public string ClientIdentifier { get; set; }

        /// <summary>
        /// Задает или получает заметки к ключу.
        /// </summary>
        public string KeyNotes { get; set; }

        /// <summary>
        /// Задает или получает признак отозванного ключа.
        /// </summary>
        public bool IsRevoked { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (UserPublicKey) entityBase;
            entity.ClientIdentifier = ClientIdentifier;
            entity.EventDate = EventDate;
            entity.KeyNotes = KeyNotes;
            entity.Number = Number;
            entity.PublicKeyData = PublicKeyData;
            entity.UserID = UserID;
            entity.UserPublicKeyID = UserPublicKeyID;
            entity.IsRevoked = IsRevoked;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return UserPublicKeyID;
        }
    }
}
