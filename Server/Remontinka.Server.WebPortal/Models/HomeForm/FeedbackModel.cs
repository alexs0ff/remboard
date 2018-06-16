using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.HomeForm
{
    /// <summary>
    /// Модель отправки отзыва для компании.
    /// </summary>
    public class FeedbackModel
    {
        /// <summary>
        /// Задает или получает заголовок сообщения.
        /// </summary>
        [DisplayName("Заголовок")]
        [Required]
        public string Caption { get; set; }

        /// <summary>
        /// Задает или получает текстовку сообщения.
        /// </summary>
        [DisplayName("Сообщение")]
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Задает или получает контактные данные.
        /// </summary>
        [DisplayName("Контактные данные")]
        [Required]
        public string Contact { get; set; }

        /// <summary>
        /// Задает или получает результирующий текст.
        /// </summary>
        public string ResultText { get; set; }
    }
}