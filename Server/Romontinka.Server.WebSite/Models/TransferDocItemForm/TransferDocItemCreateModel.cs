﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.GoodsItemSingleLookupForm;

namespace Romontinka.Server.WebSite.Models.TransferDocItemForm
{
    /// <summary>
    /// Модель пункта документа перемещения между складами.
    /// </summary>
    public class TransferDocItemCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код документа перемещения.
        /// </summary>
        public Guid? TransferDocID { get; set; }

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

        /// <summary>
        /// Задает или получает количество элементов.
        /// </summary>
        [DisplayName("Количество")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public decimal Total { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        [DisplayName("Описание")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        [UIHint("MultilineString")]
        [MultilineString(3, 5)]
        public string Description { get; set; }
    }
}