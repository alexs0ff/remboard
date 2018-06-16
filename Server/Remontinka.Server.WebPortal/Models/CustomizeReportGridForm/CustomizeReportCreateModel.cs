using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.CustomizeReportGridForm
{
    /// <summary>
    /// Модель редактирования пользовательского документа.
    /// </summary>
    public class CustomizeReportCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код пользовательского отчета.
        /// </summary>
        public Guid? CustomReportID { get; set; }

        /// <summary>
        /// Задает или получает текст.
        /// </summary>
        [DisplayName("Документ")]
        [Required]
        public string HtmlContent { get; set; }

        /// <summary>
        /// Задает или получает название документа.
        /// </summary>
        [DisplayName("Название")]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает тип настраеваемого документа.
        /// </summary>
        [DisplayName("Тип документа")]
        [Required]
        public byte? DocumentKindID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return CustomReportID;
        }
    }
}