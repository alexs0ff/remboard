using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.AutocompleteItemGridForm
{
    /// <summary>
    /// Модель редактирования автодополнением.
    /// </summary>
    public class AutocompleteItemCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код пункта автодополнения.
        /// </summary>
        public Guid? AutocompleteItemID { get; set; }

        /// <summary>
        /// Задает или получает код типа документа.
        /// </summary>
        [DisplayName("Тип")]
        [Required]
        public byte? AutocompleteKindID { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        [DisplayName("Название")]
        [Required]
        public string Title { get; set; }


        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return AutocompleteItemID;
        }
    }
}