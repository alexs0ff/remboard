using System;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Выполненные работы.
    /// </summary>
    public class WorkItem:EntityBase<Guid>
    {
        private string _workItemID;

        /// <summary>
        /// Задает или получает пункт выполненной работы.
        /// </summary>
        public string WorkItemID
        {
            get { return _workItemID; }
            set { FormatUtils.ExchangeFields(ref _workItemIDGuid, ref _workItemID, value); }
        }

        private Guid? _workItemIDGuid;

        /// <summary>
        /// Задает или получает пункт выполненной работы.
        /// </summary>
        public Guid? WorkItemIDGuid
        {
            get { return _workItemIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _workItemIDGuid, ref _workItemID,value);
            }
        }

        private string _userID;

        /// <summary>
        /// Задает или получает код инженера.
        /// </summary>
        public string UserID
        {
            get { return _userID; }
            set { FormatUtils.ExchangeFields(ref _userIDGuid, ref _userID, value); }
        }

        private Guid? _userIDGuid;

        /// <summary>
        /// Задает или получает код инженера.
        /// </summary>
        public Guid? UserIDGuid
        {
            get { return _userIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _userIDGuid, ref _userID,value);
            }
        }

        /// <summary>
        /// Задает или получает наименование работы.
        /// </summary>
        public string Title { get; set; }

        private string _eventDate;

        /// <summary>
        /// Задает или получает дату выполненной работы.
        /// </summary>
        public string EventDate
        {
            get { return _eventDate; }
            set { FormatUtils.ExchangeFields(ref _eventDateDateTime, ref _eventDate, value); }
        }

        private DateTime _eventDateDateTime;

        /// <summary>
        /// Задает или получает дату выполненной работы.
        /// </summary>
        public DateTime EventDateDateTime
        {
            get { return _eventDateDateTime; }
            set
            {
                FormatUtils.ExchangeFields(ref _eventDateDateTime, ref _eventDate,value);
            }
        }

        /// <summary>
        /// Задает или получает стоимость работ.
        /// </summary>
        public double Price { get; set; }

        private string _repairOrderID;

        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        public string RepairOrderID
        {
            get { return _repairOrderID; }
            set { FormatUtils.ExchangeFields(ref _repairOrderIDGuid, ref _repairOrderID, value); }
        }

        private Guid? _repairOrderIDGuid;

        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        public Guid? RepairOrderIDGuid
        {
            get { return _repairOrderIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _repairOrderIDGuid, ref _repairOrderID, value);
            }
        }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (WorkItem) entityBase;
            entity.EventDate = EventDate;
            entity.Price = Price;
            entity.RepairOrderID = RepairOrderID;
            entity.Title = Title;
            entity.UserID = UserID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return WorkItemIDGuid;
        }
    }
}
