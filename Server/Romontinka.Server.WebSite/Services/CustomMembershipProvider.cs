using System;
using System.Web.Security;
using Romontinka.Server.Core;

namespace Romontinka.Server.WebSite.Services
{
    /// <summary>
    ///   Поставщик для интеграции с пользователями сайта.
    /// </summary>
    public class CustomMembershipProvider : MembershipProvider
    {

        /// <summary>
        ///   Добавляет нового пользователя членства в источник данных.
        /// </summary>
        /// <returns> Объект <see cref="T:System.Web.Security.MembershipUser" /> , заполненный информацией для вновь созданного пользователя. </returns>
        /// <param name="username"> Имя для нового пользователя. </param>
        /// <param name="password"> Пароль для нового пользователя. </param>
        /// <param name="email"> Адрес электронной почты для нового пользователя. </param>
        /// <param name="passwordQuestion"> Контрольный вопрос для пароля нового пользователя. </param>
        /// <param name="passwordAnswer"> Контрольный ответ для пароля нового пользователя. </param>
        /// <param name="isApproved"> Одобрена ли проверка нового пользователя. </param>
        /// <param name="providerUserKey"> Уникальный идентификатор из источника данных членства для пользователя. </param>
        /// <param name="status"> Значение перечисления <see cref="T:System.Web.Security.MembershipCreateStatus" /> , показывающее успешно ли создан пользователь. </param>
        public override MembershipUser CreateUser(string username, string password, string email,
                                                  string passwordQuestion, string passwordAnswer, bool isApproved,
                                                  object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Обрабатывает запрос на обновление контрольного вопроса и ответа при вводе пароля для пользователя членства.
        /// </summary>
        /// <returns> Значение true, если контрольный вопрос и ответ для пароля успешно обновлены; в противном случае — значение false. </returns>
        /// <param name="username"> Пользователь, для которого изменяется контрольный вопрос и ответ. </param>
        /// <param name="password"> Пароль заданного пользователя. </param>
        /// <param name="newPasswordQuestion"> Новый контрольный вопрос заданного пользователя. </param>
        /// <param name="newPasswordAnswer"> Новый контрольный ответ заданного пользователя. </param>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password,
                                                             string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Возвращает из источника данных пароль для указанного имени пользователя.
        /// </summary>
        /// <returns> Пароль для заданного имени пользователя. </returns>
        /// <param name="username"> Пользователь, для которого извлекается пароль. </param>
        /// <param name="answer"> Контрольный ответ для пароля пользователя. </param>
        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Обрабатывает запрос на обновление пароля для пользователя членства.
        /// </summary>
        /// <returns> Значение true, если пароль был успешно обновлен; в противном случае — false. </returns>
        /// <param name="username"> Пользователь, для которого обновляется пароль. </param>
        /// <param name="oldPassword"> Текущий пароль заданного пользователя. </param>
        /// <param name="newPassword"> Новый пароль заданного пользователя. </param>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return RemontinkaServer.Instance.SecurityService.ChangePassword(username, oldPassword, newPassword);
        }

        /// <summary>
        ///   Заменяет пароль пользователя на новый, автоматически сгенерированный пароль.
        /// </summary>
        /// <returns> Новый пароль заданного пользователя. </returns>
        /// <param name="username"> Пользователь, для которого сбрасывается пароль. </param>
        /// <param name="answer"> Контрольный ответ для пароля заданного пользователя. </param>
        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Обновляет информацию о пользователе в источнике данных.
        /// </summary>
        /// <param name="user"> Объект <see cref="T:System.Web.Security.MembershipUser" /> , который представляет пользователя для обновления и обновленную информацию для пользователя. </param>
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Проверяет существование указанного имени пользователя и пароля в источнике данных.
        /// </summary>
        /// <returns> Значение true, если указанные имя пользователя и пароль действительны; в противном случае — значение false. </returns>
        /// <param name="username"> Имя пользователя для проверки. </param>
        /// <param name="password"> Пароль заданного пользователя. </param>
        public override bool ValidateUser(string username, string password)
        {
            var user = RemontinkaServer.Instance.SecurityService.Login(username, password);

            return user != null;
        }

