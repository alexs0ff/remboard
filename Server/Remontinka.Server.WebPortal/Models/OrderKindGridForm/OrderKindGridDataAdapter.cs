using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Remontinka.Server.WebPortal.Services;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.OrderKindGridForm
{
    /// <summary>
    /// Адаптер данных для типов заказов.
    /// </summary>
    public class OrderKindGridDataAdapter : DataAdapterBase<Guid, OrderKindGridModel, OrderKindCreateModel, OrderKindCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override OrderKindGridModel CreateGridModel(SecurityToken token)
        {
            return new OrderKindGridModel();
        }

        /// <summary>
        /// Получает данные для грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parentId">Код родительской записи.</param>
        /// <returns>Данные.</returns>
        public override IQueryable GedData(SecurityToken token, string parentId)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetOrderKinds(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override OrderKindCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new OrderKindCreateModel();
        }

        /// <summary>
        /// Инициализирует модель Обновления сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код связанной сущности.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель обновления.</returns>
        public override OrderKindCreateModel CreateEditModel(SecurityToken token, Guid? entityId, OrderKindGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetOrderKind(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Тип заказа");

            return new OrderKindCreateModel
            {
                OrderKindID = item.OrderKindID,
                Title = item.Title,
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, OrderKindCreateModel model, GridSaveModelResult result)
        {
            var entity = new OrderKind
            {
                Title = model.Title,
                OrderKindID = model.OrderKindID
            };
            RemontinkaServer.Instance.EntitiesFacade.SaveOrderKind(token, entity);
            RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().CleanOrderKinds(token);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, OrderKindCreateModel model, GridSaveModelResult result)
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
            RemontinkaServer.Instance.EntitiesFacade.DeleteOrderKind(token, entityId);
            RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().CleanOrderKinds(token);
        }
    }
}