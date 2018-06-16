using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.WorkItemForm
{
    /// <summary>
    /// Модель создания или редактирования выполненных работ.
    /// </summary>
    public class WorkItemCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        public Guid? RepairOrderID { get; set; }

        /// <summary>
        /// Задает или получает код инженера.
        /// </summary>
        [DisplayName("Инженер")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("workitem-edit")]
        [LabelHtmlClass("workitem-label")]
        [AjaxComboBox("AjaxEngineerComboBox")]
        [Required]
        public Guid? WorkItemUserID { get; set; }

        /// <summary>
        /// Задает или получает наименование работы.
        /// </summary>
        [Required]
        [DisplayName("Наименование работы")]
        [EditorHtmlClass("workitem-edit")]
        [LabelHtmlClass("workitem-label")]
        public string WorkItemTitle { get; set; }

        /// <summary>
        /// Задает или получает дату выполненной работы.
        /// </summary>
        [DisplayName("Дата выполнения")]
        [EditorHtmlClass("workitem-edit")]
        [LabelHtmlClass("workitem-label")]
        [Required]
        public DateTime WorkItemEventDate { get; set; }

        /// <summary>
        /// Задает или получает стоимость работ.
        /// </summary>
        [UIHint("Decimal")]
        [DisplayName("Стоимость")]
        [EditorHtmlClass("workitem-edit")]
        [LabelHtmlClass("workitem-label")]
        [Required]
        public decimal WorkItemPrice { get; set; }

        /// <summary>
        /// Задает или получает описание работы.
        /// </summary>
        [DisplayName("Описание")]
        [EditorHtmlClass("workitem-edit")]
        [LabelHtmlClass("workitem-label")]
        [UIHint("MultilineString")]
        [MultilineString(2, 3)]
        public string WorkItemNotes { get; set; }
    }
}