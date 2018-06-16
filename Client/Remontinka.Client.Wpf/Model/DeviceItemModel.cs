using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.DataLayer.Entities;

namespace Remontinka.Client.Wpf.Model
{
    /// <summary>
    /// Модель установленной запчасти.
    /// </summary>
    public class DeviceItemModel
    {
        public DeviceItemModel(DeviceItem deviceItem)
        {
            Id = deviceItem.DeviceItemIDGuid;
            EventDate = WpfUtils.DateTimeToString(deviceItem.EventDateDateTime);
            DeviceItemTitle = deviceItem.Title;
            DeviceItemCount = WpfUtils.DecimalToString((decimal?) deviceItem.Count);
            DeviceItemCostPrice = WpfUtils.DecimalToString((decimal?)deviceItem.CostPrice);
            DeviceItemPrice = WpfUtils.DecimalToString((decimal?)deviceItem.Price);

            if (!string.IsNullOrWhiteSpace(DeviceItemCount))
            {
                if (DeviceItemCount.EndsWith(".00"))
                {
                    DeviceItemCount = DeviceItemCount.Substring(0, DeviceItemCount.Length - 3);
                }
            }
        }

        /// <summary>
        /// Задает или получает код устройства.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Дата установки запчасти.
        /// </summary>
        public string EventDate { get; set; }

        /// <summary>
        /// Наименование запчасти.
        /// </summary>
        public string DeviceItemTitle { get; set; }

        /// <summary>
        /// Задает или получает количество запчастей.
        /// </summary>
        public string DeviceItemCount { get; set; }

        /// <summary>
        /// Задает или получает себестоимость.
        /// </summary>
        public string DeviceItemCostPrice { get; set; }

        /// <summary>
        /// Задает или получает окончательную цену.
        /// </summary>
        public string DeviceItemPrice { get; set; }
    }
}
