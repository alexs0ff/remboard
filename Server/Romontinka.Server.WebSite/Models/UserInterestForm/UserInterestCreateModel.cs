using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.UserInterestForm
{
    /// <summary>
    /// Модель для управления пунктами автозаполнения.
    /// </summary>
    public class UserInterestCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает дату введения правила.
        /// </summary>
        [DisplayName("Дата")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает тип вознаграждения за запчасти.
        /// </summary>
        [DisplayName("Тип вознаграждения по запчастям")]
        [UIHint("AjaxComboBox")]
        [AjaxComboBox("AjaxAutocompleteInterestKind")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public byte? DeviceInterestKindID { get; set; }

        /// <summary>
        /// Задает или получает значение вознаграждения за запчасти.
        /// </summary>
        [UIHint("Decimal")]
        [DisplayName("Вознаграждения за запчасть")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public decimal? DeviceValue { get; set; }

        /// <summary>
        /// Задает или получает тип вознаграждения за работу.
        /// </summary>
        [DisplayName("Тип вознаграждения по работам")]
        [UIHint("AjaxComboBox")]
        [AjaxComboBox("AjaxAutocompleteInterestKind")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        [Required]
        public byte? WorkInterestKindID { get; set; }

        /// <summary>
        /// Задает или получает значение вознаграждения за работу.
        /// </summary>
        [UIHint("Decimal")]
        [DisplayName("Вознаграждения за работу")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public decimal? WorkValue { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        [DisplayName("Описание")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        [UIHint("MultilineString")]
        [MultilineString(3, 5)]
        public string Description { get; set; }

        /// <summary>
        /// Задает или получает код пользователя.
        /// </summary>
        [DisplayName("Пользователь")]
        [UIHint("AjaxComboBox")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        [AjaxComboBox("AjaxUserComboBox")]
        [Required]
        public Guid? UserID { get; set; }
    }
}