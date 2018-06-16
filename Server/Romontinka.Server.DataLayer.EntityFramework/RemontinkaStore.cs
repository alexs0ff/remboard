using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;
using System.Text.RegularExpressions;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.DataLayer.Entities.HashItems;
using Romontinka.Server.DataLayer.Entities.ReportItems;
using log4net;

namespace Romontinka.Server.DataLayer.EntityFramework
{
    //select CONVERT(NVARCHAR(32),HashBytes('MD5','БОрис' COLLATE Cyrillic_General_100_CS_AI),2)

    public class RemontinkaStore : IDataStore
    {
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
            if (value==null)
            {
                value = string.Empty;
            }
        }

        #region Deploy

        /// <summary>
        /// Вызывает скрипт развертывания данных в определенном домене.
        /// </summary>
        /// <param name="userDomainID">Код домена.</param>
        public void Deploy(Guid? userDomainID)
        {
            _logger.InfoFormat("Вызываем скрипт деплоя для домена {0}",userDomainID);

            var commonSqlReader = new EmbeddedResourceReader("DomainDeploy.sql", typeof (RemontinkaStore), "Resources");
            var sqlToDeploy = commonSqlReader.Read();

            var report1Reader = new EmbeddedResourceReader("Report1.txt", typeof(RemontinkaStore), "Resources");//Гарантийная
            var report2Reader = new EmbeddedResourceReader("Report2.txt", typeof(RemontinkaStore), "Resources");//Приемная
            var report3Reader = new EmbeddedResourceReader("Report3.txt", typeof(RemontinkaStore), "Resources");//Товарный чек



            sqlToDeploy = string.Format(sqlToDeploy, userDomainID, report1Reader.Read(), report2Reader.Read(),
                                        report3Reader.Read());

            var context = CreateContext();
            context.ExecuteStoreCommand(sqlToDeploy, null);
        }

        #endregion Deploy

        #region Branch

        /// <summary>
        /// Получает список филиалов с фильтром.
        /// </summary>
        /// <param name="userDomainID">Задает или получает код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список филиалов.</returns>
        public IEnumerable<Branch> GetBranches(Guid? userDomainID,string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка филиалов со строкой поиска {0} страница {1} для домена {2}", name, page,userDomainID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.Branches.Where(i => i.Title.Contains(name) && i.UserDomainID == userDomainID);
            count = items.Count();

            return items.OrderBy(i => i.Title).Page(page, pageSize);
        }

        /// <summary>
        /// Получает список филиалов без фильтра.
        /// </summary>
        /// <returns>Список филиалов.</returns>
        public IQueryable<Branch> GetBranches(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списка филиалов без строки поиска по домену {0}",userDomainID);
            var context = CreateContext();
            return context.Branches.Where(i => i.UserDomainID == userDomainID);
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
                    di => di.BranchID == branch.BranchID && di.UserDomainID == branch.UserDomainID);

