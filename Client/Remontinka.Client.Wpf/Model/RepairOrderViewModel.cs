using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Remontinka.Client.Core.Interception;

namespace Remontinka.Client.Wpf.Model
{
    /// <summary>
    /// Модель для представления заказов.
    /// </summary>
    public class RepairOrderViewModel : BindableModelObject
    {
        /// <summary>
        /// Задает или получает список пунктов грида заказов.
        /// </summary>
        public Collection<RepairOrderItemModel> Orders { get; set; }

        /// <summary>
        /// Задает или получает список текущих работ выделеного заказа.
        /// </summary>
        public Collection<WorkItemModel> CurrentWorkItems { get; set; }

        /// <summary>
        /// Задает или получает список установленных запчастей выделеного заказа.
        /// </summary>
        public Collection<DeviceItemModel> CurrentDeviceItems { get; set; }

        /// <summary>
        /// Задает или получает текущий список событий выделенного заказа.
        /// </summary>
        public Collection<OrderTimelineModel> CurrentOrderTimelineItems { get; set; }
    }
}
