using System;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using Remontinka.Server.Crypto;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.Core.UnitOfWorks;
using Romontinka.Server.DataLayer.Entities;
using log4net;

namespace Romontinka.Server.WebSite.Services
{
    /// <summary>
    ///   Сервис безопасности.
    /// </summary>
    public class SecurityService : ISecurityService
    {
        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SecurityService));

        /// <summary>
        ///   Хранилище данных.
        /// </summary>
        [Dependency]
        public IDataStore DataStore { get; set; }

        /// <summary>
        /// Получает текущий менеджер для токенов.
        /// </summary>
        [Dependency]
        public ITokenManager TokenManager { get; set; }

        /// <summary>
        ///   Аутентификация пользователя по логину и паролю.
        /// </summary>
        /// <param name="login"> Логин пользователя. </param>
        /// <param name="password"> Пароль пользователя. </param>
        /// <returns> Сущность пользователя или null. </returns>
        public User Login(string login, string password)
        {
            _logger.InfoFormat("Старт авторизации пользователя {0}", login);
            
            var user = DataStore.GetUser(login);

            if (user == null)
            {
                _logger.WarnFormat("В системе нет пользователя {0}", login);
                return null;
            } //if

            var passwordHash = BcryptHash(password);

            if (!StringComparer.OrdinalIgnoreCase.Equals(passwordHash, user.PasswordHash))
            {
                _logger.WarnFormat("Пользователь {0} не прошел авторизацию c хешем {1}", login, passwordHash);
                //Не даем логина
                user = null;
            }
            else
            {
                _logger.InfoFormat("Пользователь {0} успешно авторизован", login);
            }

            return user;
        }

        /// <summary>
        /// Смена пароля для пользователя.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="oldPassword">Старый пароль.</param>
        /// <param name="newPassword">Новый пароль.</param>
        /// <returns>Признак успешности смены.</returns>
        public bool ChangePassword(string login, string oldPassword, string newPassword)
        {
            _logger.InfoFormat("Начало обновления пароля для пользователя {0}", login);

            var newPasswordHash = BcryptHash(newPassword);
            var oldPasswordHash = BcryptHash(oldPassword);

            var user = DataStore.GetUser(login, oldPasswordHash);

            if (user == null)
            {
                _logger.InfoFormat("Не удалось получить пользователя по логину {0} и старому паролю {1}", login,
                                   oldPassword);
                return false;
            } //if

            return DataStore.UpdatePasswordHash(user.UserID, newPasswordHash);
        }

        /// <summary>
        /// Создает нового пользователя с новым паролем.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="user">Пользователь для сохранения.</param>
        /// <remarks>Подразумевается что в поле PasswordHash - хранится "сырой пароль"</remarks>
        public void CreateUser(SecurityToken token, User user)
        {
            _logger.InfoFormat("Сохранение нового пользователя с логином {0} из под логина = {1}", user.LoginName,
                               token.LoginName);
            //Подразумеваем что в поле PasswordHash хранится все-таки "сырой пароль" 
            user.PasswordHash = BcryptHash(user.PasswordHash);
            
            user.UserDomainID = token.User.UserDomainID;

            EvaluateCreateOrUpdateUserRights(token, user);

            DataStore.CreateUser(user);
        }

        /// <summary>
        /// Проверяет на наличие прав по изменению и созданию новых пользователей, а также привязки к существующим участниками системы.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="user">Пользователь для сохранения или обновления.</param>
        /// <exception cref="SecurityException">Выбрасывается если текущему пользователю не хватает прав на данный вид операции.</exception>
        private void EvaluateCreateOrUpdateUserRights(SecurityToken token, User user)
        {
            
            if (!IsUserInRole(token.LoginName,UserRole.Admin))
            {
                _logger.ErrorFormat("Пользователь {0} не может создать пользователя с привязкой к MemberID {1}",
                                    token.LoginName, user.UserID);
                throw new SecurityException(
                    string.Format("Текущий пользователь не обладает достаточными правами для привязки к {0}",
                                  user.UserID));
            }
        }

        /// <summary>
        /// Обновляет информацию по пользователем.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="user">Пользователь для сохранения.</param>
        /// <remarks>Полностью игнорируется поле PasswordHash</remarks>
        public void UpdateUser(SecurityToken token, User user)
        {
            _logger.InfoFormat("Обновление существующего пользователя с логином {0} из под логина = {1}", user.LoginName,
                               token.LoginName);

            EvaluateCreateOrUpdateUserRights(token, user);
            DataStore.UpdateUser(user);
        }

        /// <summary>
        /// Удаляет пользователя из базы данных.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="userID">Код пользователя для удаления.</param>
        /// <remarks>Удалять пользователей может только админ, т.к.  отсутствует смысл для обычных этих делать.</remarks>
        public void DeleteUser(SecurityToken token, Guid? userID)
        {
            _logger.InfoFormat("Удаление пользователя {0} пользователем = {1}", userID,
                               token.LoginName);
            AdminRightsEvaluate(token);
            DataStore.DeleteUser(userID);
        }
        

        /// <summary>
        /// Определяет принадлежит ли пользователь определенной роли.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="roleName">Имя роли.</param>
        /// <returns>True если принадлежит.</returns>
        public bool IsUserInRole(string login, string roleName)
        {
            _logger.InfoFormat("Проверка соответствия пользователю роли {0} {1}", login, roleName);
            User user;

            return IsUserInRole(login, roleName, out user);
        }

        /// <summary>
        /// Определяет принадлежит ли пользователь определенной роли.
        /// </summary>
        /// <param name="login">Логин пользователя. </param>
        /// <param name="roleName">Имя интересующей роли.</param>
        /// <param name="user">Пользователь.</param>
        /// <returns>True если принадлежит.</returns>
        public bool IsUserInRole(string login, string roleName, out User user)
        {
            _logger.InfoFormat("Проверка соответствия пользователю роли {0} {1}", login, roleName);

            user = DataStore.GetUser(login);

            if (user == null)
            {
                _logger.InfoFormat("Пользователь с логином не найден {0}", login);
                return false;
            }

            if (user.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                return StringComparer.OrdinalIgnoreCase.Equals(UserRole.Admin, roleName);
            }

            if (user.ProjectRoleID == ProjectRoleSet.Manager.ProjectRoleID)
            {
                return StringComparer.OrdinalIgnoreCase.Equals(UserRole.Manager, roleName);
            }

            if (user.ProjectRoleID == ProjectRoleSet.Engineer.ProjectRoleID)
            {
                return StringComparer.OrdinalIgnoreCase.Equals(UserRole.Engineer, roleName);
            }

            return false;
        }

        /// <summary>
        /// Проверяет соответствует ли пользователю хоть одна роль.
        /// </summary>
        /// <param name="login"> Логин пользователя.</param>
        /// <param name="checkedRoles">Проверяемые роли.</param>
        /// <returns>True если содержит.</returns>
        public bool IsUserInRoles(string login, params string[] checkedRoles)
        {
            var roles = GetRolesForUser(login);

            return checkedRoles != null && roles.Intersect(checkedRoles).Any();
        }

        /// <summary>
        ///  Проверяет соответствует ли пользователю хоть одна роль.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="user">Пользователь.</param>
        /// <param name="checkedRoles">Проверяемые роли.</param>
        /// <returns>True если содержит.</returns>
        public bool IsUserInRoles(string login, out User user, params string[] checkedRoles)
        {
            var roles = GetRolesForUser(login, out user);

            return checkedRoles != null && roles.Intersect(checkedRoles).Any();
        }

        /// <summary>
        /// Получает список всех ролей для пользователя.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <returns>Список ролей.</returns>
        public string[] GetRolesForUser(string login)
        {
            User user;
            return GetRolesForUser(login, out user);
        }

        /// <summary>
        /// Получает список всех ролей для пользователя.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="user">Пользователь.</param>
        /// <returns>Список ролей.</returns>
        public string[] GetRolesForUser(string login, out User user)
        {
            //_logger.InfoFormat("Получение ролей для пользователя {0}", login);
            user = DataStore.GetUser(login);

            if (user == null)
            {
                _logger.InfoFormat("Пользователь с логином не найден {0}", login);
                return new string[0];
            }

            var result = new string[1];

            if (user.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                result[0] = UserRole.Admin;
            }

            if (user.ProjectRoleID == ProjectRoleSet.Manager.ProjectRoleID)
            {
                result[0] = UserRole.Manager;
            }

            if (user.ProjectRoleID == ProjectRoleSet.Engineer.ProjectRoleID)
            {
                result[0] = UserRole.Engineer;
            }

            return result;
        }

        /// <summary>
        /// Возвращает список всех ролей в системе.
        /// </summary>
        /// <returns>Список ролей.</returns>
        public string[] GetAllRoles()
        {
            return new[] {UserRole.Admin, UserRole.Engineer, UserRole.Manager};
        }

        /// <summary>
        /// Получает токен безопасности ассоциированный с пользователем по его логину.
        /// </summary>
        /// <param name="loginName">Логин пользователя.</param>
        /// <returns>Токен безопасности.</returns>
        public SecurityToken GetToken(string loginName)
        {

            if (string.IsNullOrWhiteSpace(loginName))
            {
                throw new Exception(string.Format("Пользователь с пустым логином существовать не может"));
            } //if

            //TODO: кеширование токенов
            var user = DataStore.GetUser(loginName);

            if (user == null)
            {
                _logger.WarnFormat("Невозможно создать токен пользователь не существует {0}", loginName);
                return null;
            } //if

            return new SecurityToken
            {
                LoginName = loginName,
                User = user,
                Email = user.Email
            };
        }

        /// <summary>
        /// Оценка прав админа.
        /// </summary>
        /// <exception cref="SecurityException">Происходит если у пользователя нет прав администратора.</exception>
        /// <param name="token">Маркер безопасности.</param>
        public void AdminRightsEvaluate(SecurityToken token)
        {
            if (!IsUserInRole(token.LoginName, UserRole.Admin))
            {
                throw new SecurityException(string.Format("Для пользователя {0} отсутствуют права {1} ", token.LoginName,
                                                          UserRole.Admin));
            } //if
        }

        /// <summary>
        /// Оценка принадлежнасти пользователя хотя бы одной роли.
        /// </summary>
        /// <exception cref="SecurityException">Происходит если у пользователя нет ни одной роли из указанных.</exception>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="checkedRoles">Параметры.</param>
        public void RightsEvaluate(string login, params string[] checkedRoles)
        {
            RightsEvaluateAndGetMember(login, checkedRoles);
        }

        /// <summary>
        /// Проверяет на соответствие права пользователя и возвращает пользователя.
        /// </summary>
        /// <exception cref="SecurityException">Происходит если у пользователя нет ни одной роли из указанных.</exception>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="checkedRoles">Роли, которые необхоимо сопоставить с данным пользователем.</param>
        /// <returns>Сопоставленый участник системы с возможностью приведения типа для "неадминских" ролей.</returns>
        public User RightsEvaluateAndGetMember(string login, params string[] checkedRoles)
        {
            User user;
            if (!IsUserInRoles(login, out user, checkedRoles))
            {
                ThrowRightsException(login, checkedRoles);
            } //if

            return user;
        }

        /// <summary>
        /// Выбрасывает исключение по отсутствию определенных прав.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="checkedRoles">Отсутствующие права.</param>
        public void ThrowRightsException(string login, params string[] checkedRoles)
        {
            throw new SecurityException(string.Format("Для пользователя {0} отсутствуют права {1} ", login,
                                                      string.Join(",", checkedRoles)));
        }

        /// <summary>
        /// Создание рандомного пароля.
        /// </summary>
        /// <returns>Пароль.</returns>
        public string CreatePassword()
        {
            return RemontinkaServer.Instance.CryptoService.GeneratePassword();
        }

        #region BCrypt

        /// <summary>
        /// Соль из библиотеки BCrypt.
        /// </summary>
        private const string BcryptSalt = "$2a$10$JgPV0GGu5/GBiJHER6Ooce";

        /// <summary>
        /// Хэширование по алгоритму bcrypt.
        /// </summary>
        /// <param name="password">Пароль для хэширования.</param>
        /// <returns>Возвращаемое значение.</returns>
        public string BcryptHash(string password)
        {
            return BCrypt.HashPassword(password, BcryptSalt);
        }

        /// <summary>
        /// Производит хэширование.
        /// </summary>
        /// <param name="password">Пароль.</param>
        /// <returns>Созданный хэш.</returns>
        public string HashPassword(string password)
        {
            return BcryptHash(password);
        }

        #endregion BCrypt

        #region Registration

        /// <summary>
        /// Содержит формат тела сообщения регистрации.
        /// </summary>
        private const string RegistrationMessageBodyFormat = @"
