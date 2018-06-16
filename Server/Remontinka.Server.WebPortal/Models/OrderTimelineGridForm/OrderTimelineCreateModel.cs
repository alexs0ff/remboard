using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.OrderTimelineGridForm
{
    /// <summary>
    /// Модель создания комментария.
    /// </summary>
    public class OrderTimelineCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код комментария.
        /// </summary>
        public Guid? OrderTimelineID { get; set; }

        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        public Guid? RepairOrderID { get; set; }

        /// <summary>
        /// Задает или получает название комментария.
        /// </summary>
        [DisplayName("Комментарий")]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return OrderTimelineID;
        }
    }
}