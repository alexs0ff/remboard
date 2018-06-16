using System;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Замененные запчасти.
    /// </summary>
    public class DeviceItem:EntityBase<Guid>
    {
        private string _deviceItemID;

        /// <summary>
        /// Задает или получает код установленной запчасти.
        /// </summary>
        public string DeviceItemID
        {
            get { return _deviceItemID; }
            set
            {
                FormatUtils.ExchangeFields(ref _deviceItemIDGuid, ref _deviceItemID,value);
            }
        }

        private Guid? _deviceItemIDGuid;

        /// <summary>
        /// Задает или получает код установленной запчасти.
        /// </summary>
        public Guid? DeviceItemIDGuid
        {
            get { return _deviceItemIDGuid; }
            set { FormatUtils.ExchangeFields(ref _deviceItemIDGuid, ref _deviceItemID, value); }
        }

        /// <summary>
        /// Наименование запчасти.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает количество запчастей.
        /// </summary>
        public double Count { get; set; }

        /// <summary>
        /// Задает или получает себестоимость.
        /// </summary>
        public double CostPrice { get; set; }

        /// <summary>
        /// Задает или получает окончательную цену.
        /// </summary>
        public double Price { get; set; }

        private string _repairOrderID;

        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        public string RepairOrderID
        {
            get { return _repairOrderID; }
            set
            {
                FormatUtils.ExchangeFields(ref _repairOrderIDGuid, ref _repairOrderID, value);
            }
        }

        private Guid? _repairOrderIDGuid;

        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        public Guid? RepairOrderIDGuid
        {
            get { return _repairOrderIDGuid; }
            set { FormatUtils.ExchangeFields(ref _repairOrderIDGuid, ref _repairOrderID, value); }
        }

        private string _eventDate;

        /// <summary>
        /// Дата установки.
        /// </summary>
        public string EventDate
        {
            get { return _eventDate; }
            set
            {
                FormatUtils.ExchangeFields(ref _eventDateDateTime, ref _eventDate, value);
            }
        }

        private DateTime _eventDateDateTime;

        /// <summary>
        /// Дата установки.
        /// </summary>
        public DateTime EventDateDateTime
        {
            get { return _eventDateDateTime; }
            set { FormatUtils.ExchangeFields(ref _eventDateDateTime, ref _eventDate, value); }
        }

        private string _userID;

        /// <summary>
        /// Задает или получает код установившего пользователя.
        /// </summary>
        public string UserID
        {
            get { return _userID; }
            set { FormatUtils.ExchangeFields(ref _userIDGuid, ref _userID, value); }
        }

        private Guid? _userIDGuid;

        /// <summary>
        /// Задает или получает код установившего пользователя.
        /// </summary>
        public Guid? UserIDGuid
        {
            get { return _userIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _userIDGuid, ref _userID, value);
            }
        }

        private string _warehouseItemID;

        /// <summary>
        /// Задает или получает код установленной запчасти со склада.
        /// </summary>
        public string WarehouseItemID
        {
            get { return _warehouseItemID; }
            set
            {
                FormatUtils.ExchangeFields(ref _warehouseItemIDGuid, ref _warehouseItemID,value);
            }
        }

        private Guid? _warehouseItemIDGuid;

        /// <summary>
        /// Задает или получает код установленной запчасти со склада.
        /// </summary>
        public Guid? WarehouseItemIDGuid
        {
            get { return _warehouseItemIDGuid; }
            set { FormatUtils.ExchangeFields(ref _warehouseItemIDGuid, ref _warehouseItemID, value); }
        }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (DeviceItem) entityBase;
            entity.CostPrice = CostPrice;
            entity.Count = Count;
            entity.DeviceItemID = DeviceItemID;
            entity.Price = Price;
            entity.RepairOrderID = RepairOrderID;
            entity.Title = Title;
            entity.EventDate = EventDate;
            entity.UserID = UserID;
            entity.WarehouseItemID = WarehouseItemID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return DeviceItemIDGuid;
        }
    }
}
