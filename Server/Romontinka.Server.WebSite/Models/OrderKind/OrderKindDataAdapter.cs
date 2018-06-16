using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Models.OrderStatus;

namespace Romontinka.Server.WebSite.Models.OrderKind
{
    /// <summary>
    /// Адаптер для управления типами заказа.
    /// </summary>
    public class OrderKindDataAdapter : JGridDataAdapterBase<Guid, OrderKindGridItemModel, OrderKindCreateModel, OrderKindCreateModel, OrderKindSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override OrderKindCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetOrderKind(token,entityId);

            RiseExceptionIfNotFound(item, entityId, "Тип");

            return new OrderKindCreateModel
            {
                Id = item.OrderKindID,
                Title = item.Title,
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override OrderKindCreateModel CreateNewModel(SecurityToken token, OrderKindSearchModel searchModel)
        {
            return new OrderKindCreateModel();
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteOrderKind(token,entityId);
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<OrderKindGridItemModel> GetPageableGridItems(SecurityToken token, OrderKindSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetOrderKinds(token,searchModel.Name, searchModel.Page, itemsPerPage, out totalCount).Select(
               i => new OrderKindGridItemModel
               {
                   Id = i.OrderKindID,
                   Title = i.Title,
               }
               );
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override OrderKindGridItemModel SaveCreateModel(SecurityToken token, OrderKindCreateModel model, JGridSaveModelResult result)
        {
            var entity = new DataLayer.Entities.OrderKind
            {
                Title = model.Title,
                OrderKindID = model.Id
            };
            RemontinkaServer.Instance.EntitiesFacade.SaveOrderKind(token,entity);
            return new OrderKindGridItemModel
            {
                Id = entity.OrderKindID,
                Title = entity.Title,
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override OrderKindGridItemModel SaveEditModel(SecurityToken token, OrderKindCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }
    }
}