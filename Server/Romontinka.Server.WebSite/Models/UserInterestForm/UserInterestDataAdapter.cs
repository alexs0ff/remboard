using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;

namespace Romontinka.Server.WebSite.Models.UserInterestForm
{
    /// <summary>
    /// Сервис данных для вознаграждений пользователей.
    /// </summary>
    public class UserInterestDataAdapter : JGridDataAdapterBase<Guid, UserInterestGridItemModel, UserInterestCreateModel, UserInterestCreateModel, UserInterestSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override UserInterestCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetUserInterest(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Вознаграждение");

            return new UserInterestCreateModel
                   {
                       Description = item.Description,
                       DeviceInterestKindID = item.DeviceInterestKindID,
                       DeviceValue = item.DeviceValue,
                       EventDate = item.EventDate,
                       Id = item.UserInterestID,
                       UserID = item.UserID,
                       WorkInterestKindID = item.WorkInterestKindID,
                       WorkValue = item.WorkValue
                   };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override UserInterestCreateModel CreateNewModel(SecurityToken token, UserInterestSearchModel searchModel)
        {
            return new UserInterestCreateModel
                   {
                       EventDate = DateTime.Today,
                       DeviceInterestKindID = InterestKindSet.Empty.InterestKindID,
                       WorkInterestKindID = InterestKindSet.Empty.InterestKindID
                   };
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteUserInterest(token, entityId);
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<UserInterestGridItemModel> GetPageableGridItems(SecurityToken token, UserInterestSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetUserInterests(token,
                                                                                 searchModel.
                                                                                     UserInterestSearchTitle,
                                                                                 searchModel.Page, itemsPerPage,
                                                                                 out totalCount).Select(CreateUserInterestGridItemModel);
        }

        /// <summary>
        /// Создает модель пункта грида из объекта вознаграждения.
        /// </summary>
        /// <param name="item">Пункт грида.</param>
        private static UserInterestGridItemModel CreateUserInterestGridItemModel(UserInterestDTO item)
        {
            return new UserInterestGridItemModel
                   {
                       Description = item.Description,
                       Id = item.UserInterestID,
                       EventDate = Utils.DateTimeToString(item.EventDate),
                       UserFullName = Utils.GetPersonFullName(item.LastName,item.FirstName,item.MiddleName),
                       Values = string.Format("Запчасти {0}:{1}; Работа {2}:{3}",
                                              InterestKindSet.GetKindByID(item.DeviceInterestKindID).Title,Utils.DecimalToString(item.DeviceValue),
                                              InterestKindSet.GetKindByID(item.WorkInterestKindID).Title,Utils.DecimalToString(item.WorkValue)
                           )
                   };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override UserInterestGridItemModel SaveCreateModel(SecurityToken token, UserInterestCreateModel model, JGridSaveModelResult result)
        {
            var entity = new UserInterest
            {
               Description = model.Description,
               DeviceInterestKindID = model.DeviceInterestKindID,
               DeviceValue = model.DeviceValue,
               EventDate = model.EventDate,
               UserID = model.UserID,
               UserInterestID = model.Id,
               WorkInterestKindID = model.WorkInterestKindID,
               WorkValue = model.WorkValue
            };

            RemontinkaServer.Instance.EntitiesFacade.SaveUserInterest(token, entity);
            var dto = RemontinkaServer.Instance.EntitiesFacade.GetUserInterest(token, entity.UserInterestID);
            return CreateUserInterestGridItemModel(dto);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override UserInterestGridItemModel SaveEditModel(SecurityToken token, UserInterestCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }
    }
}