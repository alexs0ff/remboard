using System;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// График заказа.
    /// </summary>
    public class OrderTimeline:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код графика.
        /// </summary>
        public Guid? OrderTimelineID { get; set; }

        /// <summary>
        /// Задает или получает тип графика.
        /// </summary>
        public byte? TimelineKindID { get; set; }

        /// <summary>
        /// Задает или получает код связанного запроса.
        /// </summary>
        public Guid? RepairOrderID { get; set; }

        /// <summary>
        /// Задает или получает дату и время события.
        /// </summary>
        public DateTime EventDateTime { get; set; }

        /// <summary>
        /// Задает или получает описание события.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (OrderTimeline) entityBase;
            entity.EventDateTime = EventDateTime;
            entity.OrderTimelineID = OrderTimelineID;
            entity.RepairOrderID = RepairOrderID;
            entity.TimelineKindID = TimelineKindID;
            entity.Title = Title;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return OrderTimelineID;
        }
    }
}
