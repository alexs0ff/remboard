using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;
using Romontinka.Server.WebSite.Models.DataGrid;
using Romontinka.Server.WebSite.Models.OrderStatus;

namespace Romontinka.Server.WebSite.Models.RepairOrderForm
{
    /// <summary>
    /// Адаптер для управления созданием и редактирования заказов.
    /// </summary>
    public class RepairOrderDataAdapter : JGridDataAdapterBase<Guid, RepairOrderGridItemModel, RepairOrderCreateModel, RepairOrderEditModel, RepairOrderSearchModel>
    {
        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="searchModel">Модель строки поиска.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override RepairOrderCreateModel CreateNewModel(SecurityToken token, RepairOrderSearchModel searchModel)
        {
            var item = RepairOrderCreateModel(token,searchModel.CopyFromRepairOrderID);

            return item;
        }

        /// <summary>
        /// Создает новую модель данных.
        /// </summary>
        /// <returns>Созданная модель.</returns>
        public static RepairOrderCreateModel RepairOrderCreateModel(SecurityToken token, Guid? copyFromRepairOrderID)
        {
            var item = new RepairOrderCreateModel();

            item.Number =
                RemontinkaServer.Instance.EntitiesFacade.GetNewOrderNumber(token).ToString(CultureInfo.InvariantCulture);

            if (copyFromRepairOrderID != null)
            {
                var savedOrder = RemontinkaServer.Instance.DataStore.GetRepairOrder(copyFromRepairOrderID,
                                                                                    token.User.UserDomainID);
                if (savedOrder != null && RemontinkaServer.Instance.EntitiesFacade.UserHasAccessToRepairOrder(token.User.UserID, savedOrder, token.User.ProjectRoleID))
                {
                    item.BranchID = savedOrder.BranchID;
                    item.ClientAddress = savedOrder.ClientAddress;
                    item.ClientEmail = savedOrder.ClientEmail;
                    item.ClientFullName = savedOrder.ClientFullName;
                    item.ClientPhone = savedOrder.ClientPhone;
                    item.Defect = savedOrder.Defect;
                    item.DeviceAppearance = savedOrder.DeviceAppearance;
                    item.DeviceModel = savedOrder.DeviceModel;
                    item.DeviceSN = savedOrder.DeviceSN;
                    item.DeviceTitle = savedOrder.DeviceTitle;
                    item.DeviceTrademark = savedOrder.DeviceTrademark;
                    item.GuidePrice = savedOrder.GuidePrice;
                    item.IsUrgent = savedOrder.IsUrgent;
                    item.ManagerID = savedOrder.ManagerID;
                    item.Notes = savedOrder.Notes;
                    item.Options = savedOrder.Options;
                    item.OrderKindID = savedOrder.OrderKindID;
                    item.PrePayment = savedOrder.PrePayment;
                } //if

            } //if

            return item;
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            DeleteRepairOrder(token, entityId);
        }

        /// <summary>
        /// Удаляет из хранилища заказ.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код заказа.</param>
        public static void DeleteRepairOrder(SecurityToken token, Guid entityId)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteOrder(token,entityId);
        }

