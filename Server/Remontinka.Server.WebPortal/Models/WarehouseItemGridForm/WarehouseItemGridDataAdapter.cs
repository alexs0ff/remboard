using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Models.WarehouseItemGridForm
{

    /// <summary>
    /// Сервис данных для складских остатков.
    /// </summary>
    public class WarehouseItemGridDataAdapter : DataAdapterBase<Guid, WarehouseItemGridModel, WarehouseItemEditModel, WarehouseItemEditModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override WarehouseItemGridModel CreateGridModel(SecurityToken token)
        {
            return new WarehouseItemGridModel
            {
                Warehouses = RemontinkaServer.Instance.EntitiesFacade.GetWarehouses(token)
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
            return RemontinkaServer.Instance.EntitiesFacade.GetWarehouseItems(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override WarehouseItemEditModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Инициализирует модель Обновления сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код связанной сущности.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель обновления.</returns>
        public override WarehouseItemEditModel CreateEditModel(SecurityToken token, Guid? entityId, WarehouseItemGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetWarehouseItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Остатки на складе");

            return new WarehouseItemEditModel
            {
                WarehouseItemID = item.WarehouseItemID,
                RepairPrice = item.RepairPrice,
                SalePrice = item.SalePrice,
                StartPrice = item.StartPrice
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, WarehouseItemEditModel model, GridSaveModelResult result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, WarehouseItemEditModel model, GridSaveModelResult result)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetWarehouseItem(token, model.WarehouseItemID);

            RiseExceptionIfNotFound(entity, model.WarehouseItemID, "Остатки на складе");

            entity.RepairPrice = model.RepairPrice;
            entity.SalePrice = model.SalePrice;
            entity.StartPrice = model.StartPrice;

            RemontinkaServer.Instance.EntitiesFacade.SaveWarehouseItem(token, entity);
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid? entityId)
        {
            throw new NotImplementedException();
        }
    }
}