using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.DeviceItemForm
{
    /// <summary>
    /// Модель пункта грида.
    /// </summary>
    public class DeviceItemGridItemModel : JGridItemModel<Guid>
    {
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