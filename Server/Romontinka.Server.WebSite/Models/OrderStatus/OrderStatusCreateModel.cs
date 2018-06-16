using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.User;

namespace Romontinka.Server.WebSite.Models.OrderStatus
{
    /// <summary>
    /// Модель для создания и редактирования статусов заказов.
    /// </summary>
    public class OrderStatusCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает название статуса.
        /// </summary>
        [DisplayName("Название:")]
        [Required]
        [EditorHtmlClass("orderstatus-edit")]
        [LabelHtmlClass("orderstatus-label")]
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает код типа статуса.
        /// </summary>
        [DisplayName("Тип:")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("orderstatus-edit")]
        [LabelHtmlClass("orderstatus-label")]
        [Required]
        [AjaxComboBox("AjaxStatusKind")]
        public byte? StatusKindID { get; set; }
    }
}