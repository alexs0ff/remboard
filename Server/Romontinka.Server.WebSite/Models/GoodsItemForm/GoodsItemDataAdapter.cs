using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.GoodsItemForm
{
    /// <summary>
    /// Адаптер управления номенклатурой
    /// </summary>
    public class GoodsItemDataAdapter : JGridDataAdapterBase<Guid, GoodsItemGridItemModel, GoodsItemCreateModel, GoodsItemCreateModel, GoodsItemSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override GoodsItemCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetGoodsItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Номенклатура");

            return new GoodsItemCreateModel
            {
                Description = item.Description,
                DimensionKindID = item.DimensionKindID,
                Id = item.GoodsItemID,
                ItemCategoryID = item.ItemCategoryID,
                Particular = item.Particular,
                Title = item.Title,
                UserCode = item.UserCode
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override GoodsItemCreateModel CreateNewModel(SecurityToken token, GoodsItemSearchModel searchModel)
        {
            return new GoodsItemCreateModel
                   {
                       DimensionKindID = DimensionKindSet.Thing.DimensionKindID
                   };
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteGoodsItem(token, entityId);
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<GoodsItemGridItemModel> GetPageableGridItems(SecurityToken token, GoodsItemSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetGoodsItems(token, searchModel.GoodsItemName, searchModel.Page, itemsPerPage, out totalCount).Select(
                i => new GoodsItemGridItemModel
                {
                    Id = i.GoodsItemID,
                    Title = i.Title,
                    Description = i.Description??string.Empty,
                    ItemCategoryTitle = i.ItemCategoryTitle,
                    Particular = i.Particular ??string.Empty,
                    UserCode = i.UserCode ?? string.Empty
                }
                );
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override GoodsItemGridItemModel SaveCreateModel(SecurityToken token, GoodsItemCreateModel model, JGridSaveModelResult result)
        {
            var entity = new GoodsItem
            {
                Description = model.Description,
                DimensionKindID = model.DimensionKindID,
                GoodsItemID = model.Id,
                ItemCategoryID = model.ItemCategoryID,
                Particular = model.Particular,
                Title = model.Title,
                UserCode = model.UserCode,
                
            };
            RemontinkaServer.Instance.EntitiesFacade.SaveGoodsItem(token, entity);

            var item = RemontinkaServer.Instance.EntitiesFacade.GetGoodsItem(token, entity.GoodsItemID);
            return new GoodsItemGridItemModel
            {
                Id = item.GoodsItemID,
                Title = item.Title,
                Description = item.Description??string.Empty,
                ItemCategoryTitle = item.ItemCategoryTitle,
                Particular = item.Particular ?? string.Empty,
                UserCode = item.UserCode ?? string.Empty
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override GoodsItemGridItemModel SaveEditModel(SecurityToken token, GoodsItemCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }
    }
}