Здравствуйте, {0}!

Вы получили это письмо, потому что Ваш адрес электронной почты был указан при регистрации на сайте учета заказов ремонтных мастерских www.remboard.ru

Чтобы подтвердить регистрацию, перейдите по этой ссылке. 

{1}
Если ссылка не открывается, скопируйте её и вставьте в адресную строку браузера.

Подтверждение необходимо для исключения несанкционированного использования Вашего e-mail. Для подтверждения достаточно перейти по ссылке, дополнительных действий не требуется. 
";

        /// <summary>
        /// Содержит формат заголовка сообщения регистрации.
        /// </summary>
        private const string RegistrationMessageTitleFormat = "Пожалуйста, подтвердите свою регистрацию";

        /// <summary>
        /// Содержит формат utl для активации.
        /// </summary>
        private const string RegistrationActivateUrlFormat = "http://www.remboard.ru/account/activate?id={0}";

        /// <summary>
        /// Регистрирует нового пользователя.
        /// </summary>
        /// <param name="unit">Юнит регистрации.</param>
        /// <returns>Результат регистрации.</returns>
        public RegistrationResult Register(RegistrationUnit unit)
        {
            _logger.InfoFormat("Начало регистрации пользователя {0}: {1}",unit.Login,unit.ClientIdentifier);
            var result = new RegistrationResult();

            try
            {
                var domain = new UserDomain();
                domain.IsActive = false;
                domain.EventDate = DateTime.UtcNow;
                domain.LegalName = unit.LegalName;
                domain.PasswordHash = BcryptHash(unit.Password);
                domain.RegistredEmail = unit.Email;
                domain.Trademark = unit.Trademark;
                domain.Address = unit.Address;
                domain.UserLogin = unit.Login;

                RemontinkaServer.Instance.DataStore.SaveUserDomain(domain);
                _logger.InfoFormat("Завели новый домен с id {0} для пользователя {1}", domain.UserDomainID, unit.Login);

                var user = new User();
                user.Email = unit.Email;
                user.FirstName = unit.FirstName;
                user.LastName = unit.LastName;
                user.LoginName = unit.Login;
                user.PasswordHash = string.Empty;
                user.MiddleName = string.Empty;
                user.Phone = string.Empty;
                user.ProjectRoleID = ProjectRoleSet.Admin.ProjectRoleID;
                user.UserDomainID = domain.UserDomainID;

                try
                {
                    RemontinkaServer.Instance.DataStore.SaveUser(user);
                    result.Success = true;

                    var body = string.Format(RegistrationMessageBodyFormat, user.FirstName,
                                             string.Format(RegistrationActivateUrlFormat, domain.UserDomainID));
                    RemontinkaServer.Instance.MailingService.Send(unit.Email,RegistrationMessageTitleFormat,body);
                    _logger.InfoFormat("Пользователь успешно зарегистрирован {0}", unit.Login);
                }
                catch (Exception ex)
                {
                    result.Description = "Ошибка регистрации пользователя домена";
                    var innerException = string.Empty;

                    if (ex.InnerException != null)
                    {
                        innerException = ex.InnerException.Message;
                    }

                    _logger.ErrorFormat("Во время сохранения пользователя {0} для домена {1}, произошла ошибка, откатываем домен: {2}, {3} {4} {5}",
                                        unit.Login,domain.UserDomainID, ex.Message, innerException, ex.GetType(), ex.StackTrace);

                    RemontinkaServer.Instance.DataStore.DeleteUserDomain(domain.UserDomainID);
                }
            }
            catch (Exception ex)
            {
                result.Description = "Ошибка";
                var innerException = string.Empty;

                if (ex.InnerException!=null)
                {
                    innerException = ex.InnerException.Message;
                }

                _logger.ErrorFormat("Во время регистрации пользователя {0}, произошла ошибка {1}, {2} {3} {4}",
                                    unit.Login, ex.Message, innerException, ex.GetType(), ex.StackTrace);
            }

            return result;
        }

        /// <summary>
        /// Активирует пользователя.
        /// </summary>
        /// <param name="userDomainID">Код пользователя.</param>
        /// <param name="clientIdentifier">Идентификатор клиента.</param>
        /// <returns>Результат активации.</returns>
        public ActivationResult Activate(Guid userDomainID, string clientIdentifier)
        {
            var result = new ActivationResult();
            _logger.InfoFormat("Начало активации домена пользователя {0}:{1}", userDomainID, clientIdentifier);

            try
            {
                var domain = RemontinkaServer.Instance.DataStore.GetUserDomain(userDomainID);
                if (domain==null)
                {
                    _logger.ErrorFormat("Домен не найден {0}:{1}", userDomainID, clientIdentifier);
                    result.Description = "Отказано в активации";
                    return result;
                }

                if (domain.IsActive)
                {
                    _logger.InfoFormat("Домен уже активирован {0}:{1}", userDomainID, clientIdentifier);
                    result.Description = "Успех";
                    result.Success = true;
                    return result;
                    
                } //if

                RemontinkaServer.Instance.DataStore.Deploy(userDomainID);
                result.Success = true;
                result.Description = "Успех";
                result.Login = domain.UserLogin;
                _logger.InfoFormat("Домен успешно активирован {0}:{1}",userDomainID, clientIdentifier);
            }
            catch (Exception ex)
            {
                result.Description = "Ошибка";
                var innerException = string.Empty;

                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                }

                _logger.ErrorFormat("Во время активации домена пользователя {0}, произошла ошибка {1}, {2} {3} {4}",
                                    userDomainID, ex.Message, innerException, ex.GetType(), ex.StackTrace);
                
                //Если домен активировали ранее
                var domain = RemontinkaServer.Instance.DataStore.GetUserDomain(userDomainID);
                if (domain!=null && domain.IsActive)
                {
                    result.Description = "Успех";
                    result.Success = true;
                }
            }
            return result;
        }

        #endregion Registration

        #region Recovery 

        /// <summary>
        /// Содержит формат тела сообщения регистрации.
        /// </summary>
        private const string RecoveryMessageBodyFormat = @"
