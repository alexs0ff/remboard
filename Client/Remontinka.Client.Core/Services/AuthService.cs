using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using Remontinka.Client.DataLayer.Entities;
using Remontinka.Server.Crypto;
using Romontinka.Server.Protocol;
using Romontinka.Server.Protocol.AuthMessages;
using log4net;

namespace Remontinka.Client.Core.Services
{
    /// <summary>
    /// Реализация сервиса авторизации.
    /// </summary>
    public class AuthService : IAuthService
    {
        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(AuthService));

        #region Registration 

        /// <summary>
        /// Стратует процесс создания пользователя и ключа.
        /// </summary>
        /// <param name="login">Логин нового пользователя.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="notes">Заметки к ключу.</param>
        public void StartUserRegistration(string login, string password,string notes)
        {
            _logger.InfoFormat("Старт процесса регистрации пользователя {0}", login);
            ThreadPool.QueueUserWorkItem(state => InternalUserRegistration(login, password, notes));
        }

        /// <summary>
        /// Содержит название файла с публичным ключем.
        /// </summary>
        private const string PublicKeyFileName = "public.pem";

        /// <summary>
        /// Содержит название файла с приватным ключем.
        /// </summary>
        private const string PrivateKeyFileName = "private.pem";

        /// <summary>
        /// Содержит кодировку ключей.
        /// </summary>
        private readonly Encoding _rsaKeyEncoding = Encoding.UTF8;

        private void InternalUserRegistration(string login, string password,string notes)
        {
            try
            {
                RiseInfoStatusChanged("Регистрация пользователя");

                var user = ClientCore.Instance.DataStore.GetUser(login);
                if (user!=null)
                {
                    if ((!string.IsNullOrWhiteSpace(user.PasswordHash)) &&(!StringComparer.Ordinal.Equals(user.PasswordHash, BcryptHash(password))))
                    {
                        RiseAuthError("Пароль не совпадает с текущим на клиенте");
                        return;
                    }
                    var userKey = ClientCore.Instance.DataStore.GetCurrentUserKey(user.UserIDGuid);
                    if (userKey!=null && userKey.IsActivatedBool)
                    {
                        _logger.InfoFormat("Пользователь с таким логином уже существует, отправка ключей невозможна {0}", login);
                        RiseAuthError("Такой пользователь уже есть");
                        return;
                    }
                }

                _logger.InfoFormat("Старт генерации ключей для пользователя {0}.",login);
                RiseInfoStatusChanged("Генерация ключей");

                var privateFilePath = Path.Combine(LocationUtils.GetFullPath(), PrivateKeyFileName);
                var publicFilePath = Path.Combine(LocationUtils.GetFullPath(), PublicKeyFileName);
                ClientCore.Instance.CryptoService.CreateRsaKeyPair(publicFilePath, privateFilePath, password);

                if (!File.Exists(privateFilePath) || !File.Exists(publicFilePath))
                {
                    throw new Exception("Ключи не создались");
                } //if

                var publicKeyData = File.ReadAllText(publicFilePath, _rsaKeyEncoding);
                var privateKeyData = File.ReadAllText(privateFilePath, _rsaKeyEncoding);

                RiseInfoStatusChanged("Отправка запроса на сервер");
                var domainId = ClientCore.Instance.DataStore.GetFirstUserDomainID();

                var request = new RegisterPublicKeyRequest();
                request.ClientUserDomainID = domainId;
                request.EventDate = DateTime.Now;
                request.KeyNotes = notes;
                request.PublicKeyData = publicKeyData;
                request.UserLogin = login;

                var response = ClientCore.Instance.WebClient.RegisterPublicKey(request);
                
                if (user==null)
                {
                    RiseInfoStatusChanged("Ответ получен, сохраняем локально пользователя");

                    user = new User();
                    user.DomainIDGuid = response.UserDomainID;
                    user.LoginName = login;

                    user.FirstName = string.Empty;
                    user.Email = string.Empty;
                    user.LastName = string.Empty;
                    user.Email = string.Empty;
                    user.ProjectRoleID = ProjectRoleSet.GetMinimumRoleID();
                    user.Phone = string.Empty;
                    user.PasswordHash = BcryptHash(password);
                    ClientCore.Instance.DataStore.SaveUser(user);
                    
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(user.PasswordHash))//при первой регистрации пользователя
                    {
                        user.PasswordHash = BcryptHash(password);
                        ClientCore.Instance.DataStore.SaveUser(user);
                    }
                }

                RiseInfoStatusChanged("Сохраняем пользовательский ключ");

                var key = new UserKey();
                key.EventDateDateTime = request.EventDate;
                key.UserIDGuid = user.UserIDGuid;
                key.Number = response.Number;
                key.PrivateKeyData = privateKeyData;
                key.PublicKeyData = publicKeyData;
                key.IsActivatedBool = false;

                ClientCore.Instance.DataStore.SaveUserKey(key);//TODO удалить пользователя, если ключ не сохранился
                RiseInfoStatusChanged("Пользователь успешно зарегистрировался");
                RiseUserRegistredEventArgs(login);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                                 string.Format("Во время регистрации ключей пользователя {0} произошла ошибка ", login));

