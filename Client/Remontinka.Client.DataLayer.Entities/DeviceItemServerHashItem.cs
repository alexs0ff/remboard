using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Хэш серверной установленной запчасти.
    /// </summary>
    public class DeviceItemServerHashItem:EntityBase<Guid>
    {
        private string _deviceItemServerHashID;

        /// <summary>
        /// Задает или получает код серверного хэша проделанной работы
        /// </summary>
        public string DeviceItemServerHashID
        {
            get { return _deviceItemServerHashID; }
            set { FormatUtils.ExchangeFields(ref _deviceItemServerHashIDGuid, ref _deviceItemServerHashID, value); }
        }

        private Guid? _deviceItemServerHashIDGuid;

        /// <summary>
        /// Задает или получает код серверного хэша проделанной работы
        /// </summary>
        public Guid? DeviceItemServerHashIDGuid
        {
            get { return _deviceItemServerHashIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _deviceItemServerHashIDGuid, ref _deviceItemServerHashID, value);
            }
        }

        private string _repairOrderServerHashID;

        /// <summary>
        /// Задает или получает код заказа.
        /// </summary>
        public string RepairOrderServerHashID
        {
            get { return _repairOrderServerHashID; }
            set { FormatUtils.ExchangeFields(ref _repairOrderServerHashIDGuid, ref _repairOrderServerHashID, value); }
        }

        private Guid? _repairOrderServerHashIDGuid;

        /// <summary>
        /// Задает или получает код заказа.
        /// </summary>
        public Guid? RepairOrderServerHashIDGuid
        {
            get { return _repairOrderServerHashIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _repairOrderServerHashIDGuid, ref _repairOrderServerHashID, value);
            }
        }

        /// <summary>
        /// Задает или получает хэш данных.
        /// </summary>
        public string DataHash { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (DeviceItemServerHashItem)entityBase;
            entity.DataHash = DataHash;
            entity.RepairOrderServerHashID = RepairOrderServerHashID;
            entity.DeviceItemServerHashID = DeviceItemServerHashID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return DeviceItemServerHashIDGuid;
        }
    }
}
