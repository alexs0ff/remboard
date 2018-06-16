using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.HomeForm
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
        [EditorHtmlClass("feedback-edit")]
        [LabelHtmlClass("feedback-label")]
        [Required]
        public string Caption { get; set; }

        /// <summary>
        /// Задает или получает текстовку сообщения.
        /// </summary>
        [UIHint("MultilineString")]
        [DisplayName("Сообщение")]
        [EditorHtmlClass("feedback-edit")]
        [LabelHtmlClass("feedback-label")]
        [MultilineString(5, 3)]
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Задает или получает контактные данные.
        /// </summary>
        [DisplayName("Контактные данные")]
        [EditorHtmlClass("feedback-edit")]
        [LabelHtmlClass("feedback-label")]
        [Required]
        public string Contact { get; set; }

        /// <summary>
        /// Задает или получает результирующий текст.
        /// </summary>
        public string ResultText { get; set; }
    }
}