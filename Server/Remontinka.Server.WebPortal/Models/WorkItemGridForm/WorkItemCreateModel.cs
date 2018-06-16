using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.WorkItemGridForm
{
    /// <summary>
    /// Модель создания или редактирования выполненных работ.
    /// </summary>
    public class WorkItemCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код выполненной работы.
        /// </summary>
        public Guid? WorkItemID { get; set; }

        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        public Guid? RepairOrderID { get; set; }

        /// <summary>
        /// Задает или получает код инженера.
        /// </summary>
        [DisplayName("Инженер")]
        [Required]
        public Guid? WorkItemUserID { get; set; }

        /// <summary>
        /// Задает или получает наименование работы.
        /// </summary>
        [Required]
        [DisplayName("Наименование работы")]
        public string WorkItemTitle { get; set; }

        /// <summary>
        /// Задает или получает дату выполненной работы.
        /// </summary>
        [DisplayName("Дата выполнения")]
        [Required]
        public DateTime WorkItemEventDate { get; set; }

        /// <summary>
        /// Задает или получает стоимость работ.
        /// </summary>
        [DisplayName("Стоимость")]
        [Required]
        public decimal WorkItemPrice { get; set; }

        /// <summary>
        /// Задает или получает описание работы.
        /// </summary>
        [DisplayName("Описание")]
        public string WorkItemNotes { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return WorkItemID;
        }
    }
}