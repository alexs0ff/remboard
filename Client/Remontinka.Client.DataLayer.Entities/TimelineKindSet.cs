using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Типы событий графика.
    /// </summary>
    public static class TimelineKindSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        static TimelineKindSet()
        {
            ManagerAssigned = new TimelineKind { TimelineKindID = 1, Title = "Назначен менеджер" };
            EngineerAssigned = new TimelineKind { TimelineKindID = 2, Title = "Назначен исполнитель" };
            WorkAdded = new TimelineKind { TimelineKindID = 3, Title = "Добавлена работа" };
            DeviceItemAdded = new TimelineKind { TimelineKindID = 4, Title = "Добавлена запчасть" };
            CommentAdded = new TimelineKind { TimelineKindID = 5, Title = "Добавлен комментарий" };
            StatusChanged = new TimelineKind { TimelineKindID = 7, Title = "Изменен статус заказа" };
            Completed = new TimelineKind { TimelineKindID = 100, Title = "Товар выдан" };
            Kinds = new Collection<TimelineKind>(new[] { ManagerAssigned, EngineerAssigned, WorkAdded, DeviceItemAdded, CommentAdded, StatusChanged, Completed });
        }

        /// <summary>
        ///   Возвращает тип по его коду.
        /// </summary>
        /// <returns> Тип или null. </returns>
        public static TimelineKind GetKindByID(byte? id)
        {
            return Kinds.FirstOrDefault(a => a.TimelineKindID == id);
        }

        /// <summary>
        ///   Список всех типов графика.
        /// </summary>
        public static ICollection<TimelineKind> Kinds { get; private set; }

        /// <summary>
        /// Назначен менеджер.
        /// </summary>
        public static TimelineKind ManagerAssigned { get; private set; }

        /// <summary>
        /// Назначен исполнитель.
        /// </summary>
        public static TimelineKind EngineerAssigned { get; private set; }

        /// <summary>
        /// Добавлена работа.
        /// </summary>
        public static TimelineKind WorkAdded { get; private set; }

        /// <summary>
        /// Добавлена Запчасть.
        /// </summary>
        public static TimelineKind DeviceItemAdded { get; private set; }

        /// <summary>
        /// Добавлен комментарий.
        /// </summary>
        public static TimelineKind CommentAdded { get; private set; }

        /// <summary>
        /// Изменен статус заказа.
        /// </summary>
        public static TimelineKind StatusChanged { get; private set; }

        /// <summary>
        /// Товар выдан.
        /// </summary>
        public static TimelineKind Completed { get; private set; }


    }
}
