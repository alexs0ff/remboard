using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.WarehouseItemGridForm
{
    /// <summary>
    /// Модель редактирования остатков на складе.
    /// </summary>
    public class WarehouseItemEditModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код элемента остатка на складе.
        /// </summary>
        public Guid? WarehouseItemID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return WarehouseItemID;
        }

        // <summary>
        /// Задает или получает нулевую цену.
        /// </summary>
        [DisplayName("Нулевая цена")]
        [Required]
        public decimal StartPrice { get; set; }

        /// <summary>
        /// Задает или получает ремонтную цену.
        /// </summary>
        [DisplayName("Ремонтная цена")]
        [Required]
        public decimal RepairPrice { get; set; }

        /// <summary>
        /// Задает или получает цену продажи.
        /// </summary>
        [DisplayName("Цена продажи")]
        [Required]
        public decimal SalePrice { get; set; }
    }
}