using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Helpers;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Models.UserPublicKeyRequestGridForm
{
    /// <summary>
    /// Сервис данных для управления запросами на активацию.
    /// </summary>
    public class UserPublicKeyRequestGridDataAdapter : DataAdapterBase<Guid, UserPublicKeyRequestGridModel, UserPublicKeyRequestEditModel, UserPublicKeyRequestEditModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override UserPublicKeyRequestGridModel CreateGridModel(SecurityToken token)
        {
            return new UserPublicKeyRequestGridModel();
        }

        /// <summary>
        /// Получает данные для грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parentId">Код родительской записи.</param>
        /// <returns>Данные.</returns>
        public override IQueryable GedData(SecurityToken token, string parentId)
        {

            return RemontinkaServer.Instance.EntitiesFacade.GetUserPublicKeyRequests(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override UserPublicKeyRequestEditModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            throw new NotImplementedException();
        }
       
        /// <summary>
        /// Инициализирует модель Обновления сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код связанной сущности.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель обновления.</returns>
        public override UserPublicKeyRequestEditModel CreateEditModel(SecurityToken token, Guid? entityId,
            UserPublicKeyRequestGridModel gridModel, GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetUserPublicKeyRequest(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Запрос");

            return new UserPublicKeyRequestEditModel
            {
                UserPublicKeyRequestID = item.UserPublicKeyRequestID,
                ClientIdentifier = item.ClientIdentifier,
                IsActivated = false,
                KeyNotes = item.KeyNotes,
                UserLogin = item.LoginName
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, UserPublicKeyRequestEditModel model, GridSaveModelResult result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, UserPublicKeyRequestEditModel model, GridSaveModelResult result)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetUserPublicKeyRequest(token, model.UserPublicKeyRequestID);

            RiseExceptionIfNotFound(item, model.UserPublicKeyRequestID, "Запрос");

           
            if (model.IsActivated)
            {
                RemontinkaServer.Instance.EntitiesFacade.SaveUserPublicKey(token, new Romontinka.Server.DataLayer.Entities.UserPublicKey
                {
                    ClientIdentifier = item.ClientIdentifier,
                    EventDate = item.EventDate,
                    IsRevoked = false,
                    KeyNotes = item.KeyNotes,
                    Number = item.Number,
                    PublicKeyData = item.PublicKeyData,
                    UserID = item.UserID
                });

                RemontinkaServer.Instance.EntitiesFacade.DeleteUserPublicKeyRequest(token, item.UserPublicKeyRequestID);
            } //if
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid? entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteUserPublicKeyRequest(token, entityId);
        }
    }
}