using System;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Пункт соответствия пользователей и филиалов.
    /// </summary>
    public class UserBranchMapItem:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код соответствия.
        /// </summary>
        public Guid? UserBranchMapID { get; set; }

        /// <summary>
        /// Задает или получает код связанного филиала.
        /// </summary>
        public Guid? BranchID { get; set; }

        /// <summary>
        /// Задает или получает дату соответствия.
        /// </summary>
        public DateTime EventDate { get; set; }

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
            var entity = (UserBranchMapItem) entityBase;
            entity.BranchID = BranchID;
            entity.UserBranchMapID = UserBranchMapID;
            entity.UserID = UserID;
            entity.EventDate = EventDate;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return UserBranchMapID;
        }
    }
}
