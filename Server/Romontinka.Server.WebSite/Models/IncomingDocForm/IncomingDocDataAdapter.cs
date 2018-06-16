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

namespace Romontinka.Server.WebSite.Models.IncomingDocForm
{
    /// <summary>
    /// Адаптер управления приходными накладными.
    /// </summary>
    public class IncomingDocDataAdapter : JGridDataAdapterBase<Guid, IncomingDocGridItemModel, IncomingDocCreateModel, IncomingDocCreateModel, IncomingDocSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override IncomingDocCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetIncomingDoc(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Приходная накладная");

            return new IncomingDocCreateModel
            {
                ContractorID = item.ContractorID,
                DocDate = item.DocDate,
                DocDescription = item.DocDescription,
                DocNumber = item.DocNumber,
                Id = item.IncomingDocID,
                WarehouseID = item.WarehouseID
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override IncomingDocCreateModel CreateNewModel(SecurityToken token, IncomingDocSearchModel searchModel)
        {
            return new IncomingDocCreateModel
                   {
                       DocDate = DateTime.Today,
                       WarehouseID = searchModel.IncomingDocWarehouseID
                   };
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            if (!RemontinkaServer.Instance.EntitiesFacade.WarehouseDocIsProcessed(token,entityId))
            {
                RemontinkaServer.Instance.EntitiesFacade.DeleteIncomingDoc(token, entityId);    
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
        public override IEnumerable<IncomingDocGridItemModel> GetPageableGridItems(SecurityToken token, IncomingDocSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return
                RemontinkaServer.Instance.EntitiesFacade.GetIncomingDocs(token, searchModel.IncomingDocWarehouseID,searchModel.IncomingDocBeginDate,searchModel.IncomingDocEndDate,searchModel.IncomingDocName,searchModel.Page, itemsPerPage, out totalCount).
                    Select(CreateModel);
        }

        /// <summary>
        /// Создает пункт для грида из сущности.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <returns>Пункт для грида.</returns>
        private IncomingDocGridItemModel CreateModel(IncomingDocDTO entity)
        {
            return new IncomingDocGridItemModel
                   {
                       ContractorLegalName = entity.ContractorLegalName,
                       Creator = Utils.GetPersonFullName(entity.CreatorLastName,entity.CreatorFirstName,entity.CreatorMiddleName),
                       DocDate = Utils.DateTimeToString(entity.DocDate),
                       DocDescription = entity.DocDescription,
                       DocNumber = entity.DocNumber,
                       Id = entity.IncomingDocID,
                       WarehouseTitle = entity.WarehouseTitle,
                       RowClass = entity.IsProcessed?GridRowColors.Info:GridRowColors.Danger
                   };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override IncomingDocGridItemModel SaveCreateModel(SecurityToken token, IncomingDocCreateModel model, JGridSaveModelResult result)
        {
            var entity = new IncomingDoc
            {
                ContractorID = model.ContractorID,
                CreatorID = token.User.UserID,
                DocDate = model.DocDate,
                DocDescription = model.DocDescription,
                DocNumber = model.DocNumber,
                IncomingDocID = model.Id,
                WarehouseID = model.WarehouseID,

            };

            RemontinkaServer.Instance.EntitiesFacade.SaveIncomingDoc(token, entity);

            var item = RemontinkaServer.Instance.EntitiesFacade.GetIncomingDoc(token, entity.IncomingDocID);
            return CreateModel(item);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override IncomingDocGridItemModel SaveEditModel(SecurityToken token, IncomingDocCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }
    }
}