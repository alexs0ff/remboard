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

namespace Romontinka.Server.WebSite.Models.TransferDocForm
{
    /// <summary>
    /// Адаптер для документов перемещения между складами.
    /// </summary>
    public class TransferDocDataAdapter : JGridDataAdapterBase<Guid, TransferDocGridItemModel, TransferDocCreateModel, TransferDocCreateModel, TransferDocSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override TransferDocCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetTransferDoc(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Документ списания");

            return new TransferDocCreateModel
            {
                DocDate = item.DocDate,
                DocDescription = item.DocDescription,
                DocNumber = item.DocNumber,
                Id = item.TransferDocID,
                RecipientWarehouseID = item.RecipientWarehouseID,
                SenderWarehouseID = item.SenderWarehouseID
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override TransferDocCreateModel CreateNewModel(SecurityToken token, TransferDocSearchModel searchModel)
        {
            return new TransferDocCreateModel
            {
                DocDate = DateTime.Today,
                SenderWarehouseID = searchModel.TransferDocSenderWarehouseID,
                RecipientWarehouseID = searchModel.TransferDocRecipientWarehouseID
            };
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            if (!RemontinkaServer.Instance.EntitiesFacade.WarehouseDocIsProcessed(token, entityId))
            {
                RemontinkaServer.Instance.EntitiesFacade.DeleteTransferDoc(token, entityId);
            } //if
            else
            {
                throw new Exception("Документ уже обработан");
            } //else
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<TransferDocGridItemModel> GetPageableGridItems(SecurityToken token, TransferDocSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return
                RemontinkaServer.Instance.EntitiesFacade.GetTransferDocs(token, searchModel.TransferDocSenderWarehouseID, searchModel.TransferDocRecipientWarehouseID, searchModel.TransferDocBeginDate, searchModel.TransferDocEndDate, searchModel.TransferDocName,searchModel.Page, itemsPerPage, out totalCount).
                    Select(CreateModel);
        }

        /// <summary>
        /// Создает пункт для грида из сущности.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <returns>Пункт для грида.</returns>
        private TransferDocGridItemModel CreateModel(TransferDocDTO entity)
        {
            return new TransferDocGridItemModel
            {
                Creator = Utils.GetPersonFullName(entity.CreatorLastName, entity.CreatorFirstName, entity.CreatorMiddleName),
                DocDate = Utils.DateTimeToString(entity.DocDate),
                DocDescription = entity.DocDescription,
                DocNumber = entity.DocNumber,
                Id = entity.TransferDocID,
                SenderWarehouseTitle = entity.SenderWarehouseTitle,
                RecipientWarehouseTitle = entity.RecipientWarehouseTitle,
                RowClass = entity.IsProcessed ? GridRowColors.Info : GridRowColors.Danger
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override TransferDocGridItemModel SaveCreateModel(SecurityToken token, TransferDocCreateModel model, JGridSaveModelResult result)
        {
            var entity = new TransferDoc
            {
                CreatorID = token.User.UserID,
                DocDate = model.DocDate,
                DocDescription = model.DocDescription,
                DocNumber = model.DocNumber,
                RecipientWarehouseID = model.RecipientWarehouseID,
                SenderWarehouseID = model.SenderWarehouseID,
                TransferDocID = model.Id

            };

            RemontinkaServer.Instance.EntitiesFacade.SaveTransferDoc(token, entity);

            var item = RemontinkaServer.Instance.EntitiesFacade.GetTransferDoc(token, entity.TransferDocID);
            return CreateModel(item);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override TransferDocGridItemModel SaveEditModel(SecurityToken token, TransferDocCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }
    }
}