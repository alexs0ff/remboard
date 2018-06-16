using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Статья бюджета.
    /// </summary>
    public class FinancialItem: EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код статьи бюджета.
        /// </summary>
        public Guid? FinancialItemID { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает описание статьи.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Задает или получает код домена пользователей.
        /// </summary>
        public Guid? UserDomainID { get; set; }

        /// <summary>
        /// Задает или получает тип статьи бюджета.
        /// </summary>
        public int? FinancialItemKindID { get; set; }

        /// <summary>
        /// Задает или получает код типа операции.
        /// </summary>
        public byte? TransactionKindID { get; set; }

        /// <summary>
        /// Задает или получает дату введения статьи.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return FinancialItemID;
        }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (FinancialItem)entityBase;
            entity.Description = Description;
            entity.EventDate = EventDate;
            entity.FinancialItemID = FinancialItemID;
            entity.TransactionKindID = TransactionKindID;
            entity.FinancialItemKindID = FinancialItemKindID;
            entity.Title = Title;
            entity.UserDomainID = UserDomainID;
        }
    }
}
