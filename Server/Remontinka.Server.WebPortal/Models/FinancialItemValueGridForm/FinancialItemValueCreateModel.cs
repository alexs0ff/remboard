using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.FinancialItemValueGridForm
{
    /// <summary>
    /// Модель редактирования статьи по доходам и расходам.
    /// </summary>
    public class FinancialItemValueCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код значения статьи по доходам и расходам.
        /// </summary>
        public Guid? FinancialItemValueID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return FinancialItemValueID;
        }

        /// <summary>
        /// Задает или получает код статьи бюджета.
        /// </summary>
        [Required]
        [DisplayName("Статья бюджета")]
        public Guid? FinancialItemID { get; set; }

        /// <summary>
        /// Задает или получает код финансовой группы.
        /// </summary>
        [DisplayName("Фингруппа")]
        [Required]
        public Guid? FinancialGroupID { get; set; }

        /// <summary>
        /// Задает или получает дату добавления значения.
        /// </summary>
        [DisplayName("Дата")]
        [Required]
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает само значение.
        /// </summary>
        [DisplayName("Сумма")]
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        /// Задает или получает себестоимость.
        /// </summary>
        [DisplayName("Себестоимость")]
        public decimal? CostAmount { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        [DisplayName("Примечание")]
        public string Description { get; set; }
    }
}