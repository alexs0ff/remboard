using System;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Статус заказа.
    /// </summary>
    public class OrderStatus:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код заказа.
        /// </summary>
        public Guid? OrderStatusID { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает код типа статуса.
        /// </summary>
        public byte? StatusKindID { get; set; }

        /// <summary>
        /// Задает или получает домен пользователей.
        /// </summary>
        public Guid? UserDomainID { get; set; }

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
            entity.UserDomainID = UserDomainID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return OrderStatusID;
        }
    }
}
