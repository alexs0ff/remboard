using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core;
using Remontinka.Client.DataLayer.Entities;
using Remontinka.Client.Wpf.Controllers.Forms;
using Remontinka.Client.Wpf.Model;
using Remontinka.Client.Wpf.Model.Items;
using Remontinka.Client.Wpf.View;

namespace Remontinka.Client.Wpf.Controllers.Items
{
    /// <summary>
    /// Контроллер управления операциями с работами.
    /// </summary>
    public class WorkItemDataController : ModelEditControllerBase<WorkItemEditView, WorkItemEditView, WorkItemEditModel, WorkItemEditModel, Guid, Guid?>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parameters">Модель параметров.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override WorkItemEditModel CreateEditedModel(SecurityToken token, Guid entityId, Guid? parameters)
        {
            var entity = ClientCore.Instance.DataStore.GetWorkItem(entityId);

            RiseExceptionIfNotFound(entity,entityId,"Работа");

            return new WorkItemEditModel
                   {
                       Id = entity.WorkItemIDGuid,
                       RepairOrderID = entity.RepairOrderIDGuid,
                       WorkItemEventDate = entity.EventDateDateTime,
                       WorkItemPrice =   WpfUtils.DecimalToString((decimal?)entity.Price),
                       WorkItemTitle = entity.Title,
                       WorkItemUserID = entity.UserIDGuid
                   };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="parameters">Модель параметров.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override WorkItemEditModel CreateNewModel(SecurityToken token, Guid? parameters)
        {
            Guid? userID = null;
            if (token.User.ProjectRoleID == ProjectRoleSet.Engineer.ProjectRoleID)
            {
                userID = token.User.UserIDGuid;
            } //if

            return new WorkItemEditModel
                   {
                       RepairOrderID = parameters,
                       WorkItemUserID = userID,
                       WorkItemEventDate = DateTime.Today
                   };
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, WorkItemEditModel model, SaveModelResult result)
        {
            SaveModel(token, model, result, false);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, WorkItemEditModel model, SaveModelResult result)
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
        private void SaveModel(SecurityToken token, WorkItemEditModel model, SaveModelResult result, bool isEdit)
        {
            var entity = new WorkItem
            {
                EventDateDateTime = model.WorkItemEventDate,
                Price = (double?)WpfUtils.StringToDecimal(model.WorkItemPrice)??0,
                RepairOrderIDGuid = model.RepairOrderID,
                Title = model.WorkItemTitle,
                UserIDGuid = model.WorkItemUserID,
                WorkItemIDGuid = model.Id
            };
            WorkItem oldItem = null;

            if (isEdit)
            {
                oldItem = ClientCore.Instance.DataStore.GetWorkItem(entity.WorkItemIDGuid);
            } //if

            ClientCore.Instance.DataStore.SaveWorkItem(entity);

            var savedItem = ClientCore.Instance.DataStore.GetWorkItem(entity.WorkItemIDGuid);
            RiseExceptionIfNotFound(savedItem, entity.WorkItemIDGuid, "Пункт выполненных работ");

            if (!isEdit)
            {
                model.Id = entity.WorkItemIDGuid;
                ClientCore.Instance.OrderTimelineManager.TrackNewWorkItem(token, savedItem);
            } //if

            if (oldItem != null)
            {
                ClientCore.Instance.OrderTimelineManager.TrackWorkItemChanges(token, oldItem, savedItem);
            } //if
        }
    }
}
