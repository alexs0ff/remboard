using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Задает или получает элемент документе перемещения со склада на склад.
    /// </summary>
    public class TransferDocItem:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код элемента в документе перемещения со склада на склад.
        /// </summary>
        public Guid? TransferDocItemID { get; set; }

        /// <summary>
        /// Задает или получает код элемента документа о перемещении со склада на склад.
        /// </summary>
        public Guid? TransferDocID { get; set; }

        /// <summary>
        /// Задает или получает код номенклатуры.
        /// </summary>
        public Guid? GoodsItemID { get; set; }

        /// <summary>
        /// Задает или получает количество элементов.
        /// </summary>
        public decimal Total { get; set; }

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
            var entity = (TransferDocItem) entityBase;
            entity.Description = Description;
            entity.GoodsItemID = GoodsItemID;
            entity.Total = Total;
            entity.TransferDocID = TransferDocID;
            entity.TransferDocItemID = TransferDocItemID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return TransferDocItemID;
        }
    }
}