                RiseAuthError(ex.Message,ex);
            } //try
            
        }

        /// <summary>
        /// Возникает при изменении статуса информации.
        /// </summary>
        public event EventHandler<UserRegistredEventArgs> UserRegistred;

        /// <summary>
        /// Вызывает события регистрации пользователя.
        /// </summary>
        /// <param name="loginName">Логин зарегистрировавшегося пользователя.</param>
        private void RiseUserRegistredEventArgs(string loginName)
        {
            EventHandler<UserRegistredEventArgs> handler = UserRegistred;
            if (handler != null)
            {
                handler(this, new UserRegistredEventArgs(loginName));
            }
        }

        #endregion Registration

        /// <summary>
        /// Создает подпись для данных изходя из Текущего ключа пользователя.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="encoding">Кодировка данных. </param>
        /// <returns>Результат.</returns>
        public string CreateSign(string data, Encoding encoding)
        {
            var token = AuthToken;
            if (token==null)
            {
                throw new Exception("Пользователь еще не авторизовался");
            } //if

            return ClientCore.Instance.CryptoService.CreateSign(token.PrivateKeyData, token.Password, data, encoding);
        }

        #region Authorization 

        /// <summary>
        /// Происходит во время успешной авторизации пользователя.
        /// </summary>
        public event EventHandler<AuthComplitedEventArgs> AuthComplited;

        /// <summary>
        /// Вызывает событие успешной авторизации пользователя.
        /// </summary>
        private void RiseAuthComplited()
        {
            EventHandler<AuthComplitedEventArgs> handler = AuthComplited;
            if (handler != null)
            {
                handler(this, new AuthComplitedEventArgs());
            }
        }

        /// <summary>
        /// Получает признак авторизации.
        /// </summary>
        public bool IsAuthorized
        {
            get { return AuthToken != null; }
        }

        /// <summary>
        /// Задает или получает текущий токен авторизации.
        /// </summary>
        public AuthToken AuthToken { get; private set; }

        /// <summary>
        /// Задает или получает текущий токен безопасности.
        /// </summary>
        public SecurityToken SecurityToken { get; private set; }

        /// <summary>
        /// Начинает процесс авторизации.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        public void StartAuth(string login, string password)
        {
            _logger.InfoFormat("Старт процесса авторизации для логина {0}", login);
            ThreadPool.QueueUserWorkItem(state => ProcessAuth(login, password));
        }

        /// <summary>
        /// Организует процес авторизации пользователя.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        private void ProcessAuth(string login, string password)
        {
            try
            {
                RiseInfoStatusChanged("Проверяем пользователя");
                var user = ClientCore.Instance.DataStore.GetUser(login,BcryptHash(password));
                if (user==null)
                {
                    RiseAuthError("Ошибочный пароль");
                    return;
                } //if

                var key = ClientCore.Instance.DataStore.GetCurrentUserKey(user.UserIDGuid);

                if (key==null)
                {
                    throw new Exception("Ключи не найдены");
                } //if

                if (!key.IsActivatedBool)
                {
                    RiseInfoStatusChanged("Ключи не активированы, отправляем запрос");
                    _logger.InfoFormat("Проверка на активацию ключей");
                    var response =
                        ClientCore.Instance.WebClient.ProbeKeyActivation(new ProbeKeyActivationRequest
                                                                         {KeyNumber = key.Number,UserDomainID = user.DomainIDGuid});
                    if (response.IsNotAccepted==false && response.IsExists && response.IsRevoked == false && response.IsExpired == false)
                    {
                        _logger.InfoFormat("Активируем ключ {0} для пользователя {1}", key.Number, user.UserID);
                        key.IsActivatedBool = true;
                        
                        ClientCore.Instance.DataStore.UpdateUserAndKeyID(key.UserIDGuid, response.UserID);//Обновляем код пользователя

                        key.UserIDGuid = response.UserID;
                        ClientCore.Instance.DataStore.SaveUserKey(key);
                        user.UserIDGuid = response.UserID;
                    } //if
                    else if (response.IsNotAccepted == false && response.IsExists==false && response.IsRevoked == false && response.IsExpired == false)
                    {
                        _logger.InfoFormat("Запрос на активацию {0} для пользователя {1} отсутствует на сервере", key.Number, user.UserID);
                        RiseAuthError("Запрос активации отсутствует на сервере, необходимо повторить регистрацию пользователя");
                        return;
                    }
                    else
                    {
                        _logger.ErrorFormat("Ошибка активации ключа IsExists={0} IsExpired={1} =IsNotAccepted{2} IsNotAccepted={3} IsRevoked={4}", response.IsExists, response.IsExpired, response.IsNotAccepted, response.IsNotAccepted, response.IsRevoked);

                        RiseAuthError("Ключ еще не активирован");
                        return;
                    } //else
                } //if

                AuthToken = new AuthToken();
                AuthToken.LoginName = login;
                AuthToken.Password = new SecureString();

                foreach (var ch in password)
                {
                    AuthToken.Password.AppendChar(ch);
                } //foreach

                AuthToken.PrivateKeyData = key.PrivateKeyData;
                AuthToken.UserID = user.UserIDGuid;
                AuthToken.UserDomainID = user.DomainIDGuid;
                AuthToken.KeyNumber = key.Number;
                
                SecurityToken = new SecurityToken();
                SecurityToken.User = user;
                SecurityToken.LoginName = user.LoginName;
                SecurityToken.Email = user.Email;

                _logger.InfoFormat("Пользователь {0} успешно авторизован", user.LoginName);
                RiseAuthComplited();
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                                 string.Format("Во время процесса авторизации пользователя {0} произошла ошибка ", login));
                RiseAuthError(ex.Message);
            } //try
        }

        /// <summary>
        /// Перегружает данные по текущему пользователю.
        /// </summary>
        public void ReloadCurrentUserData()
        {
            if (AuthToken!=null)
            {
                var user = ClientCore.Instance.DataStore.GetUser(AuthToken.UserID);
                if (user!=null)
                {
                    AuthToken.UserID = user.UserIDGuid;
                    AuthToken.UserDomainID = user.DomainIDGuid;
                    
                    SecurityToken.User = user;
                    SecurityToken.LoginName = user.LoginName;
                    SecurityToken.Email = user.Email;
                } //if
            } //if
        }

        #endregion Authorization

        #region BCrypt

        /// <summary>
        /// Соль из библиотеки BCrypt.
        /// </summary>
        private const string BcryptSalt = "$2a$10$7orcHri3qlj840VLcWTMc.";

        /// <summary>
        /// Хэширование по алгоритму bcrypt.
        /// </summary>
        /// <param name="password">Пароль для хэширования.</param>
        /// <returns>Возвращаемое значение.</returns>
        public string BcryptHash(string password)
        {
            return BCrypt.HashPassword(password, BcryptSalt);
        }

        #endregion BCrypt

        /// <summary>
        /// Возникает при изменении статуса информации.
        /// </summary>
        public event EventHandler<InfoEventArgs> InfoStatusChanged;

        /// <summary>
        /// Вызывает события смены статуса.
        /// </summary>
        /// <param name="text">Текст.</param>
        private void RiseInfoStatusChanged(string text)
        {
            EventHandler<InfoEventArgs> handler = InfoStatusChanged;
            if (handler != null)
            {
                handler(this, new InfoEventArgs(text));
            }
        }

        /// <summary>
        /// Возникает при ошибке авторизации или регистрации.
        /// </summary>
        public event EventHandler<ErrorEventArgs> AuthError;

        /// <summary>
        /// Вызывает событие ошибки авторизации или регистрации.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        private void RiseAuthError(string message)
        {
            EventHandler<ErrorEventArgs> handler = AuthError;
            if (handler != null)
            {
                handler(this, new ErrorEventArgs(message));
            }
        }

        /// <summary>
        /// Вызывает событие ошибки авторизации или регистрации.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="exception">Ошибка.</param>
        private void RiseAuthError(string message,Exception exception)
        {
            EventHandler<ErrorEventArgs> handler = AuthError;
            if (handler != null)
            {
                handler(this, new ErrorEventArgs(message, exception));
            }
        }


    }
}
