using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Remontinka.Server.WebPortal.Services;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.OrderStatusGridForm
{
    /// <summary>
    /// Сервис данных для статусов заказа.
    /// </summary>
    public class OrderStatusGridDataAdapter : DataAdapterBase<Guid, OrderStatusGridModel, OrderStatusCreateModel, OrderStatusCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override OrderStatusGridModel CreateGridModel(SecurityToken token)
        {
            return new OrderStatusGridModel
            {
                StatusKinds = StatusKindSet.Statuses
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
            return RemontinkaServer.Instance.EntitiesFacade.GetOrderStatuses(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override OrderStatusCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new OrderStatusCreateModel();
        }

        /// <summary>
        /// Инициализирует модель Обновления сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код связанной сущности.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель обновления.</returns>
        public override OrderStatusCreateModel CreateEditModel(SecurityToken token, Guid? entityId, OrderStatusGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetOrderStatus(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Статус");

            return new OrderStatusCreateModel
            {
                StatusKindID = item.StatusKindID,
                OrderStatusID = item.OrderStatusID,
                Title = item.Title,
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, OrderStatusCreateModel model, GridSaveModelResult result)
        {
            var entity = new OrderStatus
            {
                Title = model.Title,
                OrderStatusID = model.OrderStatusID,
                StatusKindID = model.StatusKindID,
            };
            RemontinkaServer.Instance.EntitiesFacade.SaveOrderStatus(token, entity);
            RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().CleanOrderStatuses(token);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, OrderStatusCreateModel model, GridSaveModelResult result)
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
            RemontinkaServer.Instance.EntitiesFacade.DeleteOrderStatus(token, entityId);
            RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().CleanOrderStatuses(token);
        }
    }
}