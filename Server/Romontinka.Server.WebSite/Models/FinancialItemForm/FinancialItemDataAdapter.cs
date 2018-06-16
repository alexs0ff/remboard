using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.FinancialItemForm
{
    /// <summary>
    /// Адаптер управления статьями бюджета.
    /// </summary>
    public class FinancialItemDataAdapter : JGridDataAdapterBase<Guid, FinancialItemGridItemModel, FinancialItemCreateModel, FinancialItemCreateModel, FinancialItemSearchModel>
    {
        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override FinancialItemCreateModel CreateNewModel(SecurityToken token, FinancialItemSearchModel searchModel)
        {
            return new FinancialItemCreateModel
                       {
                           EventDate = DateTime.Today,
                           FinancialItemKindID = FinancialItemKindSet.Custom.FinancialItemKindID
                       };
        }

        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override FinancialItemCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetFinancialItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Статья бюджета");

            return new FinancialItemCreateModel
            {
                Description = item.Description,
                Id = item.FinancialItemID,
                Title = item.Title,
                EventDate = item.EventDate,
                FinancialItemKindID = item.FinancialItemKindID,
                TransactionKindID = item.TransactionKindID
            };
        }

        /// <summary>
        /// Получает класс цвета для грида по коду типа транзакции.
        /// </summary>
        /// <param name="transactionKindID">Код транзакции.</param>
        /// <returns>Название класса.</returns>
        public static string GetTransactionKindRowClass(byte? transactionKindID)
        {
            return transactionKindID == TransactionKindSet.Revenue.TransactionKindID ? GridRowColors.Success : GridRowColors.Warning;
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<FinancialItemGridItemModel> GetPageableGridItems(SecurityToken token, FinancialItemSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetFinancialItems(token, searchModel.FinancialItemName, searchModel.Page, itemsPerPage, out totalCount).Select(
                i => new FinancialItemGridItemModel
                {
                    Id = i.FinancialItemID,
                    Title = i.Title,
                    TransactionKindTitle = TransactionKindSet.GetKindByID(i.TransactionKindID).Title,
                    RowClass = GetTransactionKindRowClass(i.TransactionKindID)
                }
                );
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override FinancialItemGridItemModel SaveCreateModel(SecurityToken token, FinancialItemCreateModel model, JGridSaveModelResult result)
        {
            var entity = new FinancialItem
            {
                Description = model.Description,
                EventDate = model.EventDate,
                FinancialItemID = model.Id,
                FinancialItemKindID = model.FinancialItemKindID,
                Title = model.Title,
                TransactionKindID = model.TransactionKindID
            };
            RemontinkaServer.Instance.EntitiesFacade.SaveFinancialItem(token, entity);
            return new FinancialItemGridItemModel
            {
                Id = entity.FinancialItemID,
                Title = entity.Title,
                TransactionKindTitle = TransactionKindSet.GetKindByID(entity.TransactionKindID).Title,
                RowClass = GetTransactionKindRowClass(entity.TransactionKindID)
            };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override FinancialItemGridItemModel SaveEditModel(SecurityToken token, FinancialItemCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateModel(token, model, result);
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteFinancialItem(token, entityId);
        }
    }
}