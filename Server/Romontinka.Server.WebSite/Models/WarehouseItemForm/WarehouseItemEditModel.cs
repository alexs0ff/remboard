using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.WarehouseItemForm
{
    /// <summary>
    /// Задает или получает остаток на складе.
    /// </summary>
    public class WarehouseItemEditModel: JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает нулевую цену.
        /// </summary>
        [DisplayName("Нулевая цена")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        [Required]
        public decimal StartPrice { get; set; }

        /// <summary>
        /// Задает или получает ремонтную цену.
        /// </summary>
        [DisplayName("Ремонтная цена")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        [Required]
        public decimal RepairPrice { get; set; }

        /// <summary>
        /// Задает или получает цену продажи.
        /// </summary>
        [DisplayName("Цена продажи")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        [Required]
        public decimal SalePrice { get; set; }
    }
}