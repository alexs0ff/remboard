using System;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Статус заказа.
    /// </summary>
    public class OrderStatus:EntityBase<Guid>
    {
        private string _orderStatusID;

        /// <summary>
        /// Задает или получает код заказа.
        /// </summary>
        public string OrderStatusID
        {
            get { return _orderStatusID; }
            set { FormatUtils.ExchangeFields(ref _orderStatusIDGuid, ref _orderStatusID, value); }
        }

        private Guid? _orderStatusIDGuid;

        /// <summary>
        /// Задает или получает код заказа.
        /// </summary>
        public Guid? OrderStatusIDGuid
        {
            get { return _orderStatusIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _orderStatusIDGuid, ref _orderStatusID, value);
            }
        }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает код типа статуса.
        /// </summary>
        public long? StatusKindID { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (OrderStatus) entityBase;
            entity.OrderStatusID = OrderStatusID;
            entity.StatusKindID = StatusKindID;
            entity.Title = Title;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return OrderStatusIDGuid;
        }
    }
}