        /// <summary>
        /// Создает элементы для грида с разбиением на страницы.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="searchModel">Модель поиска.</param>
        /// <param name="itemsPerPage">Элементов на странице грида.</param>
        /// <param name="totalCount">Общее количество элементов.</param>
        /// <returns>Списко элементов грида.</returns>
        public override IEnumerable<RepairOrderGridItemModel> GetPageableGridItems(SecurityToken token, RepairOrderSearchModel searchModel, int itemsPerPage, out int totalCount)
        {
            if (searchModel.FilterID == OrderSearchSet.All.Key || searchModel.FilterID==null)
            {
                if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
                {

                    return
                        RemontinkaServer.Instance.EntitiesFacade.GetRepairOrders(token,searchModel.OrderStatusID,null, searchModel.Name, searchModel.Page,
                                                                            itemsPerPage,
                                                                            out totalCount).Select(CreateItemModel);
                } //if

                return
                    RemontinkaServer.Instance.EntitiesFacade.GetRepairOrdersUserBranch(token, searchModel.OrderStatusID, null,
                                                                                  token.User.UserID, searchModel.Name,
                                                                                  searchModel.Page,
                                                                                  itemsPerPage,
                                                                                  out totalCount).Select(CreateItemModel);

            } //if

            if (searchModel.FilterID==OrderSearchSet.CurrentUser.Key)
            {
                return
                    RemontinkaServer.Instance.EntitiesFacade.GetRepairOrdersUser(token,searchModel.OrderStatusID,null,token.User.UserID, searchModel.Name,
                                                                               searchModel.Page,
                                                                               itemsPerPage,
                                                                               out totalCount).Select(CreateItemModel);
            } //if

            if (searchModel.FilterID==OrderSearchSet.OnlyUrgents.Key)
            {
                if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
                {
                    return
                        RemontinkaServer.Instance.EntitiesFacade.GetRepairOrders(token,searchModel.OrderStatusID,true,searchModel.Name,
                                                                                   searchModel.Page,
                                                                                   itemsPerPage,
                                                                                   out totalCount).Select(CreateItemModel);    
                } //if

                return
                    RemontinkaServer.Instance.EntitiesFacade.GetRepairOrdersUserBranch(token,searchModel.OrderStatusID, true,
                                                                                  token.User.UserID, searchModel.Name,
                                                                                  searchModel.Page,
                                                                                  itemsPerPage,
                                                                                  out totalCount).Select(CreateItemModel);
            } //if

            if (searchModel.FilterID==OrderSearchSet.SpecificUser.Key)
            {
                return
                    RemontinkaServer.Instance.EntitiesFacade.GetRepairOrdersUser(token, searchModel.OrderStatusID, null, searchModel.UserID, searchModel.Name,
                                                                               searchModel.Page,
                                                                               itemsPerPage,
                                                                               out totalCount).Select(CreateItemModel);
            }

            totalCount = 0;
            return new RepairOrderGridItemModel[0];

        }

        /// <summary>
        /// Создает из сущности модель для грида.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <returns>Модель.</returns>
        public static RepairOrderGridItemModel CreateItemModel(RepairOrderDTO entity)
        {
            var deviceSum = RemontinkaServer.Instance.DataStore.GetDeviceItemsSum(entity.RepairOrderID)??decimal.Zero;
            var workSum = RemontinkaServer.Instance.DataStore.GetWorkItemsSum(entity.RepairOrderID)??decimal.Zero;
            var item = new RepairOrderGridItemModel
                   {
                       ClientFullName = entity.ClientFullName,
                       DeviceTitle = entity.DeviceTitle,
                       StatusTitle = entity.IsUrgent? "Срочный "+entity.OrderStatusTitle:entity.OrderStatusTitle,
                       ManagerFullName = entity.ManagerFullName,
                       EngineerFullName = entity.EngineerFullName??"Не назначен",
                       EventDate = Utils.DateTimeToString(entity.EventDate),
                       EventDateOfBeReady = Utils.DateTimeToString(entity.DateOfBeReady),
                       Id = entity.RepairOrderID,
                       Number = entity.Number,
                       RowClass = entity.IsUrgent ? GridRowColors.Danger : OrderStatusDataAdapter.StatusesColors[entity.StatusKind],
                       Totals = string.Format("Общая:{0:0.00}; запчасти: {1:0.00}; работа: {2:0.00}", deviceSum + workSum, deviceSum, workSum),
                       Defect = entity.Defect
                   };

            var diff = entity.DateOfBeReady -DateTime.Today;
            if (diff.Days<3)
            {
                item.RowClass = GridRowColors.Danger;
            }

            return item;
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат выполнения..</param>
        public override RepairOrderGridItemModel SaveCreateModel(SecurityToken token, RepairOrderCreateModel model, JGridSaveModelResult result)
        {
            return SaveCreateRepairOrderGridItemModel(token, model,result);
        }

        /// <summary>
        /// Сохраняет модель создаваемого заказа в хранилище.
        /// </summary>
        /// <param name="token">Токен безопасности</param>
        /// <param name="model">Модель для сохранения.</param>
        /// <param name="result">Модель результата модели. </param>
        /// <returns>Созданный пункт грида.</returns>
        public static RepairOrderGridItemModel SaveCreateRepairOrderGridItemModel(SecurityToken token,
                                                                                        RepairOrderCreateModel model, JGridSaveModelResult result)
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
            entity.DeviceAppearance = model.DeviceAppearance;
            entity.DeviceModel = model.DeviceModel;
            entity.DeviceSN = model.DeviceSN;
            entity.DeviceTitle = model.DeviceTitle;
            entity.DeviceTrademark = model.DeviceTrademark;
            entity.EngineerID = model.EngineerID;
            entity.EventDate = DateTime.Now;
            entity.GuidePrice = model.GuidePrice;
            entity.IsUrgent = model.IsUrgent;
            entity.IssueDate = null;
            entity.IssuerID = null;
            entity.ManagerID = model.ManagerID;
            entity.Notes = model.Notes;
            entity.Number = model.Number;
            entity.Options = model.Options;
            entity.OrderKindID = model.OrderKindID;
            DataLayer.Entities.OrderStatus status;
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

            return CreateItemModel(savedItem);
        }

        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override RepairOrderEditModel CreateEditedModel(SecurityToken token, Guid entityId)
        {
            return CreateRepairOrderEditModel(token, entityId);
        }

