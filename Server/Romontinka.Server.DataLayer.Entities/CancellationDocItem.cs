using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Задает или получает элемент документа списания.
    /// </summary>
    public class CancellationDocItem:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код элемента документа списания.
        /// </summary>
        public Guid? CancellationDocItemID { get; set; }

        /// <summary>
        /// Задает или получает код документа списания.
        /// </summary>
        public Guid? CancellationDocID { get; set; }

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
            var entity = (CancellationDocItem) entityBase;
            entity.CancellationDocID = CancellationDocID;
            entity.CancellationDocItemID = CancellationDocItemID;
            entity.Description = Description;
            entity.GoodsItemID = GoodsItemID;
            entity.Total = Total;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return CancellationDocItemID;
        }
    }
}
