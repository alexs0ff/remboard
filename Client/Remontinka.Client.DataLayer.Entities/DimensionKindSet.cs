using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Список измерений.
    /// </summary>
    public static class DimensionKindSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        static DimensionKindSet()
        {
            Thing = new DimensionKind {ShortTitle = "шт", Title = "Штуки", DimensionKindID = 1};
            Kinds = new Collection<DimensionKind> {Thing};
        }

        /// <summary>
        ///   Список всех типов измерений.
        /// </summary>
        public static ICollection<DimensionKind> Kinds { get; private set; }

        /// <summary>
        /// Штука.
        /// </summary>
        public static DimensionKind Thing { get; private set; }

        /// <summary>
        ///   Возвращает тип измерения по его коду.
        /// </summary>
        /// <returns> Тип документа или null. </returns>
        public static DimensionKind GetKindByID(byte? id)
        {
            return Kinds.FirstOrDefault(a => a.DimensionKindID == id);
        }
    }
}
