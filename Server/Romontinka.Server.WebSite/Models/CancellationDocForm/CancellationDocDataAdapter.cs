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

namespace Romontinka.Server.WebSite.Models.CancellationDocForm
{
    /// <summary>
    /// Адаптер грида для управления документами списания со склада.
    /// </summary>
    public class CancellationDocDataAdapter : JGridDataAdapterBase<Guid, CancellationDocGridItemModel, CancellationDocCreateModel, CancellationDocCreateModel, CancellationDocSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override CancellationDocCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetCancellationDoc(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Документ списания");

            return new CancellationDocCreateModel
            {
                DocDate = item.DocDate,
                DocDescription = item.DocDescription,
                DocNumber = item.DocNumber,
                Id = item.CancellationDocID,
                WarehouseID = item.WarehouseID,
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override CancellationDocCreateModel CreateNewModel(SecurityToken token, CancellationDocSearchModel searchModel)
        {
            return new CancellationDocCreateModel
            {
                DocDate = DateTime.Today,
                WarehouseID = searchModel.CancellationDocWarehouseID
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
                RemontinkaServer.Instance.EntitiesFacade.DeleteCancellationDoc(token, entityId);
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
        public override IEnumerable<CancellationDocGridItemModel> GetPageableGridItems(SecurityToken token, CancellationDocSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return
                RemontinkaServer.Instance.EntitiesFacade.GetCancellationDocs(token, searchModel.CancellationDocWarehouseID, searchModel.CancellationDocBeginDate, searchModel.CancellationDocEndDate, searchModel.CancellationDocName, searchModel.Page, itemsPerPage, out totalCount).
                    Select(CreateModel);
        }

        /// <summary>
        /// Создает пункт для грида из сущности.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <returns>Пункт для грида.</returns>
        private CancellationDocGridItemModel CreateModel(CancellationDocDTO entity)
        {
            return new CancellationDocGridItemModel
            {
                Creator = Utils.GetPersonFullName(entity.CreatorLastName, entity.CreatorFirstName, entity.CreatorMiddleName),
                DocDate = Utils.DateTimeToString(entity.DocDate),
                DocDescription = entity.DocDescription,
                DocNumber = entity.DocNumber,
                Id = entity.CancellationDocID,
                WarehouseTitle = entity.WarehouseTitle,
                RowClass = entity.IsProcessed ? GridRowColors.Info : GridRowColors.Danger
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override CancellationDocGridItemModel SaveCreateModel(SecurityToken token, CancellationDocCreateModel model, JGridSaveModelResult result)
        {
            var entity = new CancellationDoc
            {
                CreatorID = token.User.UserID,
                DocDate = model.DocDate,
                DocDescription = model.DocDescription,
                DocNumber = model.DocNumber,
                WarehouseID = model.WarehouseID,
                CancellationDocID = model.Id

            };

            RemontinkaServer.Instance.EntitiesFacade.SaveCancellationDoc(token, entity);

            var item = RemontinkaServer.Instance.EntitiesFacade.GetCancellationDoc(token, entity.CancellationDocID);
            return CreateModel(item);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override CancellationDocGridItemModel SaveEditModel(SecurityToken token, CancellationDocCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }
    }
}