using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;

namespace Romontinka.Server.WebSite.Models.UserPublicKeyForm
{
    /// <summary>
    /// Адаптер управления публичных ключей.
    /// </summary>
    public class UserPublicKeyDataAdapter : JGridDataAdapterBase<Guid, UserPublicKeyGridItemModel, UserPublicKeyCreateModel, UserPublicKeyCreateModel, UserPublicKeySearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override UserPublicKeyCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetUserPublicKey(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Публичный ключ");

            return new UserPublicKeyCreateModel
            {
                ClientIdentifier = item.ClientIdentifier,
                Id = item.UserPublicKeyID,
                IsRevoked = item.IsRevoked,
                KeyNotes = item.KeyNotes,
                PublicKeyData = item.PublicKeyData
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override UserPublicKeyCreateModel CreateNewModel(SecurityToken token, UserPublicKeySearchModel searchModel)
        {
            return null;
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteUserPublicKey(token, entityId);
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<UserPublicKeyGridItemModel> GetPageableGridItems(SecurityToken token, UserPublicKeySearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetUserPublicKeys(token, searchModel.UserPublicKeyName, searchModel.Page, itemsPerPage, out totalCount).ToList().Select(
                CreateGridItemModel 
                );
        }

        /// <summary>
        /// Создает модель пункта грида.
        /// </summary>
        private UserPublicKeyGridItemModel CreateGridItemModel(UserPublicKeyDTO item)
        {
            return new UserPublicKeyGridItemModel
                   {
                       Id = item.UserPublicKeyID,
                       EventDate = Utils.DateTimeToStringWithTime(item.EventDate),
                       LoginName = item.LoginName,
                       Status = item.IsRevoked ? "отозван" : "действующий",
                       UserFullName = string.Format("{0} {1} {2}",item.FirstName,item.LastName,item.MiddleName)
                   };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override UserPublicKeyGridItemModel SaveCreateModel(SecurityToken token, UserPublicKeyCreateModel model, JGridSaveModelResult result)
        {
            return null;
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override UserPublicKeyGridItemModel SaveEditModel(SecurityToken token, UserPublicKeyCreateModel model, JGridSaveModelResult result)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetUserPublicKey(token, model.Id);
            
            RiseExceptionIfNotFound(entity, model.Id, "Публичный ключ");

            entity.IsRevoked = model.IsRevoked;
            entity.PublicKeyData = model.PublicKeyData;

            RemontinkaServer.Instance.EntitiesFacade.SaveUserPublicKey(token, entity);
            
            return CreateGridItemModel(entity);
        }
    }
}