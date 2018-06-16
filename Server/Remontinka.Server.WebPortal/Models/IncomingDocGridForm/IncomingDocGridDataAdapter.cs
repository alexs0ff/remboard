using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.IncomingDocGridForm
{
    /// <summary>
    /// Сервис данных по приходным накладным.
    /// </summary>
    public class IncomingDocGridDataAdapter : DataAdapterBase<Guid, IncomingDocGridModel, IncomingDocCreateModel, IncomingDocCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override IncomingDocGridModel CreateGridModel(SecurityToken token)
        {
            return new IncomingDocGridModel
            {
                Contractors = RemontinkaServer.Instance.EntitiesFacade.GetContractors(token),
                Warehouses = RemontinkaServer.Instance.EntitiesFacade.GetWarehouses(token)
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
            return RemontinkaServer.Instance.EntitiesFacade.GetIncomingDocs(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override IncomingDocCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new IncomingDocCreateModel
            {
                DocDate = DateTime.Today
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
        public override IncomingDocCreateModel CreateEditModel(SecurityToken token, Guid? entityId, IncomingDocGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetIncomingDoc(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Приходная накладная");

            return new IncomingDocCreateModel
            {
                ContractorID = item.ContractorID,
                DocDate = item.DocDate,
                DocDescription = item.DocDescription,
                DocNumber = item.DocNumber,
                IncomingDocID = item.IncomingDocID,
                WarehouseID = item.WarehouseID
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, IncomingDocCreateModel model, GridSaveModelResult result)
        {
            var entity = new IncomingDoc
            {
                ContractorID = model.ContractorID,
                CreatorID = token.User.UserID,
                DocDate = model.DocDate,
                DocDescription = model.DocDescription,
                DocNumber = model.DocNumber,
                IncomingDocID = model.IncomingDocID,
                WarehouseID = model.WarehouseID,

            };

            RemontinkaServer.Instance.EntitiesFacade.SaveIncomingDoc(token, entity);

            var item = RemontinkaServer.Instance.EntitiesFacade.GetIncomingDoc(token, entity.IncomingDocID);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, IncomingDocCreateModel model, GridSaveModelResult result)
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
            if (!RemontinkaServer.Instance.EntitiesFacade.WarehouseDocIsProcessed(token, entityId))
            {
                RemontinkaServer.Instance.EntitiesFacade.DeleteIncomingDoc(token, entityId);
            } //if
            else
            {
                throw new Exception("Документ уже обработан");
            } //else
        }
    }
}