using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.WarehouseGridForm
{
    public class WarehouseCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код склада.
        /// </summary>
        public Guid? WarehouseID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return WarehouseID;
        }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        [DisplayName("Название склада")]
        [Required]
        public string Title { get; set; }
    }
}