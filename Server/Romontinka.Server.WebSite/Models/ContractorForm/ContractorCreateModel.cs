using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.ContractorForm
{
    /// <summary>
    /// Модель для создания и редактирования конр агентов.
    /// </summary>
    public class ContractorCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает юрназвание.
        /// </summary>
        [DisplayName("Юр наименование")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает торговую марку.
        /// </summary>
        [DisplayName("Торговая марка")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string Trademark { get; set; }

        /// <summary>
        /// Задает или получает адрес.
        /// </summary>
        [DisplayName("Адрес")]
        [Required]
        [UIHint("MultilineString")]
        [MultilineString(3, 5)]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string Address { get; set; }

        /// <summary>
        /// Задает или получает дату заведения.
        /// </summary>
        [DisplayName("Телефон")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string Phone { get; set; }

        /// <summary>
        /// Задает или получает дату заведения.
        /// </summary>
        [DisplayName("Дата заведения")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public DateTime EventDate { get; set; }
    }
}