        /// <summary>
        /// Создает модель для редактирования 
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код заказа.</param>
        /// <returns>Созданная модель</returns>
        public static RepairOrderEditModel CreateRepairOrderEditModel(SecurityToken token, Guid entityId)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetOrder(token, entityId);
            RiseExceptionIfNotFound(entity, entityId, "Заказ");
            var issuerFullName = string.Empty;

            if (entity.IssuerID != null)
            {
                var user = RemontinkaServer.Instance.EntitiesFacade.GetUser(token,entity.IssuerID);
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
                            DeviceAppearance = entity.DeviceAppearance,
                            DeviceTitle = entity.DeviceModel,
                            DeviceTrademark = entity.DeviceTrademark,
                            EngineerID = entity.EngineerID,
                            GuidePrice = entity.GuidePrice,
                            Id = entity.RepairOrderID,
                            IsUrgent = entity.IsUrgent,
                            IssueDate = entity.IssueDate,
                            ManagerID = entity.ManagerID,
                            Notes = entity.Notes,
                            Options = entity.Options,
                            OrderKindID = entity.OrderKindID,
                            RepairOrderStatusID = entity.OrderStatusID,
                            PrePayment = entity.PrePayment,
                            IssuerFullName = issuerFullName,
                            Recommendation = entity.Recommendation,
                            WarrantyTo = entity.WarrantyTo,
                            IsItemForEngeener = token.User.ProjectRoleID == ProjectRoleSet.Engineer.ProjectRoleID
                        };

            return model;
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат выполнения.</param>
        public override RepairOrderGridItemModel SaveEditModel(SecurityToken token, RepairOrderEditModel model, JGridSaveModelResult result)
        {
            return SaveEditRepairOrderGridItemModel(token, model, result);
        }

        /// <summary>
        /// Сохраняет в базе модель редактирования заказа.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования.</param>
        /// <param name="result">Результат корректности модели.</param>
        /// <returns>Модель пункта грида.</returns>
        public static RepairOrderGridItemModel SaveEditRepairOrderGridItemModel(SecurityToken token, RepairOrderEditModel model, JGridSaveModelResult result)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetOrder(token, model.Id);
            
            RiseExceptionIfNotFound(entity, model.Id, "Заказ");

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
                entity.DeviceAppearance = model.DeviceAppearance;
                entity.DeviceModel = model.DeviceModel;
                entity.DeviceSN = model.DeviceSN;
                entity.DeviceTitle = model.DeviceTitle;
                entity.DeviceTrademark = model.DeviceTrademark;
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
                entity.Options = model.Options;
                entity.OrderKindID = model.OrderKindID;
                entity.OrderStatusID = model.RepairOrderStatusID;
                entity.PrePayment = model.PrePayment;
                entity.Recommendation = model.Recommendation;
                entity.WarrantyTo = model.WarrantyTo;
            } //else

            RemontinkaServer.Instance.EntitiesFacade.SaveRepairOrder(token, entity);
            entity = RemontinkaServer.Instance.EntitiesFacade.GetOrder(token, entity.RepairOrderID);
            RiseExceptionIfNotFound(entity, model.Id, "Заказ");

            RemontinkaServer.Instance.OrderTimelineManager.TrackOrderChange(token, oldEntity, entity);

            return CreateItemModel(entity);
        }
    }
}
