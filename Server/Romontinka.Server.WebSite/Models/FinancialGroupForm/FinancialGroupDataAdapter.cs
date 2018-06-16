using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.FinancialGroupForm
{
    /// <summary>
    /// Адаптер управления данными финансовых групп.
    /// </summary>
    public class FinancialGroupDataAdapter : JGridDataAdapterBase<Guid, FinancialGroupGridItemModel, FinancialGroupCreateModel, FinancialGroupCreateModel, FinancialGroupSearchModel>
    {
        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override FinancialGroupCreateModel CreateNewModel(SecurityToken token, FinancialGroupSearchModel searchModel)
        {
            var domain = RemontinkaServer.Instance.DataStore.GetUserDomain(token.User.UserDomainID);

            return new FinancialGroupCreateModel
                       {
                           LegalName = domain.LegalName,
                           Title = domain.LegalName,
                           Trademark = domain.Trademark
                       };
        }

        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override FinancialGroupCreateModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetFinancialGroupItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Финансовая группа");

            var model = new FinancialGroupCreateModel
            {
                
                Id = item.FinancialGroupID,
                Title = item.Title,
                LegalName = item.LegalName,
                Trademark = item.Trademark
            };

            model.BranchIds =
                RemontinkaServer.Instance.DataStore.GetFinancialGroupBranchMapItemsByFinancialGroup(
                    item.FinancialGroupID).Select(i => i.BranchID).ToArray();

            model.WarehouseIds =
                RemontinkaServer.Instance.DataStore.GetFinancialGroupWarehouseMapItemsByFinancialGroup(
                    item.FinancialGroupID).Select(i => i.WarehouseID).ToArray();

            return model;
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<FinancialGroupGridItemModel> GetPageableGridItems(SecurityToken token, FinancialGroupSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetFinancialGroupItems(token, searchModel.FinancialGroupName,
                                                                                   searchModel.Page, itemsPerPage,
                                                                                   out totalCount).
                Select(i => new FinancialGroupGridItemModel
                                {
                                    Id = i.FinancialGroupID,
                                    LegalName = i.LegalName,
                                    Title = i.Title
                                });
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override FinancialGroupGridItemModel SaveCreateModel(SecurityToken token, FinancialGroupCreateModel model, JGridSaveModelResult result)
        {
            var entity = new FinancialGroupItem
                             {
                                 FinancialGroupID = model.Id,
                                 Title = model.Title,
                                 LegalName = model.LegalName,
                                 Trademark = model.Trademark
                             };

            RemontinkaServer.Instance.EntitiesFacade.SaveFinancialGroupItem(token, entity);
            RemontinkaServer.Instance.DataStore.DeleteFinancialGroupBranchMapItems(entity.FinancialGroupID);
            
            foreach (var branchId in model.BranchIds)
            {
                RemontinkaServer.Instance.DataStore.SaveFinancialGroupMapBranchItem(new FinancialGroupBranchMapItem
                {
                    BranchID = branchId,
                    FinancialGroupID = entity.FinancialGroupID
                });
            }

            RemontinkaServer.Instance.DataStore.DeleteFinancialGroupWarehouseMapItems(entity.FinancialGroupID);

            if (model.WarehouseIds != null)
            {
                foreach (var warehouseID in model.WarehouseIds)
                {
                    RemontinkaServer.Instance.DataStore.SaveFinancialGroupMapWarehouseItem(new FinancialGroupWarehouseMapItem
                                                                                               {
                                                                                                   WarehouseID =
                                                                                                       warehouseID,
                                                                                                   FinancialGroupID =
                                                                                                       entity.
                                                                                                       FinancialGroupID
                                                                                               });
                }
            }

            return new FinancialGroupGridItemModel
                       {
                           Id = entity.FinancialGroupID,
                           LegalName = entity.LegalName,
                           Title = entity.Title
                       };
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override FinancialGroupGridItemModel SaveEditModel(SecurityToken token, FinancialGroupCreateModel model, JGridSaveModelResult result)
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
            var item = RemontinkaServer.Instance.EntitiesFacade.GetFinancialGroupItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Финансовая группа");
            RemontinkaServer.Instance.DataStore.DeleteFinancialGroupBranchMapItems(item.FinancialGroupID);
            RemontinkaServer.Instance.DataStore.DeleteFinancialGroupWarehouseMapItems(item.FinancialGroupID);
            RemontinkaServer.Instance.EntitiesFacade.DeleteFinancialGroupItem(token, item.FinancialGroupID);
        }
    }
}