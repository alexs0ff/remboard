using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Сревис авторизации.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Стратует процесс создания пользователя и ключа.
        /// </summary>
        /// <param name="login">Логин нового пользователя.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="notes">Заметки к ключу.</param>
        void StartUserRegistration(string login, string password,string notes);

        /// <summary>
        /// Возникает при изменении статуса информации.
        /// </summary>
        event EventHandler<InfoEventArgs> InfoStatusChanged;

        /// <summary>
        /// Возникает при ошибке авторизации или регистрации.
        /// </summary>
        event EventHandler<ErrorEventArgs> AuthError;

        /// <summary>
        /// Возникает при изменении статуса информации.
        /// </summary>
        event EventHandler<UserRegistredEventArgs> UserRegistred;

        /// <summary>
        /// Происходит во время успешной авторизации пользователя.
        /// </summary>
        event EventHandler<AuthComplitedEventArgs> AuthComplited;

        /// <summary>
        /// Получает признак авторизации.
        /// </summary>
        bool IsAuthorized { get; }

        /// <summary>
        /// Задает или получает текущий токен авторизации.
        /// </summary>
        AuthToken AuthToken { get; }

        /// <summary>
        /// Задает или получает текущий токен безопасности.
        /// </summary>
        SecurityToken SecurityToken { get; }

        /// <summary>
        /// Начинает процесс авторизации.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        void StartAuth(string login, string password);

        /// <summary>
        /// Создает подпись для данных изходя из Текущего ключа пользователя.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="encoding">Кодировка данных. </param>
        /// <returns>Результат.</returns>
        string CreateSign(string data, Encoding encoding);

        /// <summary>
        /// Перегружает данные по текущему пользователю.
        /// </summary>
        void ReloadCurrentUserData();
    }
}
