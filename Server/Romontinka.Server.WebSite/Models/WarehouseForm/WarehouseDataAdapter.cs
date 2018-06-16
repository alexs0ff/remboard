using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.WarehouseForm
{
    /// <summary>
    /// Адаптер для грида складов.
    /// </summary>
    public class WarehouseDataAdapter :
        JGridDataAdapterBase
            <Guid, WarehouseGridItemModel, WarehouseCreateModel, WarehouseCreateModel, WarehouseSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override WarehouseCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetWarehouse(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Склад");

            return new WarehouseCreateModel
                   {
                       Id = item.WarehouseID,
                       Title = item.Title
                   };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override WarehouseCreateModel CreateNewModel(SecurityToken token, WarehouseSearchModel searchModel)
        {
            return new WarehouseCreateModel();
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteWarehouse(token, entityId);
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<WarehouseGridItemModel> GetPageableGridItems(SecurityToken token,
                                                                                 WarehouseSearchModel searchModel,
                                                                                 int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetWarehouses(token, searchModel.WarehouseName,
                                                                          searchModel.Page, itemsPerPage, out totalCount)
                .Select(
                    i => new WarehouseGridItemModel
                         {
                             Id = i.WarehouseID,
                             Title = i.Title
                         }
                );
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override WarehouseGridItemModel SaveCreateModel(SecurityToken token, WarehouseCreateModel model,
                                                               JGridSaveModelResult result)
        {
            var entity = new Warehouse
                         {
                             WarehouseID = model.Id,
                             Title = model.Title

                         };

            RemontinkaServer.Instance.EntitiesFacade.SaveWarehouse(token, entity);
            return new WarehouseGridItemModel
                   {
                       Id = entity.WarehouseID,
                       Title = entity.Title
                   };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override WarehouseGridItemModel SaveEditModel(SecurityToken token, WarehouseCreateModel model,
                                                             JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }
    }
}