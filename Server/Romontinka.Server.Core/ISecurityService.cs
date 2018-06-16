using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romontinka.Server.Core.Security;
using Romontinka.Server.Core.UnitOfWorks;
using Romontinka.Server.DataLayer.Entities;

namespace Romontinka.Server.Core
{
    /// <summary>
    ///   Сервис безопасности.
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        ///   Получает текущий менеджер для токенов.
        /// </summary>
        ITokenManager TokenManager { get; set; }

        /// <summary>
        ///   Аутентификация пользователя по логину и паролю.
        /// </summary>
        /// <param name="login"> Логин пользователя. </param>
        /// <param name="passwordHash"> Пароль. </param>
        /// <returns> Сущность пользователя или null. </returns>
        User Login(string login, string passwordHash);

        /// <summary>
        ///   Определяет принадлежит ли пользователь определенной роли.
        /// </summary>
        /// <param name="login"> Логин пользователя. </param>
        /// <param name="roleName"> Имя роли. </param>
        /// <returns> True если принадлежит. </returns>
        bool IsUserInRole(string login, string roleName);

        /// <summary>
        /// Регистрирует нового пользователя.
        /// </summary>
        /// <param name="unit">Юнит регистрации.</param>
        /// <returns>Результат регистрации.</returns>
        RegistrationResult Register(RegistrationUnit unit);

        /// <summary>
        /// Активирует пользователя.
        /// </summary>
        /// <param name="userDomainID">Код пользователя.</param>
        /// <param name="clientIdentifier">Идентификатор клиента.</param>
        /// <returns>Результат активации.</returns>
        ActivationResult Activate(Guid userDomainID, string clientIdentifier);

        /// <summary>
        ///   Получает список всех ролей для пользователя.
        /// </summary>
        /// <param name="login"> Логин пользователя. </param>
        /// <returns> Список ролей. </returns>
        string[] GetRolesForUser(string login);

        /// <summary>
        ///   Возвращает список всех ролей в системе.
        /// </summary>
        /// <returns> Список ролей. </returns>
        string[] GetAllRoles();

        /// <summary>
        ///   Оценка прав админа.
        /// </summary>
        /// <exception cref="SecurityException">Происходит если у пользователя нет прав администратора.</exception>
        /// <param name="token"> Маркер безопасности. </param>
        void AdminRightsEvaluate(SecurityToken token);

        /// <summary>
        ///   Получает токен безопасности ассоциированный с пользователем по его логину.
        /// </summary>
        /// <param name="loginName"> Логин пользователя. </param>
        /// <returns> Токен безопасности. </returns>
        SecurityToken GetToken(string loginName);

        /// <summary>
        ///   Оценка принадлежнасти пользователя хотя бы одной роли.
        /// </summary>
        /// <exception cref="SecurityException">Происходит если у пользователя нет ни одной роли из указанных.</exception>
        /// <param name="login"> </param>
        /// <param name="checkedRoles"> Параметры. </param>
        void RightsEvaluate(string login, params string[] checkedRoles);

        /// <summary>
        ///   Проверяет соответствует ли пользователю хоть одна роль.
        /// </summary>
        /// <param name="login"> </param>
        /// <param name="checkedRoles"> Проверяемые роли. </param>
        /// <returns> True если содержит. </returns>
        bool IsUserInRoles(string login, params string[] checkedRoles);

        /// <summary>
        ///   Создает нового пользователя с новым паролем.
        /// </summary>
        /// <param name="token"> Маркер безопасности. </param>
        /// <param name="user"> Пользователь для сохранения. </param>
        /// <remarks>
        ///   Подразумевается что в поле PasswordHash - хранится "сырой пароль"
        /// </remarks>
        void CreateUser(SecurityToken token, User user);

        /// <summary>
        ///   Обновляет информацию по пользователем.
        /// </summary>
        /// <param name="token"> Маркер безопасности. </param>
        /// <param name="user"> Пользователь для сохранения. </param>
        /// <remarks>
        ///   Полностью игнорируется поле PasswordHash
        /// </remarks>
        void UpdateUser(SecurityToken token, User user);

        /// <summary>
        ///   Удаляет пользователя из базы данных.
        /// </summary>
        /// <param name="token"> Маркер безопасности. </param>
        /// <param name="userID"> Код пользователя для удаления. </param>
        /// <remarks>
        ///   Удалять пользователей может только админ, т.к. отсутствует смысл для обычных этих делать.
        /// </remarks>
        void DeleteUser(SecurityToken token, Guid? userID);

        /// <summary>
        /// Выбрасывает исключение по отсутствию определенных прав.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="checkedRoles">Отсутствующие права.</param>
        void ThrowRightsException(string login, params string[] checkedRoles);

        /// <summary>
        /// Смена пароля для пользователя.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="oldPassword">Старый пароль.</param>
        /// <param name="newPassword">Новый пароль.</param>
        /// <returns>Признак успешности смены.</returns>
        bool ChangePassword(string login, string oldPassword, string newPassword);

        /// <summary>
        /// Создание рандомного пароля.
        /// </summary>
        /// <returns>Пароль.</returns>
        string CreatePassword();

        /// <summary>
        /// Производит отправку письма для восстановления пароля определенного логина.
        /// </summary>
        /// <param name="loginName">Логин для восстановления.</param>
        /// <param name="clientIdentifier">Идентификатор клиента. </param>
        void RecoveryLogin(string loginName,string clientIdentifier);

        /// <summary>
        /// Проверка номера для восстановления.
        /// </summary>
        /// <param name="number">Номер для восстановления.</param>
        /// <returns>Номер для восстановления.</returns>
        bool CheckRecoveryNumber(string number);

        /// <summary>
        /// Производит установку нового пароля для логина по номеру восстановления.
        /// </summary>
        /// <param name="password">Новый пароль.</param>
        /// <param name="number">Номер для восстановления.</param>
        /// <param name="clientIdentifier">Идентификатор клиента. </param>
        /// <returns>Результат восстановления.</returns>
        bool RecoveryPassword(string password, string number, string clientIdentifier);

        /// <summary>
        /// Производит хэширование.
        /// </summary>
        /// <param name="password">Пароль.</param>
        /// <returns>Созданный хэш.</returns>
        string HashPassword(string password);
    }
}
