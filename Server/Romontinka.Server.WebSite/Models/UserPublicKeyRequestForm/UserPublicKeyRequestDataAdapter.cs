using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.UserPublicKeyRequestForm
{
    /// <summary>
    /// Адаптер управления запросами.
    /// </summary>
    public class UserPublicKeyRequestDataAdapter : JGridDataAdapterBase<Guid, UserPublicKeyRequestGridItemModel, UserPublicKeyRequestCreateModel, UserPublicKeyRequestCreateModel, UserPublicKeyRequestSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override UserPublicKeyRequestCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetUserPublicKeyRequest(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Запрос");

            return new UserPublicKeyRequestCreateModel
            {
                Id = item.UserPublicKeyRequestID,
                ClientIdentifier = item.ClientIdentifier,
                IsActivated = false,
                KeyNotes = item.KeyNotes,
                UserLogin = item.LoginName
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override UserPublicKeyRequestCreateModel CreateNewModel(SecurityToken token, UserPublicKeyRequestSearchModel searchModel)
        {
            return new UserPublicKeyRequestCreateModel();
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteUserPublicKeyRequest(token, entityId);
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<UserPublicKeyRequestGridItemModel> GetPageableGridItems(SecurityToken token, UserPublicKeyRequestSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetUserPublicKeyRequests(token,
                                                                                     searchModel.
                                                                                         UserPublicKeyRequestName,
                                                                                     searchModel.Page, itemsPerPage,
                                                                                     out totalCount).
                Select(i => new UserPublicKeyRequestGridItemModel
                            {
                                ClientIdentifier = i.ClientIdentifier,
                                Id = i.UserPublicKeyRequestID,
                                KeyNotes = i.KeyNotes,
                                EventDate = Utils.DateTimeToStringWithTime(i.EventDate),
                                LoginName = i.LoginName
                            });
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override UserPublicKeyRequestGridItemModel SaveCreateModel(SecurityToken token, UserPublicKeyRequestCreateModel model, JGridSaveModelResult result)
        {
            return null;
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override UserPublicKeyRequestGridItemModel SaveEditModel(SecurityToken token, UserPublicKeyRequestCreateModel model, JGridSaveModelResult result)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetUserPublicKeyRequest(token, model.Id);

            RiseExceptionIfNotFound(item, model.Id, "Запрос");

            var rs = new UserPublicKeyRequestGridItemModel
                     {
                         ClientIdentifier = item.ClientIdentifier,
                         Id = item.UserPublicKeyRequestID,
                         KeyNotes = item.KeyNotes,
                         EventDate = Utils.DateTimeToStringWithTime(item.EventDate),
                         LoginName = item.LoginName
                     };

            if (model.IsActivated)
            {
                rs.RowClass = GridRowColors.Success;
                RemontinkaServer.Instance.EntitiesFacade.SaveUserPublicKey(token, new UserPublicKey
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

            return rs;
        }
    }
}