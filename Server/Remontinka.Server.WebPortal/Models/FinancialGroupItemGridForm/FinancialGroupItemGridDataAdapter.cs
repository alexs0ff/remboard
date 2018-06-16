using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;


namespace Remontinka.Server.WebPortal.Models.FinancialGroupItemGridForm
{
    /// <summary>
    /// Сервис данных для финансовых групп филиалов.
    /// </summary>
    public class FinancialGroupItemGridDataAdapter : DataAdapterBase<Guid, FinancialGroupItemGridModel, FinancialGroupItemCreateModel, FinancialGroupItemCreateModel>
    {
        /// <summary>
        /// Переопределяется для обработки несвязанных полей для формы редактирования сущности.
        /// </summary>
        /// <param name="editModel">Модель редактирования.</param>
        /// <param name="parentId">Код родительской сущности.</param>
        /// <param name="modelState">Состояние модели.</param>
        public override void ProcessEditUnboundItems(FinancialGroupItemCreateModel editModel, string parentId, ModelStateDictionary modelState)
        {
            editModel.BranchIds = CheckBranchIds(modelState);
            editModel.WarehouseIds = CheckBoxListExtension.GetSelectedValues<Guid?>(FinancialGroupItemCreateModel.WarehouseIdsCheckListPropertyName);
        }

        /// <summary>
        /// Проверяет филиалы для пользователя.
        /// </summary>
        /// <param name="modelState">Состояние модели.</param>
        /// <returns></returns>
        private static Guid?[] CheckBranchIds(ModelStateDictionary modelState)
        {
            Guid?[] ids = CheckBoxListExtension.GetSelectedValues<Guid?>(FinancialGroupItemCreateModel.BranchIdsCheckListPropertyName);

            if (!ids.Any())
            {
                modelState.AddModelError(FinancialGroupItemCreateModel.BranchIdsCheckListPropertyName, "Необходим хотя бы один филиал");
            }
            return ids;
        }

        /// <summary>
        /// Переопределяется для обработки несвязанных полей для формы создания сущности.
        /// </summary>
        /// <param name="createModel">Модель создания.</param>
        /// <param name="parentId">Код родительской сущности.</param>
        /// <param name="modelState">Состояние модели.</param>
        public override void ProcessCreateUnboundItems(FinancialGroupItemCreateModel createModel, string parentId, ModelStateDictionary modelState)
        {
            ProcessEditUnboundItems(createModel, parentId, modelState);
        }

        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override FinancialGroupItemGridModel CreateGridModel(SecurityToken token)
        {
            return new FinancialGroupItemGridModel
            {
                Branches = RemontinkaServer.Instance.EntitiesFacade.GetBranches(token),
                Warehouses = RemontinkaServer.Instance.EntitiesFacade.GetWarehouses(token),
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
            return RemontinkaServer.Instance.EntitiesFacade.GetFinancialGroupItems(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override FinancialGroupItemCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            var domain = RemontinkaServer.Instance.DataStore.GetUserDomain(token.User.UserDomainID);

            return new FinancialGroupItemCreateModel
            {
                LegalName = domain.LegalName,
                Title = domain.LegalName,
                Trademark = domain.Trademark
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
        public override FinancialGroupItemCreateModel CreateEditModel(SecurityToken token, Guid? entityId, FinancialGroupItemGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetFinancialGroupItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Финансовая группа");

            var model = new FinancialGroupItemCreateModel
            {

                FinancialGroupID = item.FinancialGroupID,
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
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, FinancialGroupItemCreateModel model, GridSaveModelResult result)
        {
            var entity = new FinancialGroupItem
            {
                FinancialGroupID = model.FinancialGroupID,
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
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, FinancialGroupItemCreateModel model, GridSaveModelResult result)
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
            var item = RemontinkaServer.Instance.EntitiesFacade.GetFinancialGroupItem(token, entityId);

            RiseExceptionIfNotFound(item, entityId, "Финансовая группа");
            RemontinkaServer.Instance.DataStore.DeleteFinancialGroupBranchMapItems(item.FinancialGroupID);
            RemontinkaServer.Instance.DataStore.DeleteFinancialGroupWarehouseMapItems(item.FinancialGroupID);
            RemontinkaServer.Instance.EntitiesFacade.DeleteFinancialGroupItem(token, item.FinancialGroupID);
        }
    }
}