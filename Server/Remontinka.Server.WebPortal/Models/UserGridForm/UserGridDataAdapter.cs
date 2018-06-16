using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Remontinka.Server.WebPortal.Models.Common;
using Remontinka.Server.WebPortal.Services;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.UserGridForm
{
    /// <summary>
    /// Сервис данных для пользователей.
    /// </summary>
    public class UserGridDataAdapter : DataAdapterBase<Guid, UserGridModel, UserCreateModel, UserEditModel>
    {
        /// <summary>
        /// Переопределяется для обработки несвязанных полей для формы редактирования сущности.
        /// </summary>
        /// <param name="editModel">Модель редактирования.</param>
        /// <param name="parentId">Код родительской сущности.</param>
        /// <param name="modelState">Состояние модели.</param>
        public override void ProcessEditUnboundItems(UserEditModel editModel, string parentId, ModelStateDictionary modelState)
        {
            editModel.BranchIds = CheckBranchIds(modelState);
        }

        /// <summary>
        /// Проверяет филиалы для пользователя.
        /// </summary>
        /// <param name="modelState">Состояние модели.</param>
        /// <returns></returns>
        private static Guid?[] CheckBranchIds(ModelStateDictionary modelState)
        {
            Guid?[] ids = CheckBoxListExtension.GetSelectedValues<Guid?>(UserEditModel.BranchIdsCheckListPropertyName);

            if (!ids.Any())
            {
                modelState.AddModelError(UserEditModel.BranchIdsCheckListPropertyName, "Необходим хотя бы один филиал");
            }
            return ids;
        }

        /// <summary>
        /// Переопределяется для обработки несвязанных полей для формы создания сущности.
        /// </summary>
        /// <param name="createModel">Модель создания.</param>
        /// <param name="parentId">Код родительской сущности.</param>
        /// <param name="modelState">Состояние модели.</param>
        public override void ProcessCreateUnboundItems(UserCreateModel createModel, string parentId, ModelStateDictionary modelState)
        {
            createModel.BranchIds = CheckBranchIds(modelState);
        }

        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override UserGridModel CreateGridModel(SecurityToken token)
        {
            return new UserGridModel
            {
                Branches = RemontinkaServer.Instance.EntitiesFacade.GetBranches(token)
            };
        }

        /// <summary>
        /// Получает данные для грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parentId">Код родительской записи.</param>
        /// <returns>Данные.</returns>
        public override IQueryable GedData(SecurityToken token, string parentId)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetUsers(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override UserCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            //var password = RemontinkaServer.Instance.CryptoService.GeneratePassword(5, 8);
            return new UserCreateModel {  };
        }

        /// <summary>
        /// Инициализирует модель Обновления сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код связанной сущности.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель обновления.</returns>
        public override UserEditModel CreateEditModel(SecurityToken token, Guid? entityId, UserGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var user = RemontinkaServer.Instance.EntitiesFacade.GetUser(token, entityId);
            RiseExceptionIfNotFound(user, entityId, "Пользователь");
            var model = new UserEditModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                UserID = user.UserID,
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
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, UserCreateModel model, GridSaveModelResult result)
        {
            var currentUser = RemontinkaServer.Instance.DataStore.GetUser(model.UserLoginName);
            if (currentUser != null)
            {
                result.ModelErrors.Add(new PairItem<string, string>(string.Empty, string.Format("Логин {0} существует уже в системе, введите другой", model.UserLoginName)));
                result.ModelErrors.Add(new PairItem<string, string>("Login", "Необходимо ввести новое значение"));
                return;
            }

            var entity = new Romontinka.Server.DataLayer.Entities.User();
            entity.PasswordHash = model.LoginPassword;
            entity.LastName = model.LastName;
            entity.MiddleName = model.MiddleName;
            entity.Phone = model.Phone;
            entity.Email = model.Email;
            entity.FirstName = model.FirstName;
            entity.LoginName = model.UserLoginName;
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

            RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().CleanUserLists(token);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, UserEditModel model, GridSaveModelResult result)
        {
            var savedEntity = RemontinkaServer.Instance.EntitiesFacade.GetUser(token, model.UserID);
            RiseExceptionIfNotFound(savedEntity, model.UserID, "Пользователь");

            var domain = RemontinkaServer.Instance.DataStore.GetUserDomain(savedEntity.UserDomainID);
            if (StringComparer.OrdinalIgnoreCase.Equals(domain.UserLogin, savedEntity.LoginName) && savedEntity.ProjectRoleID != model.ProjectRoleID)
            {
                result.ModelErrors.Add(new PairItem<string, string>(string.Empty, "Смена роли главному пользователю запрещена"));
                return;
            }

            savedEntity.LastName = model.LastName;
            savedEntity.MiddleName = model.MiddleName;
            savedEntity.FirstName = model.FirstName;
            savedEntity.ProjectRoleID = model.ProjectRoleID;
            savedEntity.Email = model.Email;
            savedEntity.Phone = model.Phone;

            RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().CleanUserLists(token);
            RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().CleanTokenForUser(savedEntity.LoginName);

            RemontinkaServer.Instance.EntitiesFacade.SaveUser(token, savedEntity);
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
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid? entityId)
        {
            if (entityId == token.User.UserID)
            {
                return;
            }
            RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().CleanUserLists(token);

            RemontinkaServer.Instance.EntitiesFacade.DeleteUser(token, entityId);
        }
    }
}