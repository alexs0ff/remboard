using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Обработанный документ склада.
    /// </summary>
    public class ProcessedWarehouseDoc:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код обработанного складского документа.
        /// </summary>
        public Guid? ProcessedWarehouseDocID { get; set; }

        /// <summary>
        /// Задает или получает код склада.
        /// </summary>
        public Guid? WarehouseID { get; set; }

        /// <summary>
        /// Задает или получает дату обработки.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает дату обработки в UTC.
        /// </summary>
        public DateTime UTCEventDateTime { get; set; }

        /// <summary>
        /// Задает или получает код пользователя который иницировал обработку.
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (ProcessedWarehouseDoc) entityBase;
            entity.EventDate = EventDate;
            entity.ProcessedWarehouseDocID = ProcessedWarehouseDocID;
            entity.UTCEventDateTime = UTCEventDateTime;
            entity.UserID = UserID;
            entity.WarehouseID = WarehouseID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return ProcessedWarehouseDocID;
        }
    }
}
