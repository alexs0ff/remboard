using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.FinancialItemValueGridForm
{
    /// <summary>
    /// Адаптер данных для текущи доходов и расходов.
    /// </summary>
    public class FinancialItemValueGridDataAdapter : DataAdapterBase<Guid, FinancialItemValueGridModel, FinancialItemValueCreateModel, FinancialItemValueCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override FinancialItemValueGridModel CreateGridModel(SecurityToken token)
        {
            return new FinancialItemValueGridModel
            {
                FinancialGroups = RemontinkaServer.Instance.EntitiesFacade.GetFinancialGroupItems(token),
                FinancialItems = RemontinkaServer.Instance.EntitiesFacade.GetFinancialItems(token)
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
            return RemontinkaServer.Instance.EntitiesFacade.GetFinancialItemValues(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override FinancialItemValueCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new FinancialItemValueCreateModel
            {
                EventDate = DateTime.Today
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
        public override FinancialItemValueCreateModel CreateEditModel(SecurityToken token, Guid? entityId, FinancialItemValueGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetFinancialItemValue(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Значение статьи бюджета");

            return new FinancialItemValueCreateModel
            {
                Description = item.Description,
                FinancialItemValueID = item.FinancialItemValueID,
                Amount = item.Amount,
                CostAmount = item.CostAmount,
                EventDate = item.EventDate,
                FinancialGroupID = item.FinancialGroupID,
                FinancialItemID = item.FinancialItemID
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, FinancialItemValueCreateModel model, GridSaveModelResult result)
        {
            var entity = new FinancialItemValue
            {
                Description = model.Description ?? string.Empty,
                EventDate = model.EventDate,
                FinancialItemValueID = model.FinancialItemValueID,
                Amount = model.Amount,
                CostAmount = model.CostAmount,
                FinancialGroupID = model.FinancialGroupID,
                FinancialItemID = model.FinancialItemID
            };
            RemontinkaServer.Instance.EntitiesFacade.SaveFinancialItemValue(token, entity);

            var entityDto = RemontinkaServer.Instance.EntitiesFacade.GetFinancialItemValue(token, entity.FinancialItemValueID); ;
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, FinancialItemValueCreateModel model, GridSaveModelResult result)
        {
            SaveCreateModel(token, model, result); ;
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid? entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteFinancialItemValue(token, entityId);
        }
    }
}