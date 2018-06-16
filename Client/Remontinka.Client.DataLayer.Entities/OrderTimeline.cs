using System;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// График заказа.
    /// </summary>
    public class OrderTimeline:EntityBase<Guid>
    {
        private string _orderTimelineID;

        /// <summary>
        /// Задает или получает код графика.
        /// </summary>
        public string OrderTimelineID
        {
            get { return _orderTimelineID; }
            set { FormatUtils.ExchangeFields(ref _orderTimelineIDGuid, ref _orderTimelineID, value); }
        }

        private Guid? _orderTimelineIDGuid;

        /// <summary>
        /// Задает или получает код графика.
        /// </summary>
        public Guid? OrderTimelineIDGuid
        {
            get { return _orderTimelineIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _orderTimelineIDGuid, ref _orderTimelineID,value);
            }
        }

        /// <summary>
        /// Задает или получает тип графика.
        /// </summary>
        public long? TimelineKindID { get; set; }

        private string _repairOrderID;

        /// <summary>
        /// Задает или получает код связанного запроса.
        /// </summary>
        public string RepairOrderID
        {
            get { return _repairOrderID; }
            set { FormatUtils.ExchangeFields(ref _repairOrderIDGuid, ref _repairOrderID, value); }
        }

        private Guid? _repairOrderIDGuid;

        /// <summary>
        /// Задает или получает код связанного запроса.
        /// </summary>
        public Guid? RepairOrderIDGuid
        {
            get { return _repairOrderIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _repairOrderIDGuid, ref _repairOrderID,value);
            }
        }

        private string _eventDateTime;

        /// <summary>
        /// Задает или получает дату и время события.
        /// </summary>
        public string EventDateTime
        {
            get { return _eventDateTime; }
            set { FormatUtils.ExchangeFields(ref _eventDateTimeDateTime, ref _eventDateTime, value); }
        }

        private DateTime _eventDateTimeDateTime;

        /// <summary>
        /// Задает или получает дату и время события.
        /// </summary>
        public DateTime EventDateTimeDateTime
        {
            get { return _eventDateTimeDateTime; }
            set
            {
                FormatUtils.ExchangeFields(ref _eventDateTimeDateTime, ref _eventDateTime,value);
            }
        }

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
            return OrderTimelineIDGuid;
        }
    }
}
