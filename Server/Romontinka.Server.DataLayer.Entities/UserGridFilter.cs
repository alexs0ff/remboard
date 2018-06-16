using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Пользовательский фильтр для грида.
    /// </summary>
    public class UserGridFilter:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код фильтра пользователя.
        /// </summary>
        public Guid? UserGridFilterID { get; set; }

        /// <summary>
        /// Задает или получает название фильтра.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает имя грида.
        /// </summary>
        public string GridName { get; set; }

        /// <summary>
        /// Задает или получает данные фильтра.
        /// </summary>
        public string FilterData { get; set; }

        /// <summary>
        /// Задает или получает код пользователя.
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (UserGridFilter)entityBase;
            entity.Title = Title;
            entity.FilterData = FilterData;
            entity.GridName = GridName;
            entity.UserGridFilterID = UserGridFilterID;
            entity.UserID = UserID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return UserGridFilterID;
        }
    }
}
