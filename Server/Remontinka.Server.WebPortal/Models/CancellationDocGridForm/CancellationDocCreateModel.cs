using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.CancellationDocGridForm
{
    /// <summary>
    /// Модель формы создания документа списания.
    /// </summary>
    public class CancellationDocCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код документа списания со склада.
        /// </summary>
        public Guid? CancellationDocID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return CancellationDocID;
        }

        /// <summary>
        /// Задает или получает код склада.
        /// </summary>
        [DisplayName("Склад")]
        [Required]
        public Guid? WarehouseID { get; set; }


        /// <summary>
        /// Задает или получает номер документа
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
        /// Задает или получает описание.
        /// </summary>
        [DisplayName("Описание")]
        [Required]
        public string DocDescription { get; set; }
    }
}