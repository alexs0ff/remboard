using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Документ перемещения со склада на склад.
    /// </summary>
    public class TransferDoc:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код документа перемещения .
        /// </summary>
        public Guid? TransferDocID { get; set; }

        /// <summary>
        /// Задает или получает код склада откуда перемещается товар.
        /// </summary>
        public Guid? SenderWarehouseID { get; set; }

        /// <summary>
        /// Задает или получает код получаещего склада.
        /// </summary>
        public Guid? RecipientWarehouseID { get; set; }

        /// <summary>
        /// Задает или получает код домена.
        /// </summary>
        public Guid? UserDomainID { get; set; }

        /// <summary>
        /// Задает или получает код создателя документа.
        /// </summary>
        public Guid? CreatorID { get; set; }

        /// <summary>
        /// Задает или получает номер документа.
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// Задает или получает дату документа.
        /// </summary>
        public DateTime DocDate { get; set; }

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
            var entity = (TransferDoc) entityBase;
            entity.CreatorID = CreatorID;
            entity.DocDescription = DocDescription;
            entity.DocNumber = DocNumber;
            entity.DocDate = DocDate;
            entity.RecipientWarehouseID = RecipientWarehouseID;
            entity.SenderWarehouseID = SenderWarehouseID;
            entity.TransferDocID = TransferDocID;
            entity.UserDomainID = UserDomainID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return TransferDocID;
        }
    }
}
