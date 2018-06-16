using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using Remontinka.Client.Core;
using Remontinka.Client.DataLayer.Entities;
using log4net;

namespace Remontinka.Client.DataLayer.EntityFramework
{
    public class RemontinkaStore : IDataStore
    {
        #region Deploy 

        /// <summary>
        /// Строка подключения к базе данных.
        /// </summary>
        private const string SQLiteConnection = "Data Source={0};Version=3";

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public const string SQLiteDataBase = "data.db";

        /// <summary>
        /// Возвращает файл базы данных.
        /// </summary>
        /// <returns>Полный путь к файлу базы данных.</returns>
        private static string GetDatabaseFilePath()
        {
            return Path.Combine(Path.GetDirectoryName(typeof(RemontinkaStore).Assembly.Location), SQLiteDataBase);
        }

        /// <summary>
        /// Разворачивает файл базы, если его несуществует по определенному пути.
        /// </summary>
        public void DeployIfNeeded()
        {
            var path = GetDatabaseFilePath();

            if (!File.Exists(path))
            {
                var reader = new EmbeddedResourceReader("data.db", typeof(RemontinkaStore), "Resources");

                using (Stream input = reader.ReadStream())
                {
                    using (Stream output = File.Create(path))
                    {
                        StreamUtils.CopyStream(input, output);
                    }
                }
            } //if

            InitVersions();
        }
         /// <summary>
        /// Инициализация версий.
        /// </summary>
        private void InitVersions()
        {
            long version = GetCurrentVersion()??1;
            switch (version)
            {
                case 1:
                    //InitVersion2();
                   
                    break;
                case 2:
                    break;
            }
        }

        private const string SelectDbVersionSql = "SELECT Version FROM SETTINGS";

        /// <summary>
        /// Получает текущую версию базы данных.
        /// </summary>
        /// <returns></returns>
        private long? GetCurrentVersion()
        {
            var context = CreateContext();

            return context.ExecuteStoreQuery<long>(SelectDbVersionSql).FirstOrDefault();
        }

        #endregion Deploy

        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(RemontinkaStore));

        /// <summary>
        ///   Создает и инициализирует контекст данных.
        /// </summary>
        /// <returns> Созданный контекст. </returns>
        protected DatabaseContext CreateContext()
        {
            var entityBuilder = new EntityConnectionStringBuilder();

            entityBuilder.Provider = RemontinkaDataLayerConfiguration.Settings.EFProviderName;
            entityBuilder.ProviderConnectionString = RemontinkaDataLayerConfiguration.Settings.EFConnectionString;
            entityBuilder.Metadata = RemontinkaDataLayerConfiguration.Settings.EFMetadata;

            return new DatabaseContext(entityBuilder.ToString());
        }

