using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Типы операций (доход или расход).
    /// </summary>
    public static class TransactionKindSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Romontinka.Server.DataLayer.Entities.TransactionKindSet"/> class.
        /// </summary>
        static TransactionKindSet()
        {
            Revenue = new TransactionKind {Title = "Доход", TransactionKindID = 1};
            Expenditure = new TransactionKind {Title = "Расход", TransactionKindID = 2};
            Kinds = new[] {Revenue, Expenditure};
        }

        /// <summary>
        /// Расход.
        /// </summary>
        public static TransactionKind Expenditure { get; private set; }

        /// <summary>
        /// Доход.
        /// </summary>
        public static TransactionKind Revenue { get; private set; }

        /// <summary>
        ///   Возвращает тип по его коду.
        /// </summary>
        /// <returns> Тип или null. </returns>
        public static TransactionKind GetKindByID(byte? id)
        {
            return Kinds.FirstOrDefault(a => a.TransactionKindID == id);
        }

        /// <summary>
        ///   Список всех типов операций.
        /// </summary>
        public static ICollection<TransactionKind> Kinds { get; private set; }
    }
}
