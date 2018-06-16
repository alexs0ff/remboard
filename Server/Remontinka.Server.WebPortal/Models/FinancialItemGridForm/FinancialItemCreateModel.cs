using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.FinancialItemGridForm
{
    /// <summary>
    /// Модель для редактирования статьи бюджета.
    /// </summary>
    public class FinancialItemCreateModel: GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код статьи бюджета.
        /// </summary>
        public Guid? FinancialItemID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return FinancialItemID;
        }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        [DisplayName("Название")]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает описание статьи.
        /// </summary>
        [DisplayName("Описание")]
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Задает или получает тип статьи бюджета.
        /// </summary>
        [DisplayName("Тип статьи")]
        [Required]
        public int? FinancialItemKindID { get; set; }

        /// <summary>
        /// Задает или получает код типа операции.
        /// </summary>
        [DisplayName("Доход/расход")]
        [Required]
        public byte? TransactionKindID { get; set; }

        /// <summary>
        /// Задает или получает дату введения статьи.
        /// </summary>
        [DisplayName("Дата")]
        [Required]
        public DateTime EventDate { get; set; }
    }
}