        /// <summary>
        /// Нормальзация строки для поиска во вхождение.
        /// </summary>
        /// <param name="value">Значение для нормализации.</param>
        protected void NormilizeString(ref string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }
        }

        /// <summary>
        /// Создание тестового представления из строки.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Текстовое представление.</returns>
        protected string CreateTextGuid(Guid? value)
        {
            var result = (string) null;

            if (value.HasValue)
            {
                result = value.Value.ToString();
            } //if

            return result;
        }

        #region DocNumbers

        /// <summary>
        /// Инкрементирует и возвращает следующий номер документа для АРМа.
        /// </summary>
        /// <returns>Инкрементнутый номер документа.</returns>
        public int GetNextDocNumber()
        {
            _logger.InfoFormat("Инкрементируем и возвращаем номер документа");
            var context = CreateContext();
            context.ExecuteStoreCommand("UPDATE DocSequence SET CurrentNumber = CurrentNumber+1");

            var current = context.ExecuteStoreQuery<int>("SELECT CurrentNumber FROM DocSequence").FirstOrDefault();

            return current;
        }

        #endregion DocNumbers

        #region User

        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        /// <returns>Список пользователей.</returns>
        public IEnumerable<User> GetUsers()
        {
            _logger.InfoFormat("Получение списка всех пользователей с ролью");
            var context = CreateContext();
            return context.Users;
        }

        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        /// <param name="projectRoleId">Код роли в проекте. </param>
        /// <returns>Список пользователей.</returns>
        public IEnumerable<User> GetUsers(long? projectRoleId)
        {
            _logger.InfoFormat("Получение списка всех пользователей с ролью {0} ", projectRoleId);
            var context = CreateContext();
            IEnumerable<User> result;

            if (projectRoleId != null)
            {
                result = context.Users.Where(u => u.ProjectRoleID == projectRoleId);
            }
            else
            {
                result = new User[0];    
            } //else
            return result;
        }

        /// <summary>
        /// Получает список пользователей с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список пользователей.</returns>
        public IEnumerable<User> GetUsers(Guid? userDomainID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка пользователей со строкой поиска {0} страница {1} и домена {2}", name, page, userDomainID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items =
                context.Users.Where(
                    i => (i.LastName + i.LastName + i.FirstName).Contains(name) && i.UserIDGuid == userDomainID);
            count = items.Count();

            return items.OrderBy(i => i.FirstName).Page(page, pageSize);
        }

        /// <summary>
        /// Получение пользователя по логину и хэшу пароля.
        /// </summary>
        /// <param name="loginName">Логин пользователя.</param>
        /// <param name="passwordHash">Хэш пароля.</param>
        /// <returns>Пользователь, если найден.</returns>
        public User GetUser(string loginName, string passwordHash)
        {
            _logger.InfoFormat("Получение пользователя по логину {0} и хэшу {1}", loginName, passwordHash);
            var context = CreateContext();
            
            return context.Users.FirstOrDefault(u => u.LoginName == loginName && u.PasswordHash == passwordHash);
        }

        /// <summary>
        /// Получение пользователя по логину.
        /// </summary>
        /// <param name="loginName">Логин пользователя.</param>
        /// <returns>Пользователь, если найден.</returns>
        public User GetUser(string loginName)
        {
            _logger.InfoFormat("Получение пользователя по логину {0}", loginName);
            var context = CreateContext();

            return context.Users.FirstOrDefault(u => u.LoginName == loginName);
        }

        /// <summary>
        /// Скрипт обновления пароля.
        /// </summary>
        private const string UserPasswordHashUpdate = "UPDATE [User] SET [PasswordHash] = {0} WHERE [UserID] = {1}";

        /// <summary>
        /// Обновляет хеш пароля на указанном пользователе.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <param name="newPasswordHash">Новый хэш пароля.</param>
        /// <returns>Признак успешности смены</returns>
        public bool UpdatePasswordHash(Guid? userID, string newPasswordHash)
        {
            _logger.InfoFormat("Обновление хэша пароля у пользователя {0}", userID);

            var context = CreateContext();
            return context.ExecuteStoreCommand(UserPasswordHashUpdate, newPasswordHash, userID) == 1;
        }

        /// <summary>
        /// Создает в системе нового пользователя.
        /// </summary>
        public void CreateUser(User user)
        {
            _logger.InfoFormat("Создание пользователя Id = {0} login = {1}", user.UserID, user.LoginName);
            var context = CreateContext();

            //Создаем ID
            user.UserIDGuid = Guid.NewGuid();

            context.Users.AddObject(user);
            context.SaveChanges(SaveOptions.None);
            _logger.InfoFormat("Пользователь Id = {0} login = {1} успешно сохранен", user.UserID, user.LoginName);
        }

        /// <summary>
        /// Обновление информации по существующему пользователю.
        /// Сохраняет все поля кроме хэша пароля.
        /// </summary>
        /// <param name="user">Пользователь для обновления информации.</param>
        public void UpdateUser(User user)
        {
            _logger.InfoFormat("Обновление пользователя Id = {0} login = {1} (пароль не сохраняем)", user.UserID, user.LoginName);

            var context = CreateContext();
            var savedItem = context.Users.FirstOrDefault(se => se.UserID == user.UserID);

            if (savedItem == null)
            {
                _logger.WarnFormat("Пользователь Id={0} Не найден, пропуск обновления полей", user.UserID);
                return;
            }

            var hash = savedItem.PasswordHash;
            user.CopyTo(savedItem);
            savedItem.PasswordHash = hash;
            context.SaveChanges();

            _logger.InfoFormat("Пользователь Id = {0} login = {1} успешно обновлен", user.UserID, user.LoginName);
        }

        /// <summary>
        ///   Сохраняет информацию пользователе.
        /// </summary>
        /// <param name="user"> Сохраняемый пользователь. </param>
        public void SaveUser(User user)
        {
            _logger.InfoFormat("Сохранение пользователя с Id = {0}", user.UserID);
            var context = CreateContext();

            var savedItem =
                context.Users.FirstOrDefault(di => di.UserID == user.UserID);

            if (user.UserID == null || user.UserIDGuid == Guid.Empty)
            {
                user.UserIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                user.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.Users.AddObject(user);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Пользователь с ID= {0} успешно сохранен",
                               user.UserID);
        }

        /// <summary>
        /// Получает пользователя по его ID.
        /// </summary>
        /// <param name="id">Код пользователя.</param>
        /// <returns>Пользователь, если существует.</returns>
        public User GetUser(Guid? id)
        {
            _logger.InfoFormat("Получение пользователя по Id = {0} для домена", id);
            var context = CreateContext();
            var rawid = CreateTextGuid(id);
            return context.Users.FirstOrDefault(fs => fs.UserID == rawid);
        }

        /// <summary>
        /// Удаляет из хранилища пользователя по его ID.
        /// </summary>
        /// <param name="id">Код пользователя.</param>
        public void DeleteUser(Guid? id)
        {
            _logger.InfoFormat("Удаление пользователя id ={0}", id);

            var context = CreateContext();
            var item = new User { UserIDGuid = id };
            context.Users.Attach(item);
            context.Users.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Пользователь id = {0} успешно удален", id);
        }

        /// <summary>
        /// Проверяет наличие логина в системе.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <returns>Наличие логина.</returns>
        public bool UserLoginExists(string login)
        {
            _logger.InfoFormat("Проверка наличия логина в системе. {0}", login);

            if (string.IsNullOrWhiteSpace(login))
            {
                return false;
            }

            var context = CreateContext();
            login = login.ToUpper();
            return context.Users.Any(u => u.LoginName.ToUpper() == login);
        }

        /// <summary>
        /// Получает первый попавшийся код домена пользователя или null.
        /// </summary>
        /// <returns>Код домена пользователя или null.</returns>
        public Guid? GetFirstUserDomainID()
        {
            _logger.InfoFormat("Получаем код домена первого пользователя");

            var context = CreateContext();

            var user = context.Users.FirstOrDefault();
            if (user==null)
            {
                return null;
            } //if

            return user.DomainIDGuid;
        }

        private const string UpdateUserIDSql = "UPDATE User SET UserID = {0} WHERE UserID = {1}";

        private const string UpdateUserKeyIDSql = "UPDATE UserKey SET UserID = {0} WHERE UserID = {1}";

        /// <summary>
        /// Обновляет код пользователя, а также связанные с ним ключи.
        /// </summary>
        /// <param name="currentUserID">Текущие код пользователя.</param>
        /// <param name="newUserID">Новый код пользователя.</param>
        public void UpdateUserAndKeyID(Guid? currentUserID, Guid? newUserID)
        {
            _logger.InfoFormat("Заменяем код пользователя с {0} на {1}",currentUserID,newUserID);
            if (currentUserID==newUserID)
            {
                return;
            } //if
            var context = CreateContext();
            context.ExecuteStoreCommand(UpdateUserIDSql, FormatUtils.GuidToString(newUserID),
                                        FormatUtils.GuidToString(currentUserID));

            context.ExecuteStoreCommand(UpdateUserKeyIDSql, FormatUtils.GuidToString(newUserID),
                                        FormatUtils.GuidToString(currentUserID));
        }

        #endregion User

        #region UserKey

        /// <summary>
        /// Получает текущий пользовательский ключ.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <returns>Ключи или null.</returns>
        public UserKey GetCurrentUserKey(Guid? userID)
        {
            _logger.InfoFormat("Получение текущего ключа для пользователя {0}",userID);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(userID);
            return context.UserKeys.Where(u => u.UserID == rawId).OrderByDescending(u => u.EventDate).FirstOrDefault();
        }

        /// <summary>
        ///   Сохраняет информацию ключе пользователя.
        /// </summary>
        /// <param name="userKey"> Сохраняемый ключ пользователя. </param>
        public void SaveUserKey(UserKey userKey)
        {
            _logger.InfoFormat("Сохранение ключа пользователя с Id = {0}", userKey.UserKeyID);
            var context = CreateContext();

            var savedItem =
                context.UserKeys.FirstOrDefault(di => di.UserKeyID == userKey.UserKeyID);

            if (userKey.UserKeyID == null || userKey.UserKeyIDGuid == Guid.Empty)
            {
                userKey.UserKeyIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                userKey.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.UserKeys.AddObject(userKey);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Пользовательский ключ с ID= {0} успешно сохранен",
                               userKey.UserKeyID);
        }

        /// <summary>
        /// Получает ключ пользователя по его ID.
        /// </summary>
        /// <param name="id">Код ключа пользователя.</param>
        /// <returns>Пользователь, если существует.</returns>
        public UserKey GetUserKey(Guid? id)
        {
            _logger.InfoFormat("Получение пользователя по Id = {0} для домена", id);
            var context = CreateContext();
            var rawid = CreateTextGuid(id);
            return context.UserKeys.FirstOrDefault(fs => fs.UserKeyID == rawid);
        }

        /// <summary>
        /// Удаляет из хранилища ключ пользователя по его ID.
        /// </summary>
        /// <param name="id">Код пользователя.</param>
        public void DeleteUserKey(Guid? id)
        {
            _logger.InfoFormat("Удаление ключа пользователя id ={0}", id);

            var context = CreateContext();
            var item = new UserKey { UserKeyIDGuid = id };
            context.UserKeys.Attach(item);
            context.UserKeys.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Ключ пользователя id = {0} успешно удален", id);
        }

        #endregion UserKey

        #region SyncOperation

        /// <summary>
        /// Проверяет наличие операций синхронизации в базе.
        /// </summary>
        /// <returns>Признак наличия успешных операций.</returns>
        public bool SyncOperationExists()
        {
            _logger.InfoFormat("Проверяем наличие операций синхронизации");
            var context = CreateContext();
            var success = FormatUtils.BoolToLong(true);
            return context.SyncOperations.Any(s => s.IsSuccess == success);
        }

        /// <summary>
        ///   Сохраняет информацию об операции синхронизации.
        /// </summary>
        /// <param name="syncOperation"> Сохраняемая операция. </param>
        public void SaveSyncOperation(SyncOperation syncOperation)
        {
            _logger.InfoFormat("Сохранение операции синхронизации с Id = {0}", syncOperation.SyncOperationID);
            var context = CreateContext();

            var savedItem =
                context.SyncOperations.FirstOrDefault(di => di.SyncOperationID == syncOperation.SyncOperationID);

            if (syncOperation.SyncOperationID == null || syncOperation.SyncOperationIDGuid == Guid.Empty)
            {
                syncOperation.SyncOperationIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                syncOperation.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.SyncOperations.AddObject(syncOperation);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Операция синхронизации с ID= {0} успешно сохранена",
                               syncOperation.SyncOperationID);
        }

        /// <summary>
        /// Получает операцию синхронизации по ее ID.
        /// </summary>
        /// <param name="id">Код ключа пользователя.</param>
        /// <returns>Пользователь, если существует.</returns>
        public SyncOperation GetSyncOperation(Guid? id)
        {
            _logger.InfoFormat("Получение операции синхронизации по Id = {0} для домена", id);
            var context = CreateContext();
            var rawid = CreateTextGuid(id);
            return context.SyncOperations.FirstOrDefault(fs => fs.SyncOperationID == rawid);
        }

        /// <summary>
        /// Удаляет из хранилища операцию синхронизации по ее ID.
        /// </summary>
        /// <param name="id">Код операции синзронизации.</param>
        public void DeleteSyncOperation(Guid? id)
        {
            _logger.InfoFormat("Удаление операции синхронизации id ={0}", id);

            var context = CreateContext();
            var item = new SyncOperation { SyncOperationIDGuid = id };
            context.SyncOperations.Attach(item);
            context.SyncOperations.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Операция синхронизации с id = {0} успешно удалена", id);
        }

        #endregion SyncOperation

        #region Branch

        /// <summary>
        /// Получает список филиалов без фильтра.
        /// </summary>
        /// <returns>Список филиалов.</returns>
        public IEnumerable<Branch> GetBranches()
        {
            _logger.InfoFormat("Получение списка филиалов без строки поиска ");
            var context = CreateContext();
            return context.Branches;
        }

        /// <summary>
        ///   Сохраняет информацию филиале.
        /// </summary>
        /// <param name="branch"> Сохраняемый филиал. </param>
        public void SaveBranch(Branch branch)
        {
            _logger.InfoFormat("Сохранение филиала с Id = {0}", branch.BranchID);
            var context = CreateContext();

            var savedItem =
                context.Branches.FirstOrDefault(
                    di => di.BranchID == branch.BranchID);

            if (branch.BranchIDGuid == null || branch.BranchIDGuid == Guid.Empty)
            {
                branch.BranchIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                branch.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.Branches.AddObject(branch);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Филиал с ID= {0} успешно сохранен",
                               branch.BranchID);
        }

        /// <summary>
        /// Получает филиал по его ID.
        /// </summary>
        /// <param name="id">Код описания филиала.</param>
        /// <returns>Филиал, если существует.</returns>
        public Branch GetBranch(Guid? id)
        {
            _logger.InfoFormat("Получение филиала по Id = {0}", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            return context.Branches.FirstOrDefault(fs => fs.BranchID == rawId);
        }

        /// <summary>
        /// Удаляет из хранилища филиал по его ID.
        /// </summary>
        /// <param name="id">Код филиала.</param>
        public void DeleteBranch(Guid? id)
        {
            _logger.InfoFormat("Удаление филиала id ={0}", id);

            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            var item = new Branch { BranchID = rawId };
            context.Branches.Attach(item);
            context.Branches.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Филиал id = {0} успешно удален", id);
        }

        private const string DeleteAllBranchesSql = "DELETE FROM Branch";

        /// <summary>
        /// Удаляет все привязки филиалов и пользователей.
        /// </summary>
        public void DeleteAllBranches()
        {
            _logger.InfoFormat("Удаление всех филиалов");

            var context = CreateContext();
            context.ExecuteStoreCommand(DeleteAllBranchesSql);
        }

        #endregion Branch

        #region UserBranchMapItem

        private const string DeleteAllUserBranchMapItemsSql = "DELETE FROM UserBranchMap";

        /// <summary>
        /// Удаляет все привязки филиалов и пользователей.
        /// </summary>
        public void DeleteAllUserBranchMapItems()
        {
            _logger.InfoFormat("Удаление всех связок пользователей и филиалов");
            
            var context = CreateContext();
            context.ExecuteStoreCommand(DeleteAllUserBranchMapItemsSql);
        }

        /// <summary>
        ///   Сохраняет информацию о соответствии пользователя и филиала.
        /// </summary>
        /// <param name="userBranchMapItem"> Сохраняемое соответствие. </param>
        public void SaveUserBranchMapItem(UserBranchMapItem userBranchMapItem)
        {
            _logger.InfoFormat("Сохранение соответствия с Id = {0} между пользователем {1} И филиалом {2}", userBranchMapItem.UserBranchMapID, userBranchMapItem.UserID, userBranchMapItem.BranchID);
            var context = CreateContext();

            var savedItem =
                context.UserBranchMapItems.FirstOrDefault(
                    di => di.UserBranchMapID == userBranchMapItem.UserBranchMapID);

            if (userBranchMapItem.UserBranchMapIDGuid == null || userBranchMapItem.UserBranchMapIDGuid == Guid.Empty)
            {
                userBranchMapItem.UserBranchMapIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                userBranchMapItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.UserBranchMapItems.AddObject(userBranchMapItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Соответствие пользователя и  с ID= {0} успешно сохранено  между пользователем {1} И филиалом {2}", userBranchMapItem.UserBranchMapID, userBranchMapItem.UserID, userBranchMapItem.BranchID);
        }


        /// <summary>
        /// Получает соответствие пользователя и филиала по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        /// <returns>Соостветствие, если существует.</returns>
        public UserBranchMapItem GetUserBranchMapItem(Guid? id)
        {
            _logger.InfoFormat("Получение соответствия пользователя и филиала по Id = {0}", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            return context.UserBranchMapItems.FirstOrDefault(fs => fs.UserBranchMapID == rawId);
        }

        /// <summary>
        /// Удаляет из хранилища соответствие между пользователем и филиалом по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        public void DeleteUserBranchMapItem(Guid? id)
        {
            _logger.InfoFormat("Удаление соответствия id ={0}", id);

            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            var item = new UserBranchMapItem { UserBranchMapID = rawId };
            context.UserBranchMapItems.Attach(item);
            context.UserBranchMapItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Соответствие с id = {0} успешно удалено", id);
        }

        /// <summary>
        /// Возвращает информацию о связах конкретного филиала с пользователями определенной роли.
        /// </summary>
        /// <param name="branchId">Код пользователя.</param>
        /// <param name="userProjectRoleId">Код роли.</param>
        /// <returns>Филиалы.</returns>
        public IEnumerable<UserBranchMapItemDTO> GetUserBranchMapByItemsByBranch(Guid? branchId, long? userProjectRoleId)
        {
            _logger.InfoFormat("Получение всех пользователей филиала {0}", branchId);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(branchId);
            return context.UserBranchMapItems.Where(i => i.BranchID == rawId).Join(context.Users.Where(u => u.ProjectRoleID == userProjectRoleId), item => item.UserID, user => user.UserID, (item, user) => new { item, user }).Join(context.Branches, arg => arg.item.BranchID, branch => branch.BranchID, (arg, branch) =>
                new UserBranchMapItemDTO
                {
                    BranchID = arg.item.BranchID,
                    BranchTitle = branch.Title,
                    EventDate = arg.item.EventDate,
                    FirstName = arg.user.FirstName,
                    LastName = arg.user.LastName,
                    MiddleName = arg.user.MiddleName,
                    UserID = arg.item.UserID,
                    UserBranchMapID = arg.item.UserBranchMapID,
                    UserLogin = arg.user.LoginName

                });
        }

        /// <summary>
        /// Возвращает информацию о связах конкретного пользователя с филиалами.
        /// </summary>
        /// <param name="userId">Код пользователя.</param>
        /// <returns>Филиалы.</returns>
        public IEnumerable<UserBranchMapItemDTO> GetUserBranchMapByItemsByUser(Guid? userId)
        {
            return GetUserBranchMapByItemsByUser(userId, null);
        }

        /// <summary>
        /// Возвращает информацию о связах конкретного пользователя с филиалами.
        /// </summary>
        /// <param name="userId">Код пользователя.</param>
        /// <param name="userProjectRoleId">Код проектной роли пользователя.</param>
        /// <returns>Филиалы.</returns>
        public IEnumerable<UserBranchMapItemDTO> GetUserBranchMapByItemsByUser(Guid? userId, byte? userProjectRoleId)
        {
            _logger.InfoFormat("Получение всех филиалов пользователя {0} для роли {1}", userId, userProjectRoleId);
            var context = CreateContext();
            IQueryable<User> users;
            var rawId = FormatUtils.GuidToString(userId);
            if (userProjectRoleId != null)
            {
                
                users = context.Users.Where(u => u.ProjectRoleID == userProjectRoleId && u.UserID == rawId);

            }
            else
            {
                users = context.Users.Where(u => u.UserID == rawId);
            }

            return users.Join(context.UserBranchMapItems, user => user.UserID, item => item.UserID, (user, item) => new { item, user }).Join(context.Branches, arg => arg.item.BranchID, branch => branch.BranchID, (arg, branch) =>
                new UserBranchMapItemDTO
                {
                    BranchID = arg.item.BranchID,
                    BranchTitle = branch.Title,
                    EventDate = arg.item.EventDate,
                    FirstName = arg.user.FirstName,
                    LastName = arg.user.LastName,
                    MiddleName = arg.user.MiddleName,
                    UserID = arg.item.UserID,
                    UserBranchMapID = arg.item.UserBranchMapID,
                    UserLogin = arg.user.LoginName

                });
        }

        #endregion UserBranchMapItem

        #region FinancialGroupItem

        private const string DeleteAllFinancialGroupItemsSql = "DELETE FROM FinancialGroup";

        /// <summary>
        /// Удаляет всех филиалов.
        /// </summary>
        public void DeleteAllFinancialGroupItems()
        {
            _logger.InfoFormat("Удаление всех филиалов");

            var context = CreateContext();
            context.ExecuteStoreCommand(DeleteAllFinancialGroupItemsSql);
        }

        /// <summary>
        /// Получает список финансовая групп без фильтра.
        /// </summary>
        /// <returns>Список финансовых групп.</returns>
        public IEnumerable<FinancialGroupItem> GetFinancialGroupItems()
        {
            _logger.InfoFormat("Получение списка финансовая группа без строки поиска ");
            var context = CreateContext();
            return context.FinancialGroupItems;
        }

        /// <summary>
        ///   Сохраняет информацию финансовой группе.
        /// </summary>
        /// <param name="financialGroupItem"> Сохраняемая финансовая группа. </param>
        public void SaveFinancialGroupItem(FinancialGroupItem financialGroupItem)
        {
            _logger.InfoFormat("Сохранение финансовой группы с Id = {0}", financialGroupItem.FinancialGroupID);
            var context = CreateContext();

            var savedItem =
                context.FinancialGroupItems.FirstOrDefault(
                    di =>
                    di.FinancialGroupID == financialGroupItem.FinancialGroupID);

            if (financialGroupItem.FinancialGroupIDGuid == null || financialGroupItem.FinancialGroupIDGuid == Guid.Empty)
            {
                financialGroupItem.FinancialGroupIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                financialGroupItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.FinancialGroupItems.AddObject(financialGroupItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Финансовая группа с ID= {0} успешно сохранена",
                               financialGroupItem.FinancialGroupID);
        }

        /// <summary>
        /// Получает финансовую группу по его ID.
        /// </summary>
        /// <param name="id">Код описания финансовой группы.</param>
        /// <returns>Финансовая группа, если существует.</returns>
        public FinancialGroupItem GetFinancialGroupItem(Guid? id)
        {
            _logger.InfoFormat("Получение финансовой группы по Id = {0}", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            return context.FinancialGroupItems.FirstOrDefault(fs => fs.FinancialGroupID == rawId);
        }

        /// <summary>
        /// Удаляет из хранилища финансовую группу по его ID.
        /// </summary>
        /// <param name="id">Код финансовой группы.</param>
        public void DeleteFinancialGroupItem(Guid? id)
        {
            _logger.InfoFormat("Удаление финансовой группы id ={0}", id);
            var context = CreateContext();

            var item = new FinancialGroupItem { FinancialGroupIDGuid = id };
            context.FinancialGroupItems.Attach(item);
            context.FinancialGroupItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Финансовая группа id = {0} успешно удалена", id);
        }

        #endregion FinancialGroupItem

        #region FinancialGroupBranchMapItem

        private const string DeleteAllFinancialGroupBranchMapItemsSql = "DELETE FROM FinancialGroupBranchMap";

        /// <summary>
        /// Удаляет все привязки филиалов и фингрупп.
        /// </summary>
        public void DeleteAllFinancialGroupBranchMapItems()
        {
            _logger.InfoFormat("Удаление всех связок филиалов и фингрупп");

            var context = CreateContext();
            context.ExecuteStoreCommand(DeleteAllFinancialGroupBranchMapItemsSql);
        }

        /// <summary>
        /// Получает соответствия финансовой группы и филиала по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        /// <returns>Соостветствие, если существует.</returns>
        public FinancialGroupBranchMapItem GetFinancialGroupMapBranchItem(Guid? id)
        {
            _logger.InfoFormat("Получение соответствия финансовой группы и филиала по Id = {0}", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            return context.FinancialGroupBranchMapItems.FirstOrDefault(fs => fs.FinancialGroupBranchMapID == rawId);
        }

        /// <summary>
        /// Возвращает информацию о связах конкретного финансовой группы с филиалами.
        /// </summary>
        /// <param name="financialGroupID">Код группы.</param>
        /// <returns>Филиалы.</returns>
        public IEnumerable<FinancialGroupBranchMapItemDTO> GetFinancialGroupBranchMapItemsByFinancialGroup(Guid? financialGroupID)
        {
            _logger.InfoFormat("Получение информацию о связях филиалла и фин группы{0}", financialGroupID);
            var context = CreateContext();

            var rawId = FormatUtils.GuidToString(financialGroupID);

            return context.FinancialGroupBranchMapItems.Where(i => i.FinancialGroupID == rawId).Join(context.FinancialGroupItems, mapItem => mapItem.FinancialGroupID, item => item.FinancialGroupID, (mapItem, groupItem) => new { groupItem, mapItem }).
                Join(context.Branches, arg => arg.mapItem.BranchID, branch => branch.BranchID, (arg, branch) =>
                new FinancialGroupBranchMapItemDTO
                {
                    BranchID = arg.mapItem.BranchID,
                    BranchTitle = branch.Title,
                    FinancialGroupBranchMapID = arg.mapItem.FinancialGroupBranchMapID,
                    FinancialGroupID = arg.groupItem.FinancialGroupID,
                    FinancialGroupTitle = arg.groupItem.Title

                });
        }

        /// <summary>
        ///   Сохраняет информацию о соответствии финансовой группы и филиала.
        /// </summary>
        /// <param name="financialGroupBranchMapItem"> Сохраняемое соответствие. </param>
        public void SaveFinancialGroupMapBranchItem(FinancialGroupBranchMapItem financialGroupBranchMapItem)
        {
            _logger.InfoFormat("Сохранение соответствия с Id = {0} между финансовой группой {1} и филиалом {2}", financialGroupBranchMapItem.FinancialGroupBranchMapID, financialGroupBranchMapItem.FinancialGroupID, financialGroupBranchMapItem.BranchID);
            var context = CreateContext();

            var savedItem =
                context.FinancialGroupBranchMapItems.FirstOrDefault(
                    di => di.FinancialGroupBranchMapID == financialGroupBranchMapItem.FinancialGroupBranchMapID);

            if (financialGroupBranchMapItem.FinancialGroupBranchMapIDGuid == null || financialGroupBranchMapItem.FinancialGroupBranchMapIDGuid == Guid.Empty)
            {
                financialGroupBranchMapItem.FinancialGroupBranchMapIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                financialGroupBranchMapItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.FinancialGroupBranchMapItems.AddObject(financialGroupBranchMapItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat(
                "Соответствие пользователя и финансовой группы с ID= {0} успешно сохранено  между пользователем {1} И филиалом {2}",
                financialGroupBranchMapItem.FinancialGroupBranchMapID, financialGroupBranchMapItem.FinancialGroupID,
                financialGroupBranchMapItem.BranchID);
        }

        /// <summary>
        /// Удаляет из хранилища соответствие между финансовой группой и филиалом по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        public void DeleteFinancialGroupBranchMapItem(Guid? id)
        {
            _logger.InfoFormat("Удаление соответствия финансовой группы и филиала id ={0}", id);

            var context = CreateContext();
            var item = new FinancialGroupBranchMapItem { FinancialGroupBranchMapIDGuid = id };
            context.FinancialGroupBranchMapItems.Attach(item);
            context.FinancialGroupBranchMapItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Соответствие финансовой группы и филиала с id = {0} успешно удалено", id);
        }

        #endregion FinancialGroupBranchMapItem

        #region Warehouses

        private const string DeleteAllWarehouseSql = "DELETE FROM Warehouse";

        /// <summary>
        /// Удаляет все привязки фингрупп и складов.
        /// </summary>
        public void DeleteAllWarehouses()
        {
            _logger.InfoFormat("Удаление всех складов");

            var context = CreateContext();
            context.ExecuteStoreCommand(DeleteAllWarehouseSql);
        }

        /// <summary>
        ///   Сохраняет информацию о складе.
        /// </summary>
        /// <param name="warehouse"> Сохраняемый склад. </param>
        public void SaveWarehouse(Warehouse warehouse)
        {
            _logger.InfoFormat("Сохранение склада с Id = {0}", warehouse.WarehouseID);
            var context = CreateContext();

            var savedItem =
                context.Warehouses.FirstOrDefault(
                    di =>
                    di.WarehouseID == warehouse.WarehouseID);

            if (warehouse.WarehouseID == null || warehouse.WarehouseIDGuid == Guid.Empty)
            {
                warehouse.WarehouseIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                warehouse.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.Warehouses.AddObject(warehouse);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Категория товара с ID= {0} успешно сохранена",
                               warehouse.WarehouseID);
        }

        /// <summary>
        /// Получает склад по его ID.
        /// </summary>
        /// <param name="id">Код склада.</param>
        /// <returns>Склад товара, если существует.</returns>
        public Warehouse GetWarehouse(Guid? id)
        {
            _logger.InfoFormat("Получение скалада по Id = {0} по домену пользователя", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            return context.Warehouses.FirstOrDefault(fs => fs.WarehouseID == rawId);
        }

        /// <summary>
        /// Удаляет из хранилища склад по его ID.
        /// </summary>
        /// <param name="id">Код склада товара.</param>
        public void DeleteWarehouse(Guid? id)
        {
            _logger.InfoFormat("Удаление склада товара id ={0}", id);

            var context = CreateContext();
            var item = new Warehouse { WarehouseIDGuid = id };
            context.Warehouses.Attach(item);
            context.Warehouses.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Склад товара id = {0} успешно удален", id);
        }

        #endregion Warehouses

        #region FinancialGroupWarehouseMapItem

        private const string DeleteAllFinancialGroupWarehouseItemsSql = "DELETE FROM FinancialGroupWarehouseMap";

        /// <summary>
        /// Удаляет все привязки фингрупп и складов.
        /// </summary>
        public void DeleteAllFinancialGroupMapWarehouseItems()
        {
            _logger.InfoFormat("Удаление всех связок финрупп и складов");

            var context = CreateContext();
            context.ExecuteStoreCommand(DeleteAllFinancialGroupWarehouseItemsSql);
        }

        /// <summary>
        ///   Сохраняет информацию о соответствии финансовой группы и склада.
        /// </summary>
        /// <param name="financialGroupWarehouseMapItem"> Сохраняемое соответствие. </param>
        public void SaveFinancialGroupWarehouseItem(FinancialGroupWarehouseMapItem financialGroupWarehouseMapItem)
        {
            _logger.InfoFormat("Сохранение соответствия с Id = {0} между финансовой группой {1} и складом {2}", financialGroupWarehouseMapItem.FinancialGroupWarehouseMapID, financialGroupWarehouseMapItem.FinancialGroupID, financialGroupWarehouseMapItem.WarehouseID);
            var context = CreateContext();

            var savedItem =
                context.FinancialGroupWarehouseMapItems.FirstOrDefault(
                    di => di.FinancialGroupWarehouseMapID == financialGroupWarehouseMapItem.FinancialGroupWarehouseMapID);

            if (financialGroupWarehouseMapItem.FinancialGroupWarehouseMapIDGuid == null || financialGroupWarehouseMapItem.FinancialGroupWarehouseMapIDGuid == Guid.Empty)
            {
                financialGroupWarehouseMapItem.FinancialGroupWarehouseMapIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                financialGroupWarehouseMapItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.FinancialGroupWarehouseMapItems.AddObject(financialGroupWarehouseMapItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat(
                "Соответствие пользователя и финансовой группы с ID= {0} успешно сохранено  между пользователем {1} И складом {2}",
                financialGroupWarehouseMapItem.FinancialGroupWarehouseMapID, financialGroupWarehouseMapItem.FinancialGroupID,
                financialGroupWarehouseMapItem.WarehouseID);
        }

        /// <summary>
        /// Получает соответствия финансовой группы и склада по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        /// <returns>Соостветствие, если существует.</returns>
        public FinancialGroupWarehouseMapItem GetFinancialGroupMapWarehouseItem(Guid? id)
        {
            _logger.InfoFormat("Получение соответствия финансовой группы и склада по Id = {0}", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            return context.FinancialGroupWarehouseMapItems.FirstOrDefault(fs => fs.FinancialGroupWarehouseMapID == rawId);
        }

        /// <summary>
        /// Удаляет из хранилища соответствие между финансовой группой и складом по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        public void DeleteFinancialGroupWarehouseMapItem(Guid? id)
        {
            _logger.InfoFormat("Удаление соответствия финансовой группы и склада id ={0}", id);

            var context = CreateContext();
            var item = new FinancialGroupWarehouseMapItem { FinancialGroupWarehouseMapIDGuid = id };
            context.FinancialGroupWarehouseMapItems.Attach(item);
            context.FinancialGroupWarehouseMapItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Соответствие финансовой группы и склада с id = {0} успешно удалено", id);
        }

        #endregion FinancialGroupWarehouseMapItem

        #region ItemCategory

        private const string DeleteAllItemCategoriesSql = "DELETE FROM ItemCategory";

        /// <summary>
        /// Удаляет все категории товаров.
        /// </summary>
        public void DeleteAllItemCategories()
        {
            _logger.InfoFormat("Удаление всех категорий товаров");

            var context = CreateContext();
            context.ExecuteStoreCommand(DeleteAllItemCategoriesSql);
        }

        /// <summary>
        /// Получает список всех категорий товара.
        /// </summary>
        /// <returns>Список категорий товаров.</returns>
        public IEnumerable<ItemCategory> GetItemCategories()
        {
            _logger.InfoFormat("Получение списка всех категорий товаров");

            var context = CreateContext();
            var items = context.ItemCategories;

            return items.OrderBy(i => i.Title);
        }

        /// <summary>
        /// Получает категорию товара по его ID.
        /// </summary>
        /// <param name="id">Код категории товара.</param>
        /// <returns>Категория товара, если существует.</returns>
        public ItemCategory GetItemCategory(Guid? id)
        {
            _logger.InfoFormat("Получение категории товара по Id = {0} по домену пользователя", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            return context.ItemCategories.FirstOrDefault(fs => fs.ItemCategoryID == rawId);
        }

        /// <summary>
        ///   Сохраняет информацию о кактегории товара.
        /// </summary>
        /// <param name="itemCategory"> Сохраняемая категория товара. </param>
        public void SaveItemCategory(ItemCategory itemCategory)
        {
            _logger.InfoFormat("Сохранение категории товара с Id = {0}", itemCategory.ItemCategoryID);
            var context = CreateContext();

            var savedItem =
                context.ItemCategories.FirstOrDefault(
                    di =>
                    di.ItemCategoryID == itemCategory.ItemCategoryID);

            if (itemCategory.ItemCategoryID == null || itemCategory.ItemCategoryIDGuid == Guid.Empty)
            {
                itemCategory.ItemCategoryIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                itemCategory.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.ItemCategories.AddObject(itemCategory);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Категория товара с ID= {0} успешно сохранена",
                               itemCategory.ItemCategoryID);
        }
        

        /// <summary>
        /// Удаляет из хранилища категорию товара по его ID.
        /// </summary>
        /// <param name="id">Код категории товара.</param>
        public void DeleteItemCategory(Guid? id)
        {
            _logger.InfoFormat("Удаление категории товара id ={0}", id);

            var context = CreateContext();
            var item = new ItemCategory { ItemCategoryIDGuid = id };
            context.ItemCategories.Attach(item);
            context.ItemCategories.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Категория товара id = {0} успешно удалена", id);
        }

        #endregion ItemCategory

        #region GoodsItems 

        private const string DeleteAllGoodsItemsSql = "DELETE FROM GoodsItem";

        /// <summary>
        /// Удаляет все номенклотуры товаров.
        /// </summary>
        public void DeleteAllGoodsItems()
        {
            _logger.InfoFormat("Удаление всех номенклатур товаров");

            var context = CreateContext();
            context.ExecuteStoreCommand(DeleteAllGoodsItemsSql);
        }

        /// <summary>
        ///   Сохраняет информацию о номенклатуре.
        /// </summary>
        /// <param name="goodsItem"> Сохраняемая номенклатура. </param>
        public void SaveGoodsItem(GoodsItem goodsItem)
        {
            _logger.InfoFormat("Сохранение номенклатуры с Id = {0}", goodsItem.GoodsItemID);
            var context = CreateContext();

            var savedItem =
                context.GoodsItems.FirstOrDefault(
                    di =>
                    di.GoodsItemID == goodsItem.GoodsItemID);

            if (goodsItem.GoodsItemID == null || goodsItem.GoodsItemIDGuid == Guid.Empty)
            {
                goodsItem.GoodsItemIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                goodsItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.GoodsItems.AddObject(goodsItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Категория товара с ID= {0} успешно сохранена",
                               goodsItem.GoodsItemID);
        }

        /// <summary>
        /// Получает номенклатуру по его ID.
        /// </summary>
        /// <param name="id">Код номенклатуры.</param>
        /// <returns>Номенклатура товара, если существует.</returns>
        public GoodsItemDTO GetGoodsItem(Guid? id)
        {
            _logger.InfoFormat("Получение номенклатуры по Id = {0}", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            return context.GoodsItems.Where(fs => fs.GoodsItemID == rawId).Join(
                context.ItemCategories,
                item => item.ItemCategoryID, category => category.ItemCategoryID, (item, category) => new GoodsItemDTO
                {
                    BarCode = item.BarCode,
                    Description = item.Description,
                    DimensionKindID = item.DimensionKindID,
                    GoodsItemID = item.GoodsItemID,
                    ItemCategoryID = item.ItemCategoryID,
                    ItemCategoryTitle = category.Title,
                    Particular = item.Particular,
                    Title = item.Title,
                    UserCode = item.UserCode
                }
                ).FirstOrDefault();
        }

        /// <summary>
        /// Удаляет из хранилища номенклатуры по его ID.
        /// </summary>
        /// <param name="id">Код номенклатуры товара.</param>
        public void DeleteGoodsItem(Guid? id)
        {
            _logger.InfoFormat("Удаление номенклатуры товара id ={0}", id);

            var context = CreateContext();
            var item = new GoodsItem { GoodsItemIDGuid = id };
            context.GoodsItems.Attach(item);
            context.GoodsItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Номенклатура товара id = {0} успешно удалена", id);
        }

        #endregion GoodsItems

        #region WarehouseItem

        private const string DeleteAllWarehouseItemsSql = "DELETE FROM WarehouseItem";

        /// <summary>
        /// Удаляет все номенклотуры товаров.
        /// </summary>
        public void DeleteAllWarehouseItems()
        {
            _logger.InfoFormat("Удаление всех остатков на складах");

            var context = CreateContext();
            context.ExecuteStoreCommand(DeleteAllWarehouseItemsSql);
        }

        /// <summary>
        ///   Сохраняет информацию о остатках на складе.
        /// </summary>
        ///<remarks>Проверять домен на наличие склада перед вызовом.</remarks>
        /// <param name="warehouseItem"> Сохраняемые остатки на складе. </param>
        public void SaveWarehouseItem(WarehouseItem warehouseItem)
        {
            _logger.InfoFormat("Сохранение остатков на складе с Id = {0}", warehouseItem.WarehouseItemID);
            var context = CreateContext();
            var savedItem =
                context.WarehouseItems.FirstOrDefault(
                    di =>
                    di.WarehouseItemID == warehouseItem.WarehouseItemID);

            if (warehouseItem.WarehouseItemID == null || warehouseItem.WarehouseItemIDGuid == Guid.Empty)
            {
                warehouseItem.WarehouseItemIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                warehouseItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.WarehouseItems.AddObject(warehouseItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Остатки на складе с ID= {0} успешно сохранены",
                               warehouseItem.WarehouseItemID);
        }

        /// <summary>
        /// Получает остатоков на складе по его ID.
        /// </summary>
        /// <returns>Остатки на складе товара, если существует.</returns>
        public IEnumerable<WarehouseItemDTO> GetWarehouseItems()
        {
            _logger.InfoFormat("Получение");
            var context = CreateContext();

            return context.WarehouseItems.Join(
                context.Warehouses,
                item => item.WarehouseID, warehouse => warehouse.WarehouseID, (item, warehouse) => item)
                .Join(context.GoodsItems, item => item.GoodsItemID, item => item.GoodsItemID,
                      (item, goodsItem) => new { item, goodsItem }).Select(
                          i => new WarehouseItemDTO
                          {
                              GoodsItemTitle = i.goodsItem.Title,
                              GoodsItemID = i.goodsItem.GoodsItemID,
                              DimensionKindID = i.goodsItem.DimensionKindID,
                              RepairPrice = i.item.RepairPrice,
                              SalePrice = i.item.SalePrice,
                              StartPrice = i.item.StartPrice,
                              Total = i.item.Total,
                              WarehouseID = i.item.WarehouseID,
                              WarehouseItemID = i.item.WarehouseItemID

                          }
                );
        }

        /// <summary>
        /// Получает остаток на складе по его ID.
        /// </summary>
        /// <param name="id">Код номенклатуры.</param>
        /// <returns>Остатки на складе товара, если существует.</returns>
        public WarehouseItemDTO GetWarehouseItem(Guid? id)
        {
            _logger.InfoFormat("Получение номенклатуры по Id = {0}", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            return context.WarehouseItems.Where(i => i.WarehouseItemID == rawId).Join(
                context.Warehouses,
                item => item.WarehouseID, warehouse => warehouse.WarehouseID, (item, warehouse) => item)
                .Join(context.GoodsItems, item => item.GoodsItemID, item => item.GoodsItemID,
                      (item, goodsItem) => new { item, goodsItem }).Select(
                          i => new WarehouseItemDTO
                          {
                              GoodsItemTitle = i.goodsItem.Title,
                              GoodsItemID = i.goodsItem.GoodsItemID,
                              DimensionKindID = i.goodsItem.DimensionKindID,
                              RepairPrice = i.item.RepairPrice,
                              SalePrice = i.item.SalePrice,
                              StartPrice = i.item.StartPrice,
                              Total = i.item.Total,
                              WarehouseID = i.item.WarehouseID,
                              WarehouseItemID = i.item.WarehouseItemID

                          }
                ).FirstOrDefault();

        }

        /// <summary>
        /// Удаляет из хранилища остаток на складе по его ID.
        /// </summary>
        /// <param name="id">Код номенклатуры товара.</param>
        public void DeleteWarehouseItem(Guid? id)
        {
            _logger.InfoFormat("Удаление остатка на складе товара id ={0}", id);

            var context = CreateContext();
            var item = new WarehouseItem { WarehouseItemIDGuid = id };
            context.WarehouseItems.Attach(item);
            context.WarehouseItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Остаток на складе товара id = {0} успешно удален", id);
        }

        #endregion WarehouseItem

        #region OrderStatus 

        private const string DeleteAllOrderStatusesSql = "DELETE FROM OrderStatus";

        /// <summary>
        /// Удаляет все номенклотуры товаров.
        /// </summary>
        public void DeleteAllOrderStatuses()
        {
            _logger.InfoFormat("Удаление всех статусов заказов");

            var context = CreateContext();
            context.ExecuteStoreCommand(DeleteAllOrderStatusesSql);
        }

        /// <summary>
        /// Получает список статусов заказов.
        /// </summary>
        /// <returns>Список статусов заказа.</returns>
        public IEnumerable<OrderStatus> GetOrderStatuses()
        {
            _logger.InfoFormat("Получение всего списка статусов");

            var context = CreateContext();
            return context.OrderStatuses;
        }

        /// <summary>
        ///   Сохраняет информацию о статусе заказа.
        /// </summary>
        /// <param name="orderStatus"> Сохраняемый статус заказа. </param>
        public void SaveOrderStatus(OrderStatus orderStatus)
        {
            _logger.InfoFormat("Сохранение статуса заказа с Id = {0}", orderStatus.OrderStatusID);
            var context = CreateContext();

            var savedItem =
                context.OrderStatuses.FirstOrDefault(
                    di => di.OrderStatusID == orderStatus.OrderStatusID);

            if (orderStatus.OrderStatusID == null)
            {
                orderStatus.OrderStatusIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                orderStatus.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.OrderStatuses.AddObject(orderStatus);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Сохранение статуса заказа с ID= {0} успешно сохранено",
                               orderStatus.OrderStatusID);
        }

        /// <summary>
        /// Получение статусов заказа по его типам.
        /// </summary>
        /// <param name="kindId">Тип статуса.</param>
        /// <returns>Если не находит пытается найти ближайший по смыслу.</returns>
        public OrderStatus GetOrderStatusByKind(long? kindId)
        {
            _logger.InfoFormat("Получение статусов заказов по его типу {0}", kindId);
            var context = CreateContext();

            var status = context.OrderStatuses.Where(s => s.StatusKindID == kindId).OrderBy(i => i.StatusKindID).FirstOrDefault();
            if (status == null)
            {
                status =
                    context.OrderStatuses.Where(s => s.StatusKindID > kindId ).OrderBy(
                        i => new { i.StatusKindID, i.Title }).FirstOrDefault();
                if (status == null)
                {
                    status = context.OrderStatuses.OrderBy(i => i.StatusKindID).FirstOrDefault();
                } //if
            } //if

            return status;
        }

        /// <summary>
        /// Получает статус заказа по его ID.
        /// </summary>
        /// <param name="id">Код статуса заказа.</param>
        /// <returns>Статус заказа, если существует.</returns>
        public OrderStatus GetOrderStatus(Guid? id)
        {
            _logger.InfoFormat("Получение статуса заказа по Id = {0} ", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            return context.OrderStatuses.FirstOrDefault(fs => fs.OrderStatusID == rawId);
        }

        /// <summary>
        /// Удаляет из хранилища статус заказа по его ID.
        /// </summary>
        /// <param name="id">Код статуса заказа.</param>
        public void DeleteOrderStatus(Guid? id)
        {
            _logger.InfoFormat("Удаление статуса заказа по id ={0}", id);

            var context = CreateContext();
            var item = new OrderStatus { OrderStatusIDGuid = id };
            context.OrderStatuses.Attach(item);
            context.OrderStatuses.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Статус заказа с id = {0} успешно удален", id);
        }


        #endregion OrderStatus

        #region OrderKind

        private const string DeleteAllOrderKindsSql = "DELETE FROM OrderKind";

        /// <summary>
        /// Удаляет все номенклотуры товаров.
        /// </summary>
        public void DeleteAllOrderKinds()
        {
            _logger.InfoFormat("Удаление всех типов заказов");

            var context = CreateContext();
            context.ExecuteStoreCommand(DeleteAllOrderKindsSql);
        }

        /// <summary>
        /// Получает полный список типов заказа.
        /// </summary>
        /// <returns>Список типов заказа.</returns>
        public IEnumerable<OrderKind> GetOrderKinds()
        {
            _logger.InfoFormat("Получение полный список типов заказа ");
            var context = CreateContext();
            return context.OrderKinds;
        }

        /// <summary>
        ///   Сохраняет информацию о типе заказа.
        /// </summary>
        /// <param name="orderKind"> Сохраняемый тип заказа. </param>
        public void SaveOrderKind(OrderKind orderKind)
        {
            _logger.InfoFormat("Сохранение типа заказа с Id = {0}", orderKind.OrderKindID);
            var context = CreateContext();

            var savedItem =
                context.OrderKinds.FirstOrDefault(
                    di => di.OrderKindID == orderKind.OrderKindID);

            if (orderKind.OrderKindID == null)
            {
                orderKind.OrderKindIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                orderKind.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.OrderKinds.AddObject(orderKind);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Сохранение типа заказа с ID= {0} успешно сохранено",
                               orderKind.OrderKindID);
        }

        /// <summary>
        /// Получает тип заказа по его ID.
        /// </summary>
        /// <param name="id">Код типа заказа.</param>
        /// <returns>Тип заказа, если существует.</returns>
        public OrderKind GetOrderKind(Guid? id)
        {
            _logger.InfoFormat("Получение типа заказа по Id = {0}", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            return context.OrderKinds.FirstOrDefault(fs => fs.OrderKindID == rawId);
        }

        /// <summary>
        /// Удаляет из хранилища тип заказа по его ID.
        /// </summary>
        /// <param name="id">Код типа заказа.</param>
        public void DeleteOrderKind(Guid? id)
        {
            _logger.InfoFormat("Удаление типа заказа по id ={0}", id);

            var context = CreateContext();
            var item = new OrderKind { OrderKindIDGuid = id };
            context.OrderKinds.Attach(item);
            context.OrderKinds.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Тип заказа с id = {0} успешно удален", id);
        }

        #endregion OrderKind

        #region RepairOrder 

        /// <summary>
        /// Получает заказа по его ID.
        /// </summary>
        /// <param name="id">Код заказа.</param>
        /// <returns>Заказ, если существует.</returns>
        public RepairOrder GetRepairOrder(Guid? id)
        {
            _logger.InfoFormat("Получение заказа по Id = {0}", id);
            var context = CreateContext();

            var rawId = FormatUtils.GuidToString(id);

            return context.RepairOrders.FirstOrDefault(fs => fs.RepairOrderID == rawId);
        }

        /// <summary>
        ///   Сохраняет информацию о заказе.
        /// </summary>
        /// <param name="repairOrder"> Сохраняемый заказ. </param>
        public void SaveRepairOrder(RepairOrder repairOrder)
        {
            _logger.InfoFormat("Сохранение заказа с с Id = {0}", repairOrder.RepairOrderID);
            var context = CreateContext();

            var savedItem =
                context.RepairOrders.FirstOrDefault(
                    di => di.RepairOrderID == repairOrder.RepairOrderID );

            if (repairOrder.RepairOrderID == null || repairOrder.RepairOrderIDGuid == Guid.Empty)
            {
                repairOrder.RepairOrderIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                repairOrder.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.RepairOrders.AddObject(repairOrder);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Заказ с ID= {0} успешно сохранен",
                               repairOrder.RepairOrderID);
        }

        /// <summary>
        /// Удаляет из хранилища заказ по его ID.
        /// </summary>
        /// <param name="id">Код заказа.</param>
        public void DeleteRepairOrder(Guid? id)
        {
            _logger.InfoFormat("Удаление заказа id ={0}", id);

            var context = CreateContext();
            var item = new RepairOrder { RepairOrderIDGuid = id };
            context.RepairOrders.Attach(item);
            context.RepairOrders.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Заказ с id = {0} успешно удален", id);
        }

       /// <summary>
        /// Получает списко заказаов за определенный период.
        /// </summary>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Заказы.</returns>
        public IEnumerable<RepairOrder> GetRepairOrders(DateTime beginDate, DateTime endDate)
        {
            _logger.InfoFormat("Получение заказов для домена {0} по {1}", beginDate, endDate);

            var context = CreateContext();

            var beginRaw = FormatUtils.DateTimeToString(beginDate);
            var endRaw = FormatUtils.DateTimeToString(endDate);

            throw new Exception("Необходимо производить через SQL");
            /*return
                context.RepairOrders.Where(
                    rp =>
                    rp.EventDate >= beginRaw &&
                    rp.EventDate <= endRaw);*/
        }

        /// <summary>
        /// Получает заказа по его ID.
        /// </summary>
        /// <param name="id">Код заказа.</param>
        /// <returns>Заказ, если существует.</returns>
        public RepairOrderDTO GetRepairOrderDTO(Guid? id)
        {
            _logger.InfoFormat("Получение DTO заказа по Id = {0}", id);
            var context = CreateContext();

            var rawId = FormatUtils.GuidToString(id);

            var orders =  context.RepairOrders.Where(fs => fs.RepairOrderID == rawId);

            return SelectRepairOrderDTO(orders, context, 00, 0).FirstOrDefault();
        }


        /// <summary>
        /// Возвращает список заказов по фильтром.
        /// </summary>
        /// <param name="orderStatusId">Код статуса задачи.</param>
        /// <param name="isUrgent">Признак срочности.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        public IEnumerable<RepairOrderDTO> GetRepairOrders( Guid? orderStatusId, bool? isUrgent, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение заказов по строке поиска {0}", name);
            var context = CreateContext();

            var orders = GetRepairOrdersFilterByName(orderStatusId, isUrgent, name, context);

            count = orders.Count();

            return SelectRepairOrderDTO(orders, context, page, pageSize);
        }

        /// <summary>
        /// Возвращает список заказов по фильтром по филиалам которые доступны пользователям.
        /// </summary>
        /// <param name="orderStatusId">Код статуса  задачи.</param>
        /// <param name="isUrgent">Признак срочности.</param>
        /// <param name="userId">Код пользователя по которому производится поиск филиалов. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        public IEnumerable<RepairOrderDTO> GetRepairOrdersUserBranch(Guid? orderStatusId, bool? isUrgent, Guid? userId, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение заказов по строке поиска {0}", name);
            var context = CreateContext();

            var orders = GetRepairOrdersFilterByNameAndUserBranches(userId, isUrgent, orderStatusId, name, context);

            count = orders.Count();

            return SelectRepairOrderDTO(orders, context, page, pageSize);
        }

        /// <summary>
        /// Возвращает список заказов по фильтром по конкретным исполнителям.
        /// </summary>
        /// <param name="orderStatusId">Код статуса  задачи.</param>
        /// <param name="isUrgent">Признак срочности.</param>
        /// <param name="userId">Код пользователя по которому производится поиск задач. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        public IEnumerable<RepairOrderDTO> GetRepairOrdersUser( Guid? orderStatusId, bool? isUrgent, Guid? userId, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение заказов по строке поиска {0}", name);
            var context = CreateContext();

            var userIdRaw = FormatUtils.GuidToString(userId);

            var orders =
                GetRepairOrdersFilterByName(orderStatusId, isUrgent, name, context).Where(
                    i => i.ManagerID == userIdRaw || i.EngineerID == userIdRaw);

            count = orders.Count();

            return SelectRepairOrderDTO(orders, context, page, pageSize);
        }


        private IQueryable<RepairOrder> GetRepairOrdersFilterByName( Guid? orderStatusId, bool? isUrgent, string name, DatabaseContext context)
        {
            NormilizeString(ref name);
            var orderStatusIdRaw = FormatUtils.GuidToString(orderStatusId);

            IQueryable<RepairOrder> step1 = context.RepairOrders;

            if (!string.IsNullOrWhiteSpace(name))
            {
                step1 =
                context.RepairOrders.Where(
                    i =>
                    (i.ClientFullName??string.Empty + i.DeviceTitle + i.ClientPhone).Contains(name));
            } //if
            

            if (orderStatusId != null)
            {
                step1 = step1.Where(i => i.OrderStatusID == orderStatusIdRaw);
            } //if

            if (isUrgent != null)
            {
                long? isUrgentRaw = FormatUtils.BoolToLong(isUrgent??false);
                step1 = step1.Where(i => i.IsUrgent == isUrgentRaw);
            } //if

            return step1;
        }

        private IQueryable<RepairOrder> GetRepairOrdersFilterByNameAndUserBranches( Guid? userID, bool? isUrgent, Guid? orderStatusId, string name, DatabaseContext context)
        {
            var userRawId = FormatUtils.GuidToString(userID);
            return GetRepairOrdersFilterByName(orderStatusId, isUrgent, name, context).Join(context.UserBranchMapItems, order => order.BranchID,
                item => item.BranchID, (order, item) => new { order, item }).Where(i => i.item.UserID == userRawId).Select(i => i.order);
        }

        private IQueryable<RepairOrderDTO> SelectRepairOrderDTO(IQueryable<RepairOrder> entityes, DatabaseContext context, int page, int pageSize)
        {
            var item1 = entityes.GroupJoin(context.Users, order => order.EngineerID, user => user.UserID,
                                           (order, users) => new { order, users }).SelectMany(
                                               a => a.users.DefaultIfEmpty(), (arg, user) => new
                                               {
                                                   Order = arg.order,
                                                   EngineerFullName = user.LastName + " " + user.FirstName + " " + (user.MiddleName ?? string.Empty)/*user.LastName + " " + user.FirstName + " " + user.MiddleName */
                                               }
                );

            var item2 = item1.Join(context.Users, arg => arg.Order.ManagerID, user => user.UserID, (arg1, user) =>
                new
                {
                    arg1.Order,
                    arg1.EngineerFullName,
                    ManagerFullName = user.LastName + " " + user.FirstName + " " + (user.MiddleName ?? string.Empty)
                }
                );

            var item3 = item2.Join(context.OrderKinds, arg => arg.Order.OrderKindID, kind => kind.OrderKindID,
                                   (arg1, kind) => new
                                   {
                                       arg1.Order,
                                       arg1.EngineerFullName,
                                       arg1.ManagerFullName,
                                       OrderKindTitle = kind.Title
                                   }
                );

            var item4 = item3.Join(context.OrderStatuses, arg => arg.Order.OrderStatusID, status => status.OrderStatusID,
                                   (arg1, status) => new
                                   {
                                       arg1.Order,
                                       arg1.EngineerFullName,
                                       arg1.ManagerFullName,
                                       arg1.OrderKindTitle,
                                       OrderStatusTitle = status.Title,
                                       StatusKind = status.StatusKindID
                                   }
                );

            var item5 = item4.Join(context.Branches, arg => arg.Order.BranchID, branch => branch.BranchID,
                                   (arg1, branch) => new
                                   {
                                       arg1.Order,
                                       arg1.EngineerFullName,
                                       arg1.ManagerFullName,
                                       arg1.OrderKindTitle,
                                       arg1.OrderStatusTitle,
                                       arg1.StatusKind,
                                       BranchTitle = branch.Title
                                   }
                );

            if (page != 0)
            {
                item5 = item5.OrderByDescending(i => i.Order.EventDate).Page(page, pageSize);
            } //if

            return item5.Select(i => new RepairOrderDTO
            {
                BranchID = i.Order.BranchID,
                BranchTitle = i.BranchTitle,
                CallEventDate = i.Order.CallEventDate,
                ClientAddress = i.Order.ClientAddress,
                ClientEmail = i.Order.ClientEmail,
                ClientFullName = i.Order.ClientFullName,
                ClientPhone = i.Order.ClientPhone,
                DateOfBeReady = i.Order.DateOfBeReady,
                Defect = i.Order.Defect,
                DeviceAppearance = i.Order.DeviceAppearance,
                DeviceModel = i.Order.DeviceModel,
                DeviceSN = i.Order.DeviceSN,
                DeviceTitle = i.Order.DeviceTitle,
                DeviceTrademark = i.Order.DeviceTrademark,
                EngineerFullName = i.EngineerFullName,
                EventDate = i.Order.EventDate,
                EngineerID = i.Order.EngineerID,
                ManagerFullName = i.ManagerFullName,
                ManagerID = i.Order.ManagerID,
                IssuerID = i.Order.IssuerID,
                IssueDate = i.Order.IssueDate,
                IsUrgent = i.Order.IsUrgent,
                GuidePrice = i.Order.GuidePrice,
                Notes = i.Order.Notes,
                Number = i.Order.Number,
                Options = i.Order.Options,
                OrderKindID = i.Order.OrderKindID,
                OrderKindTitle = i.OrderKindTitle,
                OrderStatusID = i.Order.OrderStatusID,
                OrderStatusTitle = i.OrderStatusTitle,
                PrePayment = i.Order.PrePayment,
                Recommendation = i.Order.Recommendation,
                RepairOrderID = i.Order.RepairOrderID,
                WarrantyTo = i.Order.WarrantyTo,
                StatusKind = i.StatusKind,

            });
        }

        #endregion RepairOrder

        #region DeviceItem

        /// <summary>
        /// Получает запчасти по ее ID.
        /// </summary>
        /// <param name="id">Код запчасти.</param>
        /// <returns>Заказ, если существует.</returns>
        public DeviceItem GetDeviceItem(Guid? id)
        {
            _logger.InfoFormat("Получение запчасти по Id = {0}", id);
            var context = CreateContext();

            var rawId = FormatUtils.GuidToString(id);

            return context.DeviceItems.FirstOrDefault(fs => fs.DeviceItemID == rawId);
        }


        /// <summary>
        /// Вычисление суммы по установленным запчастям конеретного заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Значение суммы.</returns>
        public decimal? GetDeviceItemsSum(Guid? repairOrderID)
        {
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(repairOrderID);
            return (decimal?)context.DeviceItems.Where(i => i.RepairOrderID == rawId).Sum(i => ((double?)i.Price));
        }

        /// <summary>
        /// Получает список устройств по ID заказа.
        /// </summary>
        /// <param name="repairOrderId">Код заказа.</param>
        /// <returns>Список установленных устройств.</returns>
        public IEnumerable<DeviceItem> GetDeviceItems(Guid? repairOrderId)
        {
            _logger.InfoFormat("Получение списка установленых запчастей по Id заказа = {0}", repairOrderId);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(repairOrderId);

            return context.DeviceItems.Where(fs => fs.RepairOrderID == rawId);
        }

        /// <summary>
        ///   Сохраняет информацию о запчати.
        /// </summary>
        /// <param name="deviceItem"> Сохраняемая запчасть. </param>
        public void SaveDeviceItem(DeviceItem deviceItem)
        {
            _logger.InfoFormat("Сохранение запчасти с с Id = {0}", deviceItem.DeviceItemIDGuid);
            var context = CreateContext();

            var savedItem =
                context.DeviceItems.FirstOrDefault(
                    di => di.DeviceItemID == deviceItem.DeviceItemID);

            if (deviceItem.DeviceItemIDGuid == null || deviceItem.DeviceItemIDGuid == Guid.Empty)
            {
                deviceItem.DeviceItemIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                deviceItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.DeviceItems.AddObject(deviceItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Запчасть с ID= {0} успешно сохранен",
                               deviceItem.DeviceItemIDGuid);
        }

        /// <summary>
        /// Удаляет из хранилища запчасти по ее ID.
        /// </summary>
        /// <param name="id">Код запчасти.</param>
        public void DeleteDeviceItem(Guid? id)
        {
            _logger.InfoFormat("Удаление запчасти id ={0}", id);

            var context = CreateContext();
            var item = new DeviceItem { DeviceItemIDGuid = id };
            context.DeviceItems.Attach(item);
            context.DeviceItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Запчасть с id = {0} успешно удалена", id);
        }

        private const string DeleteAllDeviceItemsSql = "DELETE FROM DeviceItem Where RepairOrderID = '{0}'";

        /// <summary>
        /// Удаляет все установленных запчастей определенного заказа.
        /// </summary>
        public void DeleteAllDeviceItems(Guid? repairOrderID)
        {
            _logger.InfoFormat("Удаление всех установленных запчастей заказа {0}", repairOrderID);

            var context = CreateContext();
            context.ExecuteStoreCommand(string.Format(DeleteAllDeviceItemsSql,
                                                      FormatUtils.GuidToString(repairOrderID)));
        }

        /// <summary>
        /// Получает статистическую информацию по установленным запчастям определенного заказа.
        /// </summary>
        /// <param name="repairOrderID">Заказ.</param>
        /// <returns>Статистика.</returns>
        public ItemsInfo GetDeviceItemsTotal(Guid? repairOrderID)
        {
            _logger.InfoFormat(
                "Получение статистики для заказа {0}", repairOrderID);

            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(repairOrderID);

            return context.DeviceItems.Where(i => i.RepairOrderID == rawId).GroupBy(i => i.RepairOrderID).Select(i => new ItemsInfo
            {
                Amount = i.Sum(l => l.Price),
                Count = i.Count(),
                SumCount = i.Sum(l => l.Count),
                TotalAmount = i.Sum(l => l.Price * l.Count)
            }).FirstOrDefault();
        }

        #endregion DeviceItem

        #region WorkItem

        private const string DeleteAllWorkItemsSql = "DELETE FROM WorkItem Where RepairOrderID = '{0}'";

        /// <summary>
        /// Удаляет все проделанные работы определенного заказа.
        /// </summary>
        public void DeleteAllWorkItems(Guid? repairOrderID)
        {
            _logger.InfoFormat("Удаление всех проделанных работ заказа {0}", repairOrderID);

            var context = CreateContext();
            context.ExecuteStoreCommand(string.Format(DeleteAllWorkItemsSql,
                                                      FormatUtils.GuidToString(repairOrderID)));
        }

        /// <summary>
        ///   Сохраняет информацию о проделанной работе.
        /// </summary>
        /// <param name="workItem"> Сохраняемая проделанная работа. </param>
        public void SaveWorkItem(WorkItem workItem)
        {
            _logger.InfoFormat("Сохранение проделанной работы с Id = {0} для заказа {1}", workItem.WorkItemID, workItem.RepairOrderID);
            var context = CreateContext();

            var savedItem =
                context.WorkItems.FirstOrDefault(
                    di => di.WorkItemID == workItem.WorkItemID);

            if (workItem.WorkItemID == null || workItem.WorkItemIDGuid == Guid.Empty)
            {
                workItem.WorkItemIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                workItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.WorkItems.AddObject(workItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Сохранение проделанной работы с ID= {0} для заказа {1} успешно сохранена",
                               workItem.WorkItemID, workItem.RepairOrderID);
        }

        /// <summary>
        /// Получает проделанную работу по его ID.
        /// </summary>
        /// <param name="id">Код проделанной работы.</param>
        /// <returns>Проделанная работа, если существует.</returns>
        public WorkItemDTO GetWorkItem(Guid? id)
        {
            _logger.InfoFormat("Получение проделанной работы по Id = {0}", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);

            return context.WorkItems.Where(fs => fs.WorkItemID == rawId).
                Join(context.Users, item => item.UserID, user => user.UserID, (item, user) =>
                    new WorkItemDTO
                    {
                        EngineerFullName = user.LastName + " " + user.FirstName + " " + (user.MiddleName ?? string.Empty),
                        EventDate = item.EventDate,
                        Price = item.Price,
                        RepairOrderID = item.RepairOrderID,
                        Title = item.Title,
                        UserID = item.UserID,
                        WorkItemID = item.WorkItemID
                    }).FirstOrDefault();
        }

        /// <summary>
        /// Вычисление суммы по выполненным работам конеретного заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Значение суммы.</returns>
        public decimal? GetWorkItemsSum(Guid? repairOrderID)
        {
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(repairOrderID);
            return (decimal?)context.WorkItems.Where(i => i.RepairOrderID == rawId).Sum(i => ((double?)i.Price));
        }

        /// <summary>
        /// Получает список работ ID заказа.
        /// </summary>
        /// <param name="repairOrderId">Код заказа.</param>
        /// <returns>Список проделанных работ.</returns>
        public IEnumerable<WorkItem> GetWorkItems(Guid? repairOrderId)
        {
            _logger.InfoFormat("Получение списка проделанных заказов по Id заказа = {0}", repairOrderId);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(repairOrderId);

            return context.WorkItems.Where(fs => fs.RepairOrderID == rawId);
        }

        /// <summary>
        /// Получает список работ ID заказа.
        /// </summary>
        /// <param name="repairOrderId">Код заказа.</param>
        /// <returns>Список проделанных работ.</returns>
        public IEnumerable<WorkItemDTO> GetWorkItemDtos(Guid? repairOrderId)
        {
            _logger.InfoFormat("Получение списка пунктов выполненных работ Для заказа {0}", repairOrderId);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(repairOrderId);
            var items = context.WorkItems.Where(i => i.RepairOrderID == rawId);

            return items.Join(context.Users, item => item.UserID, user => user.UserID, (item, user) => new WorkItemDTO
            {
                EngineerFullName = user.LastName + " " + user.FirstName + " " + (user.MiddleName ?? string.Empty),
                EventDate = item.EventDate,
                Price = item.Price,
                RepairOrderID = item.RepairOrderID,
                Title = item.Title,
                UserID = item.UserID,
                WorkItemID = item.WorkItemID
            }).OrderBy(i => i.EventDate);
        }

        /// <summary>
        /// Удаляет из хранилища проделанную работу по его ID.
        /// </summary>
        /// <param name="id">Код проделанной работы.</param>
        public void DeleteWorkItem(Guid? id)
        {
            _logger.InfoFormat("Удаление проделанной работы по id ={0}", id);

            var context = CreateContext();
            var item = new WorkItem { WorkItemIDGuid = id };
            context.WorkItems.Attach(item);
            context.WorkItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Проделанная работа с id = {0} успешно удалена", id);
        }

        /// <summary>
        /// Получает статистическую информацию по выполненным работам определенного заказа.
        /// </summary>
        /// <param name="repairOrderID">Заказ.</param>
        /// <returns>Статистика.</returns>
        public ItemsInfo GetWorkItemsTotal(Guid? repairOrderID)
        {
            _logger.InfoFormat(
                "Получение статистики выполненных работ для заказа {0}", repairOrderID);

            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(repairOrderID);
            return context.WorkItems.Where(i => i.RepairOrderID == rawId).GroupBy(i => i.RepairOrderID).Select(i => new ItemsInfo
            {
                Amount = i.Sum(l => l.Price),
                Count = i.Count()
            }).FirstOrDefault();
        }

       #endregion WorkItem

        #region OrderTimeline

        /// <summary>
        ///   Сохраняет информацию о графике заказа.
        /// </summary>
        /// <param name="orderTimeline"> Сохраняемый пункт о графике работы над заказом. </param>
        public void SaveOrderTimeline(OrderTimeline orderTimeline)
        {
            _logger.InfoFormat("Сохранение графика заказа с Id = {0} для заказа {1}", orderTimeline.OrderTimelineID, orderTimeline.RepairOrderID);
            var context = CreateContext();

            var savedItem =
                context.OrderTimelines.FirstOrDefault(
                    di => di.OrderTimelineID == orderTimeline.OrderTimelineID);

            if (orderTimeline.OrderTimelineID == null || orderTimeline.OrderTimelineIDGuid == Guid.Empty)
            {
                orderTimeline.OrderTimelineIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                orderTimeline.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.OrderTimelines.AddObject(orderTimeline);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Сохранение графике проделанной работы с ID= {0} для заказа {1} успешно сохранена",
                               orderTimeline.OrderTimelineID, orderTimeline.RepairOrderID);
        }

        /// <summary>
        /// Получает график проделанной работы над заказом по его ID.
        /// </summary>
        /// <param name="id">Код графика заказа.</param>
        /// <returns>График заказа, если существует.</returns>
        public OrderTimeline GetOrderTimeline(Guid? id)
        {
            _logger.InfoFormat("Получение графика заказа по Id = {0}", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            return context.OrderTimelines.FirstOrDefault(fs => fs.OrderTimelineID == rawId);
        }

        /// <summary>
        /// Получает графики проделанной работы у заказов.
        /// </summary>
        /// <param name="repairOrderId">Код заказа.</param>
        /// <returns>График заказа, если существует.</returns>
        public IEnumerable<OrderTimeline> GetOrderTimelines(Guid? repairOrderId)
        {
            _logger.InfoFormat("Получение графиков заказа по Id = {0}", repairOrderId);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(repairOrderId);
            return context.OrderTimelines.Where(fs => fs.RepairOrderID == rawId).OrderBy(i=>i.EventDateTime);
        }

        /// <summary>
        /// Получение количества грфиков закзов.
        /// </summary>
        /// <param name="repairOrderId">Код заказа.</param>
        /// <returns>Количество.</returns>
        public int GetOrderTimelineCount(Guid? repairOrderId)
        {
            _logger.InfoFormat("Получение количества графиков заказа по Id = {0}", repairOrderId);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(repairOrderId);
            return context.OrderTimelines.Count(fs => fs.RepairOrderID == rawId);
        }

        private const string DeleteAllOrderTimelinesSql = "DELETE FROM OrderTimeline Where RepairOrderID = '{0}'";

        /// <summary>
        /// Удаляет все графики заказа определенного заказа.
        /// </summary>
        public void DeleteAllOrderTimelines(Guid? repairOrderID)
        {
            _logger.InfoFormat("Удаление всех графиков заказа {0}", repairOrderID);

            var context = CreateContext();
            context.ExecuteStoreCommand(string.Format(DeleteAllOrderTimelinesSql,
                                                      FormatUtils.GuidToString(repairOrderID)));
        }

        /// <summary>
        /// Удаляет из хранилища график заказа по его ID.
        /// </summary>
        /// <param name="id">Код графика заказа.</param>
        public void DeleteOrderTimeline(Guid? id)
        {
            _logger.InfoFormat("Удаление графика заказа по id ={0}", id);

            var context = CreateContext();
            var item = new OrderTimeline { OrderTimelineIDGuid = id };
            context.OrderTimelines.Attach(item);
            context.OrderTimelines.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("График заказас id = {0} успешно удален", id);
        }

        #endregion OrderTimeline

        #region RepairOrderServerHashItem

        /// <summary>
        /// Проверяет наличие серверного хэша заказа.
        /// </summary>
        /// <param name="repairOrderID">Код Заказа.</param>
        /// <returns>Признак существования.</returns>
        public bool RepairOrderServerHashItemExists(Guid? repairOrderID)
        {
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(repairOrderID);
            return context.RepairOrderServerHashItems.Any(i => i.RepairOrderServerHashID == rawId);
        }

        /// <summary>
        ///   Сохраняет информацию о серверном хэше заказа.
        /// </summary>
        /// <param name="repairOrderServerHashItem"> Сохраняемый пункт о серверном хэше заказа. </param>
        public void SaveRepairOrderServerHashItem(RepairOrderServerHashItem repairOrderServerHashItem)
        {
            _logger.InfoFormat("Сохранение серверного хэша заказа с Id = {0}", repairOrderServerHashItem.RepairOrderServerHashID);
            var context = CreateContext();

            var savedItem =
                context.RepairOrderServerHashItems.FirstOrDefault(
                    di => di.RepairOrderServerHashID == repairOrderServerHashItem.RepairOrderServerHashID);

            if (repairOrderServerHashItem.RepairOrderServerHashID == null || repairOrderServerHashItem.RepairOrderServerHashIDGuid == Guid.Empty)
            {
                repairOrderServerHashItem.RepairOrderServerHashIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                repairOrderServerHashItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.RepairOrderServerHashItems.AddObject(repairOrderServerHashItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Сохранение серверного хэша заказа с ID= {0} ",
                               repairOrderServerHashItem.RepairOrderServerHashID);
        }

        /// <summary>
        /// Получает серверных хэ заказа по его ID.
        /// </summary>
        /// <param name="id">Код серверного хэша заказа.</param>
        /// <returns>Серверный хэш заказа, если существует.</returns>
        public RepairOrderServerHashItem GetRepairOrderServerHashItem(Guid? id)
        {
            _logger.InfoFormat("Получение серверного хэша заказа по Id = {0}", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            return context.RepairOrderServerHashItems.FirstOrDefault(fs => fs.RepairOrderServerHashID == rawId);
        }

        /// <summary>
        /// Удаляет из хранилища серверный хэш заказа по его ID.
        /// </summary>
        /// <param name="id">Код серверного хэша заказа.</param>
        public void DeleteRepairOrderServerHashItem(Guid? id)
        {
            _logger.InfoFormat("Удаление графика заказа по id ={0}", id);

            var context = CreateContext();
            var item = new RepairOrderServerHashItem { RepairOrderServerHashIDGuid = id };
            context.RepairOrderServerHashItems.Attach(item);
            context.RepairOrderServerHashItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Серверный хэш заказа с id = {0} успешно удален", id);
        }

        private const string GetNewRepairOrderIdsSql = @"SELECT RepairOrderID FROM RepairOrder WHERE NOT EXISTS(SELECT * FROM RepairOrderServerHash WHERE RepairOrderServerHashID = RepairOrderID)";

        /// <summary>
        /// Возваращает все идентификаторы новых заказов.
        /// </summary>
        /// <returns>Заказ.</returns>
        public IEnumerable<Guid?> GetNewRepairOrderIds()
        {
            _logger.InfoFormat("Старт выполнения запроса на забор новых идентификаторов");
            var context = CreateContext();

            return context.ExecuteStoreQuery<string>(GetNewRepairOrderIdsSql).ToList().Select(FormatUtils.StringToNullGuid);
        }

        #endregion RepairOrderServerHashItem

        #region WorkItemServerHashItem

        /// <summary>
        ///   Сохраняет информацию о серверном хэше проделанной работы.
        /// </summary>
        /// <param name="workItemServerHashItem"> Сохраняемый пункт о серверном хэше проделанной работы. </param>
        public void SaveWorkItemServerHashItem(WorkItemServerHashItem workItemServerHashItem)
        {
            _logger.InfoFormat("Сохранение серверного хэша проделанной работы с Id = {0}", workItemServerHashItem.WorkItemServerHashID);
            var context = CreateContext();

            var savedItem =
                context.WorkItemServerHashItems.FirstOrDefault(
                    di => di.WorkItemServerHashID == workItemServerHashItem.WorkItemServerHashID);

            if (workItemServerHashItem.WorkItemServerHashID == null || workItemServerHashItem.WorkItemServerHashIDGuid == Guid.Empty)
            {
                workItemServerHashItem.WorkItemServerHashIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                workItemServerHashItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.WorkItemServerHashItems.AddObject(workItemServerHashItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Сохранение серверного хэша проделанной работы с ID= {0} ",
                               workItemServerHashItem.WorkItemServerHashID);
        }

        /// <summary>
        /// Получает серверный хэш проделенной работы по его ID.
        /// </summary>
        /// <param name="id">Код серверного хэша проделанной работы.</param>
        /// <returns>Серверный хэш проделенной работы, если существует.</returns>
        public WorkItemServerHashItem GetWorkItemServerHashItem(Guid? id)
        {
            _logger.InfoFormat("Получение серверного хэша проделанной работы по Id = {0}", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            return context.WorkItemServerHashItems.FirstOrDefault(fs => fs.WorkItemServerHashID == rawId);
        }

        /// <summary>
        /// Удаляет из хранилища серверный хэш проделанной работы по его ID.
        /// </summary>
        /// <param name="id">Код серверного хэша проделанной работы.</param>
        public void DeleteWorkItemServerHashItem(Guid? id)
        {
            _logger.InfoFormat("Удаление графика проделанной работы по id ={0}", id);

            var context = CreateContext();
            var item = new WorkItemServerHashItem { WorkItemServerHashIDGuid = id };
            context.WorkItemServerHashItems.Attach(item);
            context.WorkItemServerHashItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Серверный хэш проделанной работы с id = {0} успешно удален", id);
        }

        private const string DeleteAllWorkItemServerHashItemsSql = "DELETE FROM WorkItemServerHash Where RepairOrderServerHashID = '{0}'";

        /// <summary>
        /// Удаляет все номенклотуры товаров.
        /// </summary>
        public void DeleteAllWorkItemServerHashItems(Guid? repairOrderID)
        {
            _logger.InfoFormat("Удаление всех серверных хэшей выполненных работ заказа {0}", repairOrderID);

            var context = CreateContext();
            context.ExecuteStoreCommand(string.Format(DeleteAllWorkItemServerHashItemsSql,
                                                      FormatUtils.GuidToString(repairOrderID)));
        }

        /// <summary>
        /// Получает серверные хэши выполненных работ заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список серверных хэшей выполненных работ.</returns>
        public IEnumerable<WorkItemServerHashItem> GetWorkItemServerHashItems(Guid? repairOrderID)
        {
            _logger.InfoFormat("Получение серверных хэшей выполненных работ заказа = {0}", repairOrderID);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(repairOrderID);
            return context.WorkItemServerHashItems.Where(fs => fs.RepairOrderServerHashID == rawId);
        }
        
        #endregion WorkItemServerHashItem

        #region DeviceItemServerHashItems

        private const string DeleteAllDeviceItemServerHashItemsSql = "DELETE FROM DeviceItemServerHash Where RepairOrderServerHashID = '{0}'";

        /// <summary>
        /// Удаляет все номенклатуры товаров.
        /// </summary>
        public void DeleteAllDeviceItemServerHashItems(Guid? repairOrderID)
        {
            _logger.InfoFormat("Удаление всех серверных хэшей установленных запчастей заказа {0}", repairOrderID);

            var context = CreateContext();
            context.ExecuteStoreCommand(string.Format(DeleteAllDeviceItemServerHashItemsSql,
                                                      FormatUtils.GuidToString(repairOrderID)));
        }

        /// <summary>
        ///   Сохраняет информацию о серверном хэше установленной запчасти.
        /// </summary>
        /// <param name="deviceItemServerHashItem"> Сохраняемый пункт о серверном хэше установленной запчасти. </param>
        public void SaveDeviceItemServerHashItem(DeviceItemServerHashItem deviceItemServerHashItem)
        {
            _logger.InfoFormat("Сохранение серверного хэша установленной запчасти с Id = {0}", deviceItemServerHashItem.DeviceItemServerHashID);
            var context = CreateContext();

            var savedItem =
                context.DeviceItemServerHashItems.FirstOrDefault(
                    di => di.DeviceItemServerHashID == deviceItemServerHashItem.DeviceItemServerHashID);

            if (deviceItemServerHashItem.DeviceItemServerHashID == null || deviceItemServerHashItem.DeviceItemServerHashIDGuid == Guid.Empty)
            {
                deviceItemServerHashItem.DeviceItemServerHashIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                deviceItemServerHashItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.DeviceItemServerHashItems.AddObject(deviceItemServerHashItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Сохранение серверного хэша установленной запчасти с ID= {0} ",
                               deviceItemServerHashItem.DeviceItemServerHashID);
        }

        /// <summary>
        /// Получает серверные хэши установленных запчастей по заказам.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список серверных хэшей установленных запчастей.</returns>
        public IEnumerable<DeviceItemServerHashItem> GetDeviceItemServerHashItems(Guid? repairOrderID)
        {
            _logger.InfoFormat("Получение серверных хэшей установленных запчастей заказов = {0}", repairOrderID);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(repairOrderID);
            return context.DeviceItemServerHashItems.Where(fs => fs.RepairOrderServerHashID == rawId);
        }

        /// <summary>
        /// Получает серверный хэш установленной запчасти по его ID.
        /// </summary>
        /// <param name="id">Код серверного хэша установленной запчасти.</param>
        /// <returns>Серверный хэш установленной запчасти, если существует.</returns>
        public DeviceItemServerHashItem GetDeviceItemServerHashItem(Guid? id)
        {
            _logger.InfoFormat("Получение серверного хэша установленной запчасти по Id = {0}", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            return context.DeviceItemServerHashItems.FirstOrDefault(fs => fs.DeviceItemServerHashID == rawId);
        }

        /// <summary>
        /// Удаляет из хранилища серверный хэш установленной запчасти по его ID.
        /// </summary>
        /// <param name="id">Код серверного хэша установленной запчасти.</param>
        public void DeleteDeviceItemServerHashItem(Guid? id)
        {
            _logger.InfoFormat("Удаление графика установленной запчасти по id ={0}", id);

            var context = CreateContext();
            var item = new DeviceItemServerHashItem { DeviceItemServerHashIDGuid = id };
            context.DeviceItemServerHashItems.Attach(item);
            context.DeviceItemServerHashItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Серверный хэш установленной запчасти с id = {0} успешно удален", id);
        }

        #endregion DeviceItemServerHashItems

        #region CustomReportItem

        /// <summary>
        /// Получает список документов по определенному типу.
        /// </summary>
        /// <param name="documentKindID">Код типа документа.</param>
        /// <returns>Список документов.</returns>
        public IEnumerable<CustomReportItem> GetCustomReportItems(long? documentKindID)
        {
            _logger.InfoFormat("Получение списка документов без строки поиска");
            var context = CreateContext();
            return context.CustomReportItems.Where(i => i.DocumentKindID == documentKindID);
        }

        /// <summary>
        /// Получает список документов без фильтра.
        /// </summary>
        /// <returns>Список документов.</returns>
        public IEnumerable<CustomReportItem> GetCustomReportItems()
        {
            _logger.InfoFormat("Получение списка документов без строки поиска");
            var context = CreateContext();
            return context.CustomReportItems;
        }

        /// <summary>
        ///   Сохраняет информацию документе.
        /// </summary>
        /// <param name="customReportItem"> Сохраняемый документ. </param>
        public void SaveCustomReportItem(CustomReportItem customReportItem)
        {
            _logger.InfoFormat("Сохранение документа с Id = {0}", customReportItem.CustomReportID);
            var context = CreateContext();

            var savedItem =
                context.CustomReportItems.FirstOrDefault(
                    di =>
                    di.CustomReportID == customReportItem.CustomReportID);

            if (customReportItem.CustomReportID == null || customReportItem.CustomReportIDGuid == Guid.Empty)
            {
                customReportItem.CustomReportIDGuid = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                customReportItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.CustomReportItems.AddObject(customReportItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Документ с ID= {0} успешно сохранен",
                               customReportItem.CustomReportID);
        }

        /// <summary>
        /// Получает документ по его ID.
        /// </summary>
        /// <param name="id">Код описания документа.</param>
        /// <returns>Документ, если существует.</returns>
        public CustomReportItem GetCustomReportItem(Guid? id)
        {
            _logger.InfoFormat("Получение документа по Id = {0}", id);
            var context = CreateContext();
            var rawId = FormatUtils.GuidToString(id);
            return
                context.CustomReportItems.FirstOrDefault(
                    fs => fs.CustomReportID == rawId);
        }

        /// <summary>
        /// Удаляет из хранилища документ по его ID.
        /// </summary>
        /// <param name="id">Код документа.</param>
        public void DeleteCustomReportItem(Guid? id)
        {
            _logger.InfoFormat("Удаление документ id ={0}", id);

            var context = CreateContext();
            var item = new CustomReportItem { CustomReportIDGuid = id };
            context.CustomReportItems.Attach(item);
            context.CustomReportItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Документ id = {0} успешно удален", id);
        }

        private const string CustomReportItemsSql = "DELETE FROM CustomReport";

        /// <summary>
        /// Удаляет все привязки филиалов и пользователей.
        /// </summary>
        public void DeleteAllCustomReportItems()
        {
            _logger.InfoFormat("Удаление всех документов");

            var context = CreateContext();
            context.ExecuteStoreCommand(CustomReportItemsSql);
        }

        #endregion CustomReportItem
    }
}
