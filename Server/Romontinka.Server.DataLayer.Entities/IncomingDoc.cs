using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Приходная накладная.
    /// </summary>
    public class IncomingDoc:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код приходной накладной.
        /// </summary>
        public Guid? IncomingDocID { get; set; }

        /// <summary>
        /// Задает или получает код создателя.
        /// </summary>
        public Guid? CreatorID { get; set; }

        /// <summary>
        /// Задает или получает код склада.
        /// </summary>
        public Guid? WarehouseID { get; set; }

        /// <summary>
        /// Задает или получает код домена пользователя.
        /// </summary>
        public Guid? UserDomainID { get; set; }

        /// <summary>
        /// Задает или получает код контрагента.
        /// </summary>
        public Guid? ContractorID { get; set; }

        /// <summary>
        /// Задает или получает дату накладной.
        /// </summary>
        public DateTime DocDate { get; set; }

        /// <summary>
        /// Задает или получает номер накладной.
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// Задает или получает описание документа.
        /// </summary>
        public string DocDescription { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (IncomingDoc) entityBase;
            entity.ContractorID = ContractorID;
            entity.CreatorID = CreatorID;
            entity.DocDate = DocDate;
            entity.DocDescription = DocDescription;
            entity.DocNumber = DocNumber;
            entity.IncomingDocID = IncomingDocID;
            entity.UserDomainID = UserDomainID;
            entity.WarehouseID = WarehouseID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return IncomingDocID;
        }
    }
}
