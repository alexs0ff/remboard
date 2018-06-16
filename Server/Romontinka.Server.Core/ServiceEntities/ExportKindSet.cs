using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Core.ServiceEntities
{
    /// <summary>
    /// Набор типов экпортируемых данных.
    /// </summary>
    public static class ExportKindSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        static ExportKindSet()
        {
            OnlyOrders = new ExportKind { KindID = 1,Title = "Только заказы"};
            Kinds = new Collection<ExportKind> {OnlyOrders};
        }

        /// <summary>
        ///   Список всех типов экспортируемых данных.
        /// </summary>
        public static ICollection<ExportKind> Kinds { get; private set; }

        /// <summary>
        /// Только заказы.
        /// </summary>
        public static ExportKind OnlyOrders { get; private set; }

        /// <summary>
        ///   Возвращает тип измерения по его коду.
        /// </summary>
        /// <returns> Тип экспортируемых данных или null. </returns>
        public static ExportKind GetKindByID(int? id)
        {
            return Kinds.FirstOrDefault(a => a.KindID == id);
        }
    }
}
