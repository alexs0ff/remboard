using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.WarehouseItemSingleLookupForm
{
    /// <summary>
    /// Модель поиска пунктов складских остатков.
    /// </summary>
    public class JLookupWarehouseItemSearchModel : JLookupSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поиска в гриде.
        /// </summary>
        [DisplayName("Наименование")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string JLookupWarehouseItemName { get; set; }

        /// <summary>
        /// Задает или получает склад.
        /// </summary>
        [AjaxComboBox("AjaxWarehouseComboBox")]
        [DisplayName("Склад")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public Guid? JLookupWarehouseItemWarehouseID { get; set; }
    }
}