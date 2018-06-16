using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.OrderKindGridForm
{
    /// <summary>
    /// Модель редактирования типа заказа.
    /// </summary>
    public class OrderKindCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает название типа.
        /// </summary>
        [DisplayName("Название:")]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает код типа заказа.
        /// </summary>
        public Guid? OrderKindID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return OrderKindID;
        }
    }
}