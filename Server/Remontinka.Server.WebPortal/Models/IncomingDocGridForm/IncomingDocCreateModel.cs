using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.IncomingDocGridForm
{
    /// <summary>
    /// Модель редактирования входящих документов.
    /// </summary>
    public class IncomingDocCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код приходной накладной.
        /// </summary>
        public Guid? IncomingDocID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return IncomingDocID;
        }

        /// <summary>
        /// Задает или получает код склада.
        /// </summary>
        [DisplayName("Склад")]
        [Required]
        public Guid? WarehouseID { get; set; }

        /// <summary>
        /// Задает или получает код контрагента.
        /// </summary>
        [DisplayName("Контрагент")]
        [Required]
        public Guid? ContractorID { get; set; }

        /// <summary>
        /// Задает или получает дату накладной.
        /// </summary>
        [DisplayName("Дата накладной")]
        [Required]
        public DateTime DocDate { get; set; }

        /// <summary>
        /// Задает или получает номер накладной.
        /// </summary>
        [DisplayName("Номер документа")]
        [Required]
        public string DocNumber { get; set; }

        /// <summary>
        /// Задает или получает описание документа.
        /// </summary>
        [DisplayName("Описание накладной")]
        [Required]
        public string DocDescription { get; set; }
    }
}