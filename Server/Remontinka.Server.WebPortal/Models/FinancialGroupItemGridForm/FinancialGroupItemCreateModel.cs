using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.FinancialGroupItemGridForm
{
    /// <summary>
    /// Модель редактирования финансовой группы.
    /// </summary>
    public class FinancialGroupItemCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код финансовой группы.
        /// </summary>
        public Guid? FinancialGroupID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return FinancialGroupID;
        }

        /// <summary>
        /// Задает или получает название финансовой группы.
        /// </summary>
        [DisplayName("Название")]
        [Required]
        
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает юр название фирмы.
        /// </summary>
        [DisplayName("Юр название")]
        [Required]
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает торговую марку фирмы.
        /// </summary>
        [DisplayName("Торговая марка")]
        [Required]
        public string Trademark { get; set; }

        /// <summary>
        /// Задает или получает список филиалов.
        /// </summary>
        [DisplayName("Филиалы")]
        public Guid?[] BranchIds { get; set; }

        /// <summary>
        /// Задает или получает список складов.
        /// </summary>
        [DisplayName("Склады")]
        public Guid?[] WarehouseIds { get; set; }

        /// <summary>
        /// Содержит название свойства для выбора филиалов.
        /// </summary>
        public const string BranchIdsCheckListPropertyName = "BranchIdsUnbound";

        /// <summary>
        /// Содержит название свойства для выбора складов.
        /// </summary>
        public const string WarehouseIdsCheckListPropertyName = "WarehouseIdsUnbound";
    }
}