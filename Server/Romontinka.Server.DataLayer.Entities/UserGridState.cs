using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Состояние грида пользователя.
    /// </summary>
    public class UserGridState:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код состояния грида пользователя.
        /// </summary>
        public Guid? UserGridStateID { get; set; }

        /// <summary>
        /// Задает или получает наименование грида.
        /// </summary>
        public string GridName { get; set; }

        /// <summary>
        /// Задает или получает текущее состояние грида.
        /// </summary>
        public string StateGrid { get; set; }

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
            var entity = (UserGridState) entityBase;
            entity.UserID = UserID;
            entity.GridName = GridName;
            entity.StateGrid = StateGrid;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return UserGridStateID;
        }
    }
}
