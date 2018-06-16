using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.CancellationDocGridForm
{
    /// <summary>
    /// Сервис данных для документов списания.
    /// </summary>
    public class CancellationDocGridDataAdapter : DataAdapterBase<Guid, CancellationDocGridModel, CancellationDocCreateModel, CancellationDocCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override CancellationDocGridModel CreateGridModel(SecurityToken token)
        {
            return new CancellationDocGridModel { Warehouses = RemontinkaServer.Instance.EntitiesFacade.GetWarehouses(token) };
        }

        /// <summary>
        /// Получает данные для грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parentId">Код родительской записи.</param>
        /// <returns>Данные.</returns>
        public override IQueryable GedData(SecurityToken token, string parentId)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetCancellationDocs(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override CancellationDocCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new CancellationDocCreateModel
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
        public override CancellationDocCreateModel CreateEditModel(SecurityToken token, Guid? entityId, CancellationDocGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetCancellationDoc(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Документ списания");

            return new CancellationDocCreateModel
            {
                DocDate = item.DocDate,
                DocDescription = item.DocDescription,
                DocNumber = item.DocNumber,
                CancellationDocID = item.CancellationDocID,
                WarehouseID = item.WarehouseID,
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, CancellationDocCreateModel model, GridSaveModelResult result)
        {
            var entity = new CancellationDoc
            {
                CreatorID = token.User.UserID,
                DocDate = model.DocDate,
                DocDescription = model.DocDescription,
                DocNumber = model.DocNumber,
                WarehouseID = model.WarehouseID,
                CancellationDocID = model.CancellationDocID

            };

            RemontinkaServer.Instance.EntitiesFacade.SaveCancellationDoc(token, entity);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, CancellationDocCreateModel model, GridSaveModelResult result)
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
                RemontinkaServer.Instance.EntitiesFacade.DeleteCancellationDoc(token, entityId);
            } //if
            else
            {
                throw new Exception("Документ уже обработан");
            } //else
        }
    }
}