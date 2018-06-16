using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.GoodsItemForm;
using Romontinka.Server.WebSite.Models.GoodsItemSingleLookupForm;

namespace Romontinka.Server.WebSite.Models.IncomingDocItemForm
{
    /// <summary>
    /// Модель для редактирования пунктов приходной накладной.
    /// </summary>
    public class IncomingDocItemCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код приходной накладной.
        /// </summary>
        public Guid? IncomingDocID { get; set; }

        /// <summary>
        /// Задает или получает количество элементов.
        /// </summary>
        [DisplayName("Количество")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public decimal Total { get; set; }

        /// <summary>
        /// Задает или получает цену закупки.
        /// </summary>
        [DisplayName("Цена закупки")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public decimal InitPrice { get; set; }

        /// <summary>
        /// Задает или получает нулевую цену.
        /// </summary>
        [DisplayName("Нулевая цена")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public decimal StartPrice { get; set; }

        /// <summary>
        /// Задает или получает ремонтную цену.
        /// </summary>
        [DisplayName("Ремонтная цена")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public decimal RepairPrice { get; set; }

        /// <summary>
        /// Задает или получает цену продажи.
        /// </summary>
        [DisplayName("Цена продажи")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public decimal SalePrice { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        [DisplayName("Описание")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        [UIHint("MultilineString")]
        [MultilineString(3, 5)]
        public string Description { get; set; }
        
        /// <summary>
        /// Задает или получает код номенклатуры.
        /// </summary>
        [DisplayName("Номенклатура")]
        [UIHint("SingleLookup")]
        [Required]
        [SingleLookup("GoodsItemSingleLookup", typeof(JLookupGoodsItemSearchModel), null, false, true)]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public Guid? GoodsItemID { get; set; }
    }
}