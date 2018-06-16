using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Представляет сущность контрагента.
    /// </summary>
    public class Contractor:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код контрагента.
        /// </summary>
        public Guid? ContractorID { get; set; }

        /// <summary>
        /// Задает или получаект код домена.
        /// </summary>
        public Guid? UserDomainID { get; set; }

        /// <summary>
        /// Задает или получает юрназвание.
        /// </summary>
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает торговую марку.
        /// </summary>
        public string Trademark { get; set; }

        /// <summary>
        /// Задает или получает адрес.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Задает или получает дату заведения.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Задает или получает дату заведения.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (Contractor) entityBase;
            entity.Address = Address;
            entity.ContractorID = ContractorID;
            entity.EventDate = EventDate;
            entity.LegalName = LegalName;
            entity.Phone = Phone;
            entity.Trademark = Trademark;
            entity.UserDomainID = UserDomainID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return ContractorID;
        }
    }
}
