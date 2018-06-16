using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;
using Romontinka.Server.WebSite.Models.FinancialItemForm;

namespace Romontinka.Server.WebSite.Models.FinancialItemValueForm
{
    /// <summary>
    /// Дата адаптер для управления значениями статьями бюджета.
    /// </summary>
    public class FinancialItemValueDataAdapter : JGridDataAdapterBase<Guid, FinancialItemValueGridItemModel, FinancialItemValueCreateModel, FinancialItemValueCreateModel, FinancialItemValueSearchModel>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override FinancialItemValueCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetFinancialItemValue(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Значение статьи бюджета");

            return new FinancialItemValueCreateModel
            {
                Description = item.Description,
                Id = item.FinancialItemValueID,
                Amount = item.Amount,
                CostAmount = item.CostAmount,
                EventDate = item.EventDate,
                FinancialGroupID = item.FinancialGroupID,
                FinancialItemID = item.FinancialItemID
            };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override FinancialItemValueCreateModel CreateNewModel(SecurityToken token, FinancialItemValueSearchModel searchModel)
        {
            return new FinancialItemValueCreateModel
                   {
                       EventDate = DateTime.Today,
                       FinancialGroupID = searchModel.FinancialItemValueSearchFinancialGroupID
                   };
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteFinancialItemValue(token, entityId);
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<FinancialItemValueGridItemModel> GetPageableGridItems(SecurityToken token, FinancialItemValueSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return
                RemontinkaServer.Instance.EntitiesFacade.GetFinancialItemValues(token,
                                                                                searchModel.
                                                                                    FinancialItemValueSearchFinancialGroupID,
                                                                                searchModel.FinancialItemSearchName,
                                                                                searchModel.
                                                                                    FinancialItemValueSearchBeginDate,
                                                                                searchModel.
                                                                                    FinancialItemValueSearchEndDate,
                                                                                searchModel.Page, itemsPerPage,
                                                                                out totalCount).Select(CreateGridItem);
        }

        /// <summary>
        /// Создает модель пункта грида из сущности.
        /// </summary>
        private FinancialItemValueGridItemModel CreateGridItem(FinancialItemValueDTO entity)
        {
            return new FinancialItemValueGridItemModel
                   {
                       Id = entity.FinancialItemValueID,
                       FinancialItemTitle = entity.FinancialItemTitle,
                       Amount = Utils.DecimalToString(entity.Amount),
                       CostAmount = Utils.DecimalToString(entity.CostAmount),
                       EventDate = Utils.DateTimeToString(entity.EventDate),
                       Description = entity.Description??string.Empty,
                       RowClass = FinancialItemDataAdapter.GetTransactionKindRowClass(entity.TransactionKindID)
                   };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override FinancialItemValueGridItemModel SaveCreateModel(SecurityToken token, FinancialItemValueCreateModel model, JGridSaveModelResult result)
        {
            var entity = new FinancialItemValue
            {
                Description = model.Description??string.Empty,
                EventDate = model.EventDate,
                FinancialItemValueID = model.Id,
                Amount = model.Amount,
                CostAmount = model.CostAmount,
                FinancialGroupID = model.FinancialGroupID,
                FinancialItemID = model.FinancialItemID
            };
            RemontinkaServer.Instance.EntitiesFacade.SaveFinancialItemValue(token, entity);

            var entityDto = RemontinkaServer.Instance.EntitiesFacade.GetFinancialItemValue(token, entity.FinancialItemValueID);

            return CreateGridItem(entityDto);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override FinancialItemValueGridItemModel SaveEditModel(SecurityToken token, FinancialItemValueCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }
    }
}