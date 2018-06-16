using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.CancellationDocItemGridForm
{
    /// <summary>
    /// Сервис данных для пунктов документа списания.
    /// </summary>
    public class CancellationDocItemGridDataAdapter : DataAdapterBase<Guid, CancellationDocItemGridModel, CancellationDocItemCreateModel, CancellationDocItemCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override CancellationDocItemGridModel CreateGridModel(SecurityToken token)
        {
            return new CancellationDocItemGridModel
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
            return RemontinkaServer.Instance.EntitiesFacade.GetCancellationDocItems(token,id);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override CancellationDocItemCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new CancellationDocItemCreateModel
            {
                CancellationDocID = Guid.Parse(createParameters.ParentId)
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
        public override CancellationDocItemCreateModel CreateEditModel(SecurityToken token, Guid? entityId,
            CancellationDocItemGridModel gridModel, GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetCancellationDocItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Элемент документа о списании");

            return new CancellationDocItemCreateModel
            {
                Description = item.Description,
                GoodsItemID = item.GoodsItemID,
                CancellationDocItemID = item.CancellationDocItemID,
                Total = item.Total,
                CancellationDocID = item.CancellationDocID
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, CancellationDocItemCreateModel model, GridSaveModelResult result)
        {
            if (!RemontinkaServer.Instance.EntitiesFacade.WarehouseDocIsProcessed(token, model.CancellationDocID))
            {
                var entity = new CancellationDocItem
                {
                    Description = model.Description,
                    GoodsItemID = model.GoodsItemID,
                    Total = model.Total,
                    CancellationDocID = model.CancellationDocID,
                    CancellationDocItemID = model.CancellationDocItemID
                };

                RemontinkaServer.Instance.EntitiesFacade.SaveCancellationDocItem(token, entity);
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
        public override void SaveEditModel(SecurityToken token, CancellationDocItemCreateModel model, GridSaveModelResult result)
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
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetCancellationDocItem(token, entityId);
            if (entity != null)
            {
                if (!RemontinkaServer.Instance.DataStore.WarehouseDocIsProcessed(entity.CancellationDocID))
                {
                    RemontinkaServer.Instance.EntitiesFacade.DeleteCancellationDocItem(token, entityId);
                } //if
                else
                {
                    throw new Exception("Документ уже обработан");
                } //else
            } //if
        }
    }
}