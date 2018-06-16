using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.ItemCategoryGridForm
{
    /// <summary>
    /// Модель создания данных категории товара.
    /// </summary>
    public class ItemCategoryCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код категории.
        /// </summary>
        public Guid? ItemCategoryID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return ItemCategoryID;
        }

        /// <summary>
        /// Задает или название категории.
        /// </summary>
        [DisplayName("Название")]
        [Required]
        public string Title { get; set; }
    }
}