using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Helpers;
using Remontinka.Server.WebPortal.Models.Common;
using Remontinka.Server.WebPortal.Services;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.WorkItemGridForm
{
    /// <summary>
    /// Менеджер данных для выполненных работ.
    /// </summary>
    public class WorkItemGridDataAdapter : DataAdapterBase<Guid, WorkItemGridModel, WorkItemCreateModel, WorkItemCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override WorkItemGridModel CreateGridModel(SecurityToken token)
        {
            return new WorkItemGridModel
            {
                Engineers = RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().GetUserList(token, ProjectRoleSet.Engineer.ProjectRoleID.Value)
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
            var id = Guid.Parse(parentId);
            return RemontinkaServer.Instance.EntitiesFacade.GetWorkItems(token, id);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override WorkItemCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            Guid? userID = null;
            if (token.User.ProjectRoleID == ProjectRoleSet.Engineer.ProjectRoleID)
            {
                userID = token.User.UserID;
            } //if

            return new WorkItemCreateModel
            {
                RepairOrderID = Guid.Parse(createParameters.ParentId),
                WorkItemUserID = userID
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
        public override WorkItemCreateModel CreateEditModel(SecurityToken token, Guid? entityId, WorkItemGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetWorkItem(token, entityId);
            RiseExceptionIfNotFound(entity, entityId, "Пункт выполненных работ");

            if (entity.UserID != null && gridModel.Engineers.All(i => i.Value != entity.UserID))
            {
                gridModel.Engineers.Add(new SelectListItem<Guid> { Value = entity.UserID, Text = entity.EngineerFullName });
            }

            return new WorkItemCreateModel
            {
                WorkItemEventDate = entity.EventDate,
                WorkItemID = entity.WorkItemID,
                RepairOrderID = entity.RepairOrderID,
                WorkItemPrice = entity.Price,
                WorkItemTitle = entity.Title,
                WorkItemUserID = entity.UserID,
                WorkItemNotes = entity.Notes
            };


        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, WorkItemCreateModel model, GridSaveModelResult result)
        {
            SaveModel(token, model, result, false);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, WorkItemCreateModel model, GridSaveModelResult result)
        {
            SaveModel(token, model, result, true);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        /// <param name="isEdit">Признак редактирования модели. </param>
        private void SaveModel(SecurityToken token, WorkItemCreateModel model, GridSaveModelResult result, bool isEdit)
        {
            var entity = new WorkItem
            {
                EventDate = model.WorkItemEventDate,
                Price = model.WorkItemPrice,
                RepairOrderID = model.RepairOrderID,
                Title = model.WorkItemTitle,
                UserID = model.WorkItemUserID,
                WorkItemID = model.WorkItemID,
                Notes = model.WorkItemNotes
            };
            WorkItem oldItem = null;

            if (isEdit)
            {
                oldItem = RemontinkaServer.Instance.DataStore.GetWorkItem(entity.WorkItemID);
            } //if

            RemontinkaServer.Instance.EntitiesFacade.SaveWorkItem(token, entity);

            var savedItem = RemontinkaServer.Instance.DataStore.GetWorkItem(entity.WorkItemID);
            RiseExceptionIfNotFound(savedItem, entity.WorkItemID, "Пункт выполненных работ");

            if (!isEdit)
            {
                RemontinkaServer.Instance.OrderTimelineManager.TrackNewWorkItem(token, savedItem);
            } //if

            if (oldItem != null)
            {
                RemontinkaServer.Instance.OrderTimelineManager.TrackWorkItemChanges(token, oldItem, savedItem);
            } //if
            
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid? entityId)
        {
            RemontinkaServer.Instance.OrderTimelineManager.TrackWorkItemDelete(token, entityId);
            RemontinkaServer.Instance.EntitiesFacade.DeleteWorkItem(token, entityId);
        }
    }
}