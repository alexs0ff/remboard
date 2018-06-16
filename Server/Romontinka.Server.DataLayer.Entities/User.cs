using System;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Представляет собой класс пользователя.
    /// </summary>
    public class User:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код домена пользователя.
        /// </summary>
        public Guid? UserDomainID { get; set; }

        /// <summary>
        /// Задает или получает код пользователя.
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        /// Задает или получает роль в проекте.
        /// </summary>
        public byte? ProjectRoleID { get; set; }

        /// <summary>
        /// Задает или получает логин пользователя.
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// Задает или получает хэш пароля.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Задает или получает имя пользователя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Задает или получает Фамилию пользователя.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Задает или получает отчетство пользователя.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Задает или получает телефон пользователя.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Задает или получает Email пользователя.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (User) entityBase;
            entity.Email = Email;
            entity.FirstName = FirstName;
            entity.LastName = LastName;
            entity.MiddleName = MiddleName;
            entity.LoginName = LoginName;
            entity.PasswordHash = PasswordHash;
            entity.Phone = Phone;
            entity.ProjectRoleID = ProjectRoleID;
            entity.UserID = UserID;
            entity.UserDomainID = UserDomainID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return UserID;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("{0} {1} {2}",LastName,FirstName,MiddleName);
        }
    }
}
