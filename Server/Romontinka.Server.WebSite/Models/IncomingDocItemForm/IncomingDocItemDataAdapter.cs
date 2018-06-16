using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;

namespace Romontinka.Server.WebSite.Models.IncomingDocItemForm
{
    /// <summary>
    /// Адаптер данных для элементов прикладных накладных.
    /// </summary>
    public class IncomingDocItemDataAdapter : JGridDataAdapterBase<Guid, IncomingDocItemGridItemModel, IncomingDocItemCreateModel, IncomingDocItemCreateModel, IncomingDocItemSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override IncomingDocItemCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetIncomingDocItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Элемент накладной");

            return new IncomingDocItemCreateModel
            {
                Description = item.Description,
                GoodsItemID = item.GoodsItemID,
                Id = item.IncomingDocItemID,
                IncomingDocID = item.IncomingDocID,
                InitPrice = item.InitPrice,
                RepairPrice = item.RepairPrice,
                SalePrice = item.SalePrice,
                StartPrice = item.StartPrice,
                Total = item.Total
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override IncomingDocItemCreateModel CreateNewModel(SecurityToken token, IncomingDocItemSearchModel searchModel)
        {
            return new IncomingDocItemCreateModel
                   {
                       IncomingDocID = searchModel.IncomingDocItemDocID
                   };
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetIncomingDocItem(token, entityId);
            if (entity!=null)
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

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<IncomingDocItemGridItemModel> GetPageableGridItems(SecurityToken token, IncomingDocItemSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return
                RemontinkaServer.Instance.EntitiesFacade.GetIncomingDocItems(token, searchModel.IncomingDocItemDocID,
                                                                             searchModel.IncomingDocItemName,
                                                                             searchModel.Page, itemsPerPage,
                                                                             out totalCount).
                    Select(CreateModel);
        }

        /// <summary>
        /// Создает модель пункта грида из сущности.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <returns>Модель пункта грида.</returns>
        private IncomingDocItemGridItemModel CreateModel(IncomingDocItemDTO entity)
        {
            return new IncomingDocItemGridItemModel
                   {
                       Description = entity.Description??string.Empty,
                       GoodsItemTitle = entity.GoodsItemTitle,
                       Id = entity.IncomingDocItemID,
                       InitPrice = Utils.DecimalToString(entity.InitPrice),
                       RepairPrice = Utils.DecimalToString(entity.RepairPrice),
                       SalePrice = Utils.DecimalToString(entity.SalePrice),
                       StartPrice = Utils.DecimalToString(entity.StartPrice),
                       Total = Utils.DecimalToString(entity.Total)
                   };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override IncomingDocItemGridItemModel SaveCreateModel(SecurityToken token, IncomingDocItemCreateModel model, JGridSaveModelResult result)
        {
            if (!RemontinkaServer.Instance.EntitiesFacade.WarehouseDocIsProcessed(token, model.IncomingDocID))
            {
                var entity = new IncomingDocItem
                {
                    Description = model.Description,
                    GoodsItemID = model.GoodsItemID,
                    IncomingDocID = model.IncomingDocID,
                    IncomingDocItemID = model.Id,
                    InitPrice = model.InitPrice,
                    RepairPrice = model.RepairPrice,
                    SalePrice = model.SalePrice,
                    StartPrice = model.StartPrice,
                    Total = model.Total
                };

                RemontinkaServer.Instance.EntitiesFacade.SaveIncomingDocItem(token, entity);
                return
                    CreateModel(RemontinkaServer.Instance.EntitiesFacade.GetIncomingDocItem(token,
                                                                                            entity.IncomingDocItemID));
            }
            
            result.ModelErrors.Add(new PairItem<string, string>(string.Empty, "Приходная накладная уже обработана"));
            
            return null;
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override IncomingDocItemGridItemModel SaveEditModel(SecurityToken token, IncomingDocItemCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }
    }
}