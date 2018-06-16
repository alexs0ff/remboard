using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.User
{
    /// <summary>
    /// Адаптер для управления пользователями
    /// </summary>
    public class UserDataAdapter : JGridDataAdapterBase<Guid, UserGridItemModel, UserCreateModel, UserEditModel, UserSearchModel>
    {
        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override UserCreateModel CreateNewModel(SecurityToken token, UserSearchModel searchModel)
        {
            var password = RemontinkaServer.Instance.CryptoService.GeneratePassword(5, 8);
            return new UserCreateModel {Password = password, PasswordCopy = password, BranchIds = new Guid?[0]};
        }

        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override UserEditModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var user = RemontinkaServer.Instance.EntitiesFacade.GetUser(token,entityId);
            RiseExceptionIfNotFound(user, entityId, "Пользователь");
            var model= new UserEditModel
                       {
                           Email = user.Email,
                           FirstName = user.FirstName,
                           Id = user.UserID,
                           LastName = user.LastName,
                           MiddleName = user.MiddleName,
                           Phone = user.Phone,
                           ProjectRoleID = user.ProjectRoleID
                       };
            model.BranchIds =
                RemontinkaServer.Instance.DataStore.GetUserBranchMapByItemsByUser(entityId).Select(i => i.BranchID).ToArray();
            return model;
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<UserGridItemModel> GetPageableGridItems(SecurityToken token, UserSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return
                RemontinkaServer.Instance.EntitiesFacade.GetUsers(token, searchModel.Name, searchModel.Page,
                                                                  itemsPerPage,
                                                                  out totalCount).Select(CreateItemModel);
        }

        /// <summary>
        /// Создает модель строки грида по конкретной сущности.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <returns>Созданная модель.</returns>
        private UserGridItemModel CreateItemModel(Romontinka.Server.DataLayer.Entities.User entity)
        {
            return new UserGridItemModel
                       {
                           Contacts = string.Format("{0} {1}", entity.Phone, entity.Email),
                           FullName = string.Format("{0} {1} {2}", entity.LastName, entity.FirstName, entity.MiddleName),
                           Id = entity.UserID,
                           Login = entity.LoginName,
                           ProjectRoleTitle = ProjectRoleSet.GetKindByID(entity.ProjectRoleID).Title,
                       };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override UserGridItemModel SaveCreateModel(SecurityToken token, UserCreateModel model, JGridSaveModelResult result)
        {
            var currentUser = RemontinkaServer.Instance.DataStore.GetUser(model.Login);
            if (currentUser!=null)
            {
                result.ModelErrors.Add(new PairItem<string, string>(string.Empty,string.Format("Логин {0} существует уже в системе, введите другой",model.Login)));
                result.ModelErrors.Add(new PairItem<string, string>("Login", "Необходимо ввести новое значение"));
                return null;
            }

            var entity = new Romontinka.Server.DataLayer.Entities.User();
            entity.PasswordHash = model.Password;
            entity.LastName = model.LastName;
            entity.MiddleName = model.MiddleName;
            entity.Phone = model.Phone;
            entity.Email = model.Email;
            entity.FirstName = model.FirstName;
            entity.LoginName = model.Login;
            entity.ProjectRoleID = model.ProjectRoleID;
            RemontinkaServer.Instance.SecurityService.CreateUser(token, entity);

            foreach (var branchId in model.BranchIds)
            {
                RemontinkaServer.Instance.DataStore.SaveUserBranchMapItem(new UserBranchMapItem
                                                                              {
                                                                                  EventDate = DateTime.Now,
                                                                                  BranchID = branchId,
                                                                                  UserID = entity.UserID
                                                                              });
            }

            return CreateItemModel(entity);

        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override UserGridItemModel SaveEditModel(SecurityToken token, UserEditModel model, JGridSaveModelResult result)
        {
            var savedEntity = RemontinkaServer.Instance.EntitiesFacade.GetUser(token,model.Id);
            RiseExceptionIfNotFound(savedEntity,model.Id,"Пользователь");
            
            var domain = RemontinkaServer.Instance.DataStore.GetUserDomain(savedEntity.UserDomainID);
            if (StringComparer.OrdinalIgnoreCase.Equals(domain.UserLogin,savedEntity.LoginName) && savedEntity.ProjectRoleID!=model.ProjectRoleID)
            {
                result.ModelErrors.Add(new PairItem<string, string>(string.Empty,"Смена роли главному пользователю запрещена"));
                return null;
            }

            savedEntity.LastName = model.LastName;
            savedEntity.MiddleName = model.MiddleName;
            savedEntity.FirstName = model.FirstName;
            savedEntity.ProjectRoleID = model.ProjectRoleID;
            savedEntity.Email = model.Email;
            savedEntity.Phone = model.Phone;

            RemontinkaServer.Instance.EntitiesFacade.SaveUser(token,savedEntity);
            RemontinkaServer.Instance.DataStore.DeleteUserBranchMapItems(savedEntity.UserID);

            foreach (var branchId in model.BranchIds)
            {
                RemontinkaServer.Instance.DataStore.SaveUserBranchMapItem(new UserBranchMapItem
                {
                    EventDate = DateTime.Now,
                    BranchID = branchId,
                    UserID = savedEntity.UserID
                });
            }

            return CreateItemModel(savedEntity);
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            if (entityId==token.User.UserID)
            {
                return;
            }
            RemontinkaServer.Instance.DataStore.DeleteUserBranchMapItems(entityId);

            RemontinkaServer.Instance.EntitiesFacade.DeleteUser(token,entityId);
        }
    }
}