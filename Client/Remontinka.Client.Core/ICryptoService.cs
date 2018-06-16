using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Интерфейс доступа к криптографическим функциям.
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
        /// Создает по указанным путям файлы содержащие RSA ключи.
        /// </summary>
        /// <param name="publicPath">Путь к публичному ключу.</param>
        /// <param name="privatePath">Путь к приватному ключу.</param>
        ///  <param name="privatePassword">Пароль к приватному ключу.</param>
        void CreateRsaKeyPair(string publicPath, string privatePath, string privatePassword);

        /// <summary>
        /// Создает MD5 хэш из строки.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="encoding">Кодировка.</param>
        /// <returns>Хэш.</returns>
        string CreateMd5Hash(string data,Encoding encoding);
    }
}
