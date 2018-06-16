using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.TransferDocGridForm
{
    /// <summary>
    /// Модель редактирования документа перемещения.
    /// </summary>
    public class TransferDocCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код документа перемещения .
        /// </summary>
        public Guid? TransferDocID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return TransferDocID;
        }

        /// <summary>
        /// Задает или получает код склада откуда перемещается товар.
        /// </summary>
        [DisplayName("Из склада")]
        [Required]
        public Guid? SenderWarehouseID { get; set; }

        /// <summary>
        /// Задает или получает код получаещего склада.
        /// </summary>
        [DisplayName("На склад")]
        [Required]
        public Guid? RecipientWarehouseID { get; set; }

        /// <summary>
        /// Задает или получает номер документа.
        /// </summary>
        [DisplayName("Номер документа")]
        [Required]
        public string DocNumber { get; set; }

        /// <summary>
        /// Задает или получает дату документа.
        /// </summary>
        [DisplayName("Дата документа")]
        [Required]
        public DateTime DocDate { get; set; }

        /// <summary>
        /// Задает или получает описание документа.
        /// </summary>
        [DisplayName("Описание")]
        [Required]
        public string DocDescription { get; set; }
    }
}