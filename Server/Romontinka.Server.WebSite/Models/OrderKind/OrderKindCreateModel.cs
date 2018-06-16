using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.OrderKind
{
    /// <summary>
    /// Модель для создания и редактирования типов заказа.
    /// </summary>
    public class OrderKindCreateModel: JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает название статуса.
        /// </summary>
        [DisplayName("Название:")]
        [Required]
        [EditorHtmlClass("orderkind-label")]
        [LabelHtmlClass("orderkind-label")]
        public string Title { get; set; }
    }
}