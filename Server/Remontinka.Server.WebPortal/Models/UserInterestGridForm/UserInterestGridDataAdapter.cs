using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Helpers;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.UserInterestGridForm
{
    /// <summary>
    /// Сервис данных для вознаграждений пользователям.
    /// </summary>
    public class UserInterestGridDataAdapter : DataAdapterBase<Guid, UserInterestGridModel, UserInterestCreateModel, UserInterestCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override UserInterestGridModel CreateGridModel(SecurityToken token)
        {
            var list = new List<SelectListItem<Guid>>();
            UserHelper.PopulateUserList(list, null, token, null);
            return new UserInterestGridModel
            {
                Users = list
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
            return RemontinkaServer.Instance.EntitiesFacade.GetUserInterests(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override UserInterestCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new UserInterestCreateModel
            {
                EventDate = DateTime.Today,
                DeviceInterestKindID = InterestKindSet.Empty.InterestKindID,
                WorkInterestKindID = InterestKindSet.Empty.InterestKindID
            };
        }

        /// <summary>
        /// Инициализирует модель Обновления сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код связанной сущности.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель обновления.</returns>
        public override UserInterestCreateModel CreateEditModel(SecurityToken token, Guid? entityId, UserInterestGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetUserInterest(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Вознаграждение");

            return new UserInterestCreateModel
            {
                Description = item.Description,
                DeviceInterestKindID = item.DeviceInterestKindID,
                DeviceValue = item.DeviceValue,
                EventDate = item.EventDate,
                UserInterestID = item.UserInterestID,
                UserID = item.UserID,
                WorkInterestKindID = item.WorkInterestKindID,
                WorkValue = item.WorkValue
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, UserInterestCreateModel model, GridSaveModelResult result)
        {
            var entity = new UserInterest
            {
                Description = model.Description,
                DeviceInterestKindID = model.DeviceInterestKindID,
                DeviceValue = model.DeviceValue,
                EventDate = model.EventDate,
                UserID = model.UserID,
                UserInterestID = model.UserInterestID,
                WorkInterestKindID = model.WorkInterestKindID,
                WorkValue = model.WorkValue
            };

            RemontinkaServer.Instance.EntitiesFacade.SaveUserInterest(token, entity); ;
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, UserInterestCreateModel model, GridSaveModelResult result)
        {
            SaveCreateModel(token, model, result);
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid? entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteUserInterest(token, entityId);
        }
    }
}