using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Домен пользователей.
    /// </summary>
    public class UserDomain:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код домена пользователя.
        /// </summary>
        public Guid? UserDomainID { get; set; }

        /// <summary>
        /// Задает или получает дату регистрации.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает email регистрации.
        /// </summary>
        public string RegistredEmail { get; set; }

        /// <summary>
        /// Задает или получает код активации.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Задает или получает юрназвание фирмы.
        /// </summary>
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает торговую марку фирмы.
        /// </summary>
        public string Trademark { get; set; }

        /// <summary>
        /// Задает или получает адрес фирмы.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Задает или получает логин пользователя.
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        /// Задает или получает номер домена.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Задает или получает хэш пароля.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (UserDomain) entityBase;
            entity.EventDate = EventDate;
            entity.IsActive = IsActive;
            entity.LegalName = LegalName;
            entity.RegistredEmail = RegistredEmail;
            entity.Trademark = Trademark;
            entity.UserLogin = UserLogin;
            entity.UserDomainID = UserDomainID;
            entity.PasswordHash = PasswordHash;
            entity.Address = Address;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return UserDomainID;
        }
    }
}
