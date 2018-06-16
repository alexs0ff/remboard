using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.ItemCategoryForm
{
    /// <summary>
    /// Адаптер для грида категорий товаров.
    /// </summary>
    public class ItemCategoryDataAdapter : JGridDataAdapterBase<Guid, ItemCategoryGridItemModel, ItemCategoryCreateModel, ItemCategoryCreateModel, ItemCategorySearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override ItemCategoryCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetItemCategory(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Категория товара");

            return new ItemCategoryCreateModel
            {
                Id = item.ItemCategoryID,
                Title = item.Title
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override ItemCategoryCreateModel CreateNewModel(SecurityToken token, ItemCategorySearchModel searchModel)
        {
            return new ItemCategoryCreateModel();
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteItemCategory(token, entityId);
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<ItemCategoryGridItemModel> GetPageableGridItems(SecurityToken token, ItemCategorySearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetItemCategories(token, searchModel.ItemCategoryName, searchModel.Page, itemsPerPage, out totalCount).Select(
                i => new ItemCategoryGridItemModel
                {
                    Id = i.ItemCategoryID,
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
        public override ItemCategoryGridItemModel SaveCreateModel(SecurityToken token, ItemCategoryCreateModel model, JGridSaveModelResult result)
        {
            var entity = new ItemCategory
            {
                ItemCategoryID = model.Id,
                Title = model.Title

            };

            RemontinkaServer.Instance.EntitiesFacade.SaveItemCategory(token, entity);
            return new ItemCategoryGridItemModel
            {
                Id = entity.ItemCategoryID,
                Title = entity.Title
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override ItemCategoryGridItemModel SaveEditModel(SecurityToken token, ItemCategoryCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }
    }
}