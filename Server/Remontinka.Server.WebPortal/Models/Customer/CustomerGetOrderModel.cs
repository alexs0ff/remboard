using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.Customer
{
    /// <summary>
    /// Модель для формы ввода глобального номера заказа.
    /// </summary>
    public class CustomerGetOrderModel
    {
        [DisplayName("Глобальный код заказа")]
        [Required]
        public string GlobalRepairOrderNumber { get; set; }

        [DisplayName("Код доступа")]
        [Required]
        public string GlobalRepairOrderAccessPassword { get; set; }
    }
}