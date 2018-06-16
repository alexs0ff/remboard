using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.BranchForm
{
    /// <summary>
    /// Модель для создания филиала.
    /// </summary>
    public class BranchCreateModel: GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает название филиала.
        /// </summary>
        [DisplayName("Название")]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает юр название.
        /// </summary>
        [DisplayName("Юр название")]
        [Required]
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает адрес филиала.
        /// </summary>
        [DisplayName("Адрес")]
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return Guid.Empty;
        }
    }
}