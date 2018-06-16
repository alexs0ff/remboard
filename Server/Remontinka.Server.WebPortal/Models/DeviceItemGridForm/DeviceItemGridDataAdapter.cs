using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Helpers;
using Remontinka.Server.WebPortal.Models.Common;
using Remontinka.Server.WebPortal.Services;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.DeviceItemGridForm
{
    /// <summary>
    /// Менеджер данных для установленных запчастей.
    /// </summary>
    public class DeviceItemGridDataAdapter : DataAdapterBase<Guid, DeviceItemGridModel, DeviceItemCreateModel, DeviceItemCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override DeviceItemGridModel CreateGridModel(SecurityToken token)
        {
            return new DeviceItemGridModel
            {
                Engineers = RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().GetUserList(token, ProjectRoleSet.Engineer.ProjectRoleID.Value),
            };
        }

        /// <summary>
        /// Получает данные для грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parentId">Код родительской записи.</param>
        /// <returns>Данные.</returns>
        public override IQueryable GedData(SecurityToken token, string parentId)
        {
            var id = Guid.Parse(parentId);
            return RemontinkaServer.Instance.EntitiesFacade.GetDeviceItems(token, id);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override DeviceItemCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new DeviceItemCreateModel
            {
                RepairOrderID = Guid.Parse(createParameters.ParentId),
                DeviceItemEventDate = DateTime.Today
            };
        }

        /// <summary>
        /// Инициализирует модель Обновления сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код связанной сущности.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель обновления.</returns>
        public override DeviceItemCreateModel CreateEditModel(SecurityToken token, Guid? entityId, DeviceItemGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetDeviceItem(token, entityId);
            RiseExceptionIfNotFound(entity, entityId, "Запчасть");

            if (entity.UserID != null && gridModel.Engineers.All(i => i.Value != entity.UserID))
            {
                gridModel.Engineers.Add(new SelectListItem<Guid> { Value = entity.UserID, Text = RemontinkaServer.Instance.EntitiesFacade.GetUser(token,entity.UserID).ToString() });
            }

            return new DeviceItemCreateModel
            {
                DeviceItemID = entity.DeviceItemID,
                RepairOrderID = entity.RepairOrderID,
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
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        /// <param name="isEdit">Признак редактирования.</param>
        public void SaveModel(SecurityToken token, DeviceItemCreateModel model, GridSaveModelResult result, bool isEdit)
        {
            if (model.WarehouseItemID == null && string.IsNullOrWhiteSpace(model.DeviceItemTitle))
            {
                result.ModelErrors.Add(new PairItem<string, string>("DeviceItemTitle", "Введите название запчасти или выберите ее из склада"));
                return;
            }

            var entity = new DeviceItem
            {
                Price = model.DeviceItemPrice,
                RepairOrderID = model.RepairOrderID,
                Title = model.DeviceItemTitle,
                CostPrice = model.DeviceItemCostPrice,
                Count = model.DeviceItemCount,
                DeviceItemID = model.DeviceItemID,
                EventDate = model.DeviceItemEventDate,
                UserID = model.DeviceItemUserID,
                WarehouseItemID = model.WarehouseItemID
            };

            DeviceItem oldItem = null;

            if (isEdit)
            {
                oldItem = RemontinkaServer.Instance.DataStore.GetDeviceItem(entity.DeviceItemID);
            } //if

            if (entity.WarehouseItemID != null)//денормализуем данные
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
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, DeviceItemCreateModel model, GridSaveModelResult result)
        {
            SaveModel(token, model, result, false);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, DeviceItemCreateModel model, GridSaveModelResult result)
        {
            SaveModel(token, model, result, true);
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid? entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteDeviceItem(token,entityId);
        }
    }
}