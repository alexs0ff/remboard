using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;

namespace Romontinka.Server.WebSite.Models.WarehouseItemForm
{
    /// <summary>
    /// Дата адаптер для грида по управлению остатками на складе.
    /// </summary>
    public class WarehouseItemDataAdapter:JGridDataAdapterBase
            <Guid, WarehouseItemGridItemModel, WarehouseItemCreateModel, WarehouseItemEditModel, WarehouseItemSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override WarehouseItemEditModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetWarehouseItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Остатки на складе");

            return new WarehouseItemEditModel
            {
                Id = item.WarehouseItemID,
                RepairPrice = item.RepairPrice,
                SalePrice = item.SalePrice,
                StartPrice = item.StartPrice
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override WarehouseItemCreateModel CreateNewModel(SecurityToken token, WarehouseItemSearchModel searchModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<WarehouseItemGridItemModel> GetPageableGridItems(SecurityToken token, WarehouseItemSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetWarehouseItems(token,
                                                                              searchModel.WarehouseItemWarehouseID,
                                                                              searchModel.WarehouseItemName,
                                                                              searchModel.Page, itemsPerPage,
                                                                              out totalCount)
                .Select(CreateModel);
        }

        /// <summary>
        /// Создает модель пункта грида по сущности.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <returns>Элемент модели грида.</returns>
        private WarehouseItemGridItemModel CreateModel(WarehouseItemDTO entity)
        {
            var itemCount = string.Empty;
            if (entity.DimensionKindID ==DimensionKindSet.Thing.DimensionKindID)
            {
                itemCount = string.Format("{0} {1}", Utils.IntToString((int) entity.Total),
                                          DimensionKindSet.Thing.ShortTitle);
            } //if

            return new WarehouseItemGridItemModel
                   {
                       Id = entity.WarehouseItemID,
                        GoodsItemTitle = entity.GoodsItemTitle,
                        RepairPrice = Utils.DecimalToString(entity.RepairPrice),
                        SalePrice = Utils.DecimalToString(entity.SalePrice),
                        StartPrice = Utils.DecimalToString(entity.StartPrice),
                       Total = itemCount
                   };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override WarehouseItemGridItemModel SaveCreateModel(SecurityToken token, WarehouseItemCreateModel model, JGridSaveModelResult result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override WarehouseItemGridItemModel SaveEditModel(SecurityToken token, WarehouseItemEditModel model, JGridSaveModelResult result)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetWarehouseItem(token, model.Id);

            RiseExceptionIfNotFound(entity, model.Id, "Остатки на складе");

            entity.RepairPrice = model.RepairPrice;
            entity.SalePrice = model.SalePrice;
            entity.StartPrice = model.StartPrice;

            RemontinkaServer.Instance.EntitiesFacade.SaveWarehouseItem(token, entity);

            return CreateModel(entity);
        }
    }
}