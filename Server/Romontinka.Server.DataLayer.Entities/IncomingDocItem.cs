using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Элемент приходной накладной.
    /// </summary>
    public class IncomingDocItem:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код элемента приходной накладной.
        /// </summary>
        public Guid? IncomingDocItemID { get; set; }

        /// <summary>
        /// Задает или получает код приходной накладной.
        /// </summary>
        public Guid? IncomingDocID { get; set; }

        /// <summary>
        /// Задает или получает код номенклатуры.
        /// </summary>
        public Guid? GoodsItemID { get; set; }

        /// <summary>
        /// Задает или получает количество элементов.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Задает или получает цену закупки.
        /// </summary>
        public decimal InitPrice { get; set; }

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
        /// Задает или получает описание.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (IncomingDocItem) entityBase;
            entity.Description = Description;
            entity.GoodsItemID = GoodsItemID;
            entity.IncomingDocID = IncomingDocID;
            entity.IncomingDocItemID = IncomingDocItemID;
            entity.InitPrice = InitPrice;
            entity.RepairPrice = RepairPrice;
            entity.SalePrice = SalePrice;
            entity.StartPrice = StartPrice;
            entity.Total = Total;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return IncomingDocItemID;
        }
    }
}
