using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.ContractorGridForm
{
    /// <summary>
    /// Сервис данных для контрагентов.
    /// </summary>
    public class ContractorGridDataAdapter : DataAdapterBase<Guid, ContractorGridModel, ContractorCreateModel, ContractorCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override ContractorGridModel CreateGridModel(SecurityToken token)
        {
            return new ContractorGridModel();
        }

        /// <summary>
        /// Получает данные для грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parentId">Код родительской записи.</param>
        /// <returns>Данные.</returns>
        public override IQueryable GedData(SecurityToken token, string parentId)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetContractors(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override ContractorCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new ContractorCreateModel { EventDate = DateTime.Today };
        }
       

        /// <summary>
        /// Инициализирует модель Обновления сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код связанной сущности.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель обновления.</returns>
        public override ContractorCreateModel CreateEditModel(SecurityToken token, Guid? entityId, ContractorGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetContractor(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Контрагент");

            return new ContractorCreateModel
            {
                Address = item.Address,
                ContractorID = item.ContractorID,
                EventDate = item.EventDate,
                LegalName = item.LegalName,
                Phone = item.Phone,
                Trademark = item.Trademark
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, ContractorCreateModel model, GridSaveModelResult result)
        {
            var entity = new Contractor
            {
                Address = model.Address,
                ContractorID = model.ContractorID,
                EventDate = model.EventDate,
                LegalName = model.LegalName,
                Phone = model.Phone,
                Trademark = model.Trademark

            };
            RemontinkaServer.Instance.EntitiesFacade.SaveContractor(token, entity);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, ContractorCreateModel model, GridSaveModelResult result)
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
            RemontinkaServer.Instance.EntitiesFacade.DeleteContractor(token, entityId);
        }
    }
}