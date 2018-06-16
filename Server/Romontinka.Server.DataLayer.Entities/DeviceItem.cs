using System;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Замененные запчасти.
    /// </summary>
    public class DeviceItem:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код установленной запчасти.
        /// </summary>
        public Guid? DeviceItemID { get; set; }

        /// <summary>
        /// Наименование запчасти.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает количество запчастей.
        /// </summary>
        public decimal Count { get; set; }

        /// <summary>
        /// Задает или получает себестоимость.
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// Задает или получает окончательную цену.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        public Guid? RepairOrderID { get; set; }

        /// <summary>
        /// Дата установки.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает код установившего пользователя.
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        /// Задает или получает код установленной запчасти со склада.
        /// </summary>
        public Guid? WarehouseItemID { get; set; }

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
            return DeviceItemID;
        }
    }
}
