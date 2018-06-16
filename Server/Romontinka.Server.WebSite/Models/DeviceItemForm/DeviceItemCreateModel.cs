using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.WarehouseItemSingleLookupForm;

namespace Romontinka.Server.WebSite.Models.DeviceItemForm
{
    /// <summary>
    /// Модель для создания и редактирования запчастей.
    /// </summary>
    public class DeviceItemCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        public Guid? DeviceItemRepairOrderID { get; set; }

        /// <summary>
        /// Задает или получает код инженера.
        /// </summary>
        [DisplayName("Инженер")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("deviceitem-edit")]
        [LabelHtmlClass("deviceitem-label")]
        [AjaxComboBox("AjaxEngineerComboBox")]
        [Required]
        public Guid? DeviceItemUserID { get; set; }

        /// <summary>
        /// Задает или получает дату выполненной работы.
        /// </summary>
        [DisplayName("Дата")]
        [EditorHtmlClass("deviceitem-edit")]
        [LabelHtmlClass("deviceitem-label")]
        [Required]
        public DateTime DeviceItemEventDate { get; set; }

        /// <summary>
        /// Наименование запчасти.
        /// </summary>
        [DisplayName("Наименование запчасти")]
        [EditorHtmlClass("deviceitem-edit")]
        [LabelHtmlClass("deviceitem-label")]
        public string DeviceItemTitle { get; set; }

        /// <summary>
        /// Задает или получает количество запчастей.
        /// </summary>
        [UIHint("Decimal")]
        [DisplayName("Количество")]
        [EditorHtmlClass("deviceitem-edit")]
        [LabelHtmlClass("deviceitem-label")]
        [Required]
        public decimal DeviceItemCount { get; set; }

        /// <summary>
        /// Задает или получает себестоимость.
        /// </summary>
        [UIHint("Decimal")]
        [DisplayName("Себестоимость")]
        [EditorHtmlClass("deviceitem-edit")]
        [LabelHtmlClass("deviceitem-label")]
        [Required]
        public decimal DeviceItemCostPrice { get; set; }

        /// <summary>
        /// Задает или получает окончательную цену.
        /// </summary>
        [UIHint("Decimal")]
        [DisplayName("Цена")]
        [EditorHtmlClass("deviceitem-edit")]
        [LabelHtmlClass("deviceitem-label")]
        [Required]
        public decimal DeviceItemPrice { get; set; }

        /// <summary>
        /// Задает или получает код номенклатуры.
        /// </summary>
        [DisplayName("Деталь со склада")]
        [UIHint("SingleLookup")]
        [SingleLookup("WarehouseItemSingleLookup", typeof(JLookupWarehouseItemSearchModel), null, true, true)]
        [EditorHtmlClass("deviceitem-edit")]
        [LabelHtmlClass("deviceitem-label")]
        public Guid? WarehouseItemID { get; set; }
    }
}