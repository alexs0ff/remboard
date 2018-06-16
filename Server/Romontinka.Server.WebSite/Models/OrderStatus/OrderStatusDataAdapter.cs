using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Models.Branch;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.OrderStatus
{
    /// <summary>
    /// Адаптер для управления статусами заказа.
    /// </summary>
    public class OrderStatusDataAdapter : JGridDataAdapterBase<Guid, OrderStatusGridItemModel, OrderStatusCreateModel, OrderStatusCreateModel, OrderStatusSearchModel>
    {
        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override OrderStatusCreateModel CreateNewModel(SecurityToken token, OrderStatusSearchModel searchModel)
        {
            return new OrderStatusCreateModel();
        }

        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override OrderStatusCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetOrderStatus(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Статус");

            return new OrderStatusCreateModel
            {
                StatusKindID = item.StatusKindID,
                Id = item.OrderStatusID,
                Title = item.Title,
            };
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<OrderStatusGridItemModel> GetPageableGridItems(SecurityToken token, OrderStatusSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetOrderStatuses(token,searchModel.Name, searchModel.Page, itemsPerPage, out totalCount).Select(
               i => new OrderStatusGridItemModel
               {
                   Id = i.OrderStatusID,
                   Title = i.Title,
                   KindTitle = StatusKindSet.GetKindByID(i.StatusKindID).Title,
                   RowClass = StatusesColors[i.StatusKindID]
               }
               );
        }

        public static readonly Dictionary<byte?,string> StatusesColors = new Dictionary<byte?, string>
                                                              {
                                                                  {StatusKindSet.Closed.StatusKindID,GridRowColors.Active},
                                                                  {StatusKindSet.New.StatusKindID,GridRowColors.Info},
                                                                  {StatusKindSet.Suspended.StatusKindID,GridRowColors.Warning},
                                                                  {StatusKindSet.OnWork.StatusKindID,GridRowColors.Success},
                                                                  {StatusKindSet.Completed.StatusKindID,GridRowColors.Success},
                                                              };

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override OrderStatusGridItemModel SaveCreateModel(SecurityToken token, OrderStatusCreateModel model, JGridSaveModelResult result)
        {
            var entity = new DataLayer.Entities.OrderStatus
            {
                Title = model.Title,
                OrderStatusID = model.Id,
                StatusKindID = model.StatusKindID,
            };
            RemontinkaServer.Instance.EntitiesFacade.SaveOrderStatus(token,entity);
            return new OrderStatusGridItemModel
            {
                KindTitle = StatusKindSet.GetKindByID(entity.StatusKindID).Title,
                Id = entity.OrderStatusID,
                Title = entity.Title,
                RowClass = StatusesColors[entity.StatusKindID]
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override OrderStatusGridItemModel SaveEditModel(SecurityToken token, OrderStatusCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteOrderStatus(token,entityId);
        }
    }
}