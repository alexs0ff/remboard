using System;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Представляет отбъект класса.
    /// </summary>
    public class Warehouse:EntityBase<Guid>
    {
        private string _warehouseID;

        /// <summary>
        /// Задает или получает код склада.
        /// </summary>
        public string WarehouseID
        {
            get { return _warehouseID; }
            set
            {
                FormatUtils.ExchangeFields(ref _warehouseIDGuid, ref _warehouseID, value);
            }
        }

        private Guid? _warehouseIDGuid;

        /// <summary>
        /// Задает или получает код склада.
        /// </summary>
        public Guid? WarehouseIDGuid
        {
            get { return _warehouseIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _warehouseIDGuid, ref _warehouseID, value);
            }
        }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (Warehouse) entityBase;
            entity.Title = Title;
            entity.WarehouseID = WarehouseID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return WarehouseIDGuid;
        }
    }
}
