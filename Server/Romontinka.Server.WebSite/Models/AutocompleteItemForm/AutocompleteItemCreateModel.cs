using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.AutocompleteItemForm
{
    /// <summary>
    /// Модель для управления пунктами автозаполнения.
    /// </summary>
    public class AutocompleteItemCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код типа документа.
        /// </summary>
        [DisplayName("Тип")]
        [UIHint("AjaxComboBox")]
        [AjaxComboBox("AjaxAutocompleteKindComboBox")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public byte? AutocompleteKindID { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        [DisplayName("Название")]
        [Required]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        public string Title { get; set; }
    }
}