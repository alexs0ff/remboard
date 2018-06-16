using Remontinka.Client.DataLayer.Entities;

namespace Remontinka.Client.Core.Context
{
    /// <summary>
    /// Контекст для заказа.
    /// </summary>
    public class RepairOrderContextItem : ContextItemBase
    {
        /// <summary>
        /// Контекст для заказа.
        /// </summary>
        /// <param name="order">Заказ.</param>
        public RepairOrderContextItem(RepairOrderDTO order)
        {
            _values[ContextConstants.ClientFullName] = order.ClientFullName;
            _values[ContextConstants.ClientPhone] = order.ClientPhone;
            _values[ContextConstants.DateOfBeReady] = order.DateOfBeReady;
            _values[ContextConstants.Defect] = order.Defect;
            _values[ContextConstants.DeviceAppearance] = order.DeviceAppearance;
            _values[ContextConstants.DeviceModel] = order.DeviceModel;
            _values[ContextConstants.DeviceSN] = order.DeviceSN;
            _values[ContextConstants.DeviceTitle] = order.DeviceTitle;
            _values[ContextConstants.DeviceTrademark] = order.DeviceTrademark;
            _values[ContextConstants.EngineerFullName] = order.EngineerFullName;
            _values[ContextConstants.EventDate] = order.EventDate;
            _values[ContextConstants.GuidePrice] = order.GuidePrice;
            _values[ContextConstants.IsUrgent] = order.IsUrgent;
            _values[ContextConstants.IssueDate] = order.IssueDate;
            _values[ContextConstants.ManagerFullName] = order.ManagerFullName;
            _values[ContextConstants.Notes] = order.Notes;
            _values[ContextConstants.Number] = order.Number;
            _values[ContextConstants.Options] = order.Options;
            _values[ContextConstants.OrderKindTitle] = order.OrderKindTitle;
            _values[ContextConstants.OrderStatusTitle] = order.OrderStatusTitle;
            _values[ContextConstants.PrePayment] = order.PrePayment;
            _values[ContextConstants.Recommendation] = order.Recommendation;
            _values[ContextConstants.WarrantyTo] = order.WarrantyTo;
        }
    }
}
