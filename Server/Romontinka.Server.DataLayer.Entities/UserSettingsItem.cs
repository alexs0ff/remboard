using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Пункт настроек пользователей.
    /// </summary>
    public class UserSettingsItem:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код настройки пользователей.
        /// </summary>
        public Guid? UserSettingsID { get; set; }

        /// <summary>
        /// Задает или получает номер настройки.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Задает или получает логин пользователя.
        /// </summary>
        public string UserLogin { get; set; }

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
            var entity = (UserSettingsItem) entityBase;
            entity.Data = Data;
            entity.Number = Number;
            entity.UserLogin = UserLogin;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return UserSettingsID;
        }
    }
}
