using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.TransferDocItemGridForm
{
    /// <summary>
    /// Модель редактирования пунктов документа перемещения.
    /// </summary>
    public class TransferDocItemCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код элемента в документе перемещения со склада на склад.
        /// </summary>
        public Guid? TransferDocItemID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return TransferDocItemID;
        }

        /// <summary>
        /// Задает или получает код документа перемещения.
        /// </summary>
        public Guid? TransferDocID { get; set; }

        /// <summary>
        /// Задает или получает код номенклатуры.
        /// </summary>
        [DisplayName("Номенклатура")]
        [Required]
        public Guid? GoodsItemID { get; set; }

        /// <summary>
        /// Задает или получает количество элементов.
        /// </summary>
        [DisplayName("Количество")]
        [Required]
        public decimal Total { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        [DisplayName("Описание")]
        public string Description { get; set; }
    }
}