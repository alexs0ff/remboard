using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.TransferDocForm
{
    /// <summary>
    /// Модель для редактирования и создания документов о перемещении между складами.
    /// </summary>
    public class TransferDocCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код склада откуда перемещается товар.
        /// </summary>
        [DisplayName("Из склада")]
        [UIHint("AjaxComboBox")]
        [AjaxComboBox("AjaxWarehouseComboBox")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public Guid? SenderWarehouseID { get; set; }

        /// <summary>
        /// Задает или получает код получаещего склада.
        /// </summary>
        [DisplayName("На склад")]
        [UIHint("AjaxComboBox")]
        [AjaxComboBox("AjaxWarehouseComboBox")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public Guid? RecipientWarehouseID { get; set; }

        /// <summary>
        /// Задает или получает номер документа.
        /// </summary>
        [DisplayName("Номер документа")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string DocNumber { get; set; }

        /// <summary>
        /// Задает или получает дату документа.
        /// </summary>
        [DisplayName("Дата документа")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        [Required]
        public DateTime DocDate { get; set; }

        /// <summary>
        /// Задает или получает описание документа.
        /// </summary>
        [DisplayName("Описание")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        [UIHint("MultilineString")]
        [MultilineString(3, 5)]
        public string DocDescription { get; set; }
    }
}