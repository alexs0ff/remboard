using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Пункт автодополнения.
    /// </summary>
    public class AutocompleteItem:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код пункта автодополнения.
        /// </summary>
        public Guid? AutocompleteItemID { get; set; }

        /// <summary>
        /// Задает или получает код домена пользователя.
        /// </summary>
        public Guid? UserDomainID { get; set; }

        /// <summary>
        /// Задает или получает тип автодополнения.
        /// </summary>
        public byte? AutocompleteKindID { get; set; }

        /// <summary>
        /// Задает или получает название автодополнения.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (AutocompleteItem) entityBase;
            entity.AutocompleteKindID = AutocompleteKindID;
            entity.Title = Title;
            entity.UserDomainID = UserDomainID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return AutocompleteItemID;
        }
    }
}
