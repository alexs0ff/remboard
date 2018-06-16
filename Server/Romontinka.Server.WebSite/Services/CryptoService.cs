using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Web;
using Romontinka.Server.Core;
using log4net;

namespace Romontinka.Server.WebSite.Services
{
    /// <summary>
    /// Реализация криптографического сервиса.
    /// </summary>
    public class CryptoService : ICryptoService
    {
        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CryptoService));

        private readonly Remontinka.Server.Crypto.CryptoService _service = new Remontinka.Server.Crypto.CryptoService();

        /// <summary>
        /// Проверяет на соответствие цифровой подписи сообщения публичным ключем с определенной кодировкой данных.
        /// </summary>
        /// <param name="publicKey">Публичный RSA ключ.</param>
        /// <param name="dataEncoding">Кодировка сообщений.</param>
        /// <param name="message">Соообщение для проверки.</param>
        /// <param name="sign">Подпись.</param>
        /// <returns>Признак успешности проверки.</returns>
        public bool Verify(string publicKey, string message, string sign, Encoding dataEncoding)
        {
            _logger.InfoFormat("Старт проверки сообщения публичным ключем");
            return _service.Verify(publicKey, message, sign, dataEncoding);
        }

        /// <summary>
        /// Создает подпись сообщению от приватного RSA ключа. 
        /// </summary>
        /// <param name="privateKey">PEM данные частного ключа.</param>
        /// <param name="password">Пароль для  </param>
        /// <param name="message">Сообщение, которое необходимо подписывать.</param>
        /// <param name="dataEncoding">Кодировка сообщений.</param>
        /// <returns>Подпись для сообщения.</returns>
        public string CreateSign(string privateKey, string password, string message, Encoding dataEncoding)
        {
            return _service.CreateSign(privateKey, password, message, dataEncoding);
        }

        /// <summary>
        /// Создает подпись сообщению от приватного RSA ключа. 
        /// </summary>
        /// <param name="privateKey">PEM данные частного ключа.</param>
        /// <param name="password">Пароль для  </param>
        /// <param name="message">Сообщение, которое необходимо подписывать.</param>
        /// <param name="dataEncoding">Кодировка сообщений.</param>
        /// <returns>Подпись для сообщения.</returns>
        public string CreateSign(string privateKey, SecureString password, string message, Encoding dataEncoding)
        {
            _logger.InfoFormat("Старт создания подписи для сообщения длиной {0}", message.Length);
            return _service.CreateSign(privateKey, password, message, dataEncoding);
        }

        /// <summary>
        /// Генерация случайного пароля.
        /// </summary>
        /// <param name="minLength">Минимальная длина.</param>
        /// <param name="maxLength">Максимальная длина.</param>
        /// <returns>Пароль.</returns>
        public string GeneratePassword(int minLength, int maxLength)
        {
            return _service.GeneratePassword(minLength, maxLength);
        }

        /// <summary>
        /// Генерация случайного пароля.
        /// </summary>
        /// <returns>Пароль.</returns>
        public string GeneratePassword()
        {
            return _service.GeneratePassword();
        }
    }
}