using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Remontinka.Server.Crypto;
using log4net;

namespace Remontinka.Client.Core.Services
{
    /// <summary>
    /// Реализация криптографического сервиса.
    /// </summary>
    public class CryptoService:ICryptoService
    {
        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CryptoService));

        private readonly Remontinka.Server.Crypto.CryptoService _service = new Server.Crypto.CryptoService();

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
        /// Создает по указанным путям файлы содержащие RSA ключи.
        /// </summary>
        /// <param name="publicPath">Путь к публичному ключу.</param>
        /// <param name="privatePath">Путь к приватному ключу.</param>
        ///  <param name="privatePassword">Пароль к приватному ключу.</param>
        public void CreateRsaKeyPair(string publicPath, string privatePath, string privatePassword)
        {
            _logger.InfoFormat("Начало создания файлов публичного \"{0}\" и приватного ключей",publicPath);
            var manager = new OpenSslManager();

            manager.GenerateRSAKeys(LocationUtils.GetFullPath(), publicPath, privatePath, privatePassword);
        }

        /// <summary>
        /// Создает MD5 хэш из строки.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="encoding">Кодировка.</param>
        /// <returns>Хэш.</returns>
        public string CreateMd5Hash(string data,Encoding encoding)
        {
            byte[] inputBytes = encoding.GetBytes(data);
            StringBuilder sb = new StringBuilder();
            using (MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hash = md5.ComputeHash(inputBytes);

                // step 2, convert byte array to hex string

                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }    
            } //using;
            
            return sb.ToString();
        }
    }
}
