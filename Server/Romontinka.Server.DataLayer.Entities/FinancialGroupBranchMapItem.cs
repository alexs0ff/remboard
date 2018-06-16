using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Пункт соответвия финансовой группы и филиала.
    /// </summary>
    public class FinancialGroupBranchMapItem : EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код пункта соответствия финансовой группы и филиала.
        /// </summary>
        public Guid? FinancialGroupBranchMapID { get; set; }

        /// <summary>
        /// Задает или получает код филиала.
        /// </summary>
        public Guid? BranchID { get; set; }

        /// <summary>
        /// Задает или получает код финансовой группы.
        /// </summary>
        public Guid? FinancialGroupID { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (FinancialGroupBranchMapItem) entityBase;
            entity.FinancialGroupBranchMapID = FinancialGroupBranchMapID;
            entity.BranchID = BranchID;
            entity.FinancialGroupID = FinancialGroupID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return FinancialGroupBranchMapID;
        }
    }
}
