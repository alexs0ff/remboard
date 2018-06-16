using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.FinancialItemGridForm
{
    /// <summary>
    /// Сервис данных для финансовых статей.
    /// </summary>
    public class FinancialItemGridDataAdapter : DataAdapterBase<Guid, FinancialItemGridModel, FinancialItemCreateModel, FinancialItemCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override FinancialItemGridModel CreateGridModel(SecurityToken token)
        {
            return new FinancialItemGridModel();
        }

        /// <summary>
        /// Получает данные для грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parentId">Код родительской записи.</param>
        /// <returns>Данные.</returns>
        public override IQueryable GedData(SecurityToken token, string parentId)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetFinancialItems(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override FinancialItemCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new FinancialItemCreateModel
            {
                EventDate = DateTime.Today,
                FinancialItemKindID = FinancialItemKindSet.Custom.FinancialItemKindID
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
        public override FinancialItemCreateModel CreateEditModel(SecurityToken token, Guid? entityId, FinancialItemGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetFinancialItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Статья бюджета");

            return new FinancialItemCreateModel
            {
                Description = item.Description,
                FinancialItemID = item.FinancialItemID,
                Title = item.Title,
                EventDate = item.EventDate,
                FinancialItemKindID = item.FinancialItemKindID,
                TransactionKindID = item.TransactionKindID
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, FinancialItemCreateModel model, GridSaveModelResult result)
        {
            var entity = new FinancialItem
            {
                Description = model.Description,
                EventDate = model.EventDate,
                FinancialItemID = model.FinancialItemID,
                FinancialItemKindID = model.FinancialItemKindID,
                Title = model.Title,
                TransactionKindID = model.TransactionKindID
            };
            RemontinkaServer.Instance.EntitiesFacade.SaveFinancialItem(token, entity);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, FinancialItemCreateModel model, GridSaveModelResult result)
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
            RemontinkaServer.Instance.EntitiesFacade.DeleteFinancialItem(token, entityId);
        }
    }
}