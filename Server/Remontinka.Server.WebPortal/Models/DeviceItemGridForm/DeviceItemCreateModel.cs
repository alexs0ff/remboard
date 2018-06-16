using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.DeviceItemGridForm
{
    /// <summary>
    /// Модель редактирования запчасти.
    /// </summary>
    public class DeviceItemCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает пункт выполненных работ.
        /// </summary>
        public Guid? DeviceItemID { get; set; }

        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        public Guid? RepairOrderID { get; set; }

        /// <summary>
        /// Задает или получает код инженера.
        /// </summary>
        [DisplayName("Инженер")]
        [Required]
        public Guid? DeviceItemUserID { get; set; }

        /// <summary>
        /// Задает или получает дату выполненной работы.
        /// </summary>
        [DisplayName("Дата")]
        [Required]
        public DateTime DeviceItemEventDate { get; set; }

        /// <summary>
        /// Наименование запчасти.
        /// </summary>
        [DisplayName("Наименование запчасти")]
        public string DeviceItemTitle { get; set; }

        /// <summary>
        /// Задает или получает количество запчастей.
        /// </summary>
        [DisplayName("Количество")]
        [Required]
        public decimal DeviceItemCount { get; set; }

        /// <summary>
        /// Задает или получает себестоимость.
        /// </summary>
        [DisplayName("Себестоимость")]
        [Required]
        public decimal DeviceItemCostPrice { get; set; }

        /// <summary>
        /// Задает или получает окончательную цену.
        /// </summary>
        [DisplayName("Цена")]
        [Required]
        public decimal DeviceItemPrice { get; set; }

        /// <summary>
        /// Задает или получает код номенклатуры.
        /// </summary>
        [DisplayName("Деталь со склада")]
        public Guid? WarehouseItemID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return DeviceItemID;
        }
    }
}