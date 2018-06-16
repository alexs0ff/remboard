using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Значение статей по доходам и расходам.
    /// </summary>
    public class FinancialItemValue:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код значения статьи по доходам и расходам.
        /// </summary>
        public Guid? FinancialItemValueID { get; set; }

        /// <summary>
        /// Задает или получает код финансовой группы.
        /// </summary>
        public Guid? FinancialGroupID { get; set; }

        /// <summary>
        /// Задает или получает код финансовой статьи.
        /// </summary>
        public Guid? FinancialItemID { get; set; }

        /// <summary>
        /// Задает или получает дату добавления значения.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает само значение.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Задает или получает себестоимость.
        /// </summary>
        public decimal? CostAmount { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (FinancialItemValue)entityBase;
            entity.Amount = Amount;
            entity.CostAmount = CostAmount;
            entity.Description = Description;
            entity.EventDate = EventDate;
            entity.FinancialGroupID = FinancialGroupID;
            entity.FinancialItemID = FinancialItemID;
            entity.FinancialItemValueID = FinancialItemValueID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return FinancialItemValueID;
        }
    }
}
