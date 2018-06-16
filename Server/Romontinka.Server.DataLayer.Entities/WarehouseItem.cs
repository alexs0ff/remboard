using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Элемент остатка на складе.
    /// </summary>
    public class WarehouseItem:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код элемента остатка на складе.
        /// </summary>
        public Guid? WarehouseItemID { get; set; }

        /// <summary>
        /// Задает или получает код склада.
        /// </summary>
        public Guid? WarehouseID { get; set; }

        /// <summary>
        /// Задает или получает код номенклатуры.
        /// </summary>
        public Guid? GoodsItemID { get; set; }

        /// <summary>
        /// Задает или получает количество.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Задает или получает нулевую цену.
        /// </summary>
        public decimal StartPrice { get; set; }

        /// <summary>
        /// Задает или получает ремонтную цену.
        /// </summary>
        public decimal RepairPrice { get; set; }

        /// <summary>
        /// Задает или получает цену продажи.
        /// </summary>
        public decimal SalePrice { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (WarehouseItem) entityBase;
            entity.GoodsItemID = GoodsItemID;
            entity.RepairPrice = RepairPrice;
            entity.SalePrice = SalePrice;
            entity.StartPrice = StartPrice;
            entity.Total = Total;
            entity.WarehouseID = WarehouseID;
            entity.WarehouseItemID = WarehouseItemID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return WarehouseItemID;
        }
    }
}
