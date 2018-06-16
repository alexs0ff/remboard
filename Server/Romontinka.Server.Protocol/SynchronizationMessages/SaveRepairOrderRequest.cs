using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Запрос на сохранение заказа.
    /// </summary>
    public class SaveRepairOrderRequest:SignedRequestBase
    {
        public SaveRepairOrderRequest()
        {
            Kind = MessageKind.SaveRepairOrderRequest;
        }

        /// <summary>
        /// Задает или получает объект заказа для сохранения.
        /// </summary>
        public RepairOrderDTO RepairOrder { get; set; }

        /// <summary>
        /// Заносит поля в сообщении для подписи.
        /// </summary>
        /// <param name="signVisitor">Реализация подписывателя.</param>
        protected override void ProcessFields(MessageSignVisitorBase signVisitor)
        {
            signVisitor.AddValue(Kind.ToString());
            signVisitor.AddValue(Utils.GuidToString(UserID));

            if (RepairOrder!=null)
            {
                signVisitor.AddValue(Utils.GuidToString(RepairOrder.RepairOrderID));
                signVisitor.AddValue(Utils.GuidToString(RepairOrder.BranchID));
                signVisitor.AddValue(Utils.DateTimeToString(RepairOrder.CallEventDate));
                signVisitor.AddValue(RepairOrder.ClientAddress);
                signVisitor.AddValue(RepairOrder.ClientEmail);
                signVisitor.AddValue(RepairOrder.ClientFullName);
                signVisitor.AddValue(RepairOrder.ClientPhone);
                signVisitor.AddValue(Utils.DateTimeToString(RepairOrder.DateOfBeReady));
                signVisitor.AddValue(RepairOrder.Defect);
                signVisitor.AddValue(RepairOrder.DeviceAppearance);
                signVisitor.AddValue(RepairOrder.DeviceModel);
                signVisitor.AddValue(RepairOrder.DeviceSN);
                signVisitor.AddValue(RepairOrder.DeviceTitle);
                signVisitor.AddValue(RepairOrder.DeviceTrademark);
                signVisitor.AddValue(Utils.GuidToString(RepairOrder.EngineerID));
                signVisitor.AddValue(Utils.DateTimeToString(RepairOrder.EventDate));
                signVisitor.AddValue(Utils.DecimalToString(RepairOrder.GuidePrice));
                signVisitor.AddValue(Utils.BooleanToString(RepairOrder.IsUrgent));
                signVisitor.AddValue(Utils.DateTimeToString(RepairOrder.IssueDate));
                signVisitor.AddValue(Utils.GuidToString(RepairOrder.IssuerID));
                signVisitor.AddValue(Utils.GuidToString(RepairOrder.ManagerID));
                signVisitor.AddValue(RepairOrder.Notes);
                signVisitor.AddValue(RepairOrder.Number);
                signVisitor.AddValue(RepairOrder.Options);
                signVisitor.AddValue(Utils.GuidToString(RepairOrder.OrderKindID));
                signVisitor.AddValue(Utils.GuidToString(RepairOrder.OrderStatusID));
                signVisitor.AddValue(Utils.DecimalToString(RepairOrder.PrePayment));
                signVisitor.AddValue(RepairOrder.Recommendation);
                signVisitor.AddValue(Utils.DateTimeToString(RepairOrder.WarrantyTo));

                foreach (var timeline in RepairOrder.OrderTimelines)
                {
                    signVisitor.AddValue(Utils.GuidToString(timeline.OrderTimelineID));
                    signVisitor.AddValue(Utils.DateTimeToString(timeline.EventDateTime));
                    signVisitor.AddValue(Utils.GuidToString(timeline.RepairOrderID));
                    signVisitor.AddValue(timeline.Title);
                    signVisitor.AddValue(Utils.IntToString(timeline.TimelineKindID));
                } //foreach

                foreach (var item in RepairOrder.WorkItems)
                {
                    signVisitor.AddValue(Utils.GuidToString(item.WorkItemID));
                    signVisitor.AddValue(Utils.GuidToString(item.RepairOrderID));
                    signVisitor.AddValue(Utils.GuidToString(item.UserID));
                    signVisitor.AddValue(item.Title);
                    signVisitor.AddValue(Utils.DateTimeToString(item.EventDate));
                    signVisitor.AddValue(Utils.DecimalToString(item.Price));
                } //foreach

                foreach (var item in RepairOrder.DeviceItems)
                {
                    signVisitor.AddValue(Utils.GuidToString(item.DeviceItemID));
                    signVisitor.AddValue(Utils.DecimalToString(item.CostPrice));
                    signVisitor.AddValue(Utils.DecimalToString(item.Count));
                    signVisitor.AddValue(Utils.DateTimeToString(item.EventDate));
                    signVisitor.AddValue(Utils.DecimalToString(item.Price));
                    signVisitor.AddValue(Utils.GuidToString(item.RepairOrderID));
                    signVisitor.AddValue(Utils.GuidToString(item.UserID));
                    signVisitor.AddValue(Utils.GuidToString(item.WarehouseItemID));
                    signVisitor.AddValue(item.Title);
                } //foreach
            } //if
        }
    }
}
