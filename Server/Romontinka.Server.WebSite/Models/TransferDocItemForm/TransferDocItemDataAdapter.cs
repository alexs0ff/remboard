using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;

namespace Romontinka.Server.WebSite.Models.TransferDocItemForm
{
    /// <summary>
    /// Адаптер пунктов документа о перемещении между складами.
    /// </summary>
    public class TransferDocItemDataAdapter : JGridDataAdapterBase<Guid, TransferDocItemGridItemModel, TransferDocItemCreateModel, TransferDocItemCreateModel, TransferDocItemSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override TransferDocItemCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetTransferDocItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Элемент документа о перемещении");

            return new TransferDocItemCreateModel
            {
                Description = item.Description,
                GoodsItemID = item.GoodsItemID,
                Id = item.TransferDocItemID,
                Total = item.Total,
                TransferDocID = item.TransferDocID
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override TransferDocItemCreateModel CreateNewModel(SecurityToken token, TransferDocItemSearchModel searchModel)
        {
            return new TransferDocItemCreateModel
            {
                TransferDocID = searchModel.TransferDocItemDocID
            };
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetTransferDocItem(token, entityId);
            if (entity != null)
            {
                if (!RemontinkaServer.Instance.DataStore.WarehouseDocIsProcessed(entity.TransferDocID))
                {
                    RemontinkaServer.Instance.EntitiesFacade.DeleteTransferDocItem(token, entityId);
                } //if
                else
                {
                    throw new Exception("Документ уже обработан");
                } //else
            } //if
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<TransferDocItemGridItemModel> GetPageableGridItems(SecurityToken token, TransferDocItemSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return
              RemontinkaServer.Instance.EntitiesFacade.GetTransferDocItems(token, searchModel.TransferDocItemDocID,
                                                                           searchModel.TransferDocItemName,
                                                                           searchModel.Page, itemsPerPage,
                                                                           out totalCount).Select(CreateModel);
        }

        /// <summary>
        /// Создает модель пункта грида из сущности.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <returns>Модель пункта грида.</returns>
        private TransferDocItemGridItemModel CreateModel(TransferDocItemDTO entity)
        {
            return new TransferDocItemGridItemModel
            {
                Description = entity.Description ?? string.Empty,
                GoodsItemTitle = entity.GoodsItemTitle,
                Id = entity.TransferDocItemID,
                Total = Utils.DecimalToString(entity.Total),
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override TransferDocItemGridItemModel SaveCreateModel(SecurityToken token, TransferDocItemCreateModel model, JGridSaveModelResult result)
        {
            if (!RemontinkaServer.Instance.EntitiesFacade.WarehouseDocIsProcessed(token, model.TransferDocID))
            {
                var entity = new TransferDocItem
                {
                    Description = model.Description,
                    GoodsItemID = model.GoodsItemID,
                    Total = model.Total,
                    TransferDocID = model.TransferDocID,
                    TransferDocItemID = model.Id
                };

                RemontinkaServer.Instance.EntitiesFacade.SaveTransferDocItem(token, entity);
                return
                    CreateModel(RemontinkaServer.Instance.EntitiesFacade.GetTransferDocItem(token,
                                                                                            entity.TransferDocItemID));
            }

            result.ModelErrors.Add(new PairItem<string, string>(string.Empty, "Документ уже обработан"));

            return null;
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override TransferDocItemGridItemModel SaveEditModel(SecurityToken token, TransferDocItemCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }
    }
}