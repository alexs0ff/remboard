using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using DevExpress.Data;
using DevExpress.Web.Mvc;
using Remontinka.Server.WebPortal.Helpers;
using Remontinka.Server.WebPortal.Models.Common;
using Remontinka.Server.WebPortal.Services;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.RepairOrderGridForm
{
    /// <summary>
    /// Адаптер для управления данными для грида заказов.
    /// </summary>
    public class RepairOrderGridDataAdapter: DataAdapterBase<Guid, RepairOrderGridModel,RepairOrderCreateModel,RepairOrderEditModel>
    {
        /// <summary>
        /// Создает и инициализирует модель грида.
        /// </summary>
        /// <returns>Инициализированная модель грида.</returns>
        public override RepairOrderGridModel CreateGridModel(SecurityToken token)
        {
            var result = new RepairOrderGridModel();
            result.Branches = RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().GetBranches(token);
            result.Engineers = RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().GetUserList(token, ProjectRoleSet.Engineer.ProjectRoleID.Value);
            result.Managers = RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().GetUserList(token, ProjectRoleSet.Manager.ProjectRoleID.Value);
            result.OrderKinds = RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().GetOrderKinds(token);
            result.OrderStatuses = RemontinkaServer.Instance.GetService<IWebSiteSettingsService>().GetOrderStatuses(token);
            result.StatusKinds = StatusKindSet.Statuses;
            return result;
        }

        /// <summary>
        /// Получает данные для грида.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parentId">Код родительской записи.</param>
        /// <returns>Данные.</returns>
        public override IQueryable GedData(SecurityToken token, string parentId)
        {
            return RemontinkaServer.Instance.EntitiesFacade.GetRepairOrders(token);
        }

        /// <summary>
        /// Инициализирует модель создания сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель создания.</returns>
        public override RepairOrderCreateModel CreateNewModel(SecurityToken token, GridCreateParameters createParameters)
        {
            var copyRepairOrderIDRaw = HttpContext.Current.Request.Params["RepairOrderCopyID"];

            
            
            var item = new RepairOrderCreateModel();
            item.Number =
                RemontinkaServer.Instance.EntitiesFacade.GetNewOrderNumber(token).ToString(CultureInfo.InvariantCulture);
            item.CallEventDate = DateTime.Now;
            item.DateOfBeReady = DateTime.Today;

            if (!string.IsNullOrWhiteSpace(copyRepairOrderIDRaw))
            {
                Guid repairOrderID;

                if (Guid.TryParse(copyRepairOrderIDRaw, out repairOrderID))
                {
                    var savedOrder = RemontinkaServer.Instance.DataStore.GetRepairOrder(repairOrderID,
                                                                                    token.User.UserDomainID);
                    if (savedOrder != null && RemontinkaServer.Instance.EntitiesFacade.UserHasAccessToRepairOrder(token.User.UserID, savedOrder, token.User.ProjectRoleID))
                    {
                        item.BranchID = savedOrder.BranchID;
                        item.ClientAddress = savedOrder.ClientAddress;
                        item.ClientEmail = savedOrder.ClientEmail;
                        item.ClientFullName = savedOrder.ClientFullName;
                        item.ClientPhone = savedOrder.ClientPhone;
                        item.Defect = savedOrder.Defect;
                        item.DeviceAppearance = AutocompleteHelper.SplitField(savedOrder.DeviceAppearance);
                        item.DeviceModel = savedOrder.DeviceModel;
                        item.DeviceSN = savedOrder.DeviceSN;
                        item.DeviceTitle = savedOrder.DeviceTitle;
                        item.DeviceTrademark = AutocompleteHelper.SplitField(savedOrder.DeviceTrademark);
                        item.GuidePrice = savedOrder.GuidePrice;
                        item.IsUrgent = savedOrder.IsUrgent;
                        item.ManagerID = savedOrder.ManagerID;
                        item.Notes = savedOrder.Notes;
                        item.Options = AutocompleteHelper.SplitField(savedOrder.Options);
                        item.OrderKindID = savedOrder.OrderKindID;
                        item.PrePayment = savedOrder.PrePayment;
                    } //if
                }
            }

            return item;
        }

        /// <summary>
        /// Инициализирует модель Обновления сущности.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код связанной сущности.</param>
        /// <param name="gridModel">Модель грида.</param>
        /// <param name="createParameters">Параметры.</param>
        /// <returns>Инициализированная модель обновления.</returns>
        public override RepairOrderEditModel CreateEditModel(SecurityToken token, Guid? entityId, RepairOrderGridModel gridModel,
            GridCreateParameters createParameters)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetOrder(token, entityId);
            RiseExceptionIfNotFound(entity, entityId, "Заказ");
            var issuerFullName = string.Empty;

            if (entity.IssuerID != null)
            {
                var user = RemontinkaServer.Instance.EntitiesFacade.GetUser(token, entity.IssuerID);
                issuerFullName = user.ToString();
            } //if

            var model = new RepairOrderEditModel
            {
                BranchID = entity.BranchID,
                CallEventDate = entity.CallEventDate,
                ClientAddress = entity.ClientAddress,
                ClientEmail = entity.ClientEmail,
                ClientFullName = entity.ClientFullName,
                ClientPhone = entity.ClientPhone,
                DateOfBeReady = entity.DateOfBeReady,
                Defect = entity.Defect,
                DeviceSN = entity.DeviceSN,
                DeviceModel = entity.DeviceModel,
                DeviceAppearance = AutocompleteHelper.SplitField(entity.DeviceAppearance),
                DeviceTitle = entity.DeviceModel,
                DeviceTrademark = AutocompleteHelper.SplitField(entity.DeviceTrademark),
                EngineerID = entity.EngineerID,
                GuidePrice = entity.GuidePrice,
                RepairOrderID = entity.RepairOrderID,
                IsUrgent = entity.IsUrgent,
                IssueDate = entity.IssueDate,
                ManagerID = entity.ManagerID,
                Notes = entity.Notes,
                Options = AutocompleteHelper.SplitField(entity.Options),
                OrderKindID = entity.OrderKindID,
                RepairOrderStatusID = entity.OrderStatusID,
                PrePayment = entity.PrePayment,
                IssuerFullName = issuerFullName,
                Recommendation = entity.Recommendation,
                WarrantyTo = entity.WarrantyTo,
                IsItemForEngeener = token.User.ProjectRoleID == ProjectRoleSet.Engineer.ProjectRoleID
            };

            if (entity.EngineerID != null && gridModel.Engineers.All(i=>i.Value!= entity.EngineerID))
            {
                gridModel.Engineers.Add(new SelectListItem<Guid> {Value = entity.EngineerID,Text = entity.EngineerFullName});
            }

            if (entity.ManagerID != null && gridModel.Managers.All(i => i.Value != entity.ManagerID))
            {
                gridModel.Managers.Add(new SelectListItem<Guid> { Value = entity.ManagerID, Text = entity.ManagerFullName });
            }

            return model;
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, RepairOrderCreateModel model, GridSaveModelResult result)
        {
            var entity = new RepairOrder();
            entity.BranchID = model.BranchID;
            entity.CallEventDate = model.CallEventDate;
            entity.ClientAddress = model.ClientAddress;
            entity.ClientEmail = model.ClientEmail;
            entity.ClientFullName = model.ClientFullName;
            entity.ClientPhone = model.ClientPhone;
            entity.DateOfBeReady = model.DateOfBeReady;
            entity.Defect = model.Defect;
            entity.DeviceAppearance = AutocompleteHelper.JoinField(model.DeviceAppearance);
            entity.DeviceModel = model.DeviceModel;
            entity.DeviceSN = model.DeviceSN;
            entity.DeviceTitle = model.DeviceTitle;
            entity.DeviceTrademark = AutocompleteHelper.JoinField(model.DeviceTrademark);
            entity.EngineerID = model.EngineerID;
            entity.EventDate = DateTime.Now;
            entity.GuidePrice = model.GuidePrice;
            entity.IsUrgent = model.IsUrgent;
            entity.IssueDate = null;
            entity.IssuerID = null;
            entity.ManagerID = model.ManagerID;
            entity.Notes = model.Notes;
            entity.Number = model.Number;
            entity.Options = AutocompleteHelper.JoinField(model.Options);
            entity.OrderKindID = model.OrderKindID;
            Romontinka.Server.DataLayer.Entities.OrderStatus status;
            if (entity.EngineerID == null)
            {
                status =
                    RemontinkaServer.Instance.EntitiesFacade.GetOrderStatusByKind(token,
                        StatusKindSet.New.StatusKindID);
            } //if
            else
            {
                status =
                    RemontinkaServer.Instance.EntitiesFacade.GetOrderStatusByKind(token,
                        StatusKindSet.OnWork.StatusKindID);
            } //else

            entity.OrderStatusID = status.OrderStatusID;
            entity.PrePayment = model.PrePayment;
            entity.Recommendation = null;
            entity.WarrantyTo = DateTime.Today;
            entity.AccessPassword = RemontinkaServer.Instance.CryptoService.GeneratePassword(6, 6);

            RemontinkaServer.Instance.EntitiesFacade.SaveRepairOrder(token, entity);
            var savedItem = RemontinkaServer.Instance.EntitiesFacade.GetOrder(token, entity.RepairOrderID);

            RemontinkaServer.Instance.OrderTimelineManager.TrackNewOrder(token, savedItem);
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, RepairOrderEditModel model, GridSaveModelResult result)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetOrder(token, model.RepairOrderID);

            RiseExceptionIfNotFound(entity, model.RepairOrderID, "Заказ");

            var oldEntity = new RepairOrder();

            entity.CopyTo(oldEntity);

            if (ProjectRoleSet.UserHasRole(token.User.ProjectRoleID, ProjectRoleSet.Engineer))//Если пользователь инженер, тогда даем ему право изменять только статус заказа
            {
                entity.OrderStatusID = model.RepairOrderStatusID;
                entity.Recommendation = model.Recommendation;
            } //if
            else
            {
                //TODO Сделать трекинг изменений в OrderTimeline
                entity.BranchID = model.BranchID;
                entity.CallEventDate = model.CallEventDate;
                entity.ClientAddress = model.ClientAddress;
                entity.ClientEmail = model.ClientEmail;
                entity.ClientFullName = model.ClientFullName;
                entity.ClientPhone = model.ClientPhone;
                entity.DateOfBeReady = model.DateOfBeReady;
                entity.Defect = model.Defect;
                entity.DeviceAppearance = AutocompleteHelper.JoinField(model.DeviceAppearance);
                entity.DeviceModel = model.DeviceModel;
                entity.DeviceSN = model.DeviceSN;
                entity.DeviceTitle = model.DeviceTitle;
                entity.DeviceTrademark = AutocompleteHelper.JoinField(model.DeviceTrademark);
                entity.EngineerID = model.EngineerID;
                entity.GuidePrice = model.GuidePrice;
                entity.IsUrgent = model.IsUrgent;
                entity.IssueDate = model.IssueDate;
                if (entity.IssueDate != null && entity.IssuerID == null)
                {
                    entity.IssuerID = token.User.UserID;
                } //if
                //entity.IssuerID = model.is
                entity.ManagerID = model.ManagerID;
                entity.Notes = model.Notes;
                entity.Options = AutocompleteHelper.JoinField(model.Options);
                entity.OrderKindID = model.OrderKindID;
                entity.OrderStatusID = model.RepairOrderStatusID;
                entity.PrePayment = model.PrePayment;
                entity.Recommendation = model.Recommendation;
                entity.WarrantyTo = model.WarrantyTo;
            } //else

            RemontinkaServer.Instance.EntitiesFacade.SaveRepairOrder(token, entity);
            entity = RemontinkaServer.Instance.EntitiesFacade.GetOrder(token, entity.RepairOrderID);
            RiseExceptionIfNotFound(entity, model.RepairOrderID, "Заказ");

            RemontinkaServer.Instance.OrderTimelineManager.TrackOrderChange(token, oldEntity, entity);
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid? entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteOrder(token, entityId);
        }
    }
}