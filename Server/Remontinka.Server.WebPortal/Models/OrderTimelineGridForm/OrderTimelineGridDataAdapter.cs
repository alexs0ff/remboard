using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Models.OrderTimelineGridForm
{
    /// <summary>
    /// Адаптер данных по истории.
    /// </summary>
    public class OrderTimelineGridDataAdapter : DataAdapterBase<Guid, OrderTimelineGridModel, OrderTimelineCreateModel, OrderTimelineCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override OrderTimelineGridModel CreateGridModel(SecurityToken token)
        {
            return new OrderTimelineGridModel();
        }

        /// <summary>
        /// Получает данные для грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parentId">Код родительской записи.</param>
        /// <returns>Данные.</returns>
        public override IQueryable GedData(SecurityToken token, string parentId)
        {
            var id = Guid.Parse(parentId);
            return RemontinkaServer.Instance.EntitiesFacade.GetOrderTimelines(token, id);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override OrderTimelineCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new OrderTimelineCreateModel
            {
                RepairOrderID = Guid.Parse(createParameters.ParentId)
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
        public override OrderTimelineCreateModel CreateEditModel(SecurityToken token, Guid? entityId, OrderTimelineGridModel gridModel,
            GridCreateParameters createParameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, OrderTimelineCreateModel model, GridSaveModelResult result)
        {
            RemontinkaServer.Instance.EntitiesFacade.AddRepairOrderComment(token, model.RepairOrderID,
                                                                                model.Title);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, OrderTimelineCreateModel model, GridSaveModelResult result)
        {
            throw new NotImplementedException();
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