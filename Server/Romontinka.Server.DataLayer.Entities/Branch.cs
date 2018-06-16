using System;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Задает или получает филиал.
    /// </summary>
    public class Branch:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код филиала.
        /// </summary>
        public Guid? BranchID { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает адрес филиала.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Задает или получает юр название филиала.
        /// </summary>
        public string LegalName { get; set; }

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
            var entity = (Branch)entityBase;
            entity.Address = Address;
            entity.BranchID = BranchID;
            entity.Title = Title;
            entity.LegalName = LegalName;
            entity.UserDomainID = UserDomainID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return BranchID;
        }
    }
}
