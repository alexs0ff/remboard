using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.ContractorForm
{
    /// <summary>
    /// Адаптер для грида контрагентов.
    /// </summary>
    public class ContractorDataAdapter : JGridDataAdapterBase<Guid, ContractorGridItemModel, ContractorCreateModel, ContractorCreateModel, ContractorSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override ContractorCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetContractor(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Контрагент");

            return new ContractorCreateModel
            {
                Address = item.Address,
                Id = item.ContractorID,
                EventDate = item.EventDate,
                LegalName = item.LegalName,
                Phone = item.Phone,
                Trademark = item.Trademark
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override ContractorCreateModel CreateNewModel(SecurityToken token, ContractorSearchModel searchModel)
        {
            return new ContractorCreateModel {EventDate = DateTime.Today};
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteContractor(token, entityId);
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<ContractorGridItemModel> GetPageableGridItems(SecurityToken token, ContractorSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetContractors(token, searchModel.ContractorName, searchModel.Page, itemsPerPage, out totalCount).Select(
                i => new ContractorGridItemModel
                {
                    Address = i.Address,
                    Id = i.ContractorID,
                    LegalName = i.LegalName,
                    Phone = i.Phone,
                   }
                );
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override ContractorGridItemModel SaveCreateModel(SecurityToken token, ContractorCreateModel model, JGridSaveModelResult result)
        {
            var entity = new Contractor
            {
                Address = model.Address,
                ContractorID = model.Id,
                EventDate = model.EventDate,
                LegalName = model.LegalName,
                Phone = model.Phone,
                Trademark = model.Trademark
                
            };
            RemontinkaServer.Instance.EntitiesFacade.SaveContractor(token, entity);
            return new ContractorGridItemModel
            {
                Address = entity.Address,
                Id = entity.ContractorID,
                LegalName = entity.LegalName,
                Phone = entity.Phone,
                
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override ContractorGridItemModel SaveEditModel(SecurityToken token, ContractorCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }
    }
}