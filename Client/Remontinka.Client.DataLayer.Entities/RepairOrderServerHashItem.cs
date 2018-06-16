using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Хэш серверных данных по заказам.
    /// </summary>
    public class RepairOrderServerHashItem:EntityBase<Guid>
    {
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
                FormatUtils.ExchangeFields(ref _repairOrderServerHashIDGuid, ref _repairOrderServerHashID,value);
            }
        }

        /// <summary>
        /// Задает или получает хэш данных.
        /// </summary>
        public string DataHash { get; set; }

        /// <summary>
        /// Задает или получает количество пунктов графика.
        /// </summary>
        public long OrderTimelinesCount { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (RepairOrderServerHashItem) entityBase;
            entity.OrderTimelinesCount = OrderTimelinesCount;
            entity.RepairOrderServerHashID = RepairOrderServerHashID;
            entity.DataHash = DataHash;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return RepairOrderServerHashIDGuid;
        }
    }
}
