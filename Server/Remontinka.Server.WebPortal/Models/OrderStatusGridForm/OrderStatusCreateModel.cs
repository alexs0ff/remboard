using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.OrderStatusGridForm
{
    /// <summary>
    /// Модель статусов заказа.
    /// </summary>
    public class OrderStatusCreateModel: GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код статуса заказа.
        /// </summary>
        public Guid? OrderStatusID { get; set; }

        /// <summary>
        /// Задает или получает название статуса.
        /// </summary>
        [DisplayName("Название:")]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает код типа статуса.
        /// </summary>
        [DisplayName("Тип:")]
        [Required]
        public byte? StatusKindID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return OrderStatusID;
        }
    }
}