        /// <summary>
        ///   Снимает блокировку так, что пользователь членства может быть проверен.
        /// </summary>
        /// <returns> Значение true, если пользователь членства успешно разблокирован; в противном случае — значение false. </returns>
        /// <param name="userName"> Пользователь членства, для которого нужно снять блокировку. </param>
        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Возвращает информацию пользователя из источника данных, основываясь на уникальном идентификаторе для пользователя членства.Предоставляет параметр для обновления метки даты и времени последней операции пользователя.
        /// </summary>
        /// <returns> Объект <see cref="T:System.Web.Security.MembershipUser" /> , заполненный информацией конкретного пользователя из источника данных. </returns>
        /// <param name="providerUserKey"> Уникальный идентификатор пользователя членства, для которого необходимо получить информацию. </param>
        /// <param name="userIsOnline"> Значение true, чтобы обновить метку даты и времени последней операции пользователя. Значение false, чтобы вернуть информацию пользователя без обновления метки даты и времени последней операции пользователя. </param>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Возвращает информацию из источника данных для пользователя.Предоставляет параметр для обновления метки даты и времени последней операции пользователя.
        /// </summary>
        /// <returns> Объект <see cref="T:System.Web.Security.MembershipUser" /> , заполненный информацией конкретного пользователя из источника данных. </returns>
        /// <param name="username"> Имя пользователя, для которого нужно получить информацию. </param>
        /// <param name="userIsOnline"> Значение true, чтобы обновить метку даты и времени последней операции пользователя. Значение false, чтобы вернуть информацию пользователя без обновления метки даты и времени последней операции пользователя. </param>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            if (userIsOnline)
            {
                var user = RemontinkaServer.Instance.SecurityService.TokenManager.GetCurrentToken();
                return new MembershipUser(GetType().Name, username, user.LoginName, user.Email, string.Empty,
                                          string.Empty, true, false, DateTime.Today, DateTime.Now, DateTime.Now,
                                          DateTime.Now, DateTime.Now);
            } //if

            return null;
        }

