using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace Romontinka.Server.Core
{
    /// <summary>
    /// Сервис криптографии.
    /// </summary>
    public interface ICryptoService
    {
        /// <summary>
        /// Проверяет на соответствие цифровой подписи сообщения публичным ключем с определенной кодировкой данных.
        /// </summary>
        /// <param name="publicKey">Публичный RSA ключ.</param>
        /// <param name="dataEncoding">Кодировка сообщений.</param>
        /// <param name="message">Соообщение для проверки.</param>
        /// <param name="sign">Подпись.</param>
        /// <returns>Признак успешности проверки.</returns>
        bool Verify(string publicKey, string message, string sign, Encoding dataEncoding);

        /// <summary>
        /// Создает подпись сообщению от приватного RSA ключа. 
        /// </summary>
        /// <param name="privateKey">PEM данные частного ключа.</param>
        /// <param name="password">Пароль для  </param>
        /// <param name="message">Сообщение, которое необходимо подписывать.</param>
        /// <param name="dataEncoding">Кодировка сообщений.</param>
        /// <returns>Подпись для сообщения.</returns>
        string CreateSign(string privateKey, string password, string message, Encoding dataEncoding);

        /// <summary>
        /// Создает подпись сообщению от приватного RSA ключа. 
        /// </summary>
        /// <param name="privateKey">PEM данные частного ключа.</param>
        /// <param name="password">Пароль для  </param>
        /// <param name="message">Сообщение, которое необходимо подписывать.</param>
        /// <param name="dataEncoding">Кодировка сообщений.</param>
        /// <returns>Подпись для сообщения.</returns>
        string CreateSign(string privateKey, SecureString password, string message, Encoding dataEncoding);

        /// <summary>
        /// Генерация случайного пароля.
        /// </summary>
        /// <param name="minLength">Минимальная длина.</param>
        /// <param name="maxLength">Максимальная длина.</param>
        /// <returns>Пароль.</returns>
        string GeneratePassword(int minLength, int maxLength);

        /// <summary>
        /// Генерация случайного пароля.
        /// </summary>
        /// <returns>Пароль.</returns>
        string GeneratePassword();
    }
}
