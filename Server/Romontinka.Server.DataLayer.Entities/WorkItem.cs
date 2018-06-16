using System;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Выполненные работы.
    /// </summary>
    public class WorkItem:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает пункт выполненной работы.
        /// </summary>
        public Guid? WorkItemID { get; set; }

        /// <summary>
        /// Задает или получает код инженера.
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        /// Задает или получает наименование работы.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает описание проделанной работы.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Задает или получает дату выполненной работы.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает стоимость работ.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        public Guid? RepairOrderID { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (WorkItem) entityBase;
            entity.EventDate = EventDate;
            entity.Price = Price;
            entity.RepairOrderID = RepairOrderID;
            entity.Title = Title;
            entity.Notes = Notes;
            entity.UserID = UserID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return WorkItemID;
        }
    }
}
