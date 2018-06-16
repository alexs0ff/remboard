using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Настройка домена пользователя.
    /// </summary>
    public class UserDomainSettingsItem:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код доменной настройки.
        /// </summary>
        public Guid? UserDomainSettingsID { get; set; }

        /// <summary>
        /// Задает или получает код домена.
        /// </summary>
        public Guid? UserDomainID { get; set; }

        /// <summary>
        /// Задает или получает номер настройки.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Задает или получает данные настройки.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (UserDomainSettingsItem) entityBase;
            entity.Data = Data;
            entity.Number = Number;
            entity.UserDomainID = UserDomainID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return UserDomainSettingsID;
        }
    }
}
