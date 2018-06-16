using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.Branch
{
    /// <summary>
    /// Модель для создания и редактирования филиалов.
    /// </summary>
    public class BranchCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает название филиала.
        /// </summary>
        [DisplayName("Название")]
        [Required]
        [EditorHtmlClass("branches-edit")]
        [LabelHtmlClass("branches-label")]
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает юр название.
        /// </summary>
        [DisplayName("Юр название")]
        [Required]
        [EditorHtmlClass("branches-edit")]
        [LabelHtmlClass("branches-label")]
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает адрес филиала.
        /// </summary>
        [DisplayName("Адрес")]
        [Required]
        [UIHint("MultilineString")]
        [MultilineString(3, 5)]
        [EditorHtmlClass("branches-edit")]
        [LabelHtmlClass("branches-label")]
        public string Address { get; set; }
    }
}