            if (branch.BranchID == null || branch.BranchID == Guid.Empty)
            {
                branch.BranchID = Guid.NewGuid();
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
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Филиал, если существует.</returns>
        public Branch GetBranch(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение филиала по Id = {0} по домену {1}", id,userDomainID);
            var context = CreateContext();
            return context.Branches.FirstOrDefault(fs => fs.BranchID == id && fs.UserDomainID==userDomainID);
        }

        /// <summary>
        /// Удаляет из хранилища филиал по его ID.
        /// </summary>
        /// <param name="id">Код филиала.</param>
        public void DeleteBranch(Guid? id)
        {
            _logger.InfoFormat("Удаление филиала id ={0}", id);

            var context = CreateContext();
            var item = new Branch { BranchID = id };
            context.Branches.Attach(item);
            context.Branches.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Филиал id = {0} успешно удален", id);
        }

        #endregion Branch

        #region UserDomain

        /// <summary>
        /// Проверяет на наличие email в доменах регистрации.
        /// </summary>
        /// <param name="email">Электронный адрес для проверки.</param>
        /// <returns>Признак наличия email.</returns>
        public bool UserDomainEmailIsExists(string email)
        {
            _logger.InfoFormat("Проверяем наличия зарегистрированного email для доменов: {0}",email);

            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var context = CreateContext();
            email = email.ToUpper();
            return context.UserDomains.Any(d => d.RegistredEmail.ToUpper() == email);
        }

        /// <summary>
        /// Осуществляет проверку, что логин пользователя существует, но не активирован.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <returns>Существование логина и его неактивация.</returns>
        public bool UserDomainLoginIsExistsAndNonActivated(string login)
        {
            _logger.InfoFormat("Проверка на существования неактивированного логина {0}",login);

            if(string.IsNullOrWhiteSpace(login))
            {
                return false;
            }

            var context = CreateContext();
            login = login.ToUpper();

            return context.UserDomains.Any(d => d.IsActive == false && d.UserLogin.ToUpper() == login);
        }

        /// <summary>
        ///   Сохраняет информацию домене пользователей.
        /// </summary>
        /// <param name="userDomain"> Сохраняемый домен пользователей. </param>
        public void SaveUserDomain(UserDomain userDomain)
        {
            _logger.InfoFormat("Сохранение домена пользователя с Id = {0}", userDomain.UserDomainID);
            var context = CreateContext();

            var savedItem =
                context.UserDomains.FirstOrDefault(
                    di => di.UserDomainID == userDomain.UserDomainID);

            if (userDomain.UserDomainID == null || userDomain.UserDomainID == Guid.Empty)
            {
                userDomain.UserDomainID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                userDomain.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.UserDomains.AddObject(userDomain);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Филиал с ID= {0} успешно сохранен",
                               userDomain.UserDomainID);
        }

        /// <summary>
        /// Получает домен пользователя по его ID.
        /// </summary>
        /// <param name="id">Код домена пользователя.</param>
        /// <returns>Домен пользователя, если существует.</returns>
        public UserDomain GetUserDomain(Guid? id)
        {
            _logger.InfoFormat("Получение домена пользователя по Id = {0}", id);
            var context = CreateContext();
            return context.UserDomains.FirstOrDefault(fs => fs.UserDomainID == id);
        }

        /// <summary>
        /// Получает домен по номеру домена.
        /// </summary>
        /// <param name="number">Номер домена.</param>
        /// <returns>Домен пользователя, если существует.</returns>
        public UserDomain GetUserDomain(int number)
        {
            _logger.InfoFormat("Получение домена пользователя по номеру = {0}", number);
            var context = CreateContext();
            return context.UserDomains.FirstOrDefault(fs => fs.Number == number);
        }

        /// <summary>
        /// Получает код домен пользователя по ID пользователя.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <returns>Код домена пользователя, если существует.</returns>
        public Guid? GetUserDomainByUserID(Guid? userID)
        {
            _logger.InfoFormat("Получение кода домена пользователя по Id пользователя = {0}", userID);
            var context = CreateContext();
            return context.Users.Where(i=>i.UserID==userID).Select(i=>i.UserDomainID).FirstOrDefault();
        }

        /// <summary>
        /// Удаляет из хранилища домен пользователя по его ID.
        /// </summary>
        /// <param name="id">Домен пользователя.</param>
        public void DeleteUserDomain(Guid? id)
        {
            _logger.InfoFormat("Удаление домена пользователя id ={0}", id);

            var context = CreateContext();
            var item = new UserDomain { UserDomainID = id };
            context.UserDomains.Attach(item);
            context.UserDomains.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Домен пользователя id = {0} успешно удален", id);
        }

        #endregion UserDomain
        
        #region Users

        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список пользователей.</returns>
        public IQueryable<User> GetUsers(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списка всех пользователей Для домена {0}",userDomainID);
            var context = CreateContext();
            return context.Users.Where(i => i.UserDomainID == userDomainID);
        }

        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        /// <param name="projectRoleId">Код роли в проекте. </param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список пользователей.</returns>
        public IEnumerable<User> GetUsers(byte? projectRoleId, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списка всех пользователей с ролью {0} для домена {1}",projectRoleId,userDomainID);
            var context = CreateContext();
            IQueryable<User> result;

            if (projectRoleId!=null)
            {
                result = context.Users.Where(u => u.ProjectRoleID == projectRoleId && u.UserDomainID == userDomainID);
            }
            else
            {
                result = context.Users.Where(u => u.UserDomainID == userDomainID);
            }

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
            _logger.InfoFormat("Получение списка пользователей со строкой поиска {0} страница {1} и домена {2}", name, page,userDomainID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items =
                context.Users.Where(
                    i => (i.LastName + i.LastName + i.FirstName).Contains(name) && i.UserDomainID == userDomainID);
            count = items.Count();

            return items.OrderBy(i => i.FirstName).Page(page, pageSize);
        }

        /// <summary>
        /// Получение пользователя по логину.
        /// </summary>
        /// <param name="loginName">Логин пользователя.</param>
        /// <returns>Пользователь, если найден.</returns>
        public User GetUser(string loginName)
        {
            var context = CreateContext();

            var user =context.Users.FirstOrDefault(u => u.LoginName == loginName);
            if (user==null)
            {
                _logger.WarnFormat("Нет пользователя с логином {0}", loginName);
            } //if
            return user;
        }

        /// <summary>
        /// Получение пользователя по логину и хэшу пароля.
        /// </summary>
        /// <param name="loginName">Логин пользователя.</param>
        /// <param name="passwordHash">Хэш пароля.</param>
        /// <returns>Пользователь, если найден.</returns>
        public User GetUser(string loginName,string passwordHash)
        {
            _logger.InfoFormat("Получение пользователя по логину {0} и хэшу {1}", loginName,passwordHash);
            var context = CreateContext();

            return context.Users.FirstOrDefault(u => u.LoginName == loginName&& u.PasswordHash==passwordHash);
        }

        /// <summary>
        /// Скрипт обновления пароля.
        /// </summary>
        private const string UserPasswordHashUpdate = "UPDATE [dbo].[User] SET [PasswordHash] = {0} WHERE [UserID] = {1}";

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
            user.UserID = Guid.NewGuid();

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
                context.Users.FirstOrDefault(di => di.UserID == user.UserID && di.UserDomainID == user.UserDomainID);

            if (user.UserID == null || user.UserID == Guid.Empty)
            {
                user.UserID = Guid.NewGuid();
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
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="id">Код пользователя.</param>
        /// <returns>Пользователь, если существует.</returns>
        public User GetUser(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение пользователя по Id = {0} для домена {1}", id,userDomainID);
            var context = CreateContext();
            return context.Users.FirstOrDefault(fs => fs.UserID == id && fs.UserDomainID == userDomainID);
        }

        /// <summary>
        /// Получает пользователя по его ID.
        /// </summary>
        /// <param name="id">Код пользователя.</param>
        /// <returns>Пользователь, если существует.</returns>
        public User GetUser(Guid? id)
        {
            _logger.InfoFormat("Получение пользователя по Id = {0}", id);
            var context = CreateContext();
            return context.Users.FirstOrDefault(fs => fs.UserID == id);
        }

        private const string CleanUpUserSql = @"

        UPDATE RepairOrder
        SET IssuerID = {1}
        WHERE IssuerID = {0}

        UPDATE RepairOrder
        SET EngineerID = {1}
        WHERE EngineerID = {0}

        UPDATE RepairOrder
        SET ManagerID = {1}
        WHERE ManagerID = {0}

        DELETE FROM UserPublicKeyRequest        
        WHERE UserID = {0}

        DELETE FROM UserPublicKey        
        WHERE UserID = {0}

        UPDATE DeviceItem
        SET UserID = {1}
        WHERE UserID = {0}

        DELETE FROM UserInterest        
        WHERE UserID = {0}

        UPDATE TransferDoc
        SET CreatorID = {1}
        WHERE CreatorID = {0}

        UPDATE CancellationDoc
        SET CreatorID = {1}
        WHERE CreatorID = {0}

        UPDATE IncomingDoc
        SET CreatorID = {1}
        WHERE CreatorID = {0}

        UPDATE WorkItem
        SET UserID = {1}
        WHERE UserID = {0}
";

        /// <summary>
        /// Подчищает все ссылки на пользователя.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="userID">Код пользователя.</param>
        public void CleanUpUser(Guid? userDomainID,Guid? userID)
        {
            _logger.InfoFormat("Старт очистки при удалении пользователя. {0}",userID);
            var context = CreateContext();
            var user = context.Users.FirstOrDefault(u => u.UserID == userID && u.UserDomainID == userDomainID);
            if (user == null)
            {
                _logger.ErrorFormat("Пользователь для очистки не найден {0} {1}",userID, userDomainID);
                return;
            }

            var domain = context.UserDomains.FirstOrDefault(d => d.UserDomainID == userDomainID);

            if (string.Equals(domain.UserLogin, user.LoginName))
            {
                _logger.ErrorFormat("Нельзя очистить главного пользователя домена {0}",domain.UserLogin);
                return;
            }

            var mainUser = context.Users.FirstOrDefault(u => u.LoginName.ToUpper() == domain.UserLogin.ToUpper());

            if (mainUser == null)
            {
                _logger.ErrorFormat("Главный пользователь не найден {0}", domain.UserLogin);
                return;
            }

            context.ExecuteStoreCommand(CleanUpUserSql, userID, mainUser.UserID);

            DeleteUserBranchMapItems(userID);

        }

        /// <summary>
        /// Удаляет из хранилища пользователя по его ID.
        /// </summary>
        /// <param name="id">Код пользователя.</param>
        public void DeleteUser(Guid? id)
        {
            _logger.InfoFormat("Удаление пользователя id ={0}", id);

            var context = CreateContext();
            var item = new User { UserID = id };
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

            if(string.IsNullOrWhiteSpace(login))
            {
                return false;
            }

            var context = CreateContext();
            login = login.ToUpper();
            return context.Users.Any(u => u.LoginName.ToUpper() == login);
        }

        #endregion Users

        #region DeviceItems

        /// <summary>
        /// Вычисление суммы по установленным запчастям конеретного заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Значение суммы.</returns>
        public decimal? GetDeviceItemsSum(Guid? repairOrderID)
        {
            var context = CreateContext();
            return context.DeviceItems.Where(i => i.RepairOrderID == repairOrderID).Sum(i => ((decimal?)i.Price));
        }

        /// <summary>
        /// Получает список пунктов установленных запчастей для определенного заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список пунктов запчастей.</returns>
        public IQueryable<DeviceItemDTO> GetDeviceItems(Guid? repairOrderID)
        {
            _logger.InfoFormat(
                "Получение списка пунктов установленных запчастей для заказа {0}", repairOrderID);
            var context = CreateContext();
            return context.DeviceItems.Where(i => i.RepairOrderID == repairOrderID).Join(context.Users,item => item.UserID,user => user.UserID,(item, user) => new DeviceItemDTO
            {
                Title = item.Title,
                Count = item.Count,
                CostPrice = item.CostPrice,
                DeviceItemID = item.DeviceItemID,
                RepairOrderID = item.RepairOrderID,
                UserID = item.UserID,
                WarehouseItemID = item.WarehouseItemID,
                EventDate = item.EventDate,
                Price = item.Price,
                EngineerFullName = user.LastName + " " + user.FirstName + " " + (user.MiddleName ?? string.Empty)
            });
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


            return context.DeviceItems.Where(i=>i.RepairOrderID==repairOrderID).GroupBy(i => i.RepairOrderID).Select(i=>new ItemsInfo
                                                                            {
                                                                               Amount = i.Sum(l=>l.Price),
                                                                               Count = i.Count(),
                                                                               SumCount = i.Sum(l=>l.Count),
                                                                               TotalAmount = i.Sum(l=>l.Price*l.Count)
                                                                            }).FirstOrDefault();
        }

        /// <summary>
        /// Получает список запчастей для заказас фильтром.
        /// </summary>
        /// <param name="repairOrderID">Код заказа выполненных работ.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список запчатей заказа.</returns>
        public IEnumerable<DeviceItem> GetDeviceItems(Guid? repairOrderID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка запчастей заказа со строкой поиска {0} страница {1} Для заказа {2}", name, page, repairOrderID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.DeviceItems.Where(i => i.RepairOrderID == repairOrderID && i.Title.Contains(name));
            count = items.Count();
            return items.OrderBy(i => i.Title).Page(page, pageSize);
        }

        private const string SaveDeviceItemSql = @"

BEGIN TRY
BEGIN TRAN

declare @DeviceItemID uniqueidentifier,
		@UserID uniqueidentifier,
		@Title varchar(255),
		@Count numeric(18,8),
		@CostPrice numeric(18,8),
		@Price numeric(18,8),
		@RepairOrderID uniqueidentifier,
		@EventDate smalldatetime,
		@WarehouseItemID uniqueidentifier,
		@OldWarehouseItemID uniqueidentifier,
		@OldCount numeric(18,8)


SELECT
	@DeviceItemID = {0},
	@UserID={1},
	@Title={2},
	@Count={3},
	@CostPrice={4},
	@Price={5},
	@RepairOrderID={6},
	@EventDate={7},
	@WarehouseItemID={8}

IF EXISTS(SELECT * FROM DeviceItem WHERE DeviceItemID=@DeviceItemID)
BEGIN
	
	SELECT @OldWarehouseItemID = WarehouseItemID,@OldCount=[Count] FROM DeviceItem WHERE DeviceItemID=@DeviceItemID
	
	IF(@OldWarehouseItemID IS NOT NULL)
	BEGIN
		UPDATE
			WarehouseItem
		SET
			Total = Total+@OldCount
		WHERE WarehouseItemID = @OldWarehouseItemID
	END


	UPDATE [DeviceItem]
	   SET 
		  [UserID] = @UserID,
		  [Title] = @Title,
		  [Count] = @Count,
		  [CostPrice] = @CostPrice,
		  [Price] = @Price,
		  [RepairOrderID] = @RepairOrderID,
		  [EventDate] = @EventDate,
		  [WarehouseItemID] = @WarehouseItemID
	 WHERE DeviceItemID=@DeviceItemID

	 
END
ELSE
BEGIN

INSERT INTO [DeviceItem]
           ([DeviceItemID]
           ,[UserID]
           ,[Title]
           ,[Count]
           ,[CostPrice]
           ,[Price]
           ,[RepairOrderID]
           ,[EventDate]
           ,[WarehouseItemID])
     VALUES
           (
			@DeviceItemID,
			@UserID,
			@Title,
			@Count,
			@CostPrice,
			@Price,
			@RepairOrderID,
			@EventDate,
			@WarehouseItemID
		   )

END
	
	IF(@WarehouseItemID IS NOT NULL)
	BEGIN
		UPDATE
			WarehouseItem
		SET
			Total = Total-@Count
		WHERE WarehouseItemID = @WarehouseItemID
	END

COMMIT TRAN
SELECT 'Успех' As ErrorMessage, Cast(1 as bit) As ProcessResult	
	
END TRY	 	
	BEGIN CATCH
	ROLLBACK TRAN
		SELECT ERROR_MESSAGE() As ErrorMessage, Cast(0 as bit) As ProcessResult	
		
END CATCH

";

        /// <summary>
        ///   Сохраняет информацию замененой запчасти.
        /// </summary>
        /// <param name="deviceItem"> Сохраняемая запчасть. </param>
        public void SaveDeviceItem(DeviceItem deviceItem)
        {
            _logger.InfoFormat("Сохранение запчасти с Id = {0} для заказа {1}", deviceItem.DeviceItemID, deviceItem.RepairOrderID);
            var context = CreateContext();

            if (deviceItem.DeviceItemID==null)
            {
                deviceItem.DeviceItemID = Guid.NewGuid();
            } //if

            var result = context.ExecuteStoreQuery<ProcessWarehouseDocResult>(SaveDeviceItemSql, deviceItem.DeviceItemID,
                                                                              deviceItem.UserID, deviceItem.Title,
                                                                              deviceItem.Count, deviceItem.CostPrice,
                                                                              deviceItem.Price, deviceItem.RepairOrderID,
                                                                              deviceItem.EventDate,
                                                                              deviceItem.WarehouseItemID).FirstOrDefault();

            if (result==null ||result.ProcessResult==false)
            {
                string message = "Ошибка обработки";
                if (result!=null)
                {
                    message =result.ErrorMessage;
                } //if
                throw new Exception(message);
            } //if

            _logger.InfoFormat("Запчасть с ID= {0} для заказа {1} успешно сохранена",
                               deviceItem.DeviceItemID, deviceItem.RepairOrderID);
        }

        /// <summary>
        /// Получает запчасти по его ID.
        /// </summary>
        /// <param name="id">Код запчасти.</param>
        /// <returns>Запчасть, если существует.</returns>
        public DeviceItem GetDeviceItem(Guid? id)
        {
            _logger.InfoFormat("Получение запчасти по Id = {0}", id);
            var context = CreateContext();
            return context.DeviceItems.FirstOrDefault(fs => fs.DeviceItemID == id);
        }

        private const string DeleteDeviceSql = @"
        BEGIN TRY
        BEGIN TRAN

        declare @DeviceItemID uniqueidentifier,		
		        @OldWarehouseItemID uniqueidentifier,
		        @OldCount numeric(18,8)

        declare	@deleteddows table (id uniqueidentifier)

        SELECT
	        @DeviceItemID = {0}	


        SELECT @OldWarehouseItemID = WarehouseItemID,@OldCount=[Count] FROM DeviceItem WHERE DeviceItemID=@DeviceItemID

        DELETE
		        DeviceItem
		        OUTPUT DELETED.DeviceItemID INTO @deleteddows
	        WHERE [DeviceItemID] = @DeviceItemID

        IF EXISTS(SELECT * FROM @deleteddows)
        BEGIN
	
	        IF(@OldWarehouseItemID IS NOT NULL)
	        BEGIN
		        UPDATE
			        WarehouseItem
		        SET
			        Total = Total+@OldCount
		        WHERE WarehouseItemID = @OldWarehouseItemID
	        END
		 
        END

        COMMIT TRAN
        SELECT 'Успех' As ErrorMessage, Cast(1 as bit) As ProcessResult	
	
        END TRY	 	
	        BEGIN CATCH
	        ROLLBACK TRAN
		        SELECT ERROR_MESSAGE() As ErrorMessage, Cast(0 as bit) As ProcessResult	
		
        END CATCH
";

        /// <summary>
        /// Удаляет из хранилища запчасть по его ID.
        /// </summary>
        /// <param name="id">Код запчасти.</param>
        public void DeleteDeviceItem(Guid? id)
        {
            _logger.InfoFormat("Удаление запчасти id ={0}", id);

            var context = CreateContext();
            var result = context.ExecuteStoreQuery<ProcessWarehouseDocResult>(DeleteDeviceSql, id).FirstOrDefault();

            if (result == null || result.ProcessResult == false)
            {
                string message = "Ошибка обработки";
                if (result != null)
                {
                    message = result.ErrorMessage;
                } //if
                throw new Exception(message);
            } //if

            _logger.InfoFormat("Запчасть id = {0} успешно удалена", id);
        }
        

        /// <summary>
        /// Удаляет из хранилища запчасти заказа по ID заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        public void DeleteDeviceItemByRepairOrder(Guid? repairOrderID)
        {
            _logger.InfoFormat("Удаление запчастей заказа по для заказа ={0}", repairOrderID);

            var context = CreateContext();

            var ids = context.DeviceItems.Where(i => i.RepairOrderID == repairOrderID).Select(i=>i.DeviceItemID).ToList();

            foreach (var deviceItem in ids)
            {
                DeleteDeviceItem(deviceItem);
            } //foreach


            _logger.InfoFormat("Запчасти по заказу с id = {0} успешно удалены", repairOrderID);
        }

        #endregion DeviceItems

        #region WorkItems

        /// <summary>
        /// Вычисление суммы по выполненным работам конеретного заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Значение суммы.</returns>
        public decimal? GetWorkItemsSum(Guid? repairOrderID)
        {
            var context = CreateContext();
            return context.WorkItems.Where(i => i.RepairOrderID == repairOrderID).Sum(i => ((decimal?)i.Price));
        }

        /// <summary>
        /// Получает список пунктов выполненных работ для определенного заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список пунктов заказа.</returns>
        public IQueryable<WorkItemDTO> GetWorkItems(Guid? repairOrderID)
        {
            _logger.InfoFormat(
                "Получение списка пунктов выполненных работ для заказа {0}",repairOrderID);
            var context = CreateContext();
            return context.WorkItems.Where(i => i.RepairOrderID == repairOrderID).Join(context.Users, item => item.UserID, user => user.UserID, (item, user) => new WorkItemDTO
            {
                EngineerFullName = user.LastName + " " + user.FirstName + " " + (user.MiddleName ?? string.Empty),
                EventDate = item.EventDate,
                Price = item.Price,
                RepairOrderID = item.RepairOrderID,
                Title = item.Title,
                UserID = item.UserID,
                WorkItemID = item.WorkItemID,
                Notes = item.Notes
            });
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

            return context.WorkItems.Where(i=>i.RepairOrderID==repairOrderID).GroupBy(i => i.RepairOrderID).Select(i => new ItemsInfo
            {
                Amount = i.Sum(l => l.Price),
                Count = i.Count()
            }).FirstOrDefault();
        }

        /// <summary>
        /// Получает список пунктов выполненных работ с фильтром.
        /// </summary>
        /// <param name="repairOrderID">Код пункта выполненных работ.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список пунктов выполненных работ.</returns>
        public IEnumerable<WorkItemDTO> GetWorkItems(Guid? repairOrderID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка пунктов выполненных работ со строкой поиска {0} страница {1} Для заказа {2}", name, page, repairOrderID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.WorkItems.Where(i => i.RepairOrderID == repairOrderID && i.Title.Contains(name));
            count = items.Count();

            return items.Join(context.Users, item => item.UserID, user => user.UserID, (item, user) => new WorkItemDTO
                                                                                                    {
                                                                                                        EngineerFullName = user.LastName + " " + user.FirstName + " " + (user.MiddleName ?? string.Empty),
                                                                                                        EventDate = item.EventDate,
                                                                                                        Price = item.Price,
                                                                                                        RepairOrderID = item.RepairOrderID,
                                                                                                        Title = item.Title,
                                                                                                        UserID = item.UserID,
                                                                                                        WorkItemID = item.WorkItemID,
                                                                                                        Notes = item.Notes
                                                                                                    }).OrderBy(i => i.EventDate).Page(page, pageSize);
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

            if (workItem.WorkItemID == null || workItem.WorkItemID == Guid.Empty)
            {
                workItem.WorkItemID = Guid.NewGuid();
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
            return context.WorkItems.Where(fs => fs.WorkItemID == id).
                Join(context.Users, item => item.UserID, user => user.UserID, (item, user) => 
                    new WorkItemDTO
                    {
                        EngineerFullName = user.LastName + " " + user.FirstName+" " +(user.MiddleName??string.Empty),
                        EventDate = item.EventDate,
                        Price = item.Price,
                        RepairOrderID = item.RepairOrderID,
                        Title = item.Title,
                        UserID = item.UserID,
                        WorkItemID = item.WorkItemID,
                        Notes = item.Notes
                    }).FirstOrDefault();
        }

        /// <summary>
        /// Удаляет из хранилища проделанную работу по его ID.
        /// </summary>
        /// <param name="id">Код проделанной работы.</param>
        public void DeleteWorkItem(Guid? id)
        {
            _logger.InfoFormat("Удаление проделанной работы по id ={0}", id);

            var context = CreateContext();
            var item = new WorkItem { WorkItemID = id };
            context.WorkItems.Attach(item);
            context.WorkItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Проделанная работа с id = {0} успешно удалена", id);
        }

        private const string DeleteWorkItemByRepairOrderSql = "DELETE FROM WorkItem WHERE RepairOrderID = {0}";

        /// <summary>
        /// Удаляет из хранилища работы заказа по ID заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        public void DeleteWorkItemByRepairOrder(Guid? repairOrderID)
        {
            _logger.InfoFormat("Удаление работы по заказа по для заказа ={0}", repairOrderID);

            var context = CreateContext();
            context.ExecuteStoreCommand(DeleteWorkItemByRepairOrderSql, repairOrderID);

            _logger.InfoFormat("Работы по заказу с id = {0} успешно удалены", repairOrderID);
        }

        #endregion WorkItems

        #region OrderKinds

        /// <summary>
        /// Получает список типов заказа с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователей. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список типов заказа.</returns>
        public IEnumerable<OrderKind> GetOrderKinds(Guid? userDomainID,string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка типов заказа со строкой поиска {0} страница {1}", name, page);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.OrderKinds.Where(i => i.Title.Contains(name) && i.UserDomainID==userDomainID);
            count = items.Count();

            return items.OrderBy(i => i.Title).Page(page, pageSize);
        }

        /// <summary>
        /// Получает полный список типов заказа.
        /// </summary>
        /// <returns>Список типов заказа.</returns>
        public IQueryable<OrderKind> GetOrderKinds(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение полный список типов заказа для домена {0}",userDomainID);
            var context = CreateContext();
            return context.OrderKinds.Where(k=>k.UserDomainID==userDomainID);
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
                    di => di.OrderKindID == orderKind.OrderKindID && di.UserDomainID == orderKind.UserDomainID);

            if (orderKind.OrderKindID ==null)
            {
                orderKind.OrderKindID = Guid.NewGuid();
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
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <returns>Тип заказа, если существует.</returns>
        public OrderKind GetOrderKind(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение типа заказа по Id = {0} для домена {1}", id,userDomainID);
            var context = CreateContext();
            return context.OrderKinds.FirstOrDefault(fs => fs.OrderKindID == id && fs.UserDomainID == userDomainID);
        }

        /// <summary>
        /// Удаляет из хранилища тип заказа по его ID.
        /// </summary>
        /// <param name="id">Код типа заказа.</param>
        public void DeleteOrderKind(Guid? id)
        {
            _logger.InfoFormat("Удаление типа заказа по id ={0}", id);

            var context = CreateContext();
            var item = new OrderKind { OrderKindID = id };
            context.OrderKinds.Attach(item);
            context.OrderKinds.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Тип заказа с id = {0} успешно удален", id);
        }

        #endregion OrderKinds

        #region OrderStatus

        /// <summary>
        /// Получает список статусов заказов с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список статусов заказа.</returns>
        public IEnumerable<OrderStatus> GetOrderStatuses(Guid? userDomainID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка статусов заказов со строкой поиска {0} страница {1}", name, page);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.OrderStatuses.Where(i => i.Title.Contains(name) && i.UserDomainID== userDomainID);
            count = items.Count();

            return items.OrderBy(i => i.Title).Page(page, pageSize);
        }

        /// <summary>
        /// Получает список статусов заказов с фильтром.
        /// </summary>
        /// <returns>Список статусов заказа.</returns>
        public IQueryable<OrderStatus> GetOrderStatuses(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение всего списка статусов");
            
            var context = CreateContext();
            return context.OrderStatuses.Where(s => s.UserDomainID == userDomainID);
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
                    di => di.OrderStatusID == orderStatus.OrderStatusID && di.UserDomainID == orderStatus.UserDomainID);

            if (orderStatus.OrderStatusID == null)
            {
                orderStatus.OrderStatusID = Guid.NewGuid();
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
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <param name="kindId">Тип статуса.</param>
        /// <returns>Если не находит пытается найти ближайший по смыслу.</returns>
        public OrderStatus GetOrderStatusByKind(Guid? userDomainID, byte? kindId)
        {
            _logger.InfoFormat("Получение статусов заказов по его id для домена {0}",userDomainID);
            var context = CreateContext();

            var status = context.OrderStatuses.Where(s => s.StatusKindID == kindId && s.UserDomainID==userDomainID).OrderBy(i=>i.StatusKindID).FirstOrDefault();
            if (status==null)
            {
                status =
                    context.OrderStatuses.Where(s => s.StatusKindID > kindId && s.UserDomainID==userDomainID).OrderBy(
                        i => new {i.StatusKindID, i.Title}).FirstOrDefault();
                if (status==null)
                {
                    status = context.OrderStatuses.Where(s=>s.UserDomainID==userDomainID).OrderBy(i => i.StatusKindID).FirstOrDefault();
                } //if
            } //if

            return status;
        }

        /// <summary>
        /// Получает статус заказа по его ID.
        /// </summary>
        /// <param name="id">Код статуса заказа.</param>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <returns>Статус заказа, если существует.</returns>
        public OrderStatus GetOrderStatus(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение статуса заказа по Id = {0} для домена {1}", id,userDomainID);
            var context = CreateContext();
            return context.OrderStatuses.FirstOrDefault(fs => fs.OrderStatusID == id && fs.UserDomainID==userDomainID);
        }

        /// <summary>
        /// Удаляет из хранилища статус заказа по его ID.
        /// </summary>
        /// <param name="id">Код статуса заказа.</param>
        public void DeleteOrderStatus(Guid? id)
        {
            _logger.InfoFormat("Удаление статуса заказа по id ={0}", id);

            var context = CreateContext();
            var item = new OrderStatus { OrderStatusID = id };
            context.OrderStatuses.Attach(item);
            context.OrderStatuses.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Статус заказа с id = {0} успешно удален", id);
        }

        #endregion OrderStatus

        #region OrderTimelines

        /// <summary>
        /// Получает пункты истории изменений по конкретному заказу.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список пунктов истории.</returns>
        public IQueryable<OrderTimeline> GetOrderTimelines(Guid? repairOrderID)
        {
            _logger.InfoFormat("Старт получения истории по заказу {0}",repairOrderID);

            var context = CreateContext();
            return context.OrderTimelines.Where(o => o.RepairOrderID == repairOrderID).OrderBy(i=>i.EventDateTime);
        }

        /// <summary>
        /// Получает признак существования пункта графика заказа.
        /// </summary>
        /// <param name="orderTimeLineID">Код пункта графика заказа.</param>
        /// <returns>Признак существования.</returns>
        public bool OrderTimeLineExists(Guid? orderTimeLineID)
        {
            var context = CreateContext();
            return context.OrderTimelines.Any(o => o.OrderTimelineID == orderTimeLineID);
        }

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

            if (orderTimeline.OrderTimelineID == null || orderTimeline.OrderTimelineID == Guid.Empty)
            {
                orderTimeline.OrderTimelineID = Guid.NewGuid();
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
            return context.OrderTimelines.FirstOrDefault(fs => fs.OrderTimelineID == id);
        }

        /// <summary>
        /// Удаляет из хранилища график заказа по его ID.
        /// </summary>
        /// <param name="id">Код графика заказа.</param>
        public void DeleteOrderTimeline(Guid? id)
        {
            _logger.InfoFormat("Удаление графика заказа по id ={0}", id);

            var context = CreateContext();
            var item = new OrderTimeline { OrderTimelineID = id };
            context.OrderTimelines.Attach(item);
            context.OrderTimelines.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("График заказас id = {0} успешно удален", id);
        }

        private const string DeleteOrderTimelineByRepairOrderSql = "DELETE FROM OrderTimeline WHERE RepairOrderID = {0}";

        /// <summary>
        /// Удаляет из хранилища график заказа по ID заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        public void DeleteOrderTimelineByRepairOrder(Guid? repairOrderID)
        {
            _logger.InfoFormat("Удаление графика заказа по для заказа ={0}", repairOrderID);

            var context = CreateContext();
            context.ExecuteStoreCommand(DeleteOrderTimelineByRepairOrderSql, repairOrderID);

            _logger.InfoFormat("График заказас по заказу с id = {0} успешно удален", repairOrderID);
        }

        #endregion OrderTimelines

        #region OrderCapacityItem

        private const string GetIncrementRepairOrderNumberSql = @"
UPDATE OrderCapacity
SET OrderNumber =OrderNumber+1
OUTPUT INSERTED.OrderNumber
WHERE UserDomainID = {0}

";

        /// <summary>
        /// Получает новый id для заказа.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Новый id.</returns>
        public long GetNewOrderNumber(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение нового номера для заказа под домен {0}", userDomainID);

            var context = CreateContext();

            return context.ExecuteStoreQuery<long>(GetIncrementRepairOrderNumberSql, userDomainID).FirstOrDefault();
        }

        #endregion OrderCapacityItem

        #region RepairOrder

        private const string GetRepairOrderHashSqlFormat =
            @"
SELECT
TOP 40
[RepairOrderID],
CONVERT(NVARCHAR(32),HashBytes('MD5',
	 CAST ([RepairOrderID] as varchar(50))
      +ISNULL( cast ([IssuerID] as varchar(50)),'')
      +cast ( [OrderStatusID]as varchar(50))
      +ISNULL(cast ([EngineerID]as varchar(50)),'') 
      +ISNULL(cast ([ManagerID]as varchar(50)),'') 
      +cast ( [OrderKindID]as varchar(50))
      +convert(varchar(20), [EventDate], 120)
      +[Number]
      +[ClientFullName]
      +ISNULL([ClientAddress],'')
      +ISNULL([ClientPhone],'')
      +ISNULL([ClientEmail],'')
      +ISNULL([DeviceTitle],'')
      +ISNULL([DeviceSN],'')
      +ISNULL([DeviceTrademark],'')
      +ISNULL([DeviceModel],'')
      +ISNULL([Defect],'')
      +ISNULL([Options],'')
      +ISNULL([DeviceAppearance],'')
      +ISNULL([Notes],'')
      +ISNULL(convert(varchar(20),[CallEventDate], 120),'')
      +ISNULL(convert(varchar(20),[DateOfBeReady], 120),'')
	  +ISNULL(convert(varchar, cast([GuidePrice] as money),0),'')
      +ISNULL(convert(varchar, cast([PrePayment] as money),0),'')      
      +cast([IsUrgent] as varchar(1))
      +ISNULL([Recommendation],'')
      +ISNULL(convert(varchar(20),[IssueDate], 120),'')
      +ISNULL(convert(varchar(20),[WarrantyTo], 120),'')
      +cast ( [BranchID]as varchar(50))
	  COLLATE Cyrillic_General_100_CS_AI),2) as DataHash,
	(SELECT count(*) FROM OrderTimeline ot where ot.RepairOrderID = o.RepairOrderID) as OrderTimelinesCount
FROM
RepairOrder o
WHERE
UserDomainID = {{0}}
{0}
ORDER BY o.EventDate
";

        private const string GetWorkItemHashSql = @"
SELECT 
WorkItemID,
CONVERT(NVARCHAR(32),HashBytes('MD5',
ISNULL(CAST ([WorkItemID]as varchar(50)),'')
      +ISNULL(CAST ([UserID]as varchar(50)),'')
      +ISNULL([Title],'')
      +ISNULL(convert(varchar(20),[EventDate], 120),'')
      +ISNULL(convert(varchar, cast([Price] as money),0),'')
      +ISNULL(CAST ([RepairOrderID]as varchar(50)),'')
	  COLLATE Cyrillic_General_100_CS_AI),2) as DataHash
  FROM [dbo].[WorkItem]
WHERE RepairOrderID = {0}

";

        private const string GetDeviceItemHashSql = @"
SELECT 
        [DeviceItemID],
CONVERT(NVARCHAR(32),HashBytes('MD5',
CAST ([DeviceItemID]as varchar(50))
      +CAST ([UserID]as varchar(50))
      +ISNULL([Title],'')
      +ISNULL(convert(varchar, cast([Count] as money),0),'')
      +ISNULL(convert(varchar, cast([CostPrice]as money),0),'')
      +ISNULL(convert(varchar, cast([Price]as money),0),'')
      +ISNULL(CAST ([RepairOrderID]as varchar(50)),'')
      +ISNULL(convert(varchar(20),[EventDate], 120),'')
      +ISNULL(CAST ([WarehouseItemID]as varchar(50)),'')
	   COLLATE Cyrillic_General_100_CS_AI),2) as DataHash
  FROM [dbo].[DeviceItem]
WHERE RepairOrderID = {0}

";

        /// <summary>
        /// Получает следующие 40 хэшей заказов.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <param name="lastRepairOrderID">Код заказа за которым необходимо взять все хэши.</param>
        /// <param name="totalCount">Общее количество заказов. </param>
        /// <returns>Список хэшей.</returns>
        public IEnumerable<RepairOrderHash> GetRepairOrderHashes(Guid? userDomainID, Guid? lastRepairOrderID, out int totalCount)
        {
            _logger.InfoFormat("Старт получения хэшей заказов ");

            var context = CreateContext();

            totalCount = context.RepairOrders.Count(o => o.UserDomainID == userDomainID);

            List<RepairOrderHash> result;

            DateTime? eventDate = null;

            if (lastRepairOrderID!=null)
            {
                eventDate = context.RepairOrders.Where(o => o.RepairOrderID == lastRepairOrderID && o.UserDomainID==userDomainID).Select(o=>o.EventDate).FirstOrDefault();
            } //if

            
            if (eventDate!=null)
            {
                var whereSql = " AND o.EventDate>={1}";
                var sql = string.Format(GetRepairOrderHashSqlFormat, whereSql);
                result = context.ExecuteStoreQuery<RepairOrderHash>(sql, userDomainID,eventDate).ToList();

            } //if
            else
            {
                var sql = string.Format(GetRepairOrderHashSqlFormat, string.Empty);
                result = context.ExecuteStoreQuery<RepairOrderHash>(sql,userDomainID).ToList();
            } //else

            foreach (var repairOrderHash in result)
            {
                repairOrderHash.WorkItemHashes =
                    context.ExecuteStoreQuery<WorkItemHash>(GetWorkItemHashSql, repairOrderHash.RepairOrderID).ToList();
                repairOrderHash.DeviceItemHashes = context.ExecuteStoreQuery<DeviceItemHash>(GetDeviceItemHashSql, repairOrderHash.RepairOrderID).ToList();

            } //foreach

            return result;
        }

        private IQueryable<RepairOrderDTO> SelectRepairOrderDTO(IQueryable<RepairOrder> entityes,DatabaseContext context, int page,int pageSize)
        {
            var item1 = entityes.GroupJoin(context.Users, order => order.EngineerID, user => user.UserID,
                                           (order, users) => new {order, users}).SelectMany(
                                               a => a.users.DefaultIfEmpty(), (arg, user) => new
                                                                                              {
                                                                                                  Order = arg.order,
                                                                                                  EngineerFullName =user.LastName +" "+ user.FirstName +" " +(user.MiddleName??string.Empty)/*user.LastName + " " + user.FirstName + " " + user.MiddleName */
                                                                                              }
                );

            var item2 = item1.Join(context.Users, arg => arg.Order.ManagerID, user => user.UserID, (arg1, user) =>
                new
                {
                    arg1.Order,
                    arg1.EngineerFullName,
                    ManagerFullName = user.LastName + " " + user.FirstName + " " + (user.MiddleName??string.Empty)
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
                                       StatusKind =status.StatusKindID
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
            
            if (page!=0)
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
                UserDomainID = i.Order.UserDomainID,
                AccessPassword = i.Order.AccessPassword

            });
        }

        private IQueryable<RepairOrder> GetRepairOrdersFilterByName(Guid? userDomainID,Guid? orderStatusId, bool? isUrgent, string name, DatabaseContext context)
        {
            NormilizeString(ref name);
            var step1 =
                context.RepairOrders.Where(
                    i =>
                    (i.ClientFullName + i.DeviceTitle + i.ClientPhone + i.DeviceSN + i.Number).Contains(name) && i.UserDomainID == userDomainID);

            if (orderStatusId != null)
            {
                step1 = step1.Where(i => i.OrderStatusID == orderStatusId);    
            } //if

            if (isUrgent != null)
            {
                step1 = step1.Where(i => i.IsUrgent == isUrgent);  
            } //if

            return step1;
        }

        private IQueryable<RepairOrder> GetRepairOrdersFilterByNameAndUserBranches(Guid? userDomainID,Guid? userID,bool? isUrgent, Guid? orderStatusId, string name, DatabaseContext context)
        {
            return GetRepairOrdersFilterByName(userDomainID,orderStatusId, isUrgent,name, context).Join(context.UserBranchMapItems, order => order.BranchID,
                item => item.BranchID, (order, item) => new { order, item }).Where(i => i.item.UserID == userID).Select(i => i.order);
        }

        /// <summary>
        /// Получает код филиала по коду заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Код филиала.</returns>
        public Guid? GetRepairOrderBranchID(Guid? repairOrderID)
        {
            var context = CreateContext();
            return
                context.RepairOrders.Where(i => i.RepairOrderID == repairOrderID).
                    Select(i => i.BranchID).FirstOrDefault();
        }

        /// <summary>
        /// Получает код домена пользователя по коду заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Код домена пользователя.</returns>
        public Guid? GetRepairOrderUserDomainID(Guid? repairOrderID)
        {
            var context = CreateContext();
            return
                context.RepairOrders.Where(i => i.RepairOrderID == repairOrderID).
                    Select(i => i.UserDomainID).FirstOrDefault();
        }

        /// <summary>
        /// Получает номер заказа по коду заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Номер заказа.</returns>
        public string GetRepairOrderNumber(Guid? repairOrderID)
        {
            var context = CreateContext();
            return
                context.RepairOrders.Where(i => i.RepairOrderID == repairOrderID).Select(i => i.Number).FirstOrDefault
                    ();
        }

        /// <summary>
        /// Получает код инженера по коду заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Код инженера.</returns>
        public Guid? GetRepairOrderEngineerID(Guid? repairOrderID)
        {
            var context = CreateContext();
            return
                context.RepairOrders.Where(i => i.RepairOrderID == repairOrderID).Select(i => i.EngineerID).FirstOrDefault();
        }

        /// <summary>
        /// Получает списко заказаов за определенный период.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Заказы.</returns>
        public IEnumerable<RepairOrder> GetRepairOrders(Guid? userDomainID,DateTime beginDate, DateTime endDate)
        {
            _logger.InfoFormat("Получение заказов для домена {0} с {1} по {2}",userDomainID,beginDate,endDate);

            var context = CreateContext();

            return
                context.RepairOrders.Where(
                    rp =>
                    rp.UserDomainID == userDomainID && EntityFunctions.TruncateTime(rp.EventDate) >= beginDate &&
                    EntityFunctions.TruncateTime(rp.EventDate) <= endDate);
        }

        /// <summary>
        /// Возвращает список заказов по фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="orderStatusId">Код статуса задачи.</param>
        /// <param name="isUrgent">Признак срочности.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        public IEnumerable<RepairOrderDTO> GetRepairOrders(Guid? userDomainID,Guid? orderStatusId,bool? isUrgent,string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение заказов по строке поиска {0}",name);
            var context = CreateContext();

            var orders = GetRepairOrdersFilterByName(userDomainID,orderStatusId,isUrgent,name, context);
            
            count = orders.Count();

            return SelectRepairOrderDTO(orders, context,page,pageSize);
        }

        /// <summary>
        /// Возвращает список заказов для определенного домена.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список заказов.</returns>
        public IQueryable<RepairOrderDTO> GetRepairOrders(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение всех заказов для домена: {0}", userDomainID);
            var context = CreateContext();

            var orders =context.RepairOrders.Where(
                     i => i.UserDomainID == userDomainID);

            return SelectRepairOrderDTO(orders, context, 0, 0);
        }

        /// <summary>
        /// Возвращает список заказов по фильтром по филиалам которые доступны пользователям.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя</param>
        /// <param name="orderStatusId">Код статуса  задачи.</param>
        /// <param name="isUrgent">Признак срочности.</param>
        /// <param name="userId">Код пользователя по которому производится поиск филиалов. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        public IEnumerable<RepairOrderDTO> GetRepairOrdersUserBranch(Guid? userDomainID, Guid? orderStatusId,bool? isUrgent,Guid? userId,string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение заказов по строке поиска {0}", name);
            var context = CreateContext();

            var orders = GetRepairOrdersFilterByNameAndUserBranches(userDomainID,userId,isUrgent ,orderStatusId, name, context);

            count = orders.Count();

            return SelectRepairOrderDTO(orders, context, page, pageSize);
        }

        /// <summary>
        /// Возвращает список заказов по фильтром по конкретным исполнителям.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователей.</param>
        /// <param name="orderStatusId">Код статуса  задачи.</param>
        /// <param name="isUrgent">Признак срочности.</param>
        /// <param name="userId">Код пользователя по которому производится поиск задач. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        public IEnumerable<RepairOrderDTO> GetRepairOrdersUser(Guid? userDomainID,Guid? orderStatusId, bool? isUrgent, Guid? userId, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение заказов по строке поиска {0}", name);
            var context = CreateContext();

            var orders =
                GetRepairOrdersFilterByName(userDomainID,orderStatusId, isUrgent,name, context).Where(
                    i => i.ManagerID == userId || i.EngineerID == userId);

            count = orders.Count();

            return SelectRepairOrderDTO(orders, context, page, pageSize);
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
                    di => di.RepairOrderID == repairOrder.RepairOrderID && di.UserDomainID == repairOrder.UserDomainID);

            if (savedItem==null)
            {
                savedItem =
                context.RepairOrders.FirstOrDefault(
                    di => di.Number == repairOrder.Number && di.UserDomainID == repairOrder.UserDomainID);
            } //if


            if (repairOrder.RepairOrderID == null || repairOrder.RepairOrderID == Guid.Empty)
            {
                repairOrder.RepairOrderID = Guid.NewGuid();
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
        /// Получает заказа по его его номеру и номеру домена.
        /// </summary>
        /// <param name="number">Номер заказа.</param>
        /// <param name="userDomainNumber">Номер домена пользователя</param>
        /// <returns>Заказ, если существует.</returns>
        public RepairOrderDTO GetRepairOrder(string number, int userDomainNumber)
        {
            _logger.InfoFormat("Получение заказа по номеру = {0} для домена {1}", number, userDomainNumber);
            var context = CreateContext();
            var userDomain = context.UserDomains.FirstOrDefault(d => d.Number == userDomainNumber);
            if (userDomain == null)
            {
                return null;
            }
            return SelectRepairOrderDTO(
                    context.RepairOrders.Where(fs => fs.Number == number && fs.UserDomainID == userDomain.UserDomainID), context,
                    0, 0).FirstOrDefault();
        }

        /// <summary>
        /// Получает заказа по его ID.
        /// </summary>
        /// <param name="id">Код заказа.</param>
        /// <param name="userDomainID">Код домена пользователя</param>
        /// <returns>Заказ, если существует.</returns>
        public RepairOrderDTO GetRepairOrder(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение заказа по Id = {0}", id);
            var context = CreateContext();
            return
                SelectRepairOrderDTO(
                    context.RepairOrders.Where(fs => fs.RepairOrderID == id && fs.UserDomainID == userDomainID), context,
                    0, 0).FirstOrDefault();
        }

        /// <summary>
        /// Получает заказа по его ID.
        /// </summary>
        /// <param name="id">Код заказа.</param>
        /// <param name="userDomainID">Код домена пользователя</param>
        /// <returns>Заказ, если существует.</returns>
        public RepairOrder GetRepairOrderLight(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение простого заказа по Id = {0}", id);
            var context = CreateContext();
            return
                context.RepairOrders.FirstOrDefault(fs => fs.RepairOrderID == id && fs.UserDomainID == userDomainID);

        }

        /// <summary>
        /// Удаляет из хранилища заказ по его ID.
        /// </summary>
        /// <param name="id">Код заказа.</param>
        public void DeleteRepairOrder(Guid? id)
        {
            _logger.InfoFormat("Удаление заказа id ={0}", id);

            var context = CreateContext();
            var item = new RepairOrder { RepairOrderID = id };
            context.RepairOrders.Attach(item);
            context.RepairOrders.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Заказ с id = {0} успешно удален", id);
        }

        /// <summary>
        /// Возвращает список заказов в работе определенных пользователей с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="engineerID">Код связанного инженера или null. </param>
        /// <param name="managerID">Код связанного менеджера или null.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        public IEnumerable<RepairOrderDTO> GetWorkRepairOrders(Guid? userDomainID, Guid? engineerID, Guid? managerID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение заказов по строке поиска {0}", name);
            var context = CreateContext();

            var orders = GetRepairOrdersFilterByName(userDomainID, null, null, name, context);

            if (managerID!=null)
            {
                orders = orders.Where(o => o.ManagerID == managerID);
            } //if

            if (engineerID != null)
            {
                orders = orders.Where(o => o.EngineerID == engineerID);
            } //if
            
            var ordersDto = SelectRepairOrderDTO(orders, context, 0, 0).Where(i => i.StatusKind != StatusKindSet.Closed.StatusKindID);

            count = ordersDto.Count();

            return ordersDto.OrderBy(i => i.DateOfBeReady).Page(page, pageSize);
        }

        #endregion RepairOrder

        #region UserBranchMapItem

        /// <summary>
        /// Осуществляет проверку привязан ли пользователь с филиалом.
        /// </summary>
        /// <param name="userId">Код пользователя.</param>
        /// <param name="branchId">Код филиала.</param>
        /// <returns>Признак существования связи.</returns>
        public bool UserHasBranch(Guid? userId,Guid? branchId)
        {
            var context = CreateContext();
            return context.UserBranchMapItems.Any(i => i.BranchID == branchId && i.UserID == userId);
        }

        /// <summary>
        /// Возвращает информацию о связах конкретного пользователя с филиалами.
        /// </summary>
        /// <param name="userId">Код пользователя.</param>
        /// <param name="userProjectRoleId">Код проектной роли пользователя.</param>
        /// <returns>Филиалы.</returns>
        public IEnumerable<UserBranchMapItemDTO> GetUserBranchMapByItemsByUser(Guid? userId,byte? userProjectRoleId)
        {
            _logger.InfoFormat("Получение всех филиалов пользователя {0} для роли {1}", userId,userProjectRoleId);
            var context = CreateContext();
            IQueryable < User > users;
            if (userProjectRoleId!=null)
            {
                users = context.Users.Where(u => u.ProjectRoleID == userProjectRoleId && u.UserID == userId);
                
            }
            else 
            {
                users = context.Users.Where(u => u.UserID == userId);
            }

            return users.Join(context.UserBranchMapItems, user => user.UserID, item => item.UserID, (user,item ) => new { item, user }).Join(context.Branches, arg => arg.item.BranchID, branch => branch.BranchID, (arg, branch) =>
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
        /// Возвращает информацию о связах конкретного филиала с пользователями определенной роли.
        /// </summary>
        /// <param name="branchId">Код пользователя.</param>
        /// <param name="userProjectRoleId">Код роли.</param>
        /// <returns>Филиалы.</returns>
        public IEnumerable<UserBranchMapItemDTO> GetUserBranchMapByItemsByBranch(Guid? branchId, byte? userProjectRoleId)
        {
            _logger.InfoFormat("Получение всех пользователей филиала {0}", branchId);
            var context = CreateContext();
            return context.UserBranchMapItems.Where(i => i.BranchID == branchId).Join(context.Users.Where(u=>u.ProjectRoleID==userProjectRoleId), item => item.UserID, user => user.UserID, (item, user) => new { item, user }).Join(context.Branches, arg => arg.item.BranchID, branch => branch.BranchID, (arg, branch) =>
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
        /// Возвращает информацию о связах филиалов и пользователей.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Связи филииалов и пользователей.</returns>
        public IEnumerable<UserBranchMapItem> GetUserBranchMapItems(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение всех связей пользователей и филиалов домена {0}", userDomainID);
            var context = CreateContext();
            return context.UserBranchMapItems.Join(context.Users, item => item.UserID, user => user.UserID,
                                                   (item, user) =>
                                                   new {item, user}).Where(i => i.user.UserDomainID == userDomainID).
                Select(i => i.item);
        }

        private const string DeleteUserBranchMapItemsSql = "DELETE FROM UserBranchMap WHERE UserID = {0}";

        /// <summary>
        /// Удаляет все связанные с пользователем филиалы.
        /// </summary>
        /// <param name="userId">Код пользователя.</param>
        public void DeleteUserBranchMapItems(Guid? userId)
        {
            _logger.InfoFormat("Удаление всех филиалов пользователя {0}",userId);

            var context = CreateContext();
            context.ExecuteStoreCommand(DeleteUserBranchMapItemsSql, userId);
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

            if (userBranchMapItem.UserBranchMapID == null || userBranchMapItem.UserBranchMapID == Guid.Empty)
            {
                userBranchMapItem.UserBranchMapID = Guid.NewGuid();
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
            return context.UserBranchMapItems.FirstOrDefault(fs => fs.UserBranchMapID == id);
        }

        /// <summary>
        /// Удаляет из хранилища соответствие между пользователем и филиалом по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        public void DeleteUserBranchMapItem(Guid? id)
        {
            _logger.InfoFormat("Удаление соответствия id ={0}", id);

            var context = CreateContext();
            var item = new UserBranchMapItem { UserBranchMapID = id };
            context.UserBranchMapItems.Attach(item);
            context.UserBranchMapItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Соответствие с id = {0} успешно удалено", id);
        }

        #endregion UserBranchMapItem

        #region CustomReportItem

        /// <summary>
        /// Получает список документов по определенному типу.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <param name="documentKindID">Код типа документа.</param>
        /// <returns>Список документов.</returns>
        public IEnumerable<CustomReportItem> GetCustomReportItems(Guid? userDomainID,byte? documentKindID)
        {
            _logger.InfoFormat("Получение списка документов без строки поиска");
            var context = CreateContext();
            return context.CustomReportItems.Where(i=>i.DocumentKindID == documentKindID && i.UserDomainID==userDomainID);
        }

        /// <summary>
        /// Получает список документов без фильтра.
        /// </summary>
        /// <returns>Список документов.</returns>
        public IQueryable<CustomReportItem> GetCustomReportItems(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списка документов без строки поиска");
            var context = CreateContext();
            return context.CustomReportItems.Where(i=>i.UserDomainID==userDomainID);
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
                    di.CustomReportID == customReportItem.CustomReportID &&
                    di.UserDomainID == customReportItem.UserDomainID);

            if (customReportItem.CustomReportID == null || customReportItem.CustomReportID == Guid.Empty)
            {
                customReportItem.CustomReportID = Guid.NewGuid();
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
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <returns>Документ, если существует.</returns>
        public CustomReportItem GetCustomReportItem(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение документа по Id = {0}", id);
            var context = CreateContext();
            return
                context.CustomReportItems.FirstOrDefault(
                    fs => fs.CustomReportID == id && fs.UserDomainID == userDomainID);
        }

        /// <summary>
        /// Удаляет из хранилища документ по его ID.
        /// </summary>
        /// <param name="id">Код документа.</param>
        public void DeleteCustomReportItem(Guid? id)
        {
            _logger.InfoFormat("Удаление документ id ={0}", id);

            var context = CreateContext();
            var item = new CustomReportItem { CustomReportID = id };
            context.CustomReportItems.Attach(item);
            context.CustomReportItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Документ id = {0} успешно удален", id);
        }

        #endregion CustomReportItem

        #region FinancialGroupItem

        /// <summary>
        /// Получает список финансовых групп с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список финансовых групп.</returns>
        public IEnumerable<FinancialGroupItem> GetFinancialGroupItems(Guid? userDomainID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка финансовых групп со строкой поиска {0} страница {1} для домена {2}", name, page, userDomainID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.FinancialGroupItems.Where(i => i.Title.Contains(name) && i.UserDomainID == userDomainID);
            count = items.Count();

            return items.OrderBy(i => i.Title).Page(page, pageSize);
        }

        /// <summary>
        /// Получает список финансовая групп без фильтра.
        /// </summary>
        /// <returns>Список финансовых группаов.</returns>
        public IQueryable<FinancialGroupItem> GetFinancialGroupItems(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списка финансовая группаов без строки поиска по домену пользователя {0}", userDomainID);
            var context = CreateContext();
            return context.FinancialGroupItems.Where(i => i.UserDomainID == userDomainID);
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
                    di.FinancialGroupID == financialGroupItem.FinancialGroupID &&
                    di.UserDomainID == financialGroupItem.UserDomainID);

            if (financialGroupItem.FinancialGroupID == null || financialGroupItem.FinancialGroupID == Guid.Empty)
            {
                financialGroupItem.FinancialGroupID = Guid.NewGuid();
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
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Финансовая группа, если существует.</returns>
        public FinancialGroupItem GetFinancialGroupItem(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение финансовой группы по Id = {0} по домену пользователя {1}", id, userDomainID);
            var context = CreateContext();
            return context.FinancialGroupItems.FirstOrDefault(fs => fs.FinancialGroupID == id && fs.UserDomainID == userDomainID);
        }

        /// <summary>
        /// Удаляет из хранилища финансовую группу по его ID.
        /// </summary>
        /// <param name="id">Код финансовой группы.</param>
        public void DeleteFinancialGroupItem(Guid? id)
        {
            _logger.InfoFormat("Удаление финансовой группы id ={0}", id);

            var context = CreateContext();
            if (context.FinancialItemValues.Any(fi=>fi.FinancialGroupID==id))
            {
                throw new Exception("Имеются связанные статьи бюджета, необходимо удалить их");
            }

            var item = new FinancialGroupItem { FinancialGroupID = id };
            context.FinancialGroupItems.Attach(item);
            context.FinancialGroupItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Финансовая группа id = {0} успешно удалена", id);
        }

        #endregion FinancialGroupItem

        #region EngineerWorkReport

        /// <summary>
        /// Получает пункты отчета для работы инженеров.
        /// </summary>
        /// <param name="userDomainID">Код домена.</param>
        /// <param name="engineerID">Код инженера, может быть null, тогда собираются данные по всем инженерам.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания периода.</param>
        /// <returns>Списко пунктов отчета.</returns>
        public IEnumerable<EngineerWorkReportItem> GetEngineerWorkReportItems(Guid? userDomainID,Guid? engineerID,DateTime beginDate,DateTime endDate)
        {
            _logger.InfoFormat("Получение отчета по инженерам {0} {1} {2} {3}", userDomainID, engineerID, beginDate,
                               endDate);

            var context = CreateContext();

            var orders =
                context.RepairOrders.Where(
                    o =>
                    o.UserDomainID == userDomainID);
            
            var workItems = context.WorkItems.Where(w => EntityFunctions.TruncateTime(w.EventDate) >= beginDate &&
                                                         EntityFunctions.TruncateTime(w.EventDate) <= endDate);
            if (engineerID != null)
            {
                workItems = workItems.Where(a => a.UserID == engineerID);
            }

            var items = workItems.
                Join(orders, item => item.RepairOrderID, order => order.RepairOrderID,
                     (item, order) => new {Item = item, Order = order}).
                Join(context.Users, arg => arg.Item.UserID, user => user.UserID,
                     (arg, user) => new {arg.Item, arg.Order, Us = user});
            

            return items.Select(i => new EngineerWorkReportItem
                                         {
                                             FirstName = i.Us.FirstName,
                                             LastName = i.Us.LastName,
                                             MiddleName = i.Us.MiddleName,
                                             OrderNumber = i.Order.Number,
                                             WorkEventDate = i.Item.EventDate,
                                             WorkPrice = i.Item.Price,
                                             WorkTitle = i.Item.Title,
                                             UserID = i.Item.UserID
                                         });

        }

        #endregion EngineerWorkReport

        #region RevenueAndExpenditureReport

        private const string RevenueAndExpenditureReportSql = @"
SELECT
    Title,
    EventDate,
    RevenueAmount,
    ExpenditureAmount
FROM
(
SELECT
    i.Title,
    iv.EventDate,
    0 as RevenueAmount,
    iv.Amount as ExpenditureAmount
FROM
    FinancialItemValue iv
INNER JOIN
    FinancialItem i
ON iv.FinancialItemID = i.FinancialItemID
WHERE iv.EventDate between {0} and {1}
AND i.UserDomainID = {2} AND iv.FinancialGroupID = {3} AND i.FinancialItemKindID = 1
AND i.TransactionKindID = 2

UNION ALL

SELECT
    i.Title,
    iv.EventDate,
    iv.Amount as RevenueAmount,
    0 as ExpenditureAmount
FROM
    FinancialItemValue iv
INNER JOIN
    FinancialItem i
ON iv.FinancialItemID = i.FinancialItemID
WHERE iv.EventDate between {0} and {1}
AND i.UserDomainID = {2} AND iv.FinancialGroupID = {3} AND i.FinancialItemKindID = 1
AND i.TransactionKindID = 1
) t
ORDER BY EventDate, Title
";

        /// <summary>
        /// Получение отчета по пользовательским данным по расходам и доходам.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="financialGroupID">Код финансовой группы филиалов.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата завершения.</param>
        /// <returns>Список пунктов отчета.</returns>
        public IEnumerable<RevenueAndExpenditureReportItem> GetRevenueAndExpenditureReportItems(Guid? userDomainID, Guid? financialGroupID, DateTime beginDate, DateTime endDate)
        {
            _logger.InfoFormat("Получение пунктов отчета по доходам и расходам группы {0} с {1} по {2}",
                               financialGroupID, beginDate, endDate);
            var context = CreateContext();

            return context.ExecuteStoreQuery<RevenueAndExpenditureReportItem>(RevenueAndExpenditureReportSql, beginDate,
                                                                              endDate, userDomainID, financialGroupID);
        }

        private const string GetOrderPaidAmountByDeviceAndWorkEventDateSql = @"
SELECT
   SUM(isnull(t.Amount,0.0))*1.0 as Amount,
   SUM(isnull(t.Count,0.0))*1.0 as Count,
   SUM(isnull(t.SumCount,0.0))*1.0 as SumCount,
   SUM(isnull(t.TotalAmount,0.0))*1.0 as TotalAmount
FROM
(
 SELECT
    SUM(wi.Price) as Amount,
    Count(*) as Count,
    0.0 As SumCount,
    SUM(wi.Price) As TotalAmount    
   FROM
    RepairOrder o
  INNER JOIN 
    FinancialGroupBranchMap gm
ON o.BranchID = gm.BranchID
  INNER JOIN
    WorkItem wi
ON wi.RepairOrderID = o.RepairOrderID
WHERE gm.FinancialGroupID = {1}
AND o.UserDomainID = {0}
AND o.IssueDate between {2} and {3}   

UNION ALL 

SELECT
    SUM(di.Price) as Amount,
    Count(*) as Count,
    Sum(Count) As SumCount,
    SUM(di.Price*di.Count) As TotalAmount
   FROM
    RepairOrder o
  INNER JOIN 
    FinancialGroupBranchMap gm
ON o.BranchID = gm.BranchID
  INNER JOIN
    DeviceItem di
ON di.RepairOrderID = o.RepairOrderID
WHERE gm.FinancialGroupID = {1}
AND o.UserDomainID = {0}
AND o.IssueDate between {2} and {3}
) t
";

        /// <summary>
        /// Получает общую информацию по установленным запчастям и выполненным работам за определенный период выдачи клиентам для фин группы.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="financialGroupID">Код финансовой группы.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата завершения.</param>
        /// <returns>Информация.</returns>
        public ItemsInfo GetOrderPaidAmountByOrderIssueDate(Guid? userDomainID, Guid? financialGroupID, DateTime beginDate, DateTime endDate)
        {
            _logger.InfoFormat(
                "Получение общей информации по установленным запчастям и выполненным работам фин группы {0} домена {1} c {2} по {3}",
                financialGroupID, userDomainID, beginDate, endDate);

            var context = CreateContext();

            return
                context.ExecuteStoreQuery<ItemsInfo>(GetOrderPaidAmountByDeviceAndWorkEventDateSql, userDomainID,
                                                     financialGroupID, beginDate, endDate).FirstOrDefault();
        }

        private const string GetWarehouseDocTotalItemsSql = @"
declare 
	@userDomainID uniqueidentifier,
	@financialGroupID uniqueidentifier,
	@beginDate smalldatetime,
	@endDate smalldatetime

SELECT
	@userDomainID = {0},
	@financialGroupID = {1},
	@beginDate = {2},
	@endDate = {3}

SELECT
	SUM(idi.InitPrice*idi.Total) as SumInitPriceTotal,
	MIN(id.DocDate) as DocDate,
	MIN(id.DocNumber) as DocNumber
FROM
	IncomingDoc id
INNER JOIN
	ProcessedWarehouseDoc pd
	ON pd.ProcessedWarehouseDocID = id.IncomingDocID
INNER JOIN 
	IncomingDocItem idi
	ON idi.IncomingDocID = id.IncomingDocID
WHERE
	id.DocDate between @beginDate and @endDate
AND
	id.UserDomainID = @userDomainID
AND
	EXISTS(	SELECT wm.FinancialGroupWarehouseMapID FROM FinancialGroupWarehouseMap wm WHERE wm.FinancialGroupID=@financialGroupID AND wm.WarehouseID=  id.WarehouseID )
GROUP BY id.IncomingDocID
";

        /// <summary>
        /// Получение пункты отчета по завершенным приходным накладным для финансовой группы за определенный период.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="financialGroupID">Код финансовой группы.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Пункты отчета.</returns>
        public IEnumerable<WarehouseDocTotalItem> GetWarehouseDocTotalItems(Guid? userDomainID, Guid? financialGroupID, DateTime beginDate, DateTime endDate)
        {
            _logger.InfoFormat(
                "Получение пунктов отчета по завершенным прикладным накладным фин группы {0} домена {1} c {2} по {3}",
                financialGroupID, userDomainID, beginDate, endDate);

            var context = CreateContext();

            return
                context.ExecuteStoreQuery<WarehouseDocTotalItem>(GetWarehouseDocTotalItemsSql, userDomainID,
                                                     financialGroupID, beginDate, endDate);
        }
        

        #endregion RevenueAndExpenditureReport

        #region UsedDeviceItemsReport

        /// <summary>
        /// Получает отчет по используемым запчастям за определенный период времени.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="branchID">Код филиала.</param>
        /// <param name="financialGroupID">Код финансовой группы.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Пункты отчета.</returns>
        public IEnumerable<UsedDeviceItemsReportItem> GetUsedDeviceItemsReportItems(Guid? userDomainID, Guid? branchID, Guid? financialGroupID, DateTime beginDate, DateTime endDate)
        {
            _logger.InfoFormat("Получение отчета по используемым запчастям для домена {0} филиала {1} группы {2}",userDomainID,branchID,financialGroupID);

            var context = CreateContext();

            var branches = context.Branches.Where(b => b.UserDomainID == userDomainID);

            if (branchID!=null)
            {
                branches = branches.Where(b => b.BranchID == branchID);
            } //if

            if (financialGroupID!=null)
            {
                branches =
                    branches.Join(
                        context.FinancialGroupBranchMapItems.Where(i => i.FinancialGroupID == financialGroupID),
                        branch => branch.BranchID, item => item.BranchID, (branch, item) => branch);
            } //if

            return context.RepairOrders.Where(o => o.UserDomainID == userDomainID).Join(branches,
                                                                                        order => order.BranchID,
                                                                                        branch => branch.BranchID,
                                                                                        (order, branch) =>
                                                                                        new {order, branch}).
                Join(context.DeviceItems.Where(di=>di.EventDate>=beginDate && di.EventDate<=endDate), arg => arg.order.RepairOrderID, item => item.RepairOrderID,
                     (arg, item) => new {arg.branch, arg.order, item}).
                Join(context.Users, arg => arg.item.UserID, user => user.UserID,
                     (arg, user) => new UsedDeviceItemsReportItem
                                    {
                                        Amount = arg.item.Price,
                                        Count = arg.item.Count,
                                        BranchLegalName = arg.branch.LegalName,
                                        BranchTitle = arg.branch.Title,
                                        EventDate = arg.item.EventDate,
                                        OrderNumber = arg.order.Number,
                                        Title = arg.item.Title,
                                        UserFirstName = user.FirstName,
                                        UserLastName = user.LastName,
                                        UserMiddleName = user.MiddleName,
                                    });

        }

        #endregion UsedDeviceItemsReport

        #region WarehouseFlowReportItem

        private const string WarehouseFlowReportSql = @"
SELECT
    DocKind,
    EventDate,
    DocDate,
    DocNumber,
    InCount,
    OutCount,
    GoodsItemTitle

FROM
(
SELECT
    1 As DocKind,
    d.EventDate,
    id.DocDate,
    id.DocNumber,    
    idi.Total as InCount,
    0 As OutCount,
    g.Title as GoodsItemTitle
FROM
    IncomingDoc id
INNER JOIN  ProcessedWarehouseDoc d
    ON id.IncomingDocID = d.ProcessedWarehouseDocID
INNER JOIN
    IncomingDocItem idi
    ON idi.IncomingDocID = id.IncomingDocID
INNER JOIN 
    GoodsItem g
ON g.GoodsItemID = idi.GoodsItemID
WHERE
    id.UserDomainID = {0}
    AND
    id.WarehouseID = {1}
    AND d.EventDate between {2} and {3}

UNION ALL

SELECT
    2 As DocKind,
    d.EventDate,
    cd.DocDate,
    cd.DocNumber,        
    0 As InCount,
	cdi.Total as OutCount,
    g.Title as GoodsItemTitle
FROM
    CancellationDoc cd
INNER JOIN  ProcessedWarehouseDoc d
    ON cd.CancellationDocID = d.ProcessedWarehouseDocID
INNER JOIN
    CancellationDocItem cdi
    ON cdi.CancellationDocID = cd.CancellationDocID
INNER JOIN 
    GoodsItem g
ON g.GoodsItemID = cdi.GoodsItemID
WHERE
    cd.UserDomainID = {0}
    AND
    cd.WarehouseID = {1}
    AND d.EventDate between {2} and {3}

UNION ALL

SELECT
    3 As DocKind,
    d.EventDate,
    td.DocDate,
    td.DocNumber,        
    0 As InCount,
	tdi.Total as OutCount,
    g.Title as GoodsItemTitle
FROM
    TransferDoc td
INNER JOIN  ProcessedWarehouseDoc d
    ON td.TransferDocID = d.ProcessedWarehouseDocID
INNER JOIN
    TransferDocItem tdi
    ON tdi.TransferDocID = td.TransferDocID
INNER JOIN 
    GoodsItem g
ON g.GoodsItemID = tdi.GoodsItemID
WHERE
    td.UserDomainID = {0}
    AND
    td.SenderWarehouseID = {1}
    AND d.EventDate between {2} and {3}

UNION ALL

SELECT
    4 As DocKind,
    d.EventDate,
    td.DocDate,
    td.DocNumber,        
    tdi.Total As InCount,
	0 as OutCount,
    g.Title as GoodsItemTitle
FROM
    TransferDoc td
INNER JOIN  ProcessedWarehouseDoc d
    ON td.TransferDocID = d.ProcessedWarehouseDocID
INNER JOIN
    TransferDocItem tdi
    ON tdi.TransferDocID = td.TransferDocID
INNER JOIN 
    GoodsItem g
ON g.GoodsItemID = tdi.GoodsItemID
WHERE
    td.UserDomainID = {0}
    AND
    td.RecipientWarehouseID = {1}
    AND d.EventDate between {2} and {3}

UNION ALL

SELECT
 5 As DocKind,
 di.EventDate,
 di.EventDate as DocDate,
 o.Number as DocNumber,
 0 As InCount,
 di.Count as OutCount,
 gi.Title as GoodsItemTitle
FROM
	DeviceItem di
INNER JOIN
	WarehouseItem wi
ON wi.WarehouseItemID = di.WarehouseItemID
INNER JOIN RepairOrder o
ON
	di.RepairOrderID=o.RepairOrderID
INNER JOIN
	GoodsItem gi
ON
	gi.GoodsItemID=wi.GoodsItemID
WHERE
	wi.WarehouseID = {1}
	AND o.UserDomainID = {0}
	AND di.EventDate between {2} and {3}

) t

ORDER BY EventDate, DocKind



";
        /// <summary>
        /// Получает пункты отчета по движениям на складе.
        /// </summary>
        /// <param name="userDomainID">Код домена.</param>
        /// <param name="warehouseID">Код склада.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Пункты отчета.</returns>
        public IEnumerable<WarehouseFlowReportItem> GetWarehouseFlowReportItems(Guid? userDomainID,Guid? warehouseID,DateTime beginDate, DateTime endDate)
        {
            _logger.InfoFormat("Получение отчета по движению на складе {0} с {1} по {2}", warehouseID, beginDate,
                               endDate);
            //Типы документов
            //1 - приходная накладная
            //2 - документы списания
            //3 - документ перемещения из склада
            //4 - документ перемещения на склад

            var context = CreateContext();

            return context.ExecuteStoreQuery<WarehouseFlowReportItem>(WarehouseFlowReportSql, userDomainID, warehouseID,
                                                                      beginDate, endDate);
        }

        #endregion WarehouseFlowReportItem

        #region FinancialItem

        /// <summary>
        /// Получает список финансовых статей с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список финансовых статей.</returns>
        public IEnumerable<FinancialItem> GetFinancialItems(Guid? userDomainID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка финансовых статей со строкой поиска {0} страница {1} для домена {2}", name, page, userDomainID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.FinancialItems.Where(i => i.Title.Contains(name) && i.UserDomainID == userDomainID);
            count = items.Count();

            return items.OrderBy(i => i.Title).Page(page, pageSize);
        }

        /// <summary>
        /// Получает список финансовых статей.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список финансовых статей.</returns>
        public IQueryable<FinancialItem> GetFinancialItems(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списка финансовых статей без строки поиска {0} ", userDomainID);
            
            var context = CreateContext();
            var items = context.FinancialItems.Where(i => i.UserDomainID == userDomainID);

            return items;
        }

        /// <summary>
        ///   Сохраняет информацию финансовой статье.
        /// </summary>
        /// <param name="financialItem"> Сохраняемая финансовая статья. </param>
        public void SaveFinancialItem(FinancialItem financialItem)
        {
            _logger.InfoFormat("Сохранение финансовой статьи с Id = {0}", financialItem.FinancialItemID);
            var context = CreateContext();

            var savedItem =
                context.FinancialItems.FirstOrDefault(
                    di =>
                    di.FinancialItemID == financialItem.FinancialItemID && di.UserDomainID == financialItem.UserDomainID);

            if (financialItem.FinancialItemID == null || financialItem.FinancialItemID == Guid.Empty)
            {
                financialItem.FinancialItemID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                financialItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.FinancialItems.AddObject(financialItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Финансовая статья с ID= {0} успешно сохранена",
                               financialItem.FinancialItemID);
        }

        /// <summary>
        /// Получает финансовую статью по его коду типа.
        ///<remarks>Если статей несколько, тогда берется самая последняя.</remarks>
        /// </summary>
        /// <param name="financialItemKindID">Код типа финансовой статьи.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Финансовая статья, если существует.</returns>
        public FinancialItem GetFinancialItemByFinancialItemKind(int? financialItemKindID, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение финансовой статьи по типу = {0} и домену пользователя {1}", financialItemKindID, userDomainID);
            var context = CreateContext();
            return
                context.FinancialItems.Where(
                    fs => fs.FinancialItemKindID == financialItemKindID && fs.UserDomainID == userDomainID).
                    OrderByDescending(i => i.EventDate).FirstOrDefault();
        }

        /// <summary>
        /// Получает финансовую статью по его ID.
        /// </summary>
        /// <param name="id">Код описания финансовой статьи.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Финансовая статья, если существует.</returns>
        public FinancialItem GetFinancialItem(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение финансовой статьи по Id = {0} по домену пользователя {1}", id, userDomainID);
            var context = CreateContext();
            return context.FinancialItems.FirstOrDefault(fs => fs.FinancialItemID == id && fs.UserDomainID == userDomainID);
        }

        /// <summary>
        /// Удаляет из хранилища финансовую статью по его ID.
        /// </summary>
        /// <param name="id">Код финансовой статьи.</param>
        public void DeleteFinancialItem(Guid? id)
        {
            _logger.InfoFormat("Удаление финансовой статьи id ={0}", id);

            var context = CreateContext();
            var item = new FinancialItem { FinancialItemID = id };
            context.FinancialItems.Attach(item);
            context.FinancialItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Финансовая статья id = {0} успешно удалена", id);
        }

        #endregion FinancialItem

        #region FinancialItemValue

        /// <summary>
        /// Получает список значений статей расходов.
        /// </summary>
        /// <param name="financialGroupID">Код финансовой группы пользователя.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="endDate">Дата окончания. </param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="beginDate">Дата окончания.</param>
        /// <returns>Список значений финансовых статей.</returns>
        public IEnumerable<FinancialItemValueDTO> GetFinancialItemValues(Guid? financialGroupID, Guid? userDomainID, string name, DateTime beginDate, DateTime endDate, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка значений финансовых статей со строкой поиска {0} страница {1} для домена {2}", name, page, userDomainID);
            NormilizeString(ref name);
            var context = CreateContext();

            var finGroups = context.FinancialGroupItems.Where(gi => gi.UserDomainID==userDomainID);

            if (financialGroupID!=null)
            {
                finGroups = finGroups.Where(i => i.FinancialGroupID == financialGroupID);
            } //if

            var finItems = context.FinancialItems.Where(fi => fi.Title.Contains(name));

            var items = context.FinancialItemValues.Where(i=>i.EventDate>=beginDate && i.EventDate<=endDate).Join(finGroups, value => value.FinancialGroupID,
                                                         item => item.FinancialGroupID,
                                                         (value, item) => new
                                                                          {
                                                                              FinGroup = item,
                                                                              FinValue = value
                                                                          }).Join(
                                                                          finItems, arg => arg.FinValue.FinancialItemID, item => item.FinancialItemID, (arg, finIms) =>
                                                                              new FinancialItemValueDTO
                                                                              {
                                                                                  Amount = arg.FinValue.Amount,
                                                                                  CostAmount = arg.FinValue.CostAmount,
                                                                                  Description = arg.FinValue.Description,
                                                                                  EventDate = arg.FinValue.EventDate,
                                                                                  FinancialGroupID = arg.FinGroup.FinancialGroupID,
                                                                                  FinancialGroupTitle = arg.FinGroup.Title,
                                                                                  FinancialItemID = arg.FinValue.FinancialItemID,
                                                                                  FinancialItemKindID = finIms.FinancialItemKindID,
                                                                                  FinancialItemTitle = finIms.Title,
                                                                                  FinancialItemValueID = arg.FinValue.FinancialItemValueID,
                                                                                  TransactionKindID = finIms.TransactionKindID
                                                                              }
                                                                          );

            count = items.Count();

            return items.OrderBy(i => new {i.EventDate, i.FinancialItemTitle}).Page(page, pageSize);
        }

        /// <summary>
        /// Получает список значений статей расходов.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список значений финансовых статей.</returns>
        public IQueryable<FinancialItemValue> GetFinancialItemValues(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списка значений финансовых статей без строки поиска {0}", userDomainID);
            var context = CreateContext();

            return
                context.FinancialItemValues.Join(context.FinancialGroupItems, iv => iv.FinancialGroupID,
                        gi => gi.FinancialGroupID, (value, item) => new {value, item})
                    .Where(i => i.item.UserDomainID == userDomainID)
                    .Select(i => i.value);

        }

        /// <summary>
        ///   Сохраняет информацию по значению финансовой статьи.
        /// </summary>
        /// <param name="financialItemValue"> Сохраняемое значение финансовой статьи. </param>
        public void SaveFinancialItemValue(FinancialItemValue financialItemValue)
        {
            _logger.InfoFormat("Сохранение значения финансовой статьи с Id = {0}", financialItemValue.FinancialItemValueID);
            var context = CreateContext();

            var savedItem =
                context.FinancialItemValues.FirstOrDefault(
                    di => di.FinancialItemValueID == financialItemValue.FinancialItemValueID);

            if (financialItemValue.FinancialItemValueID == null || financialItemValue.FinancialItemValueID == Guid.Empty)
            {
                financialItemValue.FinancialItemValueID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                financialItemValue.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.FinancialItemValues.AddObject(financialItemValue);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Значение финансовой статьи с ID= {0} успешно сохранено",
                               financialItemValue.FinancialItemValueID);
        }

        /// <summary>
        /// Получает значение финансовой статьи по ее ID.
        /// </summary>
        /// <param name="id">Код значения финансовой статьи.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Значение финансовой статьи, если существует.</returns>
        public FinancialItemValueDTO GetFinancialItemValue(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение значения финансовой статьи по Id = {0} по домену пользователя {1}", id, userDomainID);
            var context = CreateContext();
            var finGroups = context.FinancialGroupItems.Where(gi=>gi.UserDomainID == userDomainID);

            return context.FinancialItemValues.Where(i => i.FinancialItemValueID ==id).Join(finGroups, value => value.FinancialGroupID,
                                                         item => item.FinancialGroupID,
                                                         (value, item) => new
                                                         {
                                                             FinGroup = item,
                                                             FinValue = value
                                                         }).Join(
                                                                          context.FinancialItems, arg => arg.FinValue.FinancialItemID, item => item.FinancialItemID, (arg, finIms) =>
                                                                              new FinancialItemValueDTO
                                                                              {
                                                                                  Amount = arg.FinValue.Amount,
                                                                                  CostAmount = arg.FinValue.CostAmount,
                                                                                  Description = arg.FinValue.Description,
                                                                                  EventDate = arg.FinValue.EventDate,
                                                                                  FinancialGroupID = arg.FinGroup.FinancialGroupID,
                                                                                  FinancialGroupTitle = arg.FinGroup.Title,
                                                                                  FinancialItemID = arg.FinValue.FinancialItemID,
                                                                                  FinancialItemKindID = finIms.FinancialItemKindID,
                                                                                  FinancialItemTitle = finIms.Title,
                                                                                  FinancialItemValueID = arg.FinValue.FinancialItemValueID,
                                                                                  TransactionKindID = finIms.TransactionKindID
                                                                              }
                                                                          ).FirstOrDefault();
        }

        /// <summary>
        /// Удаляет из хранилища финансовую статью по ее ID.
        /// <remarks>Проверять перед удалением на доступ пользователя к значению финансовой статьи.</remarks>
        /// </summary>
        /// <param name="id">Код финансовой статьи.</param>
        public void DeleteFinancialItemValue(Guid? id)
        {
            _logger.InfoFormat("Удаление значения финансовой статьи id ={0}", id);

            var context = CreateContext();
            var item = new FinancialItemValue { FinancialItemValueID = id };
            context.FinancialItemValues.Attach(item);
            context.FinancialItemValues.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Значение финансовой статьи id = {0} успешно удалено", id);
        }

        #endregion FinancialItemValue

        #region FinancialGroupBranchMapItem

        /// <summary>
        /// Возвращает информацию о связах домена финансовых групп с филиалами.
        /// </summary>
        /// <param name="userDomainID">Код домена.</param>
        /// <returns>Связи филиалов и фингрупп.</returns>
        public IEnumerable<FinancialGroupBranchMapItem> GetFinancialGroupBranchMapItems(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение всех связей фингрупп и филиалов домена {0}", userDomainID);
            var context = CreateContext();


            return
                context.FinancialGroupBranchMapItems.Join(context.FinancialGroupItems,
                                                          mapItem => mapItem.FinancialGroupID,
                                                          item => item.FinancialGroupID,
                                                          (mapItem, groupItem) => new {groupItem, mapItem}).Where(
                                                              i => i.groupItem.UserDomainID == userDomainID).Select(
                                                                  i => i.mapItem);
        }

        
        /// <summary>
        /// Возвращает информацию о связах конкретного финансовой группы с филиалами.
        /// </summary>
        /// <param name="financialGroupID">Код группы.</param>
        /// <returns>Филиалы.</returns>
        public IEnumerable<FinancialGroupBranchMapItemDTO> GetFinancialGroupBranchMapItemsByFinancialGroup(Guid? financialGroupID)
        {
            _logger.InfoFormat("Получение всех филиалов группы {0}", financialGroupID);
            var context = CreateContext();


            return context.FinancialGroupBranchMapItems.Where(i => i.FinancialGroupID == financialGroupID).Join(context.FinancialGroupItems, mapItem => mapItem.FinancialGroupID, item => item.FinancialGroupID, (mapItem, groupItem) => new { groupItem, mapItem }).
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


        private const string DeleteFinancialGroupBranchMapItemsSql = "DELETE FROM FinancialGroupBranchMap WHERE FinancialGroupID = {0}";

        /// <summary>
        /// Удаляет все связанные с финансовой группы филиалы.
        /// </summary>
        /// <param name="financialGroupID">Код финансовой группы.</param>
        public void DeleteFinancialGroupBranchMapItems(Guid? financialGroupID)
        {
            _logger.InfoFormat("Удаление всех филиалов финансовой группы пользователя {0}", financialGroupID);

            var context = CreateContext();
            context.ExecuteStoreCommand(DeleteFinancialGroupBranchMapItemsSql, financialGroupID);
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

            if (financialGroupBranchMapItem.FinancialGroupBranchMapID == null || financialGroupBranchMapItem.FinancialGroupBranchMapID == Guid.Empty)
            {
                financialGroupBranchMapItem.FinancialGroupBranchMapID = Guid.NewGuid();
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
        /// Получает соответствия финансовой группы и филиала по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        /// <returns>Соостветствие, если существует.</returns>
        public FinancialGroupBranchMapItem GetFinancialGroupMapBranchItem(Guid? id)
        {
            _logger.InfoFormat("Получение соответствия финансовой группы и филиала по Id = {0}", id);
            var context = CreateContext();
            return context.FinancialGroupBranchMapItems.FirstOrDefault(fs => fs.FinancialGroupBranchMapID == id);
        }

        /// <summary>
        /// Удаляет из хранилища соответствие между финансовой группой и филиалом по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        public void DeleteFinancialGroupBranchMapItem(Guid? id)
        {
            _logger.InfoFormat("Удаление соответствия финансовой группы и филиала id ={0}", id);

            var context = CreateContext();
            var item = new FinancialGroupBranchMapItem { FinancialGroupBranchMapID = id };
            context.FinancialGroupBranchMapItems.Attach(item);
            context.FinancialGroupBranchMapItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Соответствие финансовой группы и филиала с id = {0} успешно удалено", id);
        }

        #endregion FinancialGroupBranchMapItem

        #region ItemCategory

        /// <summary>
        /// Получает список категорий товара с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список категорий товаров.</returns>
        public IEnumerable<ItemCategory> GetItemCategories(Guid? userDomainID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка категорий товаров со строкой поиска {0} страница {1} для домена {2}", name, page, userDomainID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.ItemCategories.Where(i => i.Title.Contains(name) && i.UserDomainID == userDomainID);
            count = items.Count();

            return items.OrderBy(i => i.Title).Page(page, pageSize);
        }

        /// <summary>
        /// Получает список всех категорий товара для домена.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список категорий товаров.</returns>
        public IQueryable<ItemCategory> GetItemCategories(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списка всех категорий товаров для домена {0}", userDomainID);
            
            var context = CreateContext();
            var items = context.ItemCategories.Where(i => i.UserDomainID == userDomainID);

            return items;
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
                    di.ItemCategoryID == itemCategory.ItemCategoryID && di.UserDomainID == itemCategory.UserDomainID);

            if (itemCategory.ItemCategoryID == null || itemCategory.ItemCategoryID == Guid.Empty)
            {
                itemCategory.ItemCategoryID = Guid.NewGuid();
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
        /// Получает категорию товара по его ID.
        /// </summary>
        /// <param name="id">Код категории товара.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Категория товара, если существует.</returns>
        public ItemCategory GetItemCategory(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение категории товара по Id = {0} по домену пользователя {1}", id, userDomainID);
            var context = CreateContext();
            return context.ItemCategories.FirstOrDefault(fs => fs.ItemCategoryID == id && fs.UserDomainID == userDomainID);
        }

        /// <summary>
        /// Удаляет из хранилища категорию товара по его ID.
        /// </summary>
        /// <param name="id">Код категории товара.</param>
        public void DeleteItemCategory(Guid? id)
        {
            _logger.InfoFormat("Удаление категории товара id ={0}", id);

            var context = CreateContext();
            var item = new ItemCategory { ItemCategoryID = id };
            context.ItemCategories.Attach(item);
            context.ItemCategories.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Категория товара id = {0} успешно удалена", id);
        }

        #endregion ItemCategory

        #region Warehouses

        /// <summary>
        /// Получает список складов с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список складов товаров.</returns>
        public IEnumerable<Warehouse> GetWarehouses(Guid? userDomainID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка складов товаров со строкой поиска {0} страница {1} для домена {2}", name, page, userDomainID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.Warehouses.Where(i => i.Title.Contains(name) && i.UserDomainID == userDomainID);
            count = items.Count();

            return items.OrderBy(i => i.Title).Page(page, pageSize);
        }

        /// <summary>
        /// Получает список всех складов для домена.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список складов товаров.</returns>
        public IQueryable<Warehouse> GetWarehouses(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списка складов товаров для домена {0}", userDomainID);
            var context = CreateContext();
            var items = context.Warehouses.Where(i => i.UserDomainID == userDomainID);

            return items.OrderBy(i => i.Title);
        }

        /// <summary>
        /// Получает список всех складов для пользователя.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список складов товаров.</returns>
        public IQueryable<Warehouse> GetWarehouses(Guid? userID,Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списка складов товаров для домена {0}", userDomainID);
            var context = CreateContext();
            
            var items=
                context.UserBranchMapItems.Where(b => b.UserID == userID).Join(context.FinancialGroupBranchMapItems,
                                                                               fitem => fitem.BranchID,
                                                                               bitem => bitem.BranchID,
                                                                               (item, mapItem) =>
                                                                               mapItem.FinancialGroupID).
                    Join(context.FinancialGroupWarehouseMapItems, i => i, item => item.FinancialGroupID,
                         (guid, item) => item.WarehouseID).Join(context.Warehouses.Where(w=>w.UserDomainID==userDomainID), i => i,
                                                                warehouse => warehouse.WarehouseID,
                                                                (guid, warehouse) => warehouse).Distinct();


            return items.OrderBy(i => i.Title);
        }

        /// <summary>
        /// Получение кода домена для склада.
        /// </summary>
        /// <param name="id">Код склада.</param>
        /// <returns>Код домена, если элемент существует.</returns>
        public Guid? GetWarehouseUserDomainID(Guid? id)
        {
            var context = CreateContext();

            return context.Warehouses.Where(i => i.WarehouseID == id).Select(i=>i.UserDomainID).FirstOrDefault();
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
                    di.WarehouseID == warehouse.WarehouseID && di.UserDomainID == warehouse.UserDomainID);

            if (warehouse.WarehouseID == null || warehouse.WarehouseID == Guid.Empty)
            {
                warehouse.WarehouseID = Guid.NewGuid();
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
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Склад товара, если существует.</returns>
        public Warehouse GetWarehouse(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение скалада по Id = {0} по домену пользователя {1}", id, userDomainID);
            var context = CreateContext();
            return context.Warehouses.FirstOrDefault(fs => fs.WarehouseID == id && fs.UserDomainID == userDomainID);
        }

        /// <summary>
        /// Удаляет из хранилища склад по его ID.
        /// </summary>
        /// <param name="id">Код склада товара.</param>
        public void DeleteWarehouse(Guid? id)
        {
            _logger.InfoFormat("Удаление склада товара id ={0}", id);

            var context = CreateContext();
            var item = new Warehouse { WarehouseID = id };
            context.Warehouses.Attach(item);
            context.Warehouses.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Склад товара id = {0} успешно удален", id);
        }

        #endregion Warehouses

        #region GoodsItem

        /// <summary>
        /// Получает список номенклатуры товара с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список номенклатуры.</returns>
        public IEnumerable<GoodsItemDTO> GetGoodsItems(Guid? userDomainID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка номенклатуры со строкой поиска {0} страница {1} для домена {2}", name, page, userDomainID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.GoodsItems.Where(i => i.Title.Contains(name) && i.UserDomainID == userDomainID);
            count = items.Count();

            return items.Join(context.ItemCategories, item => item.ItemCategoryID, category => category.ItemCategoryID, (item, category) => new GoodsItemDTO
                                                                                                                                            {
                                                                                                                                               BarCode = item.BarCode,
                                                                                                                                               Description = item.Description,
                                                                                                                                               DimensionKindID = item.DimensionKindID,
                                                                                                                                               GoodsItemID = item.GoodsItemID,
                                                                                                                                               ItemCategoryID = item.ItemCategoryID,
                                                                                                                                               ItemCategoryTitle = category.Title,
                                                                                                                                               Particular = item.Particular,
                                                                                                                                               Title = item.Title,
                                                                                                                                               UserCode = item.UserCode,
                                                                                                                                               UserDomainID = item.UserDomainID
                                                                                                                                            }).OrderBy(i => i.Title).Page(page, pageSize);
        }

        /// <summary>
        /// Получает весь список номенклатуры для домена.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список номенклатуры.</returns>
         public IQueryable<GoodsItem> GetGoodsItems(Guid? userDomainID)
         {
             _logger.InfoFormat("Получение списка номенклатуры для домена {0}", userDomainID);
             var context = CreateContext();
             return context.GoodsItems.Where(i =>  i.UserDomainID == userDomainID);
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
                    di.GoodsItemID == goodsItem.GoodsItemID && di.UserDomainID == goodsItem.UserDomainID);

            if (goodsItem.GoodsItemID == null || goodsItem.GoodsItemID == Guid.Empty)
            {
                goodsItem.GoodsItemID = Guid.NewGuid();
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
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Номенклатура товара, если существует.</returns>
        public GoodsItemDTO GetGoodsItem(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение номенклатуры по Id = {0} по домену пользователя {1}", id, userDomainID);
            var context = CreateContext();
            return context.GoodsItems.Where(fs => fs.GoodsItemID == id && fs.UserDomainID == userDomainID).Join(
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
                                                                                                          UserCode = item.UserCode,
                                                                                                          UserDomainID = item.UserDomainID
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
            var item = new GoodsItem { GoodsItemID = id };
            context.GoodsItems.Attach(item);
            context.GoodsItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Номенклатура товара id = {0} успешно удалена", id);
        }

        #endregion GoodsItem

        #region WarehouseItem

        /// <summary>
        /// Получает список остатков на складе по домену пользователя.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список остатков.</returns>
        public IQueryable<WarehouseItemDTO> GetWarehouseItems(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение остатков на складах в домене {0}",userDomainID);

            var context = CreateContext();

            return
                context.WarehouseItems.Join(context.Warehouses, item => item.WarehouseID,
                        warehouse => warehouse.WarehouseID,
                        (item, warehouse) => new {item, warehouse}).
                    Where(
                        i => i.warehouse.UserDomainID == userDomainID)
                    .Join(context.GoodsItems, item => item.item.GoodsItemID, item => item.GoodsItemID,
                        (arg, goodsItem) => new WarehouseItemDTO
                        {
                            GoodsItemTitle = goodsItem.Title,
                            GoodsItemID = goodsItem.GoodsItemID,
                            DimensionKindID = goodsItem.DimensionKindID,
                            RepairPrice = arg.item.RepairPrice,
                            SalePrice = arg.item.SalePrice,
                            StartPrice = arg.item.StartPrice,
                            Total = arg.item.Total,
                            WarehouseID = arg.item.WarehouseID,
                            WarehouseItemID = arg.item.WarehouseItemID
                        });

        }

        /// <summary>
        /// Получает список остатков на складе с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="warehouseID">Код склада. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список остатков.</returns>
        public IEnumerable<WarehouseItemDTO> GetWarehouseItems(Guid? userDomainID, Guid? warehouseID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat(
                "Получение списка номенклатуры со строкой поиска {0} страница {1} для домена {2} и склада {3}", name,
                page, userDomainID, warehouseID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.WarehouseItems.Join(
                context.Warehouses.Where(w => w.UserDomainID == userDomainID && w.WarehouseID == warehouseID),
                item => item.WarehouseID, warehouse => warehouse.WarehouseID, (item, warehouse) => item)
                .Join(context.GoodsItems, item => item.GoodsItemID, item => item.GoodsItemID,
                      (item, goodsItem) => new {item, goodsItem}).Where(i => i.goodsItem.Title.Contains(name));
            count = items.Count();

            return items.Select(i => new WarehouseItemDTO
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

            }).OrderBy(i => i.GoodsItemTitle).Page(page, pageSize);
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

            if (warehouseItem.WarehouseItemID == null || warehouseItem.WarehouseItemID == Guid.Empty)
            {
                warehouseItem.WarehouseItemID = Guid.NewGuid();
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
        /// Получение кода домена для остатка на складе.
        /// </summary>
        /// <param name="id">Код остатка на складе.</param>
        /// <returns>Код домена, если элемент существует.</returns>
        public Guid? GetWarehouseItemUserDomainID(Guid? id)
        {
            var context = CreateContext();

            return context.WarehouseItems.Where(i => i.WarehouseItemID == id).Join(context.Warehouses,
                                                                                       item => item.WarehouseID,
                                                                                       doc => doc.WarehouseID,
                                                                                       (item, warehouse) => warehouse.UserDomainID).FirstOrDefault();
        }

        /// <summary>
        /// Получает остаток на складе по его ID.
        /// </summary>
        /// <param name="id">Код номенклатуры.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Остатки на складе товара, если существует.</returns>
        public WarehouseItemDTO GetWarehouseItem(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение номенклатуры по Id = {0} по домену пользователя {1}", id, userDomainID);
            var context = CreateContext();
            return context.WarehouseItems.Where(i=>i.WarehouseItemID==id).Join(
                context.Warehouses.Where(w => w.UserDomainID == userDomainID),
                item => item.WarehouseID, warehouse => warehouse.WarehouseID, (item, warehouse) => item)
                .Join(context.GoodsItems, item => item.GoodsItemID, item => item.GoodsItemID,
                      (item, goodsItem) => new {item, goodsItem}).Select(
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
            var item = new WarehouseItem { WarehouseItemID = id };
            context.WarehouseItems.Attach(item);
            context.WarehouseItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Остаток на складе товара id = {0} успешно удален", id);
        }

        #endregion WarehouseItem

        #region Contractor

        /// <summary>
        /// Получает список контрагентов с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список контрагентов товаров.</returns>
        public IEnumerable<Contractor> GetContractors(Guid? userDomainID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка контрагентов со строкой поиска {0} страница {1} для домена {2}", name, page, userDomainID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.Contractors.Where(i => i.LegalName.Contains(name) && i.UserDomainID == userDomainID);
            count = items.Count();

            return items.OrderBy(i => i.LegalName).Page(page, pageSize);
        }

        /// <summary>
        /// Получает список всех контрагентов для домена.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список контрагентов.</returns>
        public IQueryable<Contractor> GetContractors(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списка контрагентов для домена {0}", userDomainID);
            
            var context = CreateContext();
            var items = context.Contractors.Where(i => i.UserDomainID == userDomainID);
            return items;
        }

        /// <summary>
        ///   Сохраняет информацию о контрагенте.
        /// </summary>
        /// <param name="contractor"> Сохраняемый контрагент. </param>
        public void SaveContractor(Contractor contractor)
        {
            _logger.InfoFormat("Сохранение контрагента с Id = {0}", contractor.ContractorID);
            var context = CreateContext();

            var savedItem =
                context.Contractors.FirstOrDefault(
                    di =>
                    di.ContractorID == contractor.ContractorID && di.UserDomainID == contractor.UserDomainID);

            if (contractor.ContractorID == null || contractor.ContractorID == Guid.Empty)
            {
                contractor.ContractorID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                contractor.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.Contractors.AddObject(contractor);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Контрагент с ID= {0} успешно сохранена",
                               contractor.ContractorID);
        }

        /// <summary>
        /// Получает контрагента по его ID.
        /// </summary>
        /// <param name="id">Код контрагента.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Контрагент, если существует.</returns>
        public Contractor GetContractor(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение контрагента по Id = {0} по домену пользователя {1}", id, userDomainID);
            var context = CreateContext();
            return context.Contractors.FirstOrDefault(fs => fs.ContractorID == id && fs.UserDomainID == userDomainID);
        }

        /// <summary>
        /// Удаляет из хранилища контрагент по его ID.
        /// </summary>
        /// <param name="id">Код контрагента.</param>
        public void DeleteContractor(Guid? id)
        {
            _logger.InfoFormat("Удаление контрагента id ={0}", id);

            var context = CreateContext();
            var item = new Contractor { ContractorID = id };
            context.Contractors.Attach(item);
            context.Contractors.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Конрагент id = {0} успешно удален", id);
        }

        #endregion Contractor

        #region IncomingDoc

        /// <summary>
        /// Получает список приходных накладных с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="warehouseID">Код склада.</param>
        /// <param name="endDate">Дата окончания накладных.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="beginDate">Дата начала создания накладных.</param>
        /// <returns>Список приходных накладных товаров.</returns>
        public IEnumerable<IncomingDocDTO> GetIncomingDocs(Guid? userDomainID, Guid? warehouseID, DateTime beginDate, DateTime endDate, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка приходных накладных со строкой поиска {0} страница {1} для домена {2} и склада {3}", name, page, userDomainID,warehouseID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items =
                context.IncomingDocs.Where(
                    i =>
                    (i.DocNumber + i.DocDescription).Contains(name) && i.UserDomainID == userDomainID &&
                    i.DocDate >= beginDate && i.DocDate <= endDate);
            if (warehouseID!=null)
            {
                items = items.Where(i => i.WarehouseID == warehouseID);
            }

            count = items.Count();

            return items.Join(context.Warehouses, doc => doc.WarehouseID, warehouse => warehouse.WarehouseID,
                                    (doc, warehouse) => new {doc, warehouse}).
                Join(context.Contractors, arg => arg.doc.ContractorID, contractor => contractor.ContractorID,
                     (arg, contractor) => new {arg.doc, arg.warehouse, contractor}).
                Join(context.Users, arg => arg.doc.CreatorID, user => user.UserID, (arg, user) => new IncomingDocDTO
                                                                                                      {
                                                                                                          ContractorID = arg.doc.ContractorID,
                                                                                                          ContractorLegalName = arg.contractor.LegalName,
                                                                                                          CreatorFirstName = user.FirstName,
                                                                                                          CreatorID = user.UserID,
                                                                                                          CreatorLastName = user.LastName,
                                                                                                          CreatorMiddleName = user.MiddleName,
                                                                                                          DocDate = arg.doc.DocDate,
                                                                                                          DocDescription = arg.doc.DocDescription,
                                                                                                          DocNumber = arg.doc.DocNumber,
                                                                                                          IncomingDocID = arg.doc.IncomingDocID,
                                                                                                          UserDomainID = arg.doc.UserDomainID,
                                                                                                          WarehouseID = arg.doc.WarehouseID,
                                                                                                          WarehouseTitle = arg.warehouse.Title,
                                                                                                          IsProcessed = context.ProcessedWarehouseDocs.Any(i=>i.ProcessedWarehouseDocID==arg.doc.IncomingDocID)
                                                                                                      }).OrderBy(i => i.DocDate).Page(page, pageSize);
            
        }

        /// <summary>
        /// Получает список приходных накладных.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список приходных накладных товаров.</returns>
        public IQueryable<IncomingDocDTO> GetIncomingDocs(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списка приходных накладных без строки поиска {0} ", userDomainID);
            
            var context = CreateContext();
            var items =
                context.IncomingDocs.Where(i=>i.UserDomainID == userDomainID);
          

            return items.Join(context.Warehouses, doc => doc.WarehouseID, warehouse => warehouse.WarehouseID,
                                    (doc, warehouse) => new { doc, warehouse }).
                Join(context.Contractors, arg => arg.doc.ContractorID, contractor => contractor.ContractorID,
                     (arg, contractor) => new { arg.doc, arg.warehouse, contractor }).
                Join(context.Users, arg => arg.doc.CreatorID, user => user.UserID, (arg, user) => new IncomingDocDTO
                {
                    ContractorID = arg.doc.ContractorID,
                    ContractorLegalName = arg.contractor.LegalName,
                    CreatorFirstName = user.FirstName,
                    CreatorID = user.UserID,
                    CreatorLastName = user.LastName,
                    CreatorMiddleName = user.MiddleName,
                    DocDate = arg.doc.DocDate,
                    DocDescription = arg.doc.DocDescription,
                    DocNumber = arg.doc.DocNumber,
                    IncomingDocID = arg.doc.IncomingDocID,
                    UserDomainID = arg.doc.UserDomainID,
                    WarehouseID = arg.doc.WarehouseID,
                    WarehouseTitle = arg.warehouse.Title,
                    IsProcessed = context.ProcessedWarehouseDocs.Any(i => i.ProcessedWarehouseDocID == arg.doc.IncomingDocID)
                });

        }

        /// <summary>
        ///   Сохраняет информацию о приходная накладной.
        /// </summary>
        /// <param name="incomingDoc"> Сохраняемая приходная накладная. </param>
        public void SaveIncomingDoc(IncomingDoc incomingDoc)
        {
            _logger.InfoFormat("Сохранение приходная накладная с Id = {0}", incomingDoc.IncomingDocID);
            var context = CreateContext();

            var savedItem =
                context.IncomingDocs.FirstOrDefault(
                    di =>
                    di.IncomingDocID == incomingDoc.IncomingDocID && di.UserDomainID == incomingDoc.UserDomainID);

            if (incomingDoc.IncomingDocID == null || incomingDoc.IncomingDocID == Guid.Empty)
            {
                incomingDoc.IncomingDocID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                incomingDoc.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.IncomingDocs.AddObject(incomingDoc);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Приходная накладная с ID= {0} успешно сохранена",
                               incomingDoc.IncomingDocID);
        }

        /// <summary>
        /// Получает приходную накладнаую по его ID.
        /// </summary>
        /// <param name="id">Код приходной накладной.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Приходная накладная, если существует.</returns>
        public IncomingDocDTO GetIncomingDoc(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение приходная накладная по Id = {0} по домену пользователя {1}", id, userDomainID);
            var context = CreateContext();
            return context.IncomingDocs.Where(i=>i.IncomingDocID==id && i.UserDomainID==userDomainID).Join(context.Warehouses, doc => doc.WarehouseID, warehouse => warehouse.WarehouseID,
                                    (doc, warehouse) => new { doc, warehouse }).
                Join(context.Contractors, arg => arg.doc.ContractorID, contractor => contractor.ContractorID,
                     (arg, contractor) => new { arg.doc, arg.warehouse, contractor }).
                Join(context.Users, arg => arg.doc.CreatorID, user => user.UserID, (arg, user) => new IncomingDocDTO
                {
                    ContractorID = arg.doc.ContractorID,
                    ContractorLegalName = arg.contractor.LegalName,
                    CreatorFirstName = user.FirstName,
                    CreatorID = user.UserID,
                    CreatorLastName = user.LastName,
                    CreatorMiddleName = user.MiddleName,
                    DocDate = arg.doc.DocDate,
                    DocDescription = arg.doc.DocDescription,
                    DocNumber = arg.doc.DocNumber,
                    IncomingDocID = arg.doc.IncomingDocID,
                    UserDomainID = arg.doc.UserDomainID,
                    WarehouseID = arg.doc.WarehouseID,
                    WarehouseTitle = arg.warehouse.Title,
                    IsProcessed = context.ProcessedWarehouseDocs.Any(i => i.ProcessedWarehouseDocID == arg.doc.IncomingDocID)
                }).FirstOrDefault();
        }

        /// <summary>
        /// Получение кода домена для приходной накладной.
        /// </summary>
        /// <param name="id">Код приходной накладной.</param>
        /// <returns>Код домена, если элемент существует.</returns>
        public Guid? GetIncomingDocUserDomainID(Guid? id)
        {
            var context = CreateContext();

            return context.IncomingDocs.Where(i => i.IncomingDocID == id).Select(i=>i.UserDomainID).FirstOrDefault();
        }

        /// <summary>
        /// Удаляет из хранилища приходную накладную по его ID.
        /// </summary>
        /// <param name="id">Код приходной накладной.</param>
        public void DeleteIncomingDoc(Guid? id)
        {
            _logger.InfoFormat("Удаление приходной накладной id ={0}", id);

            var context = CreateContext();

            if (context.IncomingDocItems.Any(i => i.IncomingDocID == id))
            {
                throw new Exception("Необходимо удалить элементы документа");
            } //if
            
            var item = new IncomingDoc { IncomingDocID = id };
            context.IncomingDocs.Attach(item);
            context.IncomingDocs.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Приходная накладная id = {0} успешно удалена", id);
        }

        #endregion IncomingDoc

        #region IncomingDocItem

        private const string ProcessIncomingDocItemsSql = @"

BEGIN TRY
BEGIN TRAN
	
	MERGE WarehouseItem AS wi
        USING
        (
	        SELECT
				di.GoodsItemID,
				SUM(di.[Total]) As Total,
				MAX(di.[InitPrice]) As InitPrice,
				MAX(di.[StartPrice]) As StartPrice,
				MAX(di.[RepairPrice]) As RepairPrice,
				MAX (di.[SalePrice]) As SalePrice
			FROM IncomingDocItem di
			WHERE di.IncomingDocID = {0}
            Group By GoodsItemID
        ) As s(GoodsItemID,[Total],[InitPrice],[StartPrice],[RepairPrice],[SalePrice])
        ON(wi.[GoodsItemID] = s.GoodsItemID AND wi.WarehouseID ={1})
        WHEN MATCHED THEN
            UPDATE 
			SET wi.Total =wi.Total+s.[Total],
				wi.StartPrice = CASE WHEN s.StartPrice = 0 THEN wi.StartPrice ELSE s.StartPrice END,
				wi.RepairPrice = CASE WHEN s.RepairPrice = 0 THEN wi.RepairPrice ELSE s.RepairPrice END,
				wi.SalePrice = CASE WHEN s.SalePrice = 0 THEN wi.SalePrice ELSE s.SalePrice END
        WHEN NOT MATCHED THEN
            INSERT (WarehouseID,GoodsItemID,Total,StartPrice,RepairPrice,SalePrice)
        VALUES({1},s.GoodsItemID,s.Total,s.StartPrice,s.RepairPrice,s.SalePrice);

		
	INSERT INTO 
		[dbo].[ProcessedWarehouseDoc]
           (
			   [ProcessedWarehouseDocID]
			   ,[WarehouseID]
			   ,[EventDate]
			   ,[UTCEventDateTime]
			   ,[UserID]
		   )
     VALUES           
		   (
		    {0},
            {1},
            {2},
            {3},
            {4}
		   )


COMMIT TRAN
SELECT 'Успех' As ErrorMessage, Cast(1 as bit) As ProcessResult	
END TRY	 	
	BEGIN CATCH
	ROLLBACK TRAN
		SELECT ERROR_MESSAGE() As ErrorMessage, Cast(0 as bit) As ProcessResult	
		
END CATCH

        
        
";

        /// <summary>
        /// Обрабатывает пункты приходной накладной.
        /// </summary>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="eventDate">Дата обработи документа </param>
        /// <param name="utcEventDateTime">UTC дата и время обработки документа. </param>
        /// <param name="userID">Код обработавшего пользователя.</param>
        public ProcessWarehouseDocResult ProcessIncomingDocItems(Guid? incomingDocID, Guid? userDomainID,DateTime eventDate,DateTime utcEventDateTime,Guid? userID)
        {
            _logger.InfoFormat("Старт обработки приходной накладной {0} домена {1}", incomingDocID, userDomainID);

            var context = CreateContext();
            var doc =
                context.IncomingDocs.FirstOrDefault(
                    i => i.IncomingDocID == incomingDocID && i.UserDomainID == userDomainID);
            if (doc==null)
            {
                _logger.ErrorFormat("Нет документа для обработки приходной накладной {0} в домене {1}",incomingDocID,userDomainID);
                return new ProcessWarehouseDocResult
                       {
                           ErrorMessage = "Нет такого документа",
                           ProcessResult = false
                       };
            }

            return context.ExecuteStoreQuery<ProcessWarehouseDocResult>(ProcessIncomingDocItemsSql, incomingDocID,
                                                                        doc.WarehouseID, eventDate, utcEventDateTime,
                                                                        userID).FirstOrDefault();
        }

        private const string UnProcessIncomingDocItemsSql = @"
    BEGIN TRY
BEGIN TRAN
	
	declare @deleteddows table (id uniqueidentifier)
	

	MERGE WarehouseItem AS wi
        USING
        (
	        SELECT
				di.GoodsItemID,
				SUM(di.[Total]) As Total				
			FROM IncomingDocItem di
			WHERE di.IncomingDocID = {0}
            Group By GoodsItemID
        ) As s(GoodsItemID,[Total])
        ON(wi.[GoodsItemID] = s.GoodsItemID AND wi.WarehouseID ={1})
        WHEN MATCHED THEN
            UPDATE 
			SET wi.Total =wi.Total-s.[Total]				
        WHEN NOT MATCHED THEN
            INSERT (WarehouseID,GoodsItemID,Total)
        VALUES({1},s.GoodsItemID,0-s.Total);

		
	DELETE
		[dbo].[ProcessedWarehouseDoc]
		OUTPUT DELETED.WarehouseID INTO @deleteddows
	WHERE [ProcessedWarehouseDocID] = {0}
	
	IF(NOT EXISTS(SELECT * FROM @deleteddows))
	BEGIN
		ROLLBACK TRAN
		SELECT 'Документ еще не сформирован' As ErrorMessage, Cast(1 as bit) As ProcessResult	
	END	           
	ELSE BEGIN
		COMMIT TRAN
		SELECT 'Успех' As ErrorMessage, Cast(1 as bit) As ProcessResult	
	END
END TRY	 	
	BEGIN CATCH
	ROLLBACK TRAN
		SELECT ERROR_MESSAGE() As ErrorMessage, Cast(0 as bit) As ProcessResult	
		
END CATCH
";

        /// <summary>
        /// Отменяет обработку пунктов приходной накладной.
        /// </summary>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        public ProcessWarehouseDocResult UnProcessIncomingDocItems(Guid? incomingDocID, Guid? userDomainID)
        {
            _logger.InfoFormat("Старт отмены обработки приходной накладной {0} домена {1}", incomingDocID, userDomainID);

            var context = CreateContext();
            var doc =
                context.IncomingDocs.FirstOrDefault(
                    i => i.IncomingDocID == incomingDocID && i.UserDomainID == userDomainID);
            if (doc == null)
            {
                _logger.ErrorFormat("Нет документа для отмены обработки приходной накладной {0} в домене {1}", incomingDocID, userDomainID);
                return new ProcessWarehouseDocResult
                {
                    ErrorMessage = "Нет такого документа",
                    ProcessResult = false
                };
            }

            return context.ExecuteStoreQuery<ProcessWarehouseDocResult>(UnProcessIncomingDocItemsSql, incomingDocID,
                                                                        doc.WarehouseID).FirstOrDefault();
        }

        /// <summary>
        /// Получает список всех элементов приходной накладной.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <returns>Список элемент приходной накладной товаров.</returns>
        public IQueryable<IncomingDocItemDTO> GetIncomingDocItems(Guid? userDomainID, Guid? incomingDocID)
        {
            _logger.InfoFormat("Получение списка всех элементов приходной накладной всех товаров  домена {0} документ {1}", userDomainID, incomingDocID);
            var context = CreateContext();

            return context.IncomingDocItems.Join(
                context.IncomingDocs.Where(d => d.IncomingDocID == incomingDocID && d.UserDomainID == userDomainID),
                item => item.IncomingDocID, doc => doc.IncomingDocID, (item, doc) => item)
                .Join(context.GoodsItems, docItem => docItem.GoodsItemID, goodsItem => goodsItem.GoodsItemID,
                      (item, goodsItem) => new { item, goodsItem }).Join(context.DimensionKinds, arg => arg.goodsItem.DimensionKindID, kind => kind.DimensionKindID, (arg, kind) =>
                          new { arg.goodsItem, arg.item, kind }).Join(context.ItemCategories, arg => arg.goodsItem.ItemCategoryID, category => category.ItemCategoryID, (arg, category) =>
                              new IncomingDocItemDTO
                              {
                                  Description = arg.item.Description,
                                  DimensionKindID = arg.goodsItem.DimensionKindID,
                                  GoodsItemID = arg.item.GoodsItemID,
                                  GoodsItemTitle = arg.goodsItem.Title,
                                  IncomingDocID = arg.item.IncomingDocID,
                                  IncomingDocItemID = arg.item.IncomingDocItemID,
                                  InitPrice = arg.item.InitPrice,
                                  RepairPrice = arg.item.RepairPrice,
                                  SalePrice = arg.item.SalePrice,
                                  StartPrice = arg.item.StartPrice,
                                  Total = arg.item.Total,
                                  DimensionKindTitle = arg.kind.Title,
                                  GoodsItemCategoryTitle = category.Title
                              }
                              );
        }

        /// <summary>
        /// Получает список элемент приходной накладной с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список элемент приходной накладнойов товаров.</returns>
        public IEnumerable<IncomingDocItemDTO> GetIncomingDocItems(Guid? userDomainID,Guid? incomingDocID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка элементов приходной накладной товаров со строкой поиска {0} страница {1} для домена {2} и накладной {3}", name, page, userDomainID,incomingDocID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.IncomingDocItems.Join(
                context.IncomingDocs.Where(d => d.IncomingDocID == incomingDocID && d.UserDomainID == userDomainID),
                item => item.IncomingDocID, doc => doc.IncomingDocID, (item, doc) => item)
                .Join(context.GoodsItems, docItem => docItem.GoodsItemID, goodsItem => goodsItem.GoodsItemID,
                      (item, goodsItem) => new IncomingDocItemDTO
                                           {
                                               Description = item.Description,
                                               DimensionKindID = goodsItem.DimensionKindID,
                                               GoodsItemID = item.GoodsItemID,
                                               GoodsItemTitle = goodsItem.Title,
                                               IncomingDocID = item.IncomingDocID,
                                               IncomingDocItemID = item.IncomingDocItemID,
                                               InitPrice = item.InitPrice,
                                               RepairPrice = item.RepairPrice,
                                               SalePrice = item.SalePrice,
                                               StartPrice = item.StartPrice,
                                               Total = item.Total
                                           }).Where(i => i.GoodsItemTitle.Contains(name));
            count = items.Count();

            return items.OrderBy(i => i.GoodsItemTitle).Page(page, pageSize);
        }

        /// <summary>
        ///   Сохраняет информацию об элементе приходной накладной.
        /// </summary>
        /// <remarks>Нужна обязательная проверка перед вызовом на доступ к вмененной накладной пользователя.</remarks>
        /// <param name="incomingDocItem"> Сохраняемый элемент приходной накладной. </param>
        public void SaveIncomingDocItem(IncomingDocItem incomingDocItem)
        {
            _logger.InfoFormat("Сохранение элемента приходной накладной с Id = {0}", incomingDocItem.IncomingDocItemID);
            var context = CreateContext();

            var savedItem =
                context.IncomingDocItems.FirstOrDefault(
                    di =>
                    di.IncomingDocItemID == incomingDocItem.IncomingDocItemID);

            if (incomingDocItem.IncomingDocItemID == null || incomingDocItem.IncomingDocItemID == Guid.Empty)
            {
                incomingDocItem.IncomingDocItemID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                incomingDocItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.IncomingDocItems.AddObject(incomingDocItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Элемент приходной накладной с ID= {0} успешно сохранен",
                               incomingDocItem.IncomingDocItemID);
        }

        /// <summary>
        /// Получает элемент приходной накладной по его ID.
        /// </summary>
        /// <param name="id">Код элемента приходной накладной.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Элемент приходной накладной, если существует.</returns>
        public IncomingDocItemDTO GetIncomingDocItem(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение элемента приходной накладной по Id = {0} по домену пользователя {1}", id, userDomainID);
            var context = CreateContext();
            return
                context.IncomingDocItems.Where(i=>i.IncomingDocItemID==id).Join(
                context.IncomingDocs.Where(d =>d.UserDomainID == userDomainID),
                item => item.IncomingDocID, doc => doc.IncomingDocID, (item, doc) => item)
                .Join(context.GoodsItems, docItem => docItem.GoodsItemID, goodsItem => goodsItem.GoodsItemID,
                      (item, goodsItem) => new IncomingDocItemDTO
                      {
                          Description = item.Description,
                          DimensionKindID = goodsItem.DimensionKindID,
                          GoodsItemID = item.GoodsItemID,
                          GoodsItemTitle = goodsItem.Title,
                          IncomingDocID = item.IncomingDocID,
                          IncomingDocItemID = item.IncomingDocItemID,
                          InitPrice = item.InitPrice,
                          RepairPrice = item.RepairPrice,
                          SalePrice = item.SalePrice,
                          StartPrice = item.StartPrice,
                          Total = item.Total
                      }).FirstOrDefault(i => i.IncomingDocItemID==id);
        }

        /// <summary>
        /// Получение кода домена для пункта приходной накладной.
        /// </summary>
        /// <param name="id">Код пункта приходной накладной.</param>
        /// <returns>Код домена, если элемент существует.</returns>
        public Guid? GetIncomingDocItemUserDomainID(Guid? id)
        {
            var context = CreateContext();

            return context.IncomingDocItems.Where(i => i.IncomingDocItemID == id).Join(context.IncomingDocs,
                                                                                       item => item.IncomingDocID,
                                                                                       doc => doc.IncomingDocID,
                                                                                       (item, doc) => doc.UserDomainID).FirstOrDefault();
        }

        /// <summary>
        /// Удаляет из хранилища элемент приходной накладной по его ID.
        /// </summary>
        /// <param name="id">Код элемента приходной накладной товара.</param>
        public void DeleteIncomingDocItem(Guid? id)
        {
            _logger.InfoFormat("Удаление элемента приходной накладной товара id ={0}", id);

            var context = CreateContext();
            var item = new IncomingDocItem { IncomingDocItemID = id };
            context.IncomingDocItems.Attach(item);
            context.IncomingDocItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Элемент приходной накладной id = {0} успешно удален", id);
        }

        #endregion IncomingDocItem

        #region CancellationDoc

        /// <summary>
        /// Получает список списаний со склада с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="warehouseID">Код склада.</param>
        /// <param name="endDate">Дата окончания накладных.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="beginDate">Дата начала создания накладных.</param>
        /// <returns>Список списаний со склада товаров.</returns>
        public IEnumerable<CancellationDocDTO> GetCancellationDocs(Guid? userDomainID, Guid? warehouseID, DateTime beginDate, DateTime endDate, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка списаний со склада со строкой поиска {0} страница {1} для домена {2} и склада {3}", name, page, userDomainID, warehouseID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items =
                context.CancellationDocs.Where(
                    i =>
                    (i.DocNumber + i.DocDescription).Contains(name) && i.UserDomainID == userDomainID &&
                    i.DocDate >= beginDate && i.DocDate <= endDate);
            if (warehouseID != null)
            {
                items = items.Where(i => i.WarehouseID == warehouseID);
            }

            count = items.Count();

            return items.Join(context.Warehouses, doc => doc.WarehouseID, warehouse => warehouse.WarehouseID,
                                    (doc, warehouse) => new { doc, warehouse }).
                Join(context.Users, arg => arg.doc.CreatorID, user => user.UserID, (arg, user) => new CancellationDocDTO
                {
                    CreatorFirstName = user.FirstName,
                    CreatorID = user.UserID,
                    CreatorLastName = user.LastName,
                    CreatorMiddleName = user.MiddleName,
                    DocDate = arg.doc.DocDate,
                    DocDescription = arg.doc.DocDescription,
                    DocNumber = arg.doc.DocNumber,
                    CancellationDocID = arg.doc.CancellationDocID,
                    UserDomainID = arg.doc.UserDomainID,
                    WarehouseID = arg.doc.WarehouseID,
                    WarehouseTitle = arg.warehouse.Title,
                    IsProcessed = context.ProcessedWarehouseDocs.Any(i => i.ProcessedWarehouseDocID == arg.doc.CancellationDocID)
                }).OrderBy(i => i.DocDate).Page(page, pageSize);

        }

        /// <summary>
        /// Получает список списаний со склада.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список списаний со склада товаров.</returns>
        public IQueryable<CancellationDocDTO> GetCancellationDocs(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списка списаний со склада без строки поиска {0}",userDomainID);
            
            var context = CreateContext();
            var items =
                context.CancellationDocs.Where(
                    i =>
                    i.UserDomainID == userDomainID);
            

            return items.Join(context.Warehouses, doc => doc.WarehouseID, warehouse => warehouse.WarehouseID,
                                    (doc, warehouse) => new { doc, warehouse }).
                Join(context.Users, arg => arg.doc.CreatorID, user => user.UserID, (arg, user) => new CancellationDocDTO
                {
                    CreatorFirstName = user.FirstName,
                    CreatorID = user.UserID,
                    CreatorLastName = user.LastName,
                    CreatorMiddleName = user.MiddleName,
                    DocDate = arg.doc.DocDate,
                    DocDescription = arg.doc.DocDescription,
                    DocNumber = arg.doc.DocNumber,
                    CancellationDocID = arg.doc.CancellationDocID,
                    UserDomainID = arg.doc.UserDomainID,
                    WarehouseID = arg.doc.WarehouseID,
                    WarehouseTitle = arg.warehouse.Title,
                    IsProcessed = context.ProcessedWarehouseDocs.Any(i => i.ProcessedWarehouseDocID == arg.doc.CancellationDocID)
                });
        }

        /// <summary>
        ///   Сохраняет информацию о списании со склада .
        /// </summary>
        /// <param name="cancellationDoc"> Сохраняемое списание со склада. </param>
        public void SaveCancellationDoc(CancellationDoc cancellationDoc)
        {
            _logger.InfoFormat("Сохранение списания со склада с Id = {0}", cancellationDoc.CancellationDocID);
            var context = CreateContext();

            var savedItem =
                context.CancellationDocs.FirstOrDefault(
                    di =>
                    di.CancellationDocID == cancellationDoc.CancellationDocID && di.UserDomainID == cancellationDoc.UserDomainID);

            if (cancellationDoc.CancellationDocID == null || cancellationDoc.CancellationDocID == Guid.Empty)
            {
                cancellationDoc.CancellationDocID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                cancellationDoc.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.CancellationDocs.AddObject(cancellationDoc);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Списание со склада с ID= {0} успешно сохранено",
                               cancellationDoc.CancellationDocID);
        }

        /// <summary>
        /// Получает списание со склада по его ID.
        /// </summary>
        /// <param name="id">Код списания со склада.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Списание со склада, если существует.</returns>
        public CancellationDocDTO GetCancellationDoc(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списание со склада по Id = {0} по домену пользователя {1}", id, userDomainID);
            var context = CreateContext();
            return context.CancellationDocs.Where(i => i.CancellationDocID == id && i.UserDomainID == userDomainID).Join(context.Warehouses, doc => doc.WarehouseID, warehouse => warehouse.WarehouseID,
                                    (doc, warehouse) => new { doc, warehouse }).
                Join(context.Users, arg => arg.doc.CreatorID, user => user.UserID, (arg, user) => new CancellationDocDTO
                {
                    CreatorFirstName = user.FirstName,
                    CreatorID = user.UserID,
                    CreatorLastName = user.LastName,
                    CreatorMiddleName = user.MiddleName,
                    DocDate = arg.doc.DocDate,
                    DocDescription = arg.doc.DocDescription,
                    DocNumber = arg.doc.DocNumber,
                    CancellationDocID = arg.doc.CancellationDocID,
                    UserDomainID = arg.doc.UserDomainID,
                    WarehouseID = arg.doc.WarehouseID,
                    WarehouseTitle = arg.warehouse.Title,
                    IsProcessed = context.ProcessedWarehouseDocs.Any(i => i.ProcessedWarehouseDocID == arg.doc.CancellationDocID)
                }).FirstOrDefault();
        }

        /// <summary>
        /// Получение кода домена для списания со склада.
        /// </summary>
        /// <param name="id">Код списания со склада.</param>
        /// <returns>Код домена, если элемент существует.</returns>
        public Guid? GetCancellationDocUserDomainID(Guid? id)
        {
            var context = CreateContext();

            return context.CancellationDocs.Where(i => i.CancellationDocID == id).Select(i => i.UserDomainID).FirstOrDefault();
        }

        /// <summary>
        /// Удаляет из хранилища списание со склада по ее ID.
        /// </summary>
        /// <param name="id">Код списания со склада.</param>
        public void DeleteCancellationDoc(Guid? id)
        {
            _logger.InfoFormat("Удаление списания со склада id ={0}", id);

            var context = CreateContext();

            if (context.CancellationDocItems.Any(i => i.CancellationDocID == id))
            {
                throw new Exception("Необходимо удалить элементы документа");
            } //if

            var item = new CancellationDoc { CancellationDocID = id };
            context.CancellationDocs.Attach(item);
            context.CancellationDocs.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Списание со склада id = {0} успешно удалено", id);
        }

        #endregion CancellationDoc

        #region CancellationDocItem

        private const string ProcessCancellationDocItemSql = @"

BEGIN TRY
BEGIN TRAN
	
	MERGE WarehouseItem AS wi
        USING
        (
	        SELECT
				di.GoodsItemID,
				SUM(di.[Total]) as [Total]
			FROM CancellationDocItem di            
			WHERE di.CancellationDocID = {0}
            GROUP BY GoodsItemID
            
        ) As s(GoodsItemID,[Total])
        ON(wi.[GoodsItemID] = s.GoodsItemID AND wi.WarehouseID ={1})
        WHEN MATCHED THEN
            UPDATE 
			SET wi.Total =wi.Total-s.[Total]				
        WHEN NOT MATCHED THEN
            INSERT (WarehouseID,GoodsItemID,Total,StartPrice,RepairPrice,SalePrice)
        VALUES({1},s.GoodsItemID,0-s.Total,0,0,0);

		
	INSERT INTO 
		[dbo].[ProcessedWarehouseDoc]
           (
			   [ProcessedWarehouseDocID]
			   ,[WarehouseID]
			   ,[EventDate]
			   ,[UTCEventDateTime]
			   ,[UserID]
		   )
     VALUES           
		   (
		    {0},
            {1},
            {2},
            {3},
            {4}
		   )


COMMIT TRAN
SELECT 'Успех' As ErrorMessage, Cast(1 as bit) As ProcessResult	
END TRY	 	
	BEGIN CATCH
	ROLLBACK TRAN
		SELECT ERROR_MESSAGE() As ErrorMessage, Cast(0 as bit) As ProcessResult	
		
END CATCH

        
        
";

        

        /// <summary>
        /// Обрабатывает пункты документа о списании.
        /// </summary>
        /// <param name="cancellationDocID">Код документа о списании.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="eventDate">Дата обработи документа </param>
        /// <param name="utcEventDateTime">UTC дата и время обработки документа. </param>
        /// <param name="userID">Код обработавшего пользователя.</param>
        public ProcessWarehouseDocResult ProcessCancellationDocItems(Guid? cancellationDocID, Guid? userDomainID, DateTime eventDate, DateTime utcEventDateTime, Guid? userID)
        {
            _logger.InfoFormat("Старт обработки документа о списании {0} домена {1}", cancellationDocID, userDomainID);

            var context = CreateContext();
            var doc =
                context.CancellationDocs.FirstOrDefault(
                    i => i.CancellationDocID == cancellationDocID && i.UserDomainID == userDomainID);
            if (doc == null)
            {
                _logger.ErrorFormat("Нет документа для обработки документа о списании {0} в домене {1}", cancellationDocID, userDomainID);
                return new ProcessWarehouseDocResult
                {
                    ErrorMessage = "Нет такого документа",
                    ProcessResult = false
                };
            }

            return context.ExecuteStoreQuery<ProcessWarehouseDocResult>(ProcessCancellationDocItemSql, cancellationDocID,
                                                                        doc.WarehouseID, eventDate, utcEventDateTime,
                                                                        userID).FirstOrDefault();
        }

        private const string UnProcessCancellationDocItemSql = @"

BEGIN TRY
BEGIN TRAN
	declare @deleteddows table (id uniqueidentifier)

	MERGE WarehouseItem AS wi
        USING
        (
	        SELECT
				di.GoodsItemID,
				SUM(di.[Total]) as [Total]
			FROM CancellationDocItem di            
			WHERE di.CancellationDocID = {0}
            GROUP BY GoodsItemID
            
        ) As s(GoodsItemID,[Total])
        ON(wi.[GoodsItemID] = s.GoodsItemID AND wi.WarehouseID ={1})
        WHEN MATCHED THEN
            UPDATE 
			SET wi.Total =wi.Total+s.[Total]				
        WHEN NOT MATCHED THEN
            INSERT (WarehouseID,GoodsItemID,Total,StartPrice,RepairPrice,SalePrice)
        VALUES({1},s.GoodsItemID,s.Total,0,0,0);

		
	DELETE
		[dbo].[ProcessedWarehouseDoc]
		OUTPUT DELETED.WarehouseID INTO @deleteddows
	WHERE [ProcessedWarehouseDocID] = {0}
	
	IF(NOT EXISTS(SELECT * FROM @deleteddows))
	BEGIN
		ROLLBACK TRAN
		SELECT 'Документ еще не сформирован' As ErrorMessage, Cast(1 as bit) As ProcessResult	
	END	           
	ELSE BEGIN
		COMMIT TRAN
		SELECT 'Успех' As ErrorMessage, Cast(1 as bit) As ProcessResult	
	END
END TRY	 	
	BEGIN CATCH
	ROLLBACK TRAN
		SELECT ERROR_MESSAGE() As ErrorMessage, Cast(0 as bit) As ProcessResult	
		
END CATCH

";

        /// <summary>
        /// Отменяет обработку пунктов документа о списании.
        /// </summary>
        /// <param name="cancellationDocID">Код документа о списании.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        public ProcessWarehouseDocResult UnProcessCancellationDocItems(Guid? cancellationDocID, Guid? userDomainID)
        {
            _logger.InfoFormat("Старт отмены обработки документа о списании {0} домена {1}", cancellationDocID, userDomainID);

            var context = CreateContext();
            var doc =
                context.CancellationDocs.FirstOrDefault(
                    i => i.CancellationDocID == cancellationDocID && i.UserDomainID == userDomainID);
            if (doc == null)
            {
                _logger.ErrorFormat("Нет документа для отмены обработки документа о списании {0} в домене {1}", cancellationDocID, userDomainID);
                return new ProcessWarehouseDocResult
                {
                    ErrorMessage = "Нет такого документа",
                    ProcessResult = false
                };
            }

            return context.ExecuteStoreQuery<ProcessWarehouseDocResult>(UnProcessCancellationDocItemSql, cancellationDocID,
                                                                        doc.WarehouseID).FirstOrDefault();
        }

        /// <summary>
        /// Получает список элемент документа о списании с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="cancellationDocID">Код документа о списании.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список элементов документа о списании товаров.</returns>
        public IEnumerable<CancellationDocItemDTO> GetCancellationDocItems(Guid? userDomainID, Guid? cancellationDocID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка элементов документа о списании товаров со строкой поиска {0} страница {1} для домена {2} и документа {3}", name, page, userDomainID, cancellationDocID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.CancellationDocItems.Join(
                context.CancellationDocs.Where(d => d.CancellationDocID == cancellationDocID && d.UserDomainID == userDomainID),
                item => item.CancellationDocID, doc => doc.CancellationDocID, (item, doc) => item)
                .Join(context.GoodsItems, docItem => docItem.GoodsItemID, goodsItem => goodsItem.GoodsItemID,
                      (item, goodsItem) => new CancellationDocItemDTO
                      {
                          Description = item.Description,
                          DimensionKindID = goodsItem.DimensionKindID,
                          GoodsItemID = item.GoodsItemID,
                          GoodsItemTitle = goodsItem.Title,
                          CancellationDocID = item.CancellationDocID,
                          CancellationDocItemID = item.CancellationDocItemID,
                          Total = item.Total
                      }).Where(i => i.GoodsItemTitle.Contains(name));
            count = items.Count();

            return items.OrderBy(i => i.GoodsItemTitle).Page(page, pageSize);
        }

        /// <summary>
        /// Получает список элемент документа о списании.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="cancellationDocID">Код документа о списании.</param>
        /// <returns>Список элементов документа о списании товаров.</returns>
        public IQueryable<CancellationDocItemDTO> GetCancellationDocItems(Guid? userDomainID, Guid? cancellationDocID)
        {
            _logger.InfoFormat("Получение списка элементов документа о списании товаров без строки поиска {0} ", cancellationDocID);
            var context = CreateContext();
            var items = context.CancellationDocItems.Join(
                context.CancellationDocs.Where(d => d.CancellationDocID == cancellationDocID && d.UserDomainID == userDomainID),
                item => item.CancellationDocID, doc => doc.CancellationDocID, (item, doc) => item)
                .Join(context.GoodsItems, docItem => docItem.GoodsItemID, goodsItem => goodsItem.GoodsItemID,
                      (item, goodsItem) => new CancellationDocItemDTO
                      {
                          Description = item.Description,
                          DimensionKindID = goodsItem.DimensionKindID,
                          GoodsItemID = item.GoodsItemID,
                          GoodsItemTitle = goodsItem.Title,
                          CancellationDocID = item.CancellationDocID,
                          CancellationDocItemID = item.CancellationDocItemID,
                          Total = item.Total
                      });
            return items;
        }

        /// <summary>
        ///   Сохраняет информацию об элементе документа о списании.
        /// </summary>
        /// <remarks>Нужна обязательная проверка перед вызовом на доступ к вмененной накладной пользователя.</remarks>
        /// <param name="cancellationDocItem"> Сохраняемый элемент документа о списании. </param>
        public void SaveCancellationDocItem(CancellationDocItem cancellationDocItem)
        {
            _logger.InfoFormat("Сохранение элемента документа о списании с Id = {0}", cancellationDocItem.CancellationDocItemID);
            var context = CreateContext();

            var savedItem =
                context.CancellationDocItems.FirstOrDefault(
                    di =>
                    di.CancellationDocItemID == cancellationDocItem.CancellationDocItemID);

            if (cancellationDocItem.CancellationDocItemID == null || cancellationDocItem.CancellationDocItemID == Guid.Empty)
            {
                cancellationDocItem.CancellationDocItemID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                cancellationDocItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.CancellationDocItems.AddObject(cancellationDocItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Элемент документа о списании с ID= {0} успешно сохранен",
                               cancellationDocItem.CancellationDocItemID);
        }

        /// <summary>
        /// Получает элемент документа о списании по его ID.
        /// </summary>
        /// <param name="id">Код элемента документа о списании.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Элемент документа о списании, если существует.</returns>
        public CancellationDocItemDTO GetCancellationDocItem(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение элемента документа о списании по Id = {0} по домену пользователя {1}", id, userDomainID);
            var context = CreateContext();
            return
                context.CancellationDocItems.Where(i=>i.CancellationDocItemID==id).Join(
                context.CancellationDocs.Where(d => d.UserDomainID == userDomainID),
                item => item.CancellationDocID, doc => doc.CancellationDocID, (item, doc) => item)
                .Join(context.GoodsItems, docItem => docItem.GoodsItemID, goodsItem => goodsItem.GoodsItemID,
                      (item, goodsItem) => new CancellationDocItemDTO
                      {
                          Description = item.Description,
                          DimensionKindID = goodsItem.DimensionKindID,
                          GoodsItemID = item.GoodsItemID,
                          GoodsItemTitle = goodsItem.Title,
                          CancellationDocID = item.CancellationDocID,
                          CancellationDocItemID = item.CancellationDocItemID,
                          Total = item.Total
                      }).FirstOrDefault(i => i.CancellationDocItemID == id);
        }

        /// <summary>
        /// Получение кода домена для пункта документа о списании.
        /// </summary>
        /// <param name="id">Код пункта документа о списании.</param>
        /// <returns>Код домена, если элемент существует.</returns>
        public Guid? GetCancellationDocItemUserDomainID(Guid? id)
        {
            var context = CreateContext();

            return context.CancellationDocItems.Where(i => i.CancellationDocItemID == id).Join(context.CancellationDocs,
                                                                                       item => item.CancellationDocID,
                                                                                       doc => doc.CancellationDocID,
                                                                                       (item, doc) => doc.UserDomainID).FirstOrDefault();
        }

        /// <summary>
        /// Удаляет из хранилища элемент документа о списании по его ID.
        /// </summary>
        /// <param name="id">Код элемента документа о списании товара.</param>
        public void DeleteCancellationDocItem(Guid? id)
        {
            _logger.InfoFormat("Удаление элемента документа о списании товара id ={0}", id);

            var context = CreateContext();
            var item = new CancellationDocItem { CancellationDocItemID = id };
            context.CancellationDocItems.Attach(item);
            context.CancellationDocItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Элемент документа о списании id = {0} успешно удален", id);
        }

        #endregion CancellationDocItem

        #region TransferDoc

        /// <summary>
        /// Получает список перемещений со склада на склад с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="senderWarehouseID">Код склада с которого делают перемещение.</param>
        /// <param name="endDate">Дата окончания накладных.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <param name="recipientWarehouseID">Код склада на который делают перемещение. </param>
        /// <param name="beginDate">Дата начала создания накладных.</param>
        /// <returns>Список перемещений со склада товаров.</returns>
        public IEnumerable<TransferDocDTO> GetTransferDocs(Guid? userDomainID, Guid? senderWarehouseID, Guid? recipientWarehouseID, DateTime beginDate, DateTime endDate, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка перемещений со склада со строкой поиска {0} страница {1} для домена {2} и склада {3}", name, page, userDomainID, senderWarehouseID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items =
                context.TransferDocs.Where(
                    i =>
                    (i.DocNumber + i.DocDescription).Contains(name) && i.UserDomainID == userDomainID &&
                    i.DocDate >= beginDate && i.DocDate <= endDate);
            if (senderWarehouseID != null)
            {
                items = items.Where(i => i.SenderWarehouseID == senderWarehouseID);
            }

            if (recipientWarehouseID != null)
            {
                items = items.Where(i => i.RecipientWarehouseID == recipientWarehouseID);
            }

            count = items.Count();

            return items.Join(context.Warehouses, doc => doc.SenderWarehouseID, warehouse => warehouse.WarehouseID,
                              (doc, warehouse) =>
                              new { doc, senderWarehouse = warehouse }).Join(context.Warehouses,
                                                                           arg => arg.doc.RecipientWarehouseID,
                                                                           warehouse => warehouse.WarehouseID,
                                                                           (arg, warehouse) => new
                                                                                               {
                                                                                                   arg.doc,
                                                                                                   arg.senderWarehouse,
                                                                                                   recipientWarehouse =
                                                                                                   warehouse
                                                                                               }
                ).Join(context.Users, arg => arg.doc.CreatorID, user => user.UserID, (arg, user)
                    => new TransferDocDTO
                    {
                        CreatorID = user.UserID,
                        DocDescription = arg.doc.DocDescription,
                        DocNumber = arg.doc.DocNumber,
                        DocDate = arg.doc.DocDate,
                        CreatorFirstName = user.FirstName,
                        CreatorLastName = user.LastName,
                        CreatorMiddleName = user.MiddleName,
                        RecipientWarehouseID = arg.doc.RecipientWarehouseID,
                        RecipientWarehouseTitle = arg.recipientWarehouse.Title,
                        SenderWarehouseID = arg.doc.SenderWarehouseID,
                        SenderWarehouseTitle = arg.senderWarehouse.Title,
                        TransferDocID = arg.doc.TransferDocID,
                        UserDomainID = arg.doc.UserDomainID,
                        IsProcessed = context.ProcessedWarehouseDocs.Any(i => i.ProcessedWarehouseDocID == arg.doc.TransferDocID)
                    }).OrderBy(i => i.DocDate)
                .Page(page, pageSize);


        }

        /// <summary>
        /// Получает список перемещений со склада на склад.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список перемещений со склада товаров.</returns>
        public IQueryable<TransferDocDTO> GetTransferDocs(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списка перемещений со склада без строки поиска {0}", userDomainID);
            var context = CreateContext();
            var items =
                context.TransferDocs.Where(
                    i =>
                    i.UserDomainID == userDomainID);
          
            return
                items.Join(context.Users, arg => arg.CreatorID, user => user.UserID, (arg, user)
                    => new TransferDocDTO
                    {
                        CreatorID = user.UserID,
                        DocDescription = arg.DocDescription,
                        DocNumber = arg.DocNumber,
                        DocDate = arg.DocDate,
                        CreatorFirstName = user.FirstName,
                        CreatorLastName = user.LastName,
                        CreatorMiddleName = user.MiddleName,
                        RecipientWarehouseID = arg.RecipientWarehouseID,
                        SenderWarehouseID = arg.SenderWarehouseID,
                        TransferDocID = arg.TransferDocID,
                        UserDomainID = arg.UserDomainID,
                        IsProcessed = context.ProcessedWarehouseDocs.Any(i => i.ProcessedWarehouseDocID == arg.TransferDocID)
                    });
        }

        /// <summary>
        ///   Сохраняет информацию о перемещении со склада .
        /// </summary>
        /// <param name="transferDoc"> Сохраняемое перемещение со склада. </param>
        public void SaveTransferDoc(TransferDoc transferDoc)
        {
            _logger.InfoFormat("Сохранение перемещения со склада с Id = {0}", transferDoc.TransferDocID);
            var context = CreateContext();

            var savedItem =
                context.TransferDocs.FirstOrDefault(
                    di =>
                    di.TransferDocID == transferDoc.TransferDocID && di.UserDomainID == transferDoc.UserDomainID);

            if (transferDoc.TransferDocID == null || transferDoc.TransferDocID == Guid.Empty)
            {
                transferDoc.TransferDocID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                transferDoc.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.TransferDocs.AddObject(transferDoc);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Перемещение со склада с ID= {0} успешно сохранено",
                               transferDoc.TransferDocID);
        }

        /// <summary>
        /// Получает перемещение со склада по его ID.
        /// </summary>
        /// <param name="id">Код перемещения со склада.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Перемещение со склада, если существует.</returns>
        public TransferDocDTO GetTransferDoc(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение перемещение со склада по Id = {0} по домену пользователя {1}", id, userDomainID);
            var context = CreateContext();
            return context.TransferDocs.Where(i => i.TransferDocID == id && i.UserDomainID == userDomainID).Join(context.Warehouses, doc => doc.SenderWarehouseID, warehouse => warehouse.WarehouseID,
                              (doc, warehouse) =>
                              new { doc, senderWarehouse = warehouse }).Join(context.Warehouses,
                                                                           arg => arg.doc.RecipientWarehouseID,
                                                                           warehouse => warehouse.WarehouseID,
                                                                           (arg, warehouse) => new
                                                                                               {
                                                                                                   arg.doc,
                                                                                                   arg.senderWarehouse,
                                                                                                   recipientWarehouse =
                                                                                                   warehouse
                                                                                               }
                ).Join(context.Users, arg => arg.doc.CreatorID, user => user.UserID, (arg, user)
                    => new TransferDocDTO
                    {
                        CreatorID = user.UserID,
                        DocDescription = arg.doc.DocDescription,
                        DocNumber = arg.doc.DocNumber,
                        DocDate = arg.doc.DocDate,
                        CreatorFirstName = user.FirstName,
                        CreatorLastName = user.LastName,
                        CreatorMiddleName = user.MiddleName,
                        RecipientWarehouseID = arg.doc.RecipientWarehouseID,
                        RecipientWarehouseTitle = arg.recipientWarehouse.Title,
                        SenderWarehouseID = arg.doc.SenderWarehouseID,
                        SenderWarehouseTitle = arg.senderWarehouse.Title,
                        TransferDocID = arg.doc.TransferDocID,
                        UserDomainID = arg.doc.UserDomainID,
                        IsProcessed = context.ProcessedWarehouseDocs.Any(i => i.ProcessedWarehouseDocID == arg.doc.TransferDocID)
                    }).FirstOrDefault();
        }

        /// <summary>
        /// Получение кода домена для перемещения со склада.
        /// </summary>
        /// <param name="id">Код перемещения со склада.</param>
        /// <returns>Код домена, если элемент существует.</returns>
        public Guid? GetTransferDocUserDomainID(Guid? id)
        {
            var context = CreateContext();

            return context.TransferDocs.Where(i => i.TransferDocID == id).Select(i => i.UserDomainID).FirstOrDefault();
        }

        /// <summary>
        /// Удаляет из хранилища перемещение со склада по ее ID.
        /// </summary>
        /// <param name="id">Код перемещения со склада.</param>
        public void DeleteTransferDoc(Guid? id)
        {
            _logger.InfoFormat("Удаление перемещения со склада id ={0}", id);

            var context = CreateContext();

            if (context.TransferDocItems.Any(i => i.TransferDocID == id))
            {
                throw new Exception("Необходимо удалить элементы документа");
            } //if

            var item = new TransferDoc { TransferDocID = id };
            context.TransferDocs.Attach(item);
            context.TransferDocs.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Перемещение со склада id = {0} успешно удалено", id);
        }

        #endregion TransferDoc

        #region TransferDocItem

        private const string ProcessTransferDocItemsSql = @"

BEGIN TRY
BEGIN TRAN
	
	MERGE WarehouseItem AS wi
        USING
        (
	        SELECT
				di.GoodsItemID,
				SUM(di.[Total]) As Total
			FROM TransferDocItem di
			WHERE di.TransferDocID = {0}
			GROUP BY GoodsItemID
        ) As s(GoodsItemID,[Total])
        ON(wi.[GoodsItemID] = s.GoodsItemID AND wi.WarehouseID ={1})
        WHEN MATCHED THEN
            UPDATE 
			SET wi.Total =wi.Total-s.[Total]				
        WHEN NOT MATCHED THEN
            INSERT (WarehouseID,GoodsItemID,Total,StartPrice,RepairPrice,SalePrice)
        VALUES({1},s.GoodsItemID,0-s.Total,0,0,0);

		MERGE WarehouseItem AS wi
        USING
        (
	        SELECT
				di.GoodsItemID,
				sum(di.[Total]) as Total
			FROM TransferDocItem di
			WHERE di.TransferDocID = {0}
			GROUP BY GoodsItemID
        ) As s(GoodsItemID,[Total])
        ON(wi.[GoodsItemID] = s.GoodsItemID AND wi.WarehouseID ={2})
        WHEN MATCHED THEN
            UPDATE 
			SET wi.Total =wi.Total+s.[Total]				
        WHEN NOT MATCHED THEN
            INSERT (WarehouseID,GoodsItemID,Total,StartPrice,RepairPrice,SalePrice)
        VALUES({2},s.GoodsItemID,s.Total,0,0,0);
		

	INSERT INTO 
		[dbo].[ProcessedWarehouseDoc]
           (
			   [ProcessedWarehouseDocID]
			   ,[WarehouseID]
			   ,[EventDate]
			   ,[UTCEventDateTime]
			   ,[UserID]
		   )
     VALUES           
		   (
		    {0},
            {1},
            {3},
            {4},
            {5}
		   )


COMMIT TRAN
SELECT 'Успех' As ErrorMessage, Cast(1 as bit) As ProcessResult	
END TRY	 	
	BEGIN CATCH
	ROLLBACK TRAN
		SELECT ERROR_MESSAGE() As ErrorMessage, Cast(0 as bit) As ProcessResult	
		
END CATCH

        
        
";

        /// <summary>
        /// Обрабатывает пункты документов перемещения остатков со склада на склад.
        /// </summary>
        /// <param name="transferDocID">Код документа перемещения.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="eventDate">Дата обработи документа </param>
        /// <param name="utcEventDateTime">UTC дата и время обработки документа. </param>
        /// <param name="userID">Код обработавшего пользователя.</param>
        public ProcessWarehouseDocResult ProcessTransferDocItems(Guid? transferDocID, Guid? userDomainID, DateTime eventDate, DateTime utcEventDateTime, Guid? userID)
        {
            _logger.InfoFormat("Старт обработки документа перемещения остатсков на слале {0} домена {1}", transferDocID, userDomainID);

            var context = CreateContext();
            var doc =
                context.TransferDocs.FirstOrDefault(
                    i => i.TransferDocID == transferDocID && i.UserDomainID == userDomainID);
            if (doc == null)
            {
                _logger.ErrorFormat("Нет документа для обработки документа перемещения остатков на складе {0} в домене {1}", transferDocID, userDomainID);
                return new ProcessWarehouseDocResult
                {
                    ErrorMessage = "Нет такого документа",
                    ProcessResult = false
                };
            }

            return context.ExecuteStoreQuery<ProcessWarehouseDocResult>(ProcessTransferDocItemsSql, transferDocID,
                                                                        doc.SenderWarehouseID,doc.RecipientWarehouseID, eventDate, utcEventDateTime,
                                                                        userID).FirstOrDefault();
        }

        private const string UnProcessTransferDocItemsSql = @"
BEGIN TRY
BEGIN TRAN
	declare @deleteddows table (id uniqueidentifier)

	MERGE WarehouseItem AS wi
        USING
        (
	        SELECT
				di.GoodsItemID,
				SUM(di.[Total]) As Total
			FROM TransferDocItem di
			WHERE di.TransferDocID = {0}
			GROUP BY GoodsItemID
        ) As s(GoodsItemID,[Total])
        ON(wi.[GoodsItemID] = s.GoodsItemID AND wi.WarehouseID ={1})
        WHEN MATCHED THEN
            UPDATE 
			SET wi.Total =wi.Total+s.[Total]				
        WHEN NOT MATCHED THEN
            INSERT (WarehouseID,GoodsItemID,Total,StartPrice,RepairPrice,SalePrice)
        VALUES({1},s.GoodsItemID,s.Total,0,0,0);

		MERGE WarehouseItem AS wi
        USING
        (
	        SELECT
				di.GoodsItemID,
				sum(di.[Total]) as Total
			FROM TransferDocItem di
			WHERE di.TransferDocID = {0}
			GROUP BY GoodsItemID
        ) As s(GoodsItemID,[Total])
        ON(wi.[GoodsItemID] = s.GoodsItemID AND wi.WarehouseID ={2})
        WHEN MATCHED THEN
            UPDATE 
			SET wi.Total =wi.Total-s.[Total]				
        WHEN NOT MATCHED THEN
            INSERT (WarehouseID,GoodsItemID,Total,StartPrice,RepairPrice,SalePrice)
        VALUES({2},s.GoodsItemID,0-s.Total,0,0,0);
		

	DELETE
		[dbo].[ProcessedWarehouseDoc]
		OUTPUT DELETED.WarehouseID INTO @deleteddows
	WHERE [ProcessedWarehouseDocID] = {0}
	
	IF(NOT EXISTS(SELECT * FROM @deleteddows))
	BEGIN
		ROLLBACK TRAN
		SELECT 'Документ еще не сформирован' As ErrorMessage, Cast(1 as bit) As ProcessResult	
	END	           
	ELSE BEGIN
		COMMIT TRAN
		SELECT 'Успех' As ErrorMessage, Cast(1 as bit) As ProcessResult	
	END
END TRY	 	
	BEGIN CATCH
	ROLLBACK TRAN
		SELECT ERROR_MESSAGE() As ErrorMessage, Cast(0 as bit) As ProcessResult	
		
END CATCH


";

        /// <summary>
        /// Отменяет обработанные пункты пункты документов перемещения остатков со склада на склад.
        /// </summary>
        /// <param name="transferDocID">Код отменяемого документа перемещения.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        public ProcessWarehouseDocResult UnProcessTransferDocItems(Guid? transferDocID, Guid? userDomainID)
        {
            _logger.InfoFormat("Старт отмены обработки документа перемещения остатсков на слале {0} домена {1}", transferDocID, userDomainID);

            var context = CreateContext();
            var doc =
                context.TransferDocs.FirstOrDefault(
                    i => i.TransferDocID == transferDocID && i.UserDomainID == userDomainID);
            if (doc == null)
            {
                _logger.ErrorFormat("Нет документа для отмены обработки перемещения остатков на складе {0} в домене {1}", transferDocID, userDomainID);
                return new ProcessWarehouseDocResult
                {
                    ErrorMessage = "Нет такого документа",
                    ProcessResult = false
                };
            }

            return context.ExecuteStoreQuery<ProcessWarehouseDocResult>(UnProcessTransferDocItemsSql, transferDocID,
                                                                        doc.SenderWarehouseID, doc.RecipientWarehouseID).FirstOrDefault();
        }

        /// <summary>
        /// Получает список элемент документа о перемещении с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="transferDocID">Код документа о перемещении.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список элементов документа о перемещении товаров.</returns>
        public IEnumerable<TransferDocItemDTO> GetTransferDocItems(Guid? userDomainID, Guid? transferDocID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка элементов документа о перемещении товаров со строкой поиска {0} страница {1} для домена {2} и документа {3}", name, page, userDomainID, transferDocID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.TransferDocItems.Join(
                context.TransferDocs.Where(d => d.TransferDocID == transferDocID && d.UserDomainID == userDomainID),
                item => item.TransferDocID, doc => doc.TransferDocID, (item, doc) => item)
                .Join(context.GoodsItems, docItem => docItem.GoodsItemID, goodsItem => goodsItem.GoodsItemID,
                      (item, goodsItem) => new TransferDocItemDTO
                      {
                          Description = item.Description,
                          DimensionKindID = goodsItem.DimensionKindID,
                          GoodsItemID = item.GoodsItemID,
                          GoodsItemTitle = goodsItem.Title,
                          TransferDocID = item.TransferDocID,
                          TransferDocItemID = item.TransferDocItemID,
                          Total = item.Total,
                      }).Where(i => i.GoodsItemTitle.Contains(name));
            count = items.Count();

            return items.OrderBy(i => i.GoodsItemTitle).Page(page, pageSize);
        }

        /// <summary>
        /// Получает список элементов документа о перемещении.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="transferDocID">Код документа о перемещении.</param>
        /// <returns>Список элементов документа о перемещении товаров.</returns>
        public IQueryable<TransferDocItemDTO> GetTransferDocItems(Guid? userDomainID, Guid? transferDocID)
        {
            _logger.InfoFormat("Получение списка элементов документа о перемещении товаров без строки поиска {0} для документа {1}", userDomainID, transferDocID);
            
            var context = CreateContext();
            var items = context.TransferDocItems.Join(
                context.TransferDocs.Where(d => d.TransferDocID == transferDocID && d.UserDomainID == userDomainID),
                item => item.TransferDocID, doc => doc.TransferDocID, (item, doc) => item)
                .Join(context.GoodsItems, docItem => docItem.GoodsItemID, goodsItem => goodsItem.GoodsItemID,
                      (item, goodsItem) => new TransferDocItemDTO
                      {
                          Description = item.Description,
                          DimensionKindID = goodsItem.DimensionKindID,
                          GoodsItemID = item.GoodsItemID,
                          GoodsItemTitle = goodsItem.Title,
                          TransferDocID = item.TransferDocID,
                          TransferDocItemID = item.TransferDocItemID,
                          Total = item.Total,
                      });
            return items;
        }

        /// <summary>
        ///   Сохраняет информацию об элементе документа о перемещении.
        /// </summary>
        /// <remarks>Нужна обязательная проверка перед вызовом на доступ к вмененной накладной пользователя.</remarks>
        /// <param name="transferDocItem"> Сохраняемый элемент документа о перемещении. </param>
        public void SaveTransferDocItem(TransferDocItem transferDocItem)
        {
            _logger.InfoFormat("Сохранение элемента документа о перемещении с Id = {0}", transferDocItem.TransferDocItemID);
            var context = CreateContext();

            var savedItem =
                context.TransferDocItems.FirstOrDefault(
                    di =>
                    di.TransferDocItemID == transferDocItem.TransferDocItemID);

            if (transferDocItem.TransferDocItemID == null || transferDocItem.TransferDocItemID == Guid.Empty)
            {
                transferDocItem.TransferDocItemID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                transferDocItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.TransferDocItems.AddObject(transferDocItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Элемент документа о перемещении с ID= {0} успешно сохранен",
                               transferDocItem.TransferDocItemID);
        }

        /// <summary>
        /// Получает элемент документа о перемещении по его ID.
        /// </summary>
        /// <param name="id">Код элемента документа о перемещении.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Элемент документа о перемещении, если существует.</returns>
        public TransferDocItemDTO GetTransferDocItem(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение элемента документа о перемещении по Id = {0} по домену пользователя {1}", id, userDomainID);
            var context = CreateContext();
            return
                context.TransferDocItems.Where(i=>i.TransferDocItemID==id).Join(
                context.TransferDocs.Where(d => d.UserDomainID == userDomainID),
                item => item.TransferDocID, doc => doc.TransferDocID, (item, doc) => item)
                .Join(context.GoodsItems, docItem => docItem.GoodsItemID, goodsItem => goodsItem.GoodsItemID,
                      (item, goodsItem) => new TransferDocItemDTO
                      {
                          Description = item.Description,
                          DimensionKindID = goodsItem.DimensionKindID,
                          GoodsItemID = item.GoodsItemID,
                          GoodsItemTitle = goodsItem.Title,
                          TransferDocID = item.TransferDocID,
                          TransferDocItemID = item.TransferDocItemID,
                          Total = item.Total
                      }).FirstOrDefault(i => i.TransferDocItemID == id);
        }

        /// <summary>
        /// Получение кода домена для пункта документа о перемещении.
        /// </summary>
        /// <param name="id">Код пункта документа о перемещении.</param>
        /// <returns>Код домена, если элемент существует.</returns>
        public Guid? GetTransferDocItemUserDomainID(Guid? id)
        {
            var context = CreateContext();

            return context.TransferDocItems.Where(i => i.TransferDocItemID == id).Join(context.TransferDocs,
                                                                                       item => item.TransferDocID,
                                                                                       doc => doc.TransferDocID,
                                                                                       (item, doc) => doc.UserDomainID).FirstOrDefault();
        }

        /// <summary>
        /// Удаляет из хранилища элемент документа о перемещении по его ID.
        /// </summary>
        /// <param name="id">Код элемента документа о перемещении товара.</param>
        public void DeleteTransferDocItem(Guid? id)
        {
            _logger.InfoFormat("Удаление элемента документа о перемещении товара id ={0}", id);

            var context = CreateContext();
            var item = new TransferDocItem { TransferDocItemID = id };
            context.TransferDocItems.Attach(item);
            context.TransferDocItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Элемент документа о перемещении id = {0} успешно удален", id);
        }

        #endregion TransferDocItem

        #region ProcessedWarehouseDoc

        /// <summary>
        ///   Сохраняет информацию о обработанный документе.
        /// </summary>
        /// <param name="processedWarehouseDoc"> Сохраняемый обработанный документ. </param>
        public void SaveProcessedWarehouseDoc(ProcessedWarehouseDoc processedWarehouseDoc)
        {
            _logger.InfoFormat("Сохранение обработанный документа с Id = {0}",
                               processedWarehouseDoc.ProcessedWarehouseDocID);
            var context = CreateContext();

            if (processedWarehouseDoc.ProcessedWarehouseDocID == null ||
                processedWarehouseDoc.ProcessedWarehouseDocID == Guid.Empty)
            {
                throw new DataException(string.Format("ID документа должно быть инициализированно для склада {0}",processedWarehouseDoc.WarehouseID));
            } //if

            context.ProcessedWarehouseDocs.AddObject(processedWarehouseDoc);
            context.SaveChanges(SaveOptions.None);


            _logger.InfoFormat("Обработанный документ с ID= {0} успешно сохранен",
                               processedWarehouseDoc.ProcessedWarehouseDocID);
        }

        /// <summary>
        /// Получает обработанный документ по его ID.
        /// </summary>
        /// <param name="id">Код обработанный документа.</param>
        /// <returns>Обработанный документ, если существует.</returns>
        public ProcessedWarehouseDoc GetProcessedWarehouseDoc(Guid? id)
        {
            _logger.InfoFormat("Получение обработанный документа по Id = {0}", id);
            var context = CreateContext();
            return context.ProcessedWarehouseDocs.FirstOrDefault(fs => fs.ProcessedWarehouseDocID == id);
        }

        /// <summary>
        /// Удаляет из хранилища обработанный документ по его ID.
        /// </summary>
        /// <param name="id">Код обработанный документа.</param>
        public void DeleteProcessedWarehouseDoc(Guid? id)
        {
            _logger.InfoFormat("Удаление обработанный документа id ={0}", id);

            var context = CreateContext();
            var item = new ProcessedWarehouseDoc { ProcessedWarehouseDocID = id };
            context.ProcessedWarehouseDocs.Attach(item);
            context.ProcessedWarehouseDocs.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Конрагент id = {0} успешно удален", id);
        }

        /// <summary>
        /// Проверяет на признак обработки пользователем конкретного складского документа.
        /// </summary>
        /// <param name="docID">Код складского документа.</param>
        /// <returns>Признак обработки.</returns>
        public bool WarehouseDocIsProcessed(Guid? docID)
        {
            _logger.InfoFormat("Проверка на обработку складского документа id ={0}", docID);

            var context = CreateContext();

            return context.ProcessedWarehouseDocs.Any(d => d.ProcessedWarehouseDocID == docID);
        }

        #endregion ProcessedWarehouseDoc

        #region FinancialGroupWarehouseMapItem

        /// <summary>
        /// Возвращает информацию о связах домена финансовых групп с филиалами.
        /// </summary>
        /// <param name="userDomainID">Код домена.</param>
        /// <returns>Связи филиалов и фингрупп.</returns>
        public IEnumerable<FinancialGroupWarehouseMapItem> GetFinancialGroupWarehouseMapItems(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение всех связей складов и фингрупп домена {0}", userDomainID);
            var context = CreateContext();

            return
                context.Warehouses.Join(context.FinancialGroupWarehouseMapItems,
                                                          mapItem => mapItem.WarehouseID,
                                                          item => item.WarehouseID,
                                                          (mapItem, groupItem) => new { groupItem, mapItem }).Where(
                                                              i => i.mapItem.UserDomainID == userDomainID).Select(
                                                                  i => i.groupItem);
        }

        /// <summary>
        /// Возвращает информацию о связах конкретного финансовой группы со складами.
        /// </summary>
        /// <param name="financialGroupID">Код группы.</param>
        /// <returns>Склады.</returns>
        public IEnumerable<FinancialGroupWarehouseMapItemDTO> GetFinancialGroupWarehouseMapItemsByFinancialGroup(Guid? financialGroupID)
        {
            _logger.InfoFormat("Получение всех складов группы {0}", financialGroupID);
            var context = CreateContext();


            return context.FinancialGroupWarehouseMapItems.Where(i => i.FinancialGroupID == financialGroupID).Join(context.FinancialGroupItems, mapItem => mapItem.FinancialGroupID, item => item.FinancialGroupID, (mapItem, groupItem) => new { groupItem, mapItem }).
                Join(context.Warehouses, arg => arg.mapItem.WarehouseID, branch => branch.WarehouseID, (arg, branch) =>
                new FinancialGroupWarehouseMapItemDTO
                {
                    WarehouseID = arg.mapItem.WarehouseID,
                    WarehouseTitle = branch.Title,
                    FinancialGroupWarehouseMapID = arg.mapItem.FinancialGroupWarehouseMapID,
                    FinancialGroupID = arg.groupItem.FinancialGroupID,
                    FinancialGroupTitle = arg.groupItem.Title

                });
        }


        private const string DeleteFinancialGroupWarehouseMapItemsSql = "DELETE FROM FinancialGroupWarehouseMap WHERE FinancialGroupID = {0}";

        /// <summary>
        /// Удаляет все связанные с финансовой группы склады.
        /// </summary>
        /// <param name="financialGroupID">Код финансовой группы.</param>
        public void DeleteFinancialGroupWarehouseMapItems(Guid? financialGroupID)
        {
            _logger.InfoFormat("Удаление всех складов финансовой группы пользователя {0}", financialGroupID);

            var context = CreateContext();
            context.ExecuteStoreCommand(DeleteFinancialGroupWarehouseMapItemsSql, financialGroupID);
        }

        /// <summary>
        ///   Сохраняет информацию о соответствии финансовой группы и склада.
        /// </summary>
        /// <param name="financialGroupWarehouseMapItem"> Сохраняемое соответствие. </param>
        public void SaveFinancialGroupMapWarehouseItem(FinancialGroupWarehouseMapItem financialGroupWarehouseMapItem)
        {
            _logger.InfoFormat("Сохранение соответствия с Id = {0} между финансовой группой {1} и складом {2}", financialGroupWarehouseMapItem.FinancialGroupWarehouseMapID, financialGroupWarehouseMapItem.FinancialGroupID, financialGroupWarehouseMapItem.WarehouseID);
            var context = CreateContext();

            var savedItem =
                context.FinancialGroupWarehouseMapItems.FirstOrDefault(
                    di => di.FinancialGroupWarehouseMapID == financialGroupWarehouseMapItem.FinancialGroupWarehouseMapID);

            if (financialGroupWarehouseMapItem.FinancialGroupWarehouseMapID == null || financialGroupWarehouseMapItem.FinancialGroupWarehouseMapID == Guid.Empty)
            {
                financialGroupWarehouseMapItem.FinancialGroupWarehouseMapID = Guid.NewGuid();
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
            return context.FinancialGroupWarehouseMapItems.FirstOrDefault(fs => fs.FinancialGroupWarehouseMapID == id);
        }

        /// <summary>
        /// Удаляет из хранилища соответствие между финансовой группой и складом по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        public void DeleteFinancialGroupWarehouseMapItem(Guid? id)
        {
            _logger.InfoFormat("Удаление соответствия финансовой группы и склада id ={0}", id);

            var context = CreateContext();
            var item = new FinancialGroupWarehouseMapItem { FinancialGroupWarehouseMapID = id };
            context.FinancialGroupWarehouseMapItems.Attach(item);
            context.FinancialGroupWarehouseMapItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Соответствие финансовой группы и склада с id = {0} успешно удалено", id);
        }

        #endregion FinancialGroupWarehouseMapItem

        #region RecoveryLoginItem

        /// <summary>
        /// Произвоит поиск пункта восстановления пароля по номеру восстановления.
        /// </summary>
        /// <param name="number">Номер восстановления.</param>
        /// <returns>Пункт для восстановления.</returns>
        public RecoveryLoginItem GetRecoveryLoginItem(string number)
        {
            _logger.InfoFormat("Получение пункта восстановления по номеру {0}", number);

            var context = CreateContext();

            return context.RecoveryLoginItems.FirstOrDefault(i => i.SentNumber == number);
        }

        /// <summary>
        ///   Сохраняет информацию о пункте восстановления пароля.
        /// </summary>
        /// <param name="recoveryLoginItem"> Сохраняемый пункт восставноления пароля. </param>
        public void SaveRecoveryLoginItem(RecoveryLoginItem recoveryLoginItem)
        {
            _logger.InfoFormat("Сохранение пункта восстановления пароля с Id = {0}", recoveryLoginItem.RecoveryLoginID);
            var context = CreateContext();

            var savedItem =
                context.RecoveryLoginItems.FirstOrDefault(
                    item =>
                    item.RecoveryLoginID == recoveryLoginItem.RecoveryLoginID);

            if (recoveryLoginItem.RecoveryLoginID == null || recoveryLoginItem.RecoveryLoginID == Guid.Empty)
            {
                recoveryLoginItem.RecoveryLoginID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                recoveryLoginItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.RecoveryLoginItems.AddObject(recoveryLoginItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Пункт посстановления пароля с ID= {0} успешно сохранен",
                               recoveryLoginItem.RecoveryLoginID);
        }

        /// <summary>
        /// Получает пункт восстановления пароля по его ID.
        /// </summary>
        /// <param name="id">Код восстановления пароля.</param>
        /// <returns>Пункт восстановления пароля, если существует.</returns>
        public RecoveryLoginItem GetRecoveryLoginItem(Guid? id)
        {
            _logger.InfoFormat("Получение пункта восстановления пароля по Id = {0}", id);
            var context = CreateContext();
            return context.RecoveryLoginItems.FirstOrDefault(fs => fs.RecoveryLoginID == id);
        }

        /// <summary>
        /// Удаляет из хранилища пункт восстановления пароя по его ID.
        /// </summary>
        /// <param name="id">Код пункта восстановления пароля.</param>
        public void DeleteRecoveryLoginItem(Guid? id)
        {
            _logger.InfoFormat("Удаление пункта восстановления пароля id ={0}", id);

            var context = CreateContext();
            var item = new RecoveryLoginItem { RecoveryLoginID = id };
            context.RecoveryLoginItems.Attach(item);
            context.RecoveryLoginItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Пункт восставления пароля id = {0} успешно удален", id);
        }

        #endregion RecoveryLoginItem

        #region UserPublicKey

        /// <summary>
        /// Получает публичный ключ по номеру для домена.
        /// </summary>
        /// <param name="domainID">Код домена.</param>
        /// <param name="number">Номер ключа.</param>
        /// <returns>Публичный ключ.</returns>
        public UserPublicKey GetPublicKey(Guid? domainID, string number)
        {
            _logger.InfoFormat("Получение публичного ключа для домена {0} по номеру {1}",domainID,number);

            var context = CreateContext();

            return
                context.Users.Join(context.UserPublicKeys, user => user.UserID, key => key.UserID,
                                   (user, key) => new {user, key}).Where(
                                       i => i.user.UserDomainID == domainID && i.key.Number == number).Select(i=>i.key).FirstOrDefault();
        }

        /// <summary>
        /// Получает текущий публичный ключ для пользователя.
        /// </summary>
        /// <param name="userID">Код домена.</param>
        /// <returns>Публичный ключ.</returns>
        public UserPublicKey GetCurrentPublicKey(Guid? userID)
        {
            _logger.InfoFormat("Получение текущего публичного ключа для пользователя {0}", userID);

            var context = CreateContext();

            return
                context.UserPublicKeys.Where(i=>i.UserID==userID).OrderByDescending(i=>i.EventDate).FirstOrDefault();
        }

        /// <summary>
        /// Получает список публичных ключей.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список публичных ключей.</returns>
        public IQueryable<UserPublicKeyDTO> GetUserPublicKeys(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списка публичных ключей без строки поиска {0} ", userDomainID);
            var context = CreateContext();
            return context.UserPublicKeys.Join(context.Users, request => request.UserID, user => user.UserID,
                    (request, user) => new {request, user}).
                Where(
                    i =>
                            i.user.UserDomainID == userDomainID).Select(i => new UserPublicKeyDTO
                            {
                    ClientIdentifier = i.request.ClientIdentifier,
                                         EventDate = i.request.EventDate,
                                         FirstName = i.user.FirstName,
                                         MiddleName = i.user.MiddleName,
                                         LastName = i.user.LastName,
                                         IsRevoked = i.request.IsRevoked,
                                         UserID = i.user.UserID,
                                         KeyNotes = i.request.KeyNotes,
                                         LoginName = i.user.LoginName,
                                         Number = i.request.Number,
                                         PublicKeyData = i.request.PublicKeyData,
                                         UserPublicKeyID = i.request.UserPublicKeyID
                });
        }

        /// <summary>
        /// Получает список публичных ключей с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список публичных ключей.</returns>
        public IEnumerable<UserPublicKeyDTO> GetUserPublicKeys(Guid? userDomainID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка публичных ключей со строкой поиска {0} страница {1} для домена {2}", name, page, userDomainID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.UserPublicKeys.Join(context.Users, request => request.UserID, user => user.UserID,
                                                           (request, user) => new { request, user }).
                Where(
                    i =>
                    i.user.UserDomainID == userDomainID &&
                    (i.request.KeyNotes + i.user.LoginName + i.user.FirstName + i.user.LastName).Contains(name));
            count = items.Count();

            return items.Select(i => new UserPublicKeyDTO
                                     {
                                         ClientIdentifier = i.request.ClientIdentifier,
                                         EventDate = i.request.EventDate,
                                         FirstName = i.user.FirstName,
                                         MiddleName = i.user.MiddleName,
                                         LastName = i.user.LastName,
                                         IsRevoked = i.request.IsRevoked,
                                         UserID = i.user.UserID,
                                         KeyNotes = i.request.KeyNotes,
                                         LoginName = i.user.LoginName,
                                         Number = i.request.Number,
                                         PublicKeyData = i.request.PublicKeyData,
                                         UserPublicKeyID = i.request.UserPublicKeyID
                                     }).OrderBy(i => i.EventDate).Page(page, pageSize);
        }

        /// <summary>
        ///   Сохраняет информацию о публичном ключе пользователя.
        /// </summary>
        /// <param name="userPublicKey"> Сохраняемый ключ пользователя. </param>
        public void SaveUserPublicKey(UserPublicKey userPublicKey)
        {
            _logger.InfoFormat("Сохранение ключа пользователя с Id = {0}", userPublicKey.UserPublicKeyID);
            var context = CreateContext();

            var savedItem =
                context.UserPublicKeys.FirstOrDefault(
                    item =>
                    item.UserPublicKeyID == userPublicKey.UserPublicKeyID);

            if (userPublicKey.UserPublicKeyID == null || userPublicKey.UserPublicKeyID == Guid.Empty)
            {
                userPublicKey.UserPublicKeyID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                userPublicKey.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.UserPublicKeys.AddObject(userPublicKey);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Публичый ключ пользователя с ID= {0} успешно сохранен",
                               userPublicKey.UserPublicKeyID);
        }

        /// <summary>
        /// Получает публичный ключ пользователя по его ID.
        /// </summary>
        /// <param name="id">Код публичного ключа пользователя.</param>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <returns>Публичный ключ пользователя, если существует.</returns>
        public UserPublicKeyDTO GetUserPublicKey(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение публичного ключа пользователя по Id = {0}", id);
            var context = CreateContext();
            return context.UserPublicKeys.Join(context.Users, request => request.UserID, user => user.UserID,
                                                           (request, user) => new { request, user }).Where(i=>i.request.UserPublicKeyID==id && i.user.UserDomainID==userDomainID).
                                                           Select(i => new UserPublicKeyDTO
                                                           {
                                                               ClientIdentifier = i.request.ClientIdentifier,
                                                               EventDate = i.request.EventDate,
                                                               FirstName = i.user.FirstName,
                                                               MiddleName = i.user.MiddleName,
                                                               LastName = i.user.LastName,
                                                               IsRevoked = i.request.IsRevoked,
                                                               UserID = i.user.UserID,
                                                               KeyNotes = i.request.KeyNotes,
                                                               LoginName = i.user.LoginName,
                                                               Number = i.request.Number,
                                                               PublicKeyData = i.request.PublicKeyData,
                                                               UserPublicKeyID = i.request.UserPublicKeyID
                                                           })
                                                           .FirstOrDefault();
        }

        /// <summary>
        /// Удаляет из хранилища публичный ключ пользователя по его ID.
        /// </summary>
        /// <param name="id">Код публичного ключа пользователя.</param>
        public void DeleteUserPublicKey(Guid? id)
        {
            _logger.InfoFormat("Удаление публичного ключа пользователя id ={0}", id);

            var context = CreateContext();
            var item = new UserPublicKey { UserPublicKeyID = id };
            context.UserPublicKeys.Attach(item);
            context.UserPublicKeys.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Публичный ключ пользователя id = {0} успешно удален", id);
        }

        #endregion UserPublicKey

        #region UserPublicKeyRequest

        /// <summary>
        /// Получает запрос на регистрацию ключа по домену и номеру.
        /// </summary>
        /// <param name="domainID">Код домена.</param>
        /// <param name="number">Номер ключа.</param>
        /// <returns>Запрос на регистрацию публичного ключа.</returns>
        public UserPublicKeyRequest GetUserPublicKeyRequest(Guid? domainID, string number)
        {
            _logger.InfoFormat("Получение запроса на регистрацию публичного ключа {0} для домена {1}", number, domainID);

            var context = CreateContext();

            return context.Users.Join(context.UserPublicKeyRequests, user => user.UserID, key => key.UserID,
                                   (user, key) => new { user, key }).Where(
                                       i => i.user.UserDomainID == domainID && i.key.Number == number).Select(i => i.key).FirstOrDefault();
        }

        /// <summary>
        /// Получает список запросов на регистрацию публичных ключей с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <returns>Список запросов на публичные ключи.</returns>
        public IQueryable<UserPublicKeyRequestDTO> GetUserPublicKeyRequests(Guid? userDomainID)
        {
            _logger.InfoFormat(
                "Получение списка запросов на регистрацию публичных ключей для домена {0}",userDomainID);
            var context = CreateContext();
            return  context.UserPublicKeyRequests.Join(context.Users, request => request.UserID, user => user.UserID,
                    (request, user) => new {request, user}).
                Where(i =>i.user.UserDomainID == userDomainID ).Select(i=>new UserPublicKeyRequestDTO
                {
                    ClientIdentifier = i.request.ClientIdentifier,
                    EventDate = i.request.EventDate,
                    FirstName = i.user.FirstName,
                    KeyNotes = i.request.KeyNotes,
                    LastName = i.user.LastName,
                    LoginName = i.user.LoginName,
                    MiddleName = i.user.MiddleName,
                    Number = i.request.Number,
                    PublicKeyData = i.request.PublicKeyData,
                    UserID = i.user.UserID,
                    UserPublicKeyRequestID = i.request.UserPublicKeyRequestID
                });
        }

        /// <summary>
        /// Получает список запросов на регистрацию публичных ключей с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список запросов на публичные ключи.</returns>
        public IEnumerable<UserPublicKeyRequestDTO> GetUserPublicKeyRequests(Guid? userDomainID, string name, int page, int pageSize, out int count)
        {

            _logger.InfoFormat("Получение списка запросов на регистрацию публичных ключей со строкой поиска {0} страница {1} для домена {2}", name, page, userDomainID);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.UserPublicKeyRequests.Join(context.Users, request => request.UserID, user => user.UserID,
                                                           (request, user) => new {request, user}).
                Where(
                    i =>
                    i.user.UserDomainID == userDomainID &&
                    (i.request.KeyNotes + i.user.LoginName + i.user.FirstName + i.user.LastName).Contains(name));
            count = items.Count();

            return items.Select(i => new UserPublicKeyRequestDTO
                                     {
                                         ClientIdentifier = i.request.ClientIdentifier,
                                         EventDate = i.request.EventDate,
                                         FirstName = i.user.FirstName,
                                         KeyNotes = i.request.KeyNotes,
                                         LastName = i.user.LastName,
                                         LoginName = i.user.LoginName,
                                         MiddleName = i.user.MiddleName,
                                         Number = i.request.Number,
                                         PublicKeyData = i.request.PublicKeyData,
                                         UserID = i.user.UserID,
                                         UserPublicKeyRequestID = i.request.UserPublicKeyRequestID
                                     }).OrderBy(i => i.EventDate).Page(page, pageSize);
        }

        /// <summary>
        ///   Сохраняет информацию о запросе публичного ключа пользователя.
        /// </summary>
        /// <param name="userPublicKeyRequest"> Сохраняемый закпрос публичного ключа пользователя. </param>
        public void SaveUserPublicKeyRequest(UserPublicKeyRequest userPublicKeyRequest)
        {
            _logger.InfoFormat("Сохранение запроса публичного ключа пользователя с Id = {0}", userPublicKeyRequest.UserPublicKeyRequestID);
            var context = CreateContext();

            var savedItem =
                context.UserPublicKeyRequests.FirstOrDefault(
                    item =>
                    item.UserPublicKeyRequestID == userPublicKeyRequest.UserPublicKeyRequestID);

            if (userPublicKeyRequest.UserPublicKeyRequestID == null || userPublicKeyRequest.UserPublicKeyRequestID == Guid.Empty)
            {
                userPublicKeyRequest.UserPublicKeyRequestID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                userPublicKeyRequest.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.UserPublicKeyRequests.AddObject(userPublicKeyRequest);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Запрос публичного ключа пользователя с ID= {0} успешно сохранен",
                               userPublicKeyRequest.UserPublicKeyRequestID);
        }

        /// <summary>
        /// Получает запрос публичного ключа пользователя по его ID.
        /// </summary>
        /// <param name="id">Код запроса публичного ключа пользователя.</param>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <returns>Запрос публичного ключа пользователя, если существует.</returns>
        public UserPublicKeyRequestDTO GetUserPublicKeyRequest(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение запроса публичного ключа пользователя по Id = {0}", id);
            var context = CreateContext();
            return context.UserPublicKeyRequests.Join(context.Users, request => request.UserID, user => user.UserID, (request, user) => new {request,user }).Where(i=>i.user.UserDomainID==userDomainID && i.request.UserPublicKeyRequestID==id).
                Select(i => new UserPublicKeyRequestDTO
                {
                    ClientIdentifier = i.request.ClientIdentifier,
                    EventDate = i.request.EventDate,
                    FirstName = i.user.FirstName,
                    KeyNotes = i.request.KeyNotes,
                    LastName = i.user.LastName,
                    LoginName = i.user.LoginName,
                    MiddleName = i.user.MiddleName,
                    Number = i.request.Number,
                    PublicKeyData = i.request.PublicKeyData,
                    UserID = i.user.UserID,
                    UserPublicKeyRequestID = i.request.UserPublicKeyRequestID
                }
                ).FirstOrDefault();
        }

        /// <summary>
        /// Удаляет из хранилища запрос публичного ключа пользователя по его ID.
        /// </summary>
        /// <param name="id">Код запроса публичного ключа пользователя.</param>
        public void DeleteUserPublicKeyRequest(Guid? id)
        {
            _logger.InfoFormat("Удаление запроса публичного ключа пользователя id ={0}", id);

            var context = CreateContext();
            
            var item = new UserPublicKeyRequest { UserPublicKeyRequestID = id };
            context.UserPublicKeyRequests.Attach(item);
            context.UserPublicKeyRequests.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Запрос публичного ключа пользователя id = {0} успешно удален", id);
        }

        #endregion UserPublicKeyRequest

        #region AutocompleteItem

        /// <summary>
        /// Получает список пунктов автозаполнения с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователей. </param>
        /// <param name="autocompleteKindID">Код типа пункта автозаполнения.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список пунктов автозаполнения.</returns>
        public IEnumerable<AutocompleteItem> GetAutocompleteItems(Guid? userDomainID, byte? autocompleteKindID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка пунктов автозаполнения со строкой поиска {0} страница {1}", name, page);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.AutocompleteItems.Where(i => i.Title.Contains(name) && i.UserDomainID == userDomainID);
            if (autocompleteKindID!=null)
            {
                items = items.Where(i => i.AutocompleteKindID == autocompleteKindID);
            } //if
            count = items.Count();

            return items.OrderBy(i => i.Title).Page(page, pageSize);
        }

        /// <summary>
        /// Получает полный список пунктов автозаполнения.
        /// </summary>
        /// <returns>Список пунктов автозаполнения.</returns>
        public IQueryable<AutocompleteItem> GetAutocompleteItems(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение полный список пунктов автозаполнения для домена {0}", userDomainID);
            var context = CreateContext();
            return context.AutocompleteItems.Where(k => k.UserDomainID == userDomainID);
        }

        /// <summary>
        /// Получает полный список пунктов автозаполнения.
        /// </summary>
        /// <param name="userDomainID">Домен пользователя.</param>
        /// <param name="autocompleteKindID">Код типа пункта автозаполнения.</param>
        /// <returns>Список пунктов автозаполнения.</returns>
        public IEnumerable<AutocompleteItem> GetAutocompleteItems(Guid? userDomainID, byte? autocompleteKindID)
        {
            _logger.InfoFormat("Получение полный список пунктов автозаполнения для домена {0} и типа {1}", userDomainID,autocompleteKindID);
            var context = CreateContext();
            return context.AutocompleteItems.Where(k => k.UserDomainID == userDomainID&& k.AutocompleteKindID==autocompleteKindID);
        }

        /// <summary>
        ///   Сохраняет информацию о пункте автозаполнения.
        /// </summary>
        /// <param name="autocompleteItem"> Сохраняемый пункт автозаполнения. </param>
        public void SaveAutocompleteItem(AutocompleteItem autocompleteItem)
        {
            _logger.InfoFormat("Сохранение пункта автозаполнения с Id = {0}", autocompleteItem.AutocompleteItemID);
            var context = CreateContext();

            var savedItem =
                context.AutocompleteItems.FirstOrDefault(
                    di => di.AutocompleteItemID == autocompleteItem.AutocompleteItemID && di.UserDomainID == autocompleteItem.UserDomainID);

            if (autocompleteItem.AutocompleteItemID == null)
            {
                autocompleteItem.AutocompleteItemID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                autocompleteItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.AutocompleteItems.AddObject(autocompleteItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Сохранение пункта автозаполнения с ID= {0} успешно сохранено",
                               autocompleteItem.AutocompleteItemID);
        }

        /// <summary>
        /// Получает пункт автозаполнения по его ID.
        /// </summary>
        /// <param name="id">Код пункта автозаполнения.</param>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <returns>Пункт автозаполнения, если существует.</returns>
        public AutocompleteItem GetAutocompleteItem(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение пункта автозаполнения по Id = {0} для домена {1}", id, userDomainID);
            var context = CreateContext();
            return context.AutocompleteItems.FirstOrDefault(fs => fs.AutocompleteItemID == id && fs.UserDomainID == userDomainID);
        }

        /// <summary>
        /// Удаляет из хранилища пункт автозаполнения по его ID.
        /// </summary>
        /// <param name="id">Код пункта автозаполнения.</param>
        public void DeleteAutocompleteItem(Guid? id)
        {
            _logger.InfoFormat("Удаление пункта автозаполнения по id ={0}", id);

            var context = CreateContext();
            var item = new AutocompleteItem { AutocompleteItemID = id };
            context.AutocompleteItems.Attach(item);
            context.AutocompleteItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Пункт автозаполнения с id = {0} успешно удален", id);
        }

        #endregion AutocompleteItem

        #region UserInterest

        /// <summary>
        /// Получает список пунктов вознаграждения.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователей. </param>
        /// <returns>Список пунктов вознаграждения.</returns>
        public IQueryable<UserInterest> GetUserInterests(Guid? userDomainID)
        {
            _logger.InfoFormat("Получение списка пунктов вознаграждения без строки поиска по домену {0}",userDomainID);

            var context = CreateContext();

            return
                context.UserInterests.Join(context.Users, interest => interest.UserID, user => user.UserID,
                        (interest, user) => new {interest, user})
                    .Where(i => i.user.UserDomainID == userDomainID)
                    .Select(i => i.interest);
        }

        /// <summary>
        /// Получает список пунктов вознаграждения с фильтром.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователей. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список пунктов вознаграждения.</returns>
        public IEnumerable<UserInterestDTO> GetUserInterests(Guid? userDomainID, string name, int page, int pageSize, out int count)
        {
            _logger.InfoFormat("Получение списка пунктов вознаграждения со строкой поиска {0} страница {1}", name, page);
            NormilizeString(ref name);
            var context = CreateContext();
            var items = context.UserInterests.Join(context.Users, interest => interest.UserID, user => user.UserID, (interest, user) => new { interest, user }).
                Where(i => i.user.UserDomainID == userDomainID && (i.user.LastName + i.user.FirstName).Contains(name)).
                Select(i => new UserInterestDTO
                            {
                                Description = i.interest.Description,
                                DeviceInterestKindID = i.interest.DeviceInterestKindID,
                                DeviceValue = i.interest.DeviceValue,
                                EventDate = i.interest.EventDate,
                                FirstName = i.user.FirstName,
                                LastName = i.user.LastName,
                                MiddleName = i.user.MiddleName,
                                UserID = i.user.UserID,
                                UserInterestID = i.interest.UserInterestID,
                                WorkInterestKindID = i.interest.WorkInterestKindID,
                                WorkValue = i.interest.WorkValue
                            });

           
            count = items.Count();

            return items.OrderBy(i => i.FirstName).Page(page, pageSize);
        }
       

        /// <summary>
        ///   Сохраняет информацию о пункте вознаграждения.
        /// </summary>
        /// <param name="userInterest"> Сохраняемый пункт вознаграждения. </param>
        public void SaveUserInterest(UserInterest userInterest)
        {
            _logger.InfoFormat("Сохранение пункта вознаграждения с Id = {0}", userInterest.UserInterestID);
            var context = CreateContext();

            var savedItem =
                context.UserInterests.FirstOrDefault(i => userInterest.UserInterestID == i.UserInterestID);

            if (userInterest.UserInterestID == null)
            {
                userInterest.UserInterestID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                if (userInterest.UserID!=savedItem.UserID)
                {
                    var userDomainId = context.Users.Where(i => i.UserID == userInterest.UserID).Select(i=>i.UserDomainID).FirstOrDefault();
                    var savedUserDomainId = context.Users.Where(i => i.UserID == savedItem.UserID).Select(i => i.UserDomainID).FirstOrDefault();

                    if (userDomainId!=savedUserDomainId)
                    {
                        return;
                    } //if

                } //if

                userInterest.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.UserInterests.AddObject(userInterest);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Сохранение пункта вознаграждения с ID= {0} успешно сохранено",
                               userInterest.UserInterestID);
        }

        /// <summary>
        /// Получает пункт вознаграждения по его ID.
        /// </summary>
        /// <param name="id">Код пункта вознаграждения.</param>
        /// <param name="userDomainID">Код домена пользователя. </param>
        /// <returns>Пункт вознаграждения, если существует.</returns>
        public UserInterestDTO GetUserInterest(Guid? id, Guid? userDomainID)
        {
            _logger.InfoFormat("Получение пункта вознаграждения по Id = {0} для домена {1}", id, userDomainID);
            var context = CreateContext();
            return
                context.UserInterests.Join(context.Users.Where(u => u.UserDomainID == userDomainID),
                                           interest => interest.UserID, u => u.UserID, (interest, user) => new UserInterestDTO
                                                                                                           {
                                                                                                               Description = interest.Description,
                                                                                                               DeviceInterestKindID = interest.DeviceInterestKindID,
                                                                                                               DeviceValue = interest.DeviceValue,
                                                                                                               EventDate = interest.EventDate,
                                                                                                               FirstName = user.FirstName,
                                                                                                               LastName = user.LastName,
                                                                                                               MiddleName = user.MiddleName,
                                                                                                               UserID = user.UserID,
                                                                                                               UserInterestID = interest.UserInterestID,
                                                                                                               WorkInterestKindID = interest.WorkInterestKindID,
                                                                                                               WorkValue = interest.WorkValue
                                                                                                           }).
                    FirstOrDefault(fs => fs.UserInterestID == id);
        }

        /// <summary>
        /// Удаляет из хранилища пункт вознаграждения по его ID.
        /// </summary>
        /// <param name="id">Код пункта вознаграждения.</param>
        public void DeleteUserInterest(Guid? id)
        {
            _logger.InfoFormat("Удаление пункта вознаграждения по id ={0}", id);

            var context = CreateContext();
            var item = new UserInterest { UserInterestID = id };
            context.UserInterests.Attach(item);
            context.UserInterests.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Пункт вознаграждения с id = {0} успешно удален", id);
        }

        /// <summary>
        /// Удаляет из хранилища пункт вознаграждения по его ID.
        /// </summary>
        /// <param name="id">Код пункта вознаграждения.</param>
        /// <param name="userDomainID">Код домена пользователя. </param>
        public void DeleteUserInterest(Guid? id,Guid? userDomainID)
        {
            _logger.InfoFormat("Удаление пункта вознаграждения по id ={0}", id);

            var context = CreateContext();
            if (context.UserInterests.Join(context.Users, interest => interest.UserID, user => user.UserID, (interest, user) => new { interest,user}).Any(i=>i.interest.UserInterestID==id && i.user.UserDomainID == userDomainID))
            {
                var item = new UserInterest { UserInterestID = id };
                context.UserInterests.Attach(item);
                context.UserInterests.DeleteObject(item);
                context.SaveChanges();

            } //if
            
            _logger.InfoFormat("Пункт вознаграждения с id = {0} успешно удален", id);
        }

        #endregion UserInterest

        #region UserInterest Report

        private const string UserInterestReportSql = @"

select
u.FirstName,
u.LastName,
u.MiddleName,
WorkValue,
DeviceValue,
(CASE WHEN uui.DeviceInterestKindID = 0 THEN 0
ELSE
 isnull((select sum(di.Price * di.[Count]) from DeviceItem di where di.UserID = uui.UserID and di.EventDate between  {1} and {2}) * (uui.DeviceValue*0.01),0)
END) AS DeviceInterest,

(CASE WHEN uui.WorkInterestKindID = 0 THEN 0
ELSE
 isnull((select sum(isnull(wi.Price,0)) from WorkItem wi where wi.UserID = uui.UserID and wi.EventDate between  {1} and {2}) * (uui.WorkValue*0.01),0)
END) AS WorkInterest


from
(

select
(select TOP 1 ui.UserInterestID from UserInterest ui where ui.EventDate = t.eventdate AND ui.UserID = t.userid) as UserInterestID
from

(
		select  ui.userid as userid,
		max(ui.EventDate) as eventdate		
	from 
UserInterest ui
INNER JOIN [User] u ON ui.UserID = u.UserID and u.UserDomainID = {0}
group by ui.UserID
) t
) tt
inner join UserInterest uui ON tt.UserInterestID = uui.UserInterestID
INNER JOIN [User] u ON uui.UserID = u.UserID


";

        /// <summary>
        /// Получает отчет по вознаграждениям пользователей за определенный период.
        /// </summary>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        /// <returns>Пункты отчета.</returns>
        public IEnumerable<InterestReportItem> GetUserInterestReportItems(Guid? userDomainID, DateTime beginDate, DateTime endDate)
        {
            _logger.InfoFormat("Получение отчета по вознаграждениям пользователей для домена {0} с {1} по {2}", userDomainID, beginDate, endDate);

            var context = CreateContext();
            return context.ExecuteStoreQuery<InterestReportItem>(UserInterestReportSql, userDomainID, beginDate, endDate);

        }

        #endregion UserInterest Report

        #region UserGridFilter

        /// <summary>
        ///   Сохраняет информацию фильтр гриде.
        /// </summary>
        /// <param name="userGridFilter"> Сохраняемый фильтр грида. </param>
        public void SaveUserGridFilter(UserGridFilter userGridFilter)
        {
            _logger.InfoFormat("Сохранение фильтр грида с Id = {0}", userGridFilter.UserGridFilterID);
            var context = CreateContext();

            var savedItem =
                context.UserGridFilters.FirstOrDefault(
                    di => di.UserGridFilterID == userGridFilter.UserGridFilterID);

            if (userGridFilter.UserGridFilterID == null || userGridFilter.UserGridFilterID == Guid.Empty)
            {
                userGridFilter.UserGridFilterID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                userGridFilter.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.UserGridFilters.AddObject(userGridFilter);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("фильтр грида с ID= {0} успешно сохранен",
                               userGridFilter.UserGridFilterID);
        }

        /// <summary>
        /// Получает фильтр грида по его ID.
        /// </summary>
        /// <param name="id">Код описания фильтр гридаа.</param>
        /// <returns>фильтр грида, если существует.</returns>
        public UserGridFilter GetUserGridFilter(Guid? id)
        {
            _logger.InfoFormat("Получение фильтр гридаа по Id = {0}", id);
            var context = CreateContext();
            return context.UserGridFilters.FirstOrDefault(fs => fs.UserGridFilterID == id );
        }

        /// <summary>
        /// Получает пользовательский фильтр грида по его ID.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <param name="id">Код описания фильтр грида.</param>
        /// <returns>Фильтр грида, если существует.</returns>
        public UserGridFilter GetUserGridFilter(Guid? userID, Guid? id)
        {
            _logger.InfoFormat("Получение фильтр грида по Id = {0} для пользователя {1}", id,userID);
            var context = CreateContext();
            return context.UserGridFilters.FirstOrDefault(fs => fs.UserGridFilterID == id && fs.UserID == userID);
        }

        /// <summary>
        /// Получает список пользовательских фильтров для определенного грида.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <param name="gridName">Название грида.</param>
        /// <returns>Фильтры грида.</returns>
        public IEnumerable<UserGridFilter> GetUserGridFilters(Guid? userID, string gridName)
        {
            _logger.InfoFormat("Получение фильтров грида  {0} для пользователя {1}", gridName, userID);
            var context = CreateContext();
            return context.UserGridFilters.Where(fs => fs.GridName == gridName && fs.UserID == userID);
        }

        /// <summary>
        /// Удаляет из хранилища фильтр грида по его ID.
        /// </summary>
        /// <param name="id">Код фильтр грида.</param>
        public void DeleteUserGridFilter(Guid? id)
        {
            _logger.InfoFormat("Удаление фильтр гридаа id ={0}", id);

            var context = CreateContext();
            var item = new UserGridFilter { UserGridFilterID = id };
            context.UserGridFilters.Attach(item);
            context.UserGridFilters.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("фильтр грида id = {0} успешно удален", id);
        }

        #endregion UserGridFilter

        #region UserGridState

        private const string SaveUserGridStateSql = @"

MERGE dbo.UserGridState as tgt
USING(values({0},{1},{2})) as src(GridName,StateGrid,UserID)
ON tgt.GridName = src.GridName and tgt.UserID = src.UserID
WHEN MATCHED THEN
UPDATE
SET StateGrid = src.StateGrid
WHEN NOT MATCHED THEN
INSERT ( UserGridStateID, GridName, StateGrid, UserID )
VALUES (NewID(),  src.GridName, src.StateGrid, src.UserID );
";

        /// <summary>
        ///   Сохраняет информацию состояние гриде.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <param name="gridName">Имя грида.</param>
        /// <param name="state">Состояние грида.</param>
        public void SaveUserGridState(Guid? userID,string gridName, string state)
        {
            var context = CreateContext();
            context.ExecuteStoreCommand(SaveUserGridStateSql, gridName, state, userID);
        }

        /// <summary>
        /// Получает информацию по состоянию грида.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="gridName"></param>
        /// <returns></returns>
        public string GetGridUserState(Guid? userID, string gridName)
        {
            var context = CreateContext();

            return
                context.ExecuteStoreQuery<string>(
                    "SELECT StateGrid FROM dbo.UserGridState WHERE UserID = {0} AND GridName= {1}", userID, gridName).FirstOrDefault();
        }

        /// <summary>
        ///   Сохраняет информацию состояние гриде.
        /// </summary>
        /// <param name="userGridState"> Сохраняемый состояние грида. </param>
        public void SaveUserGridState(UserGridState userGridState)
        {
            _logger.InfoFormat("Сохранение состояние грида с Id = {0}", userGridState.UserGridStateID);
            var context = CreateContext();

            var savedItem =
                context.UserGridStates.FirstOrDefault(
                    di => di.UserGridStateID == userGridState.UserGridStateID);

            if (userGridState.UserGridStateID == null || userGridState.UserGridStateID == Guid.Empty)
            {
                userGridState.UserGridStateID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                userGridState.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.UserGridStates.AddObject(userGridState);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("состояние грида с ID= {0} успешно сохранено",
                               userGridState.UserGridStateID);
        }

        /// <summary>
        /// Получает состояние грида по его ID.
        /// </summary>
        /// <param name="id">Код описания состояние гридаа.</param>
        /// <returns>состояние грида, если существует.</returns>
        public UserGridState GetUserGridState(Guid? id)
        {
            _logger.InfoFormat("Получение состояние гридаа по Id = {0}", id);
            var context = CreateContext();
            return context.UserGridStates.FirstOrDefault(fs => fs.UserGridStateID == id);
        }

        /// <summary>
        /// Удаляет из хранилища состояние грида по его ID.
        /// </summary>
        /// <param name="id">Код состояние грида.</param>
        public void DeleteUserGridState(Guid? id)
        {
            _logger.InfoFormat("Удаление состояние грида id ={0}", id);

            var context = CreateContext();
            var item = new UserGridState { UserGridStateID = id };
            context.UserGridStates.Attach(item);
            context.UserGridStates.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("состояние грида id = {0} успешно удален", id);
        }

        #endregion UserGridState

        #region UserSettingsItem

        private const string SaveUserSettingsItemSql = @"

MERGE dbo.UserSettings as tgt
USING(values({0},{1},{2})) as src(Number,Data,UserLogin)
ON tgt.Number = src.Number and tgt.UserLogin = src.UserLogin
WHEN MATCHED THEN
UPDATE
SET Data = src.Data
WHEN NOT MATCHED THEN
INSERT ( UserSettingsID, Number, Data, UserLogin )
VALUES (NewID(),  src.Number, src.Data, src.UserLogin );
";

        /// <summary>
        ///   Сохраняет информацию о настройке пользователя.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="number">Номер настройки.</param>
        /// <param name="data">Значение настройки.</param>
        public void SaveUserSettingsItem(string login, string number, string data)
        {
            if (data == null)
            {
                return;
            }
            var context = CreateContext();
            context.ExecuteStoreCommand(SaveUserSettingsItemSql, number, data, login);
        }

        /// <summary>
        /// Получает значение настройки пользователя.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="number">Номер настройки.</param>
        /// <returns></returns>
        public string GetUserSettingsItem(string login, string number)
        {
            var context = CreateContext();
            return context.UserSettingsItems.Where(i => i.Number == number && i.UserLogin == login).Select(i=>i.Data).FirstOrDefault();
        }

        /// <summary>
        ///   Сохраняет информацию настройках пользователя.
        /// </summary>
        /// <param name="userSettingsItem"> Сохраняемая настройка пользователя. </param>
        public void SaveUserSettingsItem(UserSettingsItem userSettingsItem)
        {
            _logger.InfoFormat("Сохранение настройек пользователяя с Id = {0}", userSettingsItem.UserSettingsID);
            var context = CreateContext();

            var savedItem =
                context.UserSettingsItems.FirstOrDefault(
                    di => di.UserSettingsID == userSettingsItem.UserSettingsID);

            if (userSettingsItem.UserSettingsID == null || userSettingsItem.UserSettingsID == Guid.Empty)
            {
                userSettingsItem.UserSettingsID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                userSettingsItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.UserSettingsItems.AddObject(userSettingsItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Настройка пользователяя с ID= {0} успешно сохранена",
                               userSettingsItem.UserSettingsID);
        }

        /// <summary>
        /// Получает настройках пользователяа по его ID.
        /// </summary>
        /// <param name="id">Код описания настройках пользователяаа.</param>
        /// <returns>настройках пользователяа, если существует.</returns>
        public UserSettingsItem GetUserSettingsItem(Guid? id)
        {
            _logger.InfoFormat("Получение настройках пользователяа по Id = {0}", id);
            var context = CreateContext();
            return context.UserSettingsItems.FirstOrDefault(fs => fs.UserSettingsID == id);
        }

        /// <summary>
        /// Удаляет из хранилища настройках пользователяа по его ID.
        /// </summary>
        /// <param name="id">Код настройках пользователяа.</param>
        public void DeleteUserSettingsItem(Guid? id)
        {
            _logger.InfoFormat("Удаление настройках пользователя id ={0}", id);

            var context = CreateContext();
            var item = new UserSettingsItem { UserSettingsID = id };
            context.UserSettingsItems.Attach(item);
            context.UserSettingsItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Настройка пользователя id = {0} успешно удалена", id);
        }

        #endregion UserSettingsItem

        #region UserDomainSettingsItem

        /// <summary>
        ///   Сохраняет информацию настройках домена.
        /// </summary>
        /// <param name="userSettingsItem"> Сохраняемая настройка домена. </param>
        public void SaveUserDomainSettingsItem(UserDomainSettingsItem userSettingsItem)
        {
            _logger.InfoFormat("Сохранение настройек доменая с Id = {0}", userSettingsItem.UserDomainSettingsID);
            var context = CreateContext();

            var savedItem =
                context.UserDomainSettingsItems.FirstOrDefault(
                    di => di.UserDomainSettingsID == userSettingsItem.UserDomainSettingsID);

            if (userSettingsItem.UserDomainSettingsID == null || userSettingsItem.UserDomainSettingsID == Guid.Empty)
            {
                userSettingsItem.UserDomainSettingsID = Guid.NewGuid();
            } //if

            if (savedItem != null)
            {
                userSettingsItem.CopyTo(savedItem);
                context.SaveChanges();
            }
            else
            {
                context.UserDomainSettingsItems.AddObject(userSettingsItem);
                context.SaveChanges(SaveOptions.None);
            } //else

            _logger.InfoFormat("Настройка домена с ID= {0} успешно сохранена",
                               userSettingsItem.UserDomainSettingsID);
        }

        /// <summary>
        /// Получает настройках доменаа по его ID.
        /// </summary>
        /// <param name="id">Код описания настройках доменааа.</param>
        /// <returns>настройках доменаа, если существует.</returns>
        public UserDomainSettingsItem GetUserDomainSettingsItem(Guid? id)
        {
            _logger.InfoFormat("Получение настройках домена по Id = {0}", id);
            var context = CreateContext();
            return context.UserDomainSettingsItems.FirstOrDefault(fs => fs.UserDomainSettingsID == id);
        }

        /// <summary>
        /// Удаляет из хранилища настройках доменаа по его ID.
        /// </summary>
        /// <param name="id">Код настройках доменаа.</param>
        public void DeleteUserDomainSettingsItem(Guid? id)
        {
            _logger.InfoFormat("Удаление настройках домена id ={0}", id);

            var context = CreateContext();
            var item = new UserDomainSettingsItem { UserDomainSettingsID = id };
            context.UserDomainSettingsItems.Attach(item);
            context.UserDomainSettingsItems.DeleteObject(item);
            context.SaveChanges();

            _logger.InfoFormat("Настройка доменаа id = {0} успешно удалена", id);
        }

        #endregion UserDomainSettingsItem
    }
}
