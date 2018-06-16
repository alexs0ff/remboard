using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.IncomingDocItemGridForm
{
    /// <summary>
    /// Сервис данных для приходных накладных.
    /// </summary>
    public class IncomingDocItemGridDataAdapter : DataAdapterBase<Guid, IncomingDocItemGridModel, IncomingDocItemCreateModel, IncomingDocItemCreateModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override IncomingDocItemGridModel CreateGridModel(SecurityToken token)
        {
            return new IncomingDocItemGridModel
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
            return RemontinkaServer.Instance.EntitiesFacade.GetIncomingDocItems(token,id);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override IncomingDocItemCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            return new IncomingDocItemCreateModel
            {
                IncomingDocID = Guid.Parse(createParameters.ParentId)
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
        public override IncomingDocItemCreateModel CreateEditModel(SecurityToken token, Guid? entityId, IncomingDocItemGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetIncomingDocItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Элемент накладной");

            return new IncomingDocItemCreateModel
            {
                Description = item.Description,
                GoodsItemID = item.GoodsItemID,
                IncomingDocItemID = item.IncomingDocItemID,
                IncomingDocID = item.IncomingDocID,
                InitPrice = item.InitPrice,
                RepairPrice = item.RepairPrice,
                SalePrice = item.SalePrice,
                StartPrice = item.StartPrice,
                Total = item.Total
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, IncomingDocItemCreateModel model, GridSaveModelResult result)
        {
            if (!RemontinkaServer.Instance.EntitiesFacade.WarehouseDocIsProcessed(token, model.IncomingDocID))
            {
                var entity = new IncomingDocItem
                {
                    Description = model.Description,
                    GoodsItemID = model.GoodsItemID,
                    IncomingDocID = model.IncomingDocID,
                    IncomingDocItemID = model.IncomingDocItemID,
                    InitPrice = model.InitPrice,
                    RepairPrice = model.RepairPrice,
                    SalePrice = model.SalePrice,
                    StartPrice = model.StartPrice,
                    Total = model.Total
                };

                RemontinkaServer.Instance.EntitiesFacade.SaveIncomingDocItem(token, entity);
            }
            else
            {
                result.ModelErrors.Add(new PairItem<string, string>(string.Empty, "Приходная накладная уже обработана"));
            }
            
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, IncomingDocItemCreateModel model, GridSaveModelResult result)
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
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetIncomingDocItem(token, entityId);
            if (entity != null)
            {
                if (!RemontinkaServer.Instance.DataStore.WarehouseDocIsProcessed(entity.IncomingDocID))
                {
                    RemontinkaServer.Instance.EntitiesFacade.DeleteIncomingDocItem(token, entityId);
                } //if
                else
                {
                    throw new Exception("Документ уже обработан");
                } //else
            } //if
        }
    }
}