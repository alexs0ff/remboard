using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.WarehouseForm
{
    /// <summary>
    /// Модель для создания и редактирования складами.
    /// </summary>
    public class WarehouseCreateModel : JGridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает название.
        /// </summary>
        [DisplayName("Название склада")]
        [EditorHtmlClass("editor-edit")]
        [LabelHtmlClass("editor-label")]
        [Required]
        public string Title { get; set; }
    }
}