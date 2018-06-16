using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Пункт восстановления паролей.
    /// </summary>
    public class RecoveryLoginItem:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код пункта восстановления паролей.
        /// </summary>
        public Guid? RecoveryLoginID { get; set; }

        /// <summary>
        /// Задает или получает логин пользователя.
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// Задает или получает дату и время запроса на восстановление.
        /// </summary>
        public DateTime UTCEventDateTime { get; set; }

        /// <summary>
        /// Задает или получает дату восстановления.
        /// </summary>
        public DateTime UTCEventDate { get; set; }

        /// <summary>
        /// Задает или получает Email для восстановления.
        /// </summary>
        public string RecoveryEmail { get; set; }

        /// <summary>
        /// Идентификатор клиента с которого был запрос на восстановление.
        /// </summary>
        public string RecoveryClientIdentifier { get; set; }

        /// <summary>
        /// Задает или получает флаг указывающий на успешность процедуры восстановления.
        /// </summary>
        public bool IsRecovered { get; set; }

        /// <summary>
        /// Задает или получает идентификатор клиента с которого пароль успешно восстановлен.
        /// </summary>
        public string RecoveredClientIdentifier { get; set; }

        /// <summary>
        /// Задает или получает дату и время восстановления пароля.
        /// </summary> 
        public DateTime? UTCRecoveredDateTime { get; set; }

        /// <summary>
        /// Задает или получает отправленный номер.
        /// </summary>
        public string SentNumber { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (RecoveryLoginItem) entityBase;
            entity.IsRecovered = IsRecovered;
            entity.LoginName = LoginName;
            entity.RecoveredClientIdentifier = RecoveredClientIdentifier;
            entity.RecoveryClientIdentifier = RecoveryClientIdentifier;
            entity.RecoveryEmail = RecoveryEmail;
            entity.RecoveryLoginID = RecoveryLoginID;
            entity.SentNumber = SentNumber;
            entity.UTCEventDate = UTCEventDate;
            entity.UTCEventDateTime = UTCEventDateTime;
            entity.UTCRecoveredDateTime = UTCRecoveredDateTime;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return RecoveryLoginID;
        }
    }
}
