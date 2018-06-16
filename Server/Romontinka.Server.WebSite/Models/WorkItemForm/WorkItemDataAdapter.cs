using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;

namespace Romontinka.Server.WebSite.Models.WorkItemForm
{
    /// <summary>
    /// Адаптер для работы с данными по выполненным работам.
    /// </summary>
    public class WorkItemDataAdapter : JGridDataAdapterBase<Guid, WorkItemGridItemModel, WorkItemCreateModel, WorkItemCreateModel, WorkItemSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override WorkItemCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetWorkItem(token, entityId);
            RiseExceptionIfNotFound(entity,entityId,"Пункт выполненных работ");
            return new WorkItemCreateModel
                   {
                       WorkItemEventDate = entity.EventDate,
                       Id = entity.WorkItemID,
                       RepairOrderID = entity.RepairOrderID,
                       WorkItemPrice = entity.Price,
                       WorkItemTitle = entity.Title,
                       WorkItemUserID = entity.UserID,
                       WorkItemNotes = entity.Notes
                   };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override WorkItemCreateModel CreateNewModel(SecurityToken token, WorkItemSearchModel searchModel)
        {
            Guid? userID = null;
            if (token.User.ProjectRoleID == ProjectRoleSet.Engineer.ProjectRoleID)
            {
                userID = token.User.UserID;
            } //if

            return new WorkItemCreateModel
                   {
                       RepairOrderID = searchModel.WorkItemRepairOrderID,
                       WorkItemUserID = userID
                   };
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            RemontinkaServer.Instance.OrderTimelineManager.TrackWorkItemDelete(token, entityId);
            RemontinkaServer.Instance.EntitiesFacade.DeleteWorkItem(token,entityId);
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<WorkItemGridItemModel> GetPageableGridItems(SecurityToken token, WorkItemSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetWorkItems(token, searchModel.WorkItemRepairOrderID,
                                                                         searchModel.WorkItemName, searchModel.Page,
                                                                         itemsPerPage, out totalCount).Select(CreateItem);
        }

        private WorkItemGridItemModel CreateItem(WorkItemDTO entity)
        {
            return new WorkItemGridItemModel
                   {
                       WorkItemEngineerFullName = entity.EngineerFullName,
                       WorkItemEventDate = Utils.DateTimeToString(entity.EventDate),
                       WorkItemPrice = Utils.DecimalToString(entity.Price),
                       Id = entity.WorkItemID,
                       WorkItemTitle = entity.Title,
                   };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override WorkItemGridItemModel SaveCreateModel(SecurityToken token, WorkItemCreateModel model, JGridSaveModelResult result)
        {
            return SaveModel(token, model, result, false);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        /// <param name="isEdit">Признак редактирования модели. </param>
        private WorkItemGridItemModel SaveModel(SecurityToken token, WorkItemCreateModel model, JGridSaveModelResult result, bool isEdit)
        {
            var entity = new WorkItem
                         {
                             EventDate = model.WorkItemEventDate,
                             Price = model.WorkItemPrice,
                             RepairOrderID = model.RepairOrderID,
                             Title = model.WorkItemTitle,
                             UserID = model.WorkItemUserID,
                             WorkItemID = model.Id,
                             Notes = model.WorkItemNotes
                         };
            WorkItem oldItem = null;

            if (isEdit)
            {
                oldItem = RemontinkaServer.Instance.DataStore.GetWorkItem(entity.WorkItemID);
            } //if

            RemontinkaServer.Instance.EntitiesFacade.SaveWorkItem(token,entity);

            var savedItem = RemontinkaServer.Instance.DataStore.GetWorkItem(entity.WorkItemID);
            RiseExceptionIfNotFound(savedItem,entity.WorkItemID,"Пункт выполненных работ");

            if (!isEdit)
            {
                RemontinkaServer.Instance.OrderTimelineManager.TrackNewWorkItem(token,savedItem);
            } //if

            if (oldItem!=null)
            {
                RemontinkaServer.Instance.OrderTimelineManager.TrackWorkItemChanges(token, oldItem, savedItem);
            } //if

            return CreateItem(savedItem);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override WorkItemGridItemModel SaveEditModel(SecurityToken token, WorkItemCreateModel model, JGridSaveModelResult result)
        {
            return SaveModel(token, model, result, true);
        }
    }
}