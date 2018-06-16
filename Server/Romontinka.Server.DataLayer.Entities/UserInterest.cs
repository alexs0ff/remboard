using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Вознаграждение пользователей.
    /// </summary>
    public class UserInterest:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код вознаграждения пользователя.
        /// </summary>
        public Guid? UserInterestID { get; set; }

        /// <summary>
        /// Задает или получает код пользователя.
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        /// Задает или получает дату введения правила.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает тип вознаграждения за работу.
        /// </summary>
        public byte? WorkInterestKindID { get; set; }

        /// <summary>
        /// Задает или получает значение вознаграждения за работу.
        /// </summary>
        public decimal? WorkValue { get; set; }

        /// <summary>
        /// Задает или получает тип вознаграждения за запчасти.
        /// </summary>
        public byte? DeviceInterestKindID { get; set; }

        /// <summary>
        /// Задает или получает значение вознаграждения за запчасти.
        /// </summary>
        public decimal? DeviceValue { get; set; }
        
        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (UserInterest) entityBase;
            entity.Description = Description;
            entity.DeviceInterestKindID = DeviceInterestKindID;
            entity.DeviceValue = DeviceValue;
            entity.EventDate = EventDate;
            entity.UserID = UserID;
            entity.UserInterestID = UserInterestID;
            entity.WorkInterestKindID = WorkInterestKindID;
            entity.WorkValue = WorkValue;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return UserInterestID;
        }
    }
}
