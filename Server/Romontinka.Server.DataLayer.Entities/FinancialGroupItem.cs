using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Финансовая группа филиалов.
    /// </summary>
    public class FinancialGroupItem: EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код финансовой группы.
        /// </summary>
        public Guid? FinancialGroupID { get; set; }

        /// <summary>
        /// Задает или получает код домена пользователя.
        /// </summary>
        public Guid? UserDomainID { get; set; }

        /// <summary>
        /// Задает или получает название финансовой группы.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает юр название фирмы.
        /// </summary>
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает торговую марку фирмы.
        /// </summary>
        public string Trademark { get; set; }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return FinancialGroupID;
        }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (FinancialGroupItem) entityBase;
            entity.FinancialGroupID = FinancialGroupID;
            entity.LegalName = LegalName;
            entity.Title = Title;
            entity.Trademark = Trademark;
            entity.UserDomainID = UserDomainID;
        }
    }
}
