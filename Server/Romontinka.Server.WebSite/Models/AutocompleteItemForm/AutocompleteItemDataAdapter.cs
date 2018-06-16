using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.AutocompleteItemForm
{
    /// <summary>
    /// Дата адаптер для пунктов автодополнения.
    /// </summary>
    public class AutocompleteItemDataAdapter : JGridDataAdapterBase<Guid, AutocompleteItemGridItemModel, AutocompleteItemCreateModel, AutocompleteItemCreateModel, AutocompleteItemSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override AutocompleteItemCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetAutocompleteItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Пункт автозаполнения");

            return new AutocompleteItemCreateModel
            {
                AutocompleteKindID = item.AutocompleteKindID,
                Id = item.AutocompleteItemID,
                Title = item.Title
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override AutocompleteItemCreateModel CreateNewModel(SecurityToken token, AutocompleteItemSearchModel searchModel)
        {
            return new AutocompleteItemCreateModel {AutocompleteKindID = searchModel.AutocompleteItemSearchKindID};
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteAutocompleteItem(token, entityId);
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<AutocompleteItemGridItemModel> GetPageableGridItems(SecurityToken token, AutocompleteItemSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetAutocompleteItems(token,
                                                                                 searchModel.
                                                                                     AutocompleteItemSearchKindID,
                                                                                 searchModel.AutocompleteItemSearchTitle,
                                                                                 searchModel.Page, itemsPerPage,
                                                                                 out totalCount).Select(item => new AutocompleteItemGridItemModel
                                                                                                                {
                                                                                                                    AutocompleteKindTitle = AutocompleteKindSet.GetKindByID(item.AutocompleteKindID).Title,
                                                                                                                    Id = item.AutocompleteItemID,
                                                                                                                    Title = item.Title
                                                                                                                });
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override AutocompleteItemGridItemModel SaveCreateModel(SecurityToken token, AutocompleteItemCreateModel model, JGridSaveModelResult result)
        {
            var entity = new AutocompleteItem
            {
                AutocompleteItemID = model.Id,
                AutocompleteKindID = model.AutocompleteKindID,
                Title = model.Title,
                UserDomainID = token.User.UserDomainID
            };

            RemontinkaServer.Instance.EntitiesFacade.SaveAutocompleteItem(token, entity);
            return new AutocompleteItemGridItemModel
                   {
                       AutocompleteKindTitle = AutocompleteKindSet.GetKindByID(entity.AutocompleteKindID).Title,
                       Id = entity.AutocompleteItemID,
                       Title = entity.Title
                   };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override AutocompleteItemGridItemModel SaveEditModel(SecurityToken token, AutocompleteItemCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }
    }
}