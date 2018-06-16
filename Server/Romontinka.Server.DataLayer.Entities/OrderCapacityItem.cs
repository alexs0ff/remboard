using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Пункт для создания последовательных номеров.
    /// </summary>
    public class OrderCapacityItem : EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код последовательного номера.
        /// </summary>
        public Guid? OrderCapacityID { get; set; }

        /// <summary>
        /// Задает или получает код домена.
        /// </summary>
        public Guid? UserDomainID { get; set; }

        /// <summary>
        /// Задает или получает номер текущего заказа.
        /// </summary>
        public long OrderNumber { get; set; }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return OrderCapacityID;
        }
    }
}
