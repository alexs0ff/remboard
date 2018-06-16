using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Список типов статусов заказа.
    /// </summary>
    public static class StatusKindSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        static StatusKindSet()
        {
            New = new StatusKind {StatusKindID = 1,Title = "Новые"};
            OnWork = new StatusKind { StatusKindID = 2, Title = "На исполнении" };
            Suspended = new StatusKind { StatusKindID = 3, Title = "Отложенные" };
            Completed = new StatusKind { StatusKindID = 4, Title = "Исполненные" };
            Closed = new StatusKind { StatusKindID = 5, Title = "Закрытые" };
            Statuses = new Collection<StatusKind>(new[] { New, OnWork, Suspended, Completed, Closed });
        }

        /// <summary>
        ///   Список всех типов статусов.
        /// </summary>
        public static ICollection<StatusKind> Statuses { get; private set; }

        /// <summary>
        ///   Возвращает статус по его коду.
        /// </summary>
        /// <returns> Тип статуса или null. </returns>
        public static StatusKind GetKindByID(byte? id)
        {
            return Statuses.FirstOrDefault(a => a.StatusKindID == id);
        }

        /// <summary>
        /// Новый статус.
        /// </summary>
        public static StatusKind New { get; private set; }

        /// <summary>
        /// На исполнении.
        /// </summary>
        public static StatusKind OnWork { get; private set; }

        /// <summary>
        /// Отложенные.
        /// </summary>
        public static StatusKind Suspended { get; private set; }

        /// <summary>
        /// Исполненные.
        /// </summary>
        public static StatusKind Completed { get; private set; }

        /// <summary>
        /// Закрытые.
        /// </summary>
        public static StatusKind Closed { get; private set; }

    }
}
