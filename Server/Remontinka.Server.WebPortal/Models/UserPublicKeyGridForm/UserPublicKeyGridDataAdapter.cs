using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Models.UserPublicKeyGridForm
{
    /// <summary>
    /// Сревис данных для управления ключами.
    /// </summary>
    public class UserPublicKeyGridDataAdapter : DataAdapterBase<Guid, UserPublicKeyGridModel, UserPublicKeyEditModel, UserPublicKeyEditModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override UserPublicKeyGridModel CreateGridModel(SecurityToken token)
        {
            return new UserPublicKeyGridModel();
        }

        /// <summary>
        /// Получает данные для грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parentId">Код родительской записи.</param>
        /// <returns>Данные.</returns>
        public override IQueryable GedData(SecurityToken token, string parentId)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetUserPublicKeys(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override UserPublicKeyEditModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
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
        public override UserPublicKeyEditModel CreateEditModel(SecurityToken token, Guid? entityId, UserPublicKeyGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetUserPublicKey(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Публичный ключ");

            return new UserPublicKeyEditModel
            {
                ClientIdentifier = item.ClientIdentifier,
                UserPublicKeyID = item.UserPublicKeyID,
                IsRevoked = item.IsRevoked,
                KeyNotes = item.KeyNotes,
                PublicKeyData = item.PublicKeyData
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, UserPublicKeyEditModel model, GridSaveModelResult result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, UserPublicKeyEditModel model, GridSaveModelResult result)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetUserPublicKey(token, model.UserPublicKeyID);

            RiseExceptionIfNotFound(entity, model.UserPublicKeyID, "Публичный ключ");

            entity.IsRevoked = model.IsRevoked;
            entity.PublicKeyData = model.PublicKeyData;

            RemontinkaServer.Instance.EntitiesFacade.SaveUserPublicKey(token, entity); ;
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid? entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteUserPublicKey(token, entityId);
        }
    }
}