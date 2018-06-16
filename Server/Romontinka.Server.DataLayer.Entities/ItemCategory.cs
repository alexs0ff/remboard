using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Категория товара.
    /// </summary>
    public class ItemCategory:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код категории.
        /// </summary>
        public Guid? ItemCategoryID { get; set; }

        /// <summary>
        /// Задает или получает код домена.
        /// </summary>
        public Guid? UserDomainID { get; set; }

        /// <summary>
        /// Задает или название категории.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (ItemCategory) entityBase;
            entity.ItemCategoryID = ItemCategoryID;
            entity.Title = Title;
            entity.UserDomainID = UserDomainID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return ItemCategoryID;
        }
    }
}