Здравствуйте, {0}!

Вы получили это письмо, потому что Ваш адрес электронной почты был указан при процедуре восстановления пароля на сайте учета заказов ремонтных мастерских www.remboard.ru

Чтобы подтвердить восстановление пароля, перейдите по этой ссылке. 

{1}
Если ссылка не открывается, скопируйте её и вставьте в адресную строку браузера.

Подтверждение необходимо для исключения несанкционированного изменения Вашего пароля. Для подтверждения достаточно перейти по ссылке, дополнительных действий не требуется. 
";

        /// <summary>
        /// Содержит формат заголовка сообщения регистрации.
        /// </summary>
        private const string RecoveryMessageTitleFormat = "Пожалуйста, подтвердите восстановление пароля";

        /// <summary>
        /// Содержит формат utl для активации.
        /// </summary>
        private const string RecoveryUrlFormat = "http://www.remboard.ru/account/recoverypassword?number={0}";

        /// <summary>
        /// Производит отправку письма для восстановления пароля определенного логина.
        /// </summary>
        /// <param name="loginName">Логин для восстановления.</param>
        /// <param name="clientIdentifier">Идентификатор клиента. </param>
        public void RecoveryLogin(string loginName, string clientIdentifier)
        {
            _logger.InfoFormat("Старт восстановления пользователя {0}",loginName);
            try
            {
                var user = RemontinkaServer.Instance.DataStore.GetUser(loginName);

                if (user==null)
                {
                    _logger.WarnFormat("Пользователя с логином не существует {0}", loginName);
                    return;
                } //if
                
                var number = BcryptHash(CreatePassword());

                var utc = DateTime.UtcNow;

                var item = new RecoveryLoginItem
                           {
                               IsRecovered = false,
                               LoginName = user.LoginName,
                               RecoveryClientIdentifier = clientIdentifier,
                               RecoveryEmail = user.Email,
                               SentNumber = number,
                               UTCEventDate = utc.Date,
                               UTCEventDateTime = utc
                               
                           };
                RemontinkaServer.Instance.DataStore.SaveRecoveryLoginItem(item);

                var body = string.Format(RecoveryMessageBodyFormat, user.FirstName,
                                             string.Format(RecoveryUrlFormat, HttpUtility.UrlEncode(number)));
                RemontinkaServer.Instance.MailingService.Send(user.Email, RecoveryMessageTitleFormat, body);
                _logger.InfoFormat("Письмо для подверждения смены пароля успешно отправлено {0}", loginName);

            }
            catch (Exception ex)
            {
                var innerException = string.Empty;

                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                }

                _logger.ErrorFormat("Во время восстановления пароля пользователя {0}, произошла ошибка {1}, {2} {3} {4}",
                                    loginName, ex.Message, innerException, ex.GetType(), ex.StackTrace);
            } //try
        }

        /// <summary>
        /// Проверка номера для восстановления.
        /// </summary>
        /// <param name="number">Номер для восстановления.</param>
        /// <returns>Номер для восстановления.</returns>
        public bool CheckRecoveryNumber(string number)
        {
            try
            {
                _logger.InfoFormat("Проверка номера для восстановления {0}", number);

                var item = RemontinkaServer.Instance.DataStore.GetRecoveryLoginItem(number);

                if (item != null)
                {
                    if (item.IsRecovered)
                    {
                        _logger.InfoFormat("Номер восстановления уже был использован {0}", number);
                        return false;
                    } //if

                    _logger.InfoFormat("Номер восстановления {0} привязан к логину {1}", number, item.LoginName);
                    return true;
                } //if

                _logger.WarnFormat("Номера для восстановления нет {0}", number);
                return false;
            }
            catch (Exception ex)
            {
                var innerException = string.Empty;

                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                }

                _logger.ErrorFormat(
                    "Во время проверки номера восстановления пароля {0}, произошла ошибка {1}, {2} {3} {4}",
                    number, ex.Message, innerException, ex.GetType(), ex.StackTrace);
            } //try

            return false;
        }

        /// <summary>
        /// Производит установку нового пароля для логина по номеру восстановления.
        /// </summary>
        /// <param name="password">Новый пароль.</param>
        /// <param name="number">Номер для восстановления.</param>
        /// <param name="clientIdentifier">Идентификатор клиента. </param>
        /// <returns>Результат восстановления.</returns>
        public bool RecoveryPassword(string password, string number, string clientIdentifier)
        {
            _logger.InfoFormat("Начало восстановления по номеру {0} с {1}", number, clientIdentifier);
            try
            {
                var item = RemontinkaServer.Instance.DataStore.GetRecoveryLoginItem(number);

                if (item != null)
                {
                    if (item.IsRecovered)
                    {
                        _logger.InfoFormat("Номер восстановления уже был использован {0}", number);
                        return false;
                    } //if

                    _logger.InfoFormat("Номер восстановления {0} привязан к логину {1}", number, item.LoginName);

                    var user = RemontinkaServer.Instance.DataStore.GetUser(item.LoginName);

                    if (user == null)
                    {
                        _logger.ErrorFormat("Для номера {0} не найден пользователь с логином {1}", number, item.LoginName);
                        return false;
                    } //if

                    var newPasswordHash = BcryptHash(password);

                    RemontinkaServer.Instance.DataStore.UpdatePasswordHash(user.UserID, newPasswordHash);

                    item.IsRecovered = true;
                    item.RecoveredClientIdentifier = clientIdentifier;
                    item.UTCRecoveredDateTime = DateTime.UtcNow;
                    RemontinkaServer.Instance.DataStore.SaveRecoveryLoginItem(item);

                    return true;
                } //if

                _logger.WarnFormat("Номера для восстановления нет {0}", number);
                return false;
            }
            catch (Exception ex)
            {
                var innerException = string.Empty;

                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                }

                _logger.ErrorFormat(
                    "Во время восстановления пароля по номеру {0}, произошла ошибка {1}, {2} {3} {4}",
                    number, ex.Message, innerException, ex.GetType(), ex.StackTrace);
            } //try
            return false;
        }

        #endregion Recovery
    }
}