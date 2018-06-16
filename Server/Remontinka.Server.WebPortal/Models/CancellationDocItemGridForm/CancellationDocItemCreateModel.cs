using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.CancellationDocItemGridForm
{
    /// <summary>
    /// Модель редактирования пункта документа списания.
    /// </summary>
    public class CancellationDocItemCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код элемента документа списания.
        /// </summary>
        public Guid? CancellationDocItemID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return CancellationDocItemID;
        }

        /// <summary>
        /// Задает или получает код документа списания.
        /// </summary>
        public Guid? CancellationDocID { get; set; }

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