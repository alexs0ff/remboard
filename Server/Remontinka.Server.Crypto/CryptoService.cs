using System.Security;
using System.Text;

namespace Remontinka.Server.Crypto
{
    /// <summary>
    /// Реализация сервиса крипто операций.
    /// </summary>
    public class CryptoService
    {
        /// <summary>
        /// Генерация случайного пароля.
        /// </summary>
        /// <param name="minLength">Минимальная длина.</param>
        /// <param name="maxLength">Максимальная длина.</param>
        /// <returns>Пароль.</returns>
        public string GeneratePassword(int minLength,
                                      int maxLength)
        {
            return RandomPasswordGenerator.Generate(minLength, maxLength);
        }

        /// <summary>
        /// Генерация случайного пароля.
        /// </summary>
        /// <returns>Пароль.</returns>
        public string GeneratePassword()
        {
            return RandomPasswordGenerator.Generate();
        }

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
            using (
                var publicManager = RSAManager.RsaFactory.CreateManager(publicKey,
                                                                        SecureHelper.CreateSecureString(string.Empty), dataEncoding))
            {
                return publicManager.VerifySign(message, sign);
            }
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
            return CreateSign(privateKey, SecureHelper.CreateSecureString(password), message, dataEncoding);
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
            using (
                var publicManager = RSAManager.RsaFactory.CreateManager(privateKey,
                                                                        password,dataEncoding))
            {
                return publicManager.Sign(message);
            }
        }
    }
}