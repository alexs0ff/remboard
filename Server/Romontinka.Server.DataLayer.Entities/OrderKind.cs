using System;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Тип заказа.
    /// </summary>
    public class OrderKind:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает тип заказа.
        /// </summary>
        public Guid? OrderKindID { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }

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
            var entity = (OrderKind) entityBase;
            entity.OrderKindID = OrderKindID;
            entity.Title = Title;
            entity.UserDomainID = UserDomainID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return OrderKindID;
        }
    }
}
