using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.IncomingDocItemGridForm
{
    /// <summary>
    /// Модель редактирования пункта приходной накладной.
    /// </summary>
    public class IncomingDocItemCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код элемента приходной накладной.
        /// </summary>
        public Guid? IncomingDocItemID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return IncomingDocItemID;
        }

        /// <summary>
        /// Задает или получает код приходной накладной.
        /// </summary>
        public Guid? IncomingDocID { get; set; }

        /// <summary>
        /// Задает или получает количество элементов.
        /// </summary>
        [DisplayName("Количество")]
        [Required]
        public decimal Total { get; set; }

        /// <summary>
        /// Задает или получает цену закупки.
        /// </summary>
        [DisplayName("Цена закупки")]
        [Required]
        public decimal InitPrice { get; set; }

        /// <summary>
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

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        [DisplayName("Описание")]
        public string Description { get; set; }

        /// <summary>
        /// Задает или получает код номенклатуры.
        /// </summary>
        [DisplayName("Номенклатура")]
        [Required]
        public Guid? GoodsItemID { get; set; }
    }
}