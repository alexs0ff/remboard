using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.Customer
{
    /// <summary>
    /// Модель для формы ввода глобального номера заказа.
    /// </summary>
    public class CustomerGetOrderModel
    {
        [DisplayName("Глобальный код заказа")]
        [Required]
        [EditorHtmlClass("customer-edit")]
        [LabelHtmlClass("customer-label")]
        public string GlobalRepairOrderNumber { get; set; }

        [DisplayName("Код доступа")]
        [Required]
        [EditorHtmlClass("customer-edit")]
        [LabelHtmlClass("customer-label")]
        public string GlobalRepairOrderAccessPassword { get; set; }
    }
}