using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.AutocompleteItemGridForm
{
    /// <summary>
    /// Сервис данных для автодополнения.
    /// </summary>
    public class AutocompleteItemGridDataAdapter : DataAdapterBase<Guid, AutocompleteItemGridModel, AutocompleteItemCreateModel, AutocompleteItemCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override AutocompleteItemGridModel CreateGridModel(SecurityToken token)
        {
            return new AutocompleteItemGridModel();
        }

        /// <summary>
        /// Получает данные для грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parentId">Код родительской записи.</param>
        /// <returns>Данные.</returns>
        public override IQueryable GedData(SecurityToken token, string parentId)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetAutocompleteItems(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override AutocompleteItemCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new AutocompleteItemCreateModel();
        }

        /// <summary>
        /// Инициализирует модель Обновления сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код связанной сущности.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель обновления.</returns>
        public override AutocompleteItemCreateModel CreateEditModel(SecurityToken token, Guid? entityId, AutocompleteItemGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetAutocompleteItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Пункт автозаполнения");

            return new AutocompleteItemCreateModel
            {
                AutocompleteKindID = item.AutocompleteKindID,
                AutocompleteItemID = item.AutocompleteItemID,
                Title = item.Title
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, AutocompleteItemCreateModel model, GridSaveModelResult result)
        {
            var entity = new AutocompleteItem
            {
                AutocompleteItemID = model.AutocompleteItemID,
                AutocompleteKindID = model.AutocompleteKindID,
                Title = model.Title,
                UserDomainID = token.User.UserDomainID
            };

            RemontinkaServer.Instance.EntitiesFacade.SaveAutocompleteItem(token, entity);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, AutocompleteItemCreateModel model, GridSaveModelResult result)
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
            RemontinkaServer.Instance.EntitiesFacade.DeleteAutocompleteItem(token, entityId);
        }
    }
}