        /// <summary>
        ///   Возвращает имя пользователя, связанное с указанным адресом электронной почты.
        /// </summary>
        /// <returns> Имя пользователя, связанное с указанным адресом электронной почты.Если совпадения не найдены, то возвращается значение null. </returns>
        /// <param name="email"> Адрес электронной почты для поиска. </param>
        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Удаляет пользователя из источника данных членства.
        /// </summary>
        /// <returns> Значение true, если пользователь успешно удален; в противном случае — значение false. </returns>
        /// <param name="username"> Имя пользователя для удаления. </param>
        /// <param name="deleteAllRelatedData"> Значение true, чтобы удалить связанные с пользователем данные из базы данных; значение false, чтобы оставить связанные с пользователем данные в базе данных. </param>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Возвращает коллекцию всех пользователей в источнике данных на страницах данных.
        /// </summary>
        /// <returns> Коллекция <see cref="T:System.Web.Security.MembershipUserCollection" /> , содержащая страницу, которая включает определяемое параметром <paramref
        ///    name="pageSize" /> количество объектов <see cref="T:System.Web.Security.MembershipUser" /> , начиная со страницы, заданной параметром <paramref
        ///    name="pageIndex" /> . </returns>
        /// <param name="pageIndex"> Индекс возвращаемой страницы результатов.Параметр <paramref name="pageIndex" /> отсчитывается с нуля. </param>
        /// <param name="pageSize"> Размер возвращаемой страницы результатов. </param>
        /// <param name="totalRecords"> Общее количество пользователей, для которых выявлены совпадения. </param>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Получает количество пользователей, осуществляющих текущий доступ к приложению.
        /// </summary>
        /// <returns> Количество пользователей, осуществляющих текущий доступ к приложению. </returns>
        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Возвращает коллекцию пользователей членства, у которых часть имени совпадает с указанным значением.
        /// </summary>
        /// <returns> Коллекция <see cref="T:System.Web.Security.MembershipUserCollection" /> , содержащая страницу, которая включает определяемое параметром <paramref
        ///    name="pageSize" /> количество объектов <see cref="T:System.Web.Security.MembershipUser" /> , начиная со страницы, заданной параметром <paramref
        ///    name="pageIndex" /> . </returns>
        /// <param name="usernameToMatch"> Имя пользователя для поиска. </param>
        /// <param name="pageIndex"> Индекс возвращаемой страницы результатов.Параметр <paramref name="pageIndex" /> отсчитывается с нуля. </param>
        /// <param name="pageSize"> Размер возвращаемой страницы результатов. </param>
        /// <param name="totalRecords"> Общее количество пользователей, для которых выявлены совпадения. </param>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
                                                                 out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Возвращает коллекцию пользователей членства, адрес электронной почты которых содержит часть, совпадающую с указанным значением.
        /// </summary>
        /// <returns> Коллекция <see cref="T:System.Web.Security.MembershipUserCollection" /> , содержащая страницу, которая включает определяемое параметром <paramref
        ///    name="pageSize" /> количество объектов <see cref="T:System.Web.Security.MembershipUser" /> , начиная со страницы, заданной параметром <paramref
        ///    name="pageIndex" /> . </returns>
        /// <param name="emailToMatch"> Адрес электронной почты для поиска. </param>
        /// <param name="pageIndex"> Индекс возвращаемой страницы результатов.Параметр <paramref name="pageIndex" /> отсчитывается с нуля. </param>
        /// <param name="pageSize"> Размер возвращаемой страницы результатов. </param>
        /// <param name="totalRecords"> Общее количество пользователей, для которых выявлены совпадения. </param>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
                                                                  out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Показывает, настроена ли в поставщике участия возможность извлечения пользователями собственных паролей.
        /// </summary>
        /// <returns>
        /// Значение true, если в поставщике участия настроена возможность извлечения пароля, в противном случае — значение false.Значение по умолчанию — false.
        /// </returns>
        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Показывает, настроена ли в поставщике участия возможность сброса пользователями собственных паролей.
        /// </summary>
        /// <returns>
        /// Значение true, если поставщик участия поддерживает сброс пароля; в противном случае — значение false.Значение по умолчанию — true.
        /// </returns>
        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Возвращает значение, показывающее, настроен ли поставщик участия, чтобы запрашивать у пользователя ответ на контрольный вопрос для изменения или извлечения пароля.
        /// </summary>
        /// <returns>
        /// Значение true, если требуется ответ на контрольный вопрос для изменения или извлечения пароля; в противном случае — значение false.Значение по умолчанию — true.
        /// </returns>
        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Возвращает количество попыток ввода недопустимых пароля или контрольного ответа для пароля, после которых пользователь членства блокируется.
        /// </summary>
        /// <returns>
        /// Количество попыток ввода недопустимых пароля или контрольного ответа для пароля, после которых пользователь членства блокируется.
        /// </returns>
        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Имя приложения, использующего пользовательского поставщика участия.
        /// </summary>
        /// <returns>
        /// Имя приложения, использующего пользовательского поставщика участия.
        /// </returns>
        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Возвращает количество минут, в пределах которого допускается ввод пароля или контрольного ответа для пароля. По истечение данного промежутка времени пользователь членства блокируется.
        /// </summary>
        /// <returns>
        /// Количество минут, в пределах которого допускается ввод пароля или контрольного ответа для пароля. По истечение данного промежутка времени пользователь членства блокируется.
        /// </returns>
        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Возвращает значение, показывающее, настроен ли поставщик участия, чтобы требовать уникальный адрес электронной почты для каждого имени пользователя.
        /// </summary>
        /// <returns>
        /// Значение true, если поставщик участия требует уникального адреса электронной почты; в противном случае — значение false.Значение по умолчанию — true.
        /// </returns>
        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Возвращает значение, показывающее формат хранения паролей в хранилище данных членства.
        /// </summary>
        /// <returns>
        /// Одно из значений <see cref="T:System.Web.Security.MembershipPasswordFormat"/>, показывающее формат хранения паролей в хранилище данных.
        /// </returns>
        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Возвращает минимально допустимую длину пароля.
        /// </summary>
        /// <returns>
        /// Минимально допустимая длина пароля. 
        /// </returns>
        public override int MinRequiredPasswordLength
        {
            get { return 8; }
        }

        /// <summary>
        /// Возвращает минимальное количество специальных знаков, которые должны присутствовать в допустимом пароле.
        /// </summary>
        /// <returns>
        /// Минимальное количество специальных знаков, которые должны присутствовать в допустимом пароле.
        /// </returns>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Возвращает регулярное выражение, используемое для оценки пароля.
        /// </summary>
        /// <returns>
        /// Регулярное выражение, используемое для оценки пароля.
        /// </returns>
        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }
    }
}