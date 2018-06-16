using System;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Тип заказа.
    /// </summary>
    public class OrderKind:EntityBase<Guid>
    {
        private string _orderKindID;

        /// <summary>
        /// Задает или получает тип заказа.
        /// </summary>
        public string OrderKindID
        {
            get { return _orderKindID; }
            set { FormatUtils.ExchangeFields(ref _orderKindIDGuid, ref _orderKindID, value); }
        }

        private Guid? _orderKindIDGuid;

        /// <summary>
        /// Задает или получает тип заказа.
        /// </summary>
        public Guid? OrderKindIDGuid
        {
            get { return _orderKindIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _orderKindIDGuid, ref _orderKindID, value);
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
            var entity = (OrderKind) entityBase;
            entity.OrderKindID = OrderKindID;
            entity.Title = Title;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return OrderKindIDGuid;
        }
    }
}
