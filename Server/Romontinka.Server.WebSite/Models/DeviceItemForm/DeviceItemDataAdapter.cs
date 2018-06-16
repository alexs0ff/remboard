using System;
using System.Collections.Generic;
using System.Linq;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;

namespace Romontinka.Server.WebSite.Models.DeviceItemForm
{
    /// <summary>
    /// Адаптер для заданий
    /// </summary>
    public class DeviceItemDataAdapter : JGridDataAdapterBase<Guid, DeviceItemGridItemModel, DeviceItemCreateModel, DeviceItemCreateModel, DeviceItemSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override DeviceItemCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetDeviceItem(token, entityId);
            RiseExceptionIfNotFound(entity, entityId, "Запчасть");
            return new DeviceItemCreateModel
            {
                Id = entity.DeviceItemID,
                DeviceItemRepairOrderID = entity.RepairOrderID,
                DeviceItemPrice = entity.Price,
                DeviceItemTitle = entity.Title,
                DeviceItemCostPrice = entity.CostPrice,
                DeviceItemCount = entity.Count,
                DeviceItemEventDate = entity.EventDate,
                DeviceItemUserID = entity.UserID,
                WarehouseItemID = entity.WarehouseItemID
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override DeviceItemCreateModel CreateNewModel(SecurityToken token, DeviceItemSearchModel searchModel)
        {
            return new DeviceItemCreateModel
            {
                DeviceItemRepairOrderID = searchModel.DeviceItemRepairOrderID
            };
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            RemontinkaServer.Instance.OrderTimelineManager.TrackDeviceItemDelete(token, entityId);
            RemontinkaServer.Instance.EntitiesFacade.DeleteDeviceItem(token, entityId);
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<DeviceItemGridItemModel> GetPageableGridItems(SecurityToken token, DeviceItemSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetDeviceItems(token, searchModel.DeviceItemRepairOrderID,
                                                                        searchModel.DeviceItemName, searchModel.Page,
                                                                        itemsPerPage, out totalCount).Select(CreateItem);
        }

        private DeviceItemGridItemModel CreateItem(DeviceItem entity)
        {
            return new DeviceItemGridItemModel
            {
                
                DeviceItemPrice = Utils.DecimalToString(entity.Price),
                Id = entity.DeviceItemID,
                DeviceItemTitle = entity.Title,
                DeviceItemCostPrice = Utils.DecimalToString(entity.CostPrice),
                DeviceItemCount = Utils.DecimalToString(entity.Count)
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override DeviceItemGridItemModel SaveCreateModel(SecurityToken token, DeviceItemCreateModel model, JGridSaveModelResult result)
        {
            return SaveModel(token, model, result, false);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        /// <param name="isEdit">Признак редактирования.</param>
        public DeviceItemGridItemModel SaveModel(SecurityToken token, DeviceItemCreateModel model, JGridSaveModelResult result, bool isEdit)
        {
            if (model.WarehouseItemID==null && string.IsNullOrWhiteSpace(model.DeviceItemTitle))
            {
                result.ModelErrors.Add(new PairItem<string, string>("DeviceItemTitle", "Введите название запчасти или выберите ее из склада"));
                return null;
            }
            
            var entity = new DeviceItem
            {
                Price = model.DeviceItemPrice,
                RepairOrderID = model.DeviceItemRepairOrderID,
                Title = model.DeviceItemTitle,
                CostPrice = model.DeviceItemCostPrice,
                Count = model.DeviceItemCount,
                DeviceItemID = model.Id,
                EventDate = model.DeviceItemEventDate,
                UserID = model.DeviceItemUserID,
                WarehouseItemID = model.WarehouseItemID
            };

            DeviceItem oldItem = null;

            if (isEdit)
            {
                oldItem = RemontinkaServer.Instance.DataStore.GetDeviceItem(entity.DeviceItemID);
            } //if

            if (entity.WarehouseItemID!=null)//денормализуем данные
            {
                var item = RemontinkaServer.Instance.EntitiesFacade.GetWarehouseItem(token, entity.WarehouseItemID);

                RiseExceptionIfNotFound(item, entity.WarehouseItemID, "Остаток на складе");

                entity.Title = item.GoodsItemTitle;
                entity.Price = item.RepairPrice;
                entity.CostPrice = item.StartPrice;
            }

            RemontinkaServer.Instance.EntitiesFacade.SaveDeviceItem(token, entity);

            if (!isEdit)
            {
                RemontinkaServer.Instance.OrderTimelineManager.TrackNewDeviceItem(token, entity);
            } //if

            if (oldItem != null)
            {
                RemontinkaServer.Instance.OrderTimelineManager.TrackDeviceItemChanges(token, oldItem, entity);
            } //if

            return CreateItem(entity);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override DeviceItemGridItemModel SaveEditModel(SecurityToken token, DeviceItemCreateModel model, JGridSaveModelResult result)
        {
            return SaveModel(token, model, result,true);
        }
    }
}