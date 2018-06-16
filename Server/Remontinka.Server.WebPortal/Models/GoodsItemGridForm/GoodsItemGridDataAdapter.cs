using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.GoodsItemGridForm
{
    /// <summary>
    /// Сервис данных для номенклатуры.
    /// </summary>
    public class GoodsItemGridDataAdapter : DataAdapterBase<Guid, GoodsItemGridModel, GoodsItemCreateModel, GoodsItemCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override GoodsItemGridModel CreateGridModel(SecurityToken token)
        {
            return new GoodsItemGridModel
            {
                ItemCategories = RemontinkaServer.Instance.EntitiesFacade.GetItemCategories(token)
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
            return RemontinkaServer.Instance.EntitiesFacade.GetGoodsItems(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override GoodsItemCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new GoodsItemCreateModel
            {
                DimensionKindID = DimensionKindSet.Thing.DimensionKindID
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
        public override GoodsItemCreateModel CreateEditModel(SecurityToken token, Guid? entityId, GoodsItemGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetGoodsItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Номенклатура");

            return new GoodsItemCreateModel
            {
                Description = item.Description,
                DimensionKindID = item.DimensionKindID,
                GoodsItemID = item.GoodsItemID,
                ItemCategoryID = item.ItemCategoryID,
                Particular = item.Particular,
                Title = item.Title,
                UserCode = item.UserCode
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, GoodsItemCreateModel model, GridSaveModelResult result)
        {
            var entity = new GoodsItem
            {
                Description = model.Description,
                DimensionKindID = model.DimensionKindID,
                GoodsItemID = model.GoodsItemID,
                ItemCategoryID = model.ItemCategoryID,
                Particular = model.Particular,
                Title = model.Title,
                UserCode = model.UserCode,

            };
            RemontinkaServer.Instance.EntitiesFacade.SaveGoodsItem(token, entity);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, GoodsItemCreateModel model, GridSaveModelResult result)
        {
            SaveCreateModel(token, model, result);
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid? entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteGoodsItem(token, entityId);
        }
    }
}