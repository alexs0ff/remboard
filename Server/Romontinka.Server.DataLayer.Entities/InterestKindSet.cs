using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Список типов вознаграждения.
    /// </summary>
    public static class InterestKindSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        static InterestKindSet()
        {
            Empty = new InterestKind { InterestKindID = 0,Title = "Отсутствует"};
            Percent = new InterestKind { InterestKindID = 1, Title = "Процент" };
            Kinds = new[]
                    {
                        Empty,
                        Percent
                    };
        }

        /// <summary>
        ///   Список всех типов вознаграждений.
        /// </summary>
        public static ICollection<InterestKind> Kinds { get; private set; }

        /// <summary>
        ///   Возвращает тип вознаграждения по его коду.
        /// </summary>
        /// <returns> Тип вознаграждения или null. </returns>
        public static InterestKind GetKindByID(byte? id)
        {
            return Kinds.FirstOrDefault(a => a.InterestKindID == id);
        }

        /// <summary>
        /// Пустой статус.
        /// </summary>
        public static InterestKind Empty { get; private set; }

        /// <summary>
        /// Процент.
        /// </summary>
        public static InterestKind Percent { get; private set; }
    }
}
