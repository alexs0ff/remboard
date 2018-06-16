using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.TransferDocItemGridForm
{
    /// <summary>
    /// Сервис данных для документов перемещения между складами.
    /// </summary>
    public class TransferDocItemGridDataAdapter : DataAdapterBase<Guid, TransferDocItemGridModel, TransferDocItemCreateModel, TransferDocItemCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override TransferDocItemGridModel CreateGridModel(SecurityToken token)
        {
            return new TransferDocItemGridModel
            {
                GoodsItems = RemontinkaServer.Instance.EntitiesFacade.GetGoodsItems(token)
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
            return RemontinkaServer.Instance.EntitiesFacade.GetTransferDocItems(token, id);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override TransferDocItemCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new TransferDocItemCreateModel
            {
                TransferDocID = Guid.Parse(createParameters.ParentId)
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
        public override TransferDocItemCreateModel CreateEditModel(SecurityToken token, Guid? entityId, TransferDocItemGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetTransferDocItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Элемент документа о перемещении");

            return new TransferDocItemCreateModel
            {
                Description = item.Description,
                GoodsItemID = item.GoodsItemID,
                TransferDocItemID = item.TransferDocItemID,
                Total = item.Total,
                TransferDocID = item.TransferDocID
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, TransferDocItemCreateModel model, GridSaveModelResult result)
        {
            if (!RemontinkaServer.Instance.EntitiesFacade.WarehouseDocIsProcessed(token, model.TransferDocID))
            {
                var entity = new TransferDocItem
                {
                    Description = model.Description,
                    GoodsItemID = model.GoodsItemID,
                    Total = model.Total,
                    TransferDocID = model.TransferDocID,
                    TransferDocItemID = model.TransferDocItemID
                };

                RemontinkaServer.Instance.EntitiesFacade.SaveTransferDocItem(token, entity);
            }
            else
            {
                result.ModelErrors.Add(new PairItem<string, string>(string.Empty, "Документ уже обработан"));
            }
            
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, TransferDocItemCreateModel model, GridSaveModelResult result)
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
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetTransferDocItem(token, entityId);
            if (entity != null)
            {
                if (!RemontinkaServer.Instance.DataStore.WarehouseDocIsProcessed(entity.TransferDocID))
                {
                    RemontinkaServer.Instance.EntitiesFacade.DeleteTransferDocItem(token, entityId);
                } //if
                else
                {
                    throw new Exception("Документ уже обработан");
                } //else
            } //if
        }
    }
}