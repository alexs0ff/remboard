using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core;
using Remontinka.Client.DataLayer.Entities;
using Remontinka.Client.Wpf.Controllers.Forms;
using Remontinka.Client.Wpf.Model;
using Remontinka.Client.Wpf.Model.Items;
using Remontinka.Client.Wpf.View;

namespace Remontinka.Client.Wpf.Controllers.Items
{
    /// <summary>
    /// Контроллер управления запчастями.
    /// </summary>
    public class DeviceItemDataController : ModelEditControllerBase<DeviceItemEditView, DeviceItemEditView, DeviceItemEditModel, DeviceItemEditModel, Guid, Guid?>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parameters">Модель параметров.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override DeviceItemEditModel CreateEditedModel(SecurityToken token, Guid entityId, Guid? parameters)
        {
            var entity = ClientCore.Instance.DataStore.GetDeviceItem(entityId);

            RiseExceptionIfNotFound(entity, entityId, "Запчасть");

            return new DeviceItemEditModel
            {
                Id = entity.DeviceItemIDGuid,
                RepairOrderID = entity.RepairOrderIDGuid,
                DeviceItemEventDate = entity.EventDateDateTime,
                DeviceItemPrice = WpfUtils.DecimalToString((decimal?)entity.Price),
                DeviceItemTitle = entity.Title,
                DeviceItemUserID = entity.UserIDGuid,
                DeviceItemCostPrice = WpfUtils.DecimalToString((decimal?)entity.CostPrice),
                DeviceItemCount = WpfUtils.DecimalToString((decimal?)entity.Count),
                WarehouseItemID = entity.WarehouseItemIDGuid
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="parameters">Модель параметров.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override DeviceItemEditModel CreateNewModel(SecurityToken token, Guid? parameters)
        {
            Guid? userID = null;
            if (token.User.ProjectRoleID == ProjectRoleSet.Engineer.ProjectRoleID)
            {
                userID = token.User.UserIDGuid;
            } //if

            return new DeviceItemEditModel
            {
                RepairOrderID = parameters,
                DeviceItemUserID = userID,
                DeviceItemEventDate = DateTime.Today
            };
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {

        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, DeviceItemEditModel model, SaveModelResult result)
        {
            SaveModel(token, model, result, false);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, DeviceItemEditModel model, SaveModelResult result)
        {
            SaveModel(token, model, result, true);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        /// <param name="isEdit">Признак редактирования модели. </param>
        private void SaveModel(SecurityToken token, DeviceItemEditModel model, SaveModelResult result, bool isEdit)
        {
            if (model.WarehouseItemID == null && string.IsNullOrWhiteSpace(model.DeviceItemTitle))
            {
                result.ModelErrors.Add(new PairItem<string, string>("DeviceItemTitle", "Введите название запчасти или выберите ее из склада"));
                return;
            }

            var entity = new DeviceItem
            {
                Price =  (double)WpfUtils.StringToDecimal(model.DeviceItemPrice),
                RepairOrderIDGuid = model.RepairOrderID,
                Title = model.DeviceItemTitle,
                CostPrice = (double)WpfUtils.StringToDecimal(model.DeviceItemCostPrice),
                Count = (double)WpfUtils.StringToDecimal(model.DeviceItemCount),
                DeviceItemIDGuid = model.Id,
                EventDateDateTime = model.DeviceItemEventDate,
                UserIDGuid = model.DeviceItemUserID,
                WarehouseItemIDGuid = model.WarehouseItemID
            };

            DeviceItem oldItem = null;

            if (isEdit)
            {
                oldItem = ClientCore.Instance.DataStore.GetDeviceItem(entity.DeviceItemIDGuid);
            } //if

            if (entity.WarehouseItemID != null)//денормализуем данные
            {
                var item = ClientCore.Instance.DataStore.GetWarehouseItem(entity.WarehouseItemIDGuid);

                RiseExceptionIfNotFound(item, entity.WarehouseItemIDGuid, "Остаток на складе");

                entity.Title = item.GoodsItemTitle;
                entity.Price = item.RepairPrice;
                entity.CostPrice = item.StartPrice;
            }

            ClientCore.Instance.DataStore.SaveDeviceItem(entity);

            if (!isEdit)
            {
                ClientCore.Instance.OrderTimelineManager.TrackNewDeviceItem(token, entity);
                model.Id = entity.DeviceItemIDGuid;
            } //if

            if (oldItem != null)
            {
                ClientCore.Instance.OrderTimelineManager.TrackDeviceItemChanges(token, oldItem, entity);
            } //if

        }
    }
}
