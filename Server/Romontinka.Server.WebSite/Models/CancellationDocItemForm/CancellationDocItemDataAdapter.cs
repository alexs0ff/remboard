using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;

namespace Romontinka.Server.WebSite.Models.CancellationDocItemForm
{
    /// <summary>
    /// Адаптер данных для пунктов документов о списании со склада.
    /// </summary>
    public class CancellationDocItemDataAdapter : JGridDataAdapterBase<Guid, CancellationDocItemGridItemModel, CancellationDocItemCreateModel, CancellationDocItemCreateModel, CancellationDocItemSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override CancellationDocItemCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetCancellationDocItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Элемент документа о списании");

            return new CancellationDocItemCreateModel
            {
                Description = item.Description,
                GoodsItemID = item.GoodsItemID,
                Id = item.CancellationDocItemID,
                Total = item.Total,
                CancellationDocID = item.CancellationDocID
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override CancellationDocItemCreateModel CreateNewModel(SecurityToken token, CancellationDocItemSearchModel searchModel)
        {
            return new CancellationDocItemCreateModel
            {
                CancellationDocID = searchModel.CancellationDocItemDocID
            };
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetCancellationDocItem(token, entityId);
            if (entity != null)
            {
                if (!RemontinkaServer.Instance.DataStore.WarehouseDocIsProcessed(entity.CancellationDocID))
                {
                    RemontinkaServer.Instance.EntitiesFacade.DeleteCancellationDocItem(token, entityId);
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
        public override IEnumerable<CancellationDocItemGridItemModel> GetPageableGridItems(SecurityToken token, CancellationDocItemSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return
               RemontinkaServer.Instance.EntitiesFacade.GetCancellationDocItems(token, searchModel.CancellationDocItemDocID,
                                                                            searchModel.CancellationDocItemName,
                                                                            searchModel.Page, itemsPerPage,
                                                                            out totalCount).
                   Select(CreateModel);
        }

        /// <summary>
        /// Создает модель пункта грида из сущности.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <returns>Модель пункта грида.</returns>
        private CancellationDocItemGridItemModel CreateModel(CancellationDocItemDTO entity)
        {
            return new CancellationDocItemGridItemModel
            {
                Description = entity.Description ?? string.Empty,
                GoodsItemTitle = entity.GoodsItemTitle,
                Id = entity.CancellationDocItemID,
                Total = Utils.DecimalToString(entity.Total),
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override CancellationDocItemGridItemModel SaveCreateModel(SecurityToken token, CancellationDocItemCreateModel model, JGridSaveModelResult result)
        {
            if (!RemontinkaServer.Instance.EntitiesFacade.WarehouseDocIsProcessed(token, model.CancellationDocID))
            {
                var entity = new CancellationDocItem
                {
                    Description = model.Description,
                    GoodsItemID = model.GoodsItemID,
                    Total = model.Total,
                    CancellationDocID = model.CancellationDocID,
                    CancellationDocItemID = model.Id
                };

                RemontinkaServer.Instance.EntitiesFacade.SaveCancellationDocItem(token, entity);
                return
                    CreateModel(RemontinkaServer.Instance.EntitiesFacade.GetCancellationDocItem(token,
                                                                                            entity.CancellationDocItemID));
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
        public override CancellationDocItemGridItemModel SaveEditModel(SecurityToken token, CancellationDocItemCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }
    }
}