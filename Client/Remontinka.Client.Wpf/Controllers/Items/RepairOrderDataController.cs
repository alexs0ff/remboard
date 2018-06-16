using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Remontinka.Client.Core;
using Remontinka.Client.DataLayer.Entities;
using Remontinka.Client.Wpf.Controllers.Forms;
using Remontinka.Client.Wpf.Model;
using Remontinka.Client.Wpf.Model.Items;
using Remontinka.Client.Wpf.View;

namespace Remontinka.Client.Wpf.Controllers.Items
{
    /// <summary>
    /// Контроллер данных для операций управлением данными заказа.
    /// </summary>
    public class RepairOrderDataController : ModelEditControllerBase<RepairOrderEditView,RepairOrderCreateView,RepairOrderEditModel,RepairOrderCreateModel,Guid,object>
    {
        /// <summary>
        /// Создает модель редактирования из сущности.
        /// </summary>
        /// <param name="entityId">Код сущности.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="parameters">Модель параметров.</param>
        /// <returns>Созданная модель редактирования.</returns>
        public override RepairOrderEditModel CreateEditedModel(SecurityToken token, Guid entityId, object parameters)
        {
            var entity = ClientCore.Instance.DataStore.GetRepairOrder(entityId);
            RiseExceptionIfNotFound(entity,entityId,"Заказ");

            return new RepairOrderEditModel
                   {
                       Id = entity.RepairOrderIDGuid,
                       BranchID = entity.BranchIDGuid,
                       CallEventDate = entity.CallEventDateDateTime,
                       ClientAddress = entity.ClientAddress,
                       ClientEmail = entity.ClientEmail,
                       ClientFullName = entity.ClientFullName,
                       ClientPhone = entity.ClientPhone,
                       DateOfBeReady = entity.DateOfBeReadyDateTime,
                       Defect = entity.Defect,
                       DeviceSN = entity.DeviceSN,
                       DeviceModel = entity.DeviceModel,
                       DeviceAppearance = entity.DeviceAppearance,
                       DeviceTitle = entity.DeviceModel,
                       DeviceTrademark = entity.DeviceTrademark,
                       EngineerID = entity.EngineerIDGuid,
                       GuidePrice = WpfUtils.DecimalToString((decimal?)entity.GuidePrice),
                       IsUrgent = entity.IsUrgentBoolean,
                       IssueDate = entity.IssueDateDateTime,
                       ManagerID = entity.ManagerIDGuid,
                       Notes = entity.Notes,
                       Options = entity.Options,
                       OrderKindID = entity.OrderKindIDGuid,
                       RepairOrderStatusID = entity.OrderStatusIDGuid,
                       PrePayment = WpfUtils.DecimalToString((decimal?)entity.PrePayment),
                       Recommendation = entity.Recommendation,
                       WarrantyTo = entity.WarrantyToDateTime

                   };
        }

        /// <summary>
        /// Создает новую модель создания сущности.
        /// </summary>
        /// <param name="parameters">Модель параметров.</param>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданная модель.</returns>
        public override RepairOrderCreateModel CreateNewModel(SecurityToken token, object parameters)
        {
            var item = new RepairOrderCreateModel();
            item.Number = string.Format("{0}{1}",ClientCore.Instance.AuthService.AuthToken.KeyNumber,
                                        ClientCore.Instance.DataStore.GetNextDocNumber().ToString(
                                            CultureInfo.InvariantCulture));
            item.DateOfBeReady = DateTime.Today;

            return item;
        }

        /// <summary>
        /// Удаляет из хранилища сущность.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="entityId">Код сущности.</param>
        public override void DeleteEntity(SecurityToken token, Guid entityId)
        {
            if (ClientCore.Instance.DataStore.RepairOrderServerHashItemExists(entityId))
            {
                throw new Exception("Серверный заказ нельзя удалять");
            }
            else
            {
                ClientCore.Instance.DataStore.DeleteAllDeviceItems(entityId);
                ClientCore.Instance.DataStore.DeleteAllWorkItems(entityId);
                ClientCore.Instance.DataStore.DeleteAllOrderTimelines(entityId);
                ClientCore.Instance.DataStore.DeleteRepairOrder(entityId);    
            } //else
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель создания сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveCreateModel(SecurityToken token, RepairOrderCreateModel model, SaveModelResult result)
        {
            var entity = new RepairOrder();
            entity.BranchIDGuid = model.BranchID;
            entity.CallEventDateDateTime = model.CallEventDate;
            entity.ClientAddress = model.ClientAddress;
            entity.ClientEmail = model.ClientEmail;
            entity.ClientFullName = model.ClientFullName;
            entity.ClientPhone = model.ClientPhone;
            entity.DateOfBeReadyDateTime = model.DateOfBeReady;
            entity.Defect = model.Defect;
            entity.DeviceAppearance = model.DeviceAppearance;
            entity.DeviceModel = model.DeviceModel;
            entity.DeviceSN = model.DeviceSN;
            entity.DeviceTitle = model.DeviceTitle;
            entity.DeviceTrademark = model.DeviceTrademark;
            entity.EngineerIDGuid = model.EngineerID;
            entity.EventDateDateTime = DateTime.Now;
            entity.GuidePrice = (double?)WpfUtils.StringToDecimal(model.GuidePrice);
            entity.IsUrgentBoolean = model.IsUrgent;
            entity.IssueDate = null;
            entity.IssuerID = null;
            entity.ManagerIDGuid = model.ManagerID;
            entity.Notes = model.Notes;
            entity.Number = model.Number;
            entity.Options = model.Options;
            entity.OrderKindIDGuid = model.OrderKindID;
            OrderStatus status;
            if (entity.EngineerID == null)
            {
                status =
                    ClientCore.Instance.DataStore.GetOrderStatusByKind(StatusKindSet.New.StatusKindID);
            } //if
            else
            {
                status =
                    ClientCore.Instance.DataStore.GetOrderStatusByKind(StatusKindSet.OnWork.StatusKindID);
            } //else

            entity.OrderStatusID = status.OrderStatusID;
            entity.PrePayment = (double?)WpfUtils.StringToDecimal(model.PrePayment);
            entity.Recommendation = null;
            entity.WarrantyToDateTime = DateTime.Today;

            ClientCore.Instance.DataStore.SaveRepairOrder(entity);

            model.Id = entity.RepairOrderIDGuid;
            

            ClientCore.Instance.OrderTimelineManager.TrackNewOrder(token, entity);

            
        }

        /// <summary>
        /// Сохраняет в базе модель создания элемента.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="model">Модель редактирования сущности для сохранения.</param>
        /// <param name="result">Результат с ошибками.</param>
        public override void SaveEditModel(SecurityToken token, RepairOrderEditModel model, SaveModelResult result)
        {
            var entity = ClientCore.Instance.DataStore.GetRepairOrder(model.Id);
            RiseExceptionIfNotFound(entity, model.Id, "Заказ");

            var oldEntity = new RepairOrder();

            entity.CopyTo(oldEntity);

            if (ProjectRoleSet.UserHasRole(token.User.ProjectRoleID, ProjectRoleSet.Engineer))
            {
                entity.OrderStatusIDGuid = model.RepairOrderStatusID;
            } //if
            else
            {

                //TODO Сделать трекинг изменений в OrderTimeline
                entity.BranchIDGuid = model.BranchID;
                entity.CallEventDateDateTime = model.CallEventDate;
                entity.ClientAddress = model.ClientAddress;
                entity.ClientEmail = model.ClientEmail;
                entity.ClientFullName = model.ClientFullName;
                entity.ClientPhone = model.ClientPhone;
                entity.DateOfBeReadyDateTime = model.DateOfBeReady;
                entity.Defect = model.Defect;
                entity.DeviceAppearance = model.DeviceAppearance;
                entity.DeviceModel = model.DeviceModel;
                entity.DeviceSN = model.DeviceSN;
                entity.DeviceTitle = model.DeviceTitle;
                entity.DeviceTrademark = model.DeviceTrademark;
                entity.EngineerIDGuid = model.EngineerID;
                entity.GuidePrice = (double?) WpfUtils.StringToDecimal(model.GuidePrice);
                entity.IsUrgentBoolean = model.IsUrgent;
                entity.IssueDateDateTime = model.IssueDate;
                if (entity.IssueDate != null && entity.IssuerID == null)
                {
                    entity.IssuerID = token.User.UserID;
                } //if

                entity.ManagerIDGuid = model.ManagerID;
                entity.Notes = model.Notes;
                entity.Options = model.Options;
                entity.OrderKindIDGuid = model.OrderKindID;
                entity.OrderStatusIDGuid = model.RepairOrderStatusID;
                entity.PrePayment = (double?) WpfUtils.StringToDecimal(model.PrePayment);
                entity.Recommendation = model.Recommendation;
                entity.WarrantyToDateTime = model.WarrantyTo;
            } //else

            ClientCore.Instance.DataStore.SaveRepairOrder(entity);
            
            
            ClientCore.Instance.OrderTimelineManager.TrackOrderChange(token, oldEntity, entity);
        }
    }
}
