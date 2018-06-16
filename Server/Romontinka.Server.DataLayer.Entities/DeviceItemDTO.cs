using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// DTO объект для запчастей.
    /// </summary>
    public class DeviceItemDTO:DeviceItem
    {
        /// <summary>
        /// Задает или получает ФИО инженера.
        /// </summary>
        public string EngineerFullName { get; set; }
    }
}
