using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.UserInterestGridForm
{
    /// <summary>
    /// Модель редактирования вознаграждений пользователей.
    /// </summary>
    public class UserInterestCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код вознаграждения пользователя.
        /// </summary>
        public Guid? UserInterestID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return UserInterestID;
        }

        /// <summary>
        /// Задает или получает дату введения правила.
        /// </summary>
        [DisplayName("Дата")]
        [Required]
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает тип вознаграждения за запчасти.
        /// </summary>
        [DisplayName("Тип вознаграждения по запчастям")]
        [Required]
        public byte? DeviceInterestKindID { get; set; }

        /// <summary>
        /// Задает или получает значение вознаграждения за запчасти.
        /// </summary>
        [DisplayName("Вознаграждение за запчасть")]
        public decimal? DeviceValue { get; set; }

        /// <summary>
        /// Задает или получает тип вознаграждения за работу.
        /// </summary>
        [DisplayName("Тип вознаграждения по работам")]
        [Required]
        public byte? WorkInterestKindID { get; set; }

        /// <summary>
        /// Задает или получает значение вознаграждения за работу.
        /// </summary>
        [DisplayName("Вознаграждение за работу")]
        public decimal? WorkValue { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        [DisplayName("Описание")]
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Задает или получает код пользователя.
        /// </summary>
        [DisplayName("Пользователь")]
        [Required]
        public Guid? UserID { get; set; }
    }
}