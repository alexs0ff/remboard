using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Пункт соответвия финансовой группы и склада.
    /// </summary>
    public class FinancialGroupWarehouseMapItem:EntityBase<Guid>
    {
        /// <summary>
        /// Код пункта соответствия финансовой группы и склада.
        /// </summary>
        public Guid? FinancialGroupWarehouseMapID { get; set; }

        /// <summary>
        /// Задает или получает код связанного склада.
        /// </summary>
        public Guid? WarehouseID { get; set; }

        /// <summary>
        /// Задает или получает код связанной группы.
        /// </summary>
        public Guid? FinancialGroupID { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (FinancialGroupWarehouseMapItem)entityBase;
            entity.FinancialGroupID = FinancialGroupID;
            entity.FinancialGroupWarehouseMapID = FinancialGroupWarehouseMapID;
            entity.WarehouseID = WarehouseID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return FinancialGroupWarehouseMapID;
        }
    }
}
