using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Документ списания со склада.
    /// </summary>
    public class CancellationDoc:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код документа списания со склада.
        /// </summary>
        public Guid? CancellationDocID { get; set; }

        /// <summary>
        /// Задает или получает код склада.
        /// </summary>
        public Guid? WarehouseID { get; set; }

        /// <summary>
        /// Задает или получает код домена.
        /// </summary>
        public Guid? UserDomainID { get; set; }

        /// <summary>
        /// Задает или получает код создателя.
        /// </summary>
        public Guid? CreatorID { get; set; }

        /// <summary>
        /// Задает или получает номер документа
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// Задает или получает дату документа.
        /// </summary>
        public DateTime DocDate { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        public string DocDescription { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (CancellationDoc) entityBase;
            entity.CancellationDocID = CancellationDocID;
            entity.CreatorID = CreatorID;
            entity.DocDescription = DocDescription;
            entity.DocNumber = DocNumber;
            entity.DocDate = DocDate;
            entity.UserDomainID = UserDomainID;
            entity.WarehouseID = WarehouseID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return CancellationDocID;
        }
    }
}
