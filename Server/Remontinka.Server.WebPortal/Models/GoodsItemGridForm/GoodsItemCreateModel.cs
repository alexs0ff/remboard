using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Models.GoodsItemGridForm
{
    /// <summary>
    /// Модель создания номенклатуры.
    /// </summary>
    public class GoodsItemCreateModel : GridDataModelBase<Guid>
    {
        /// <summary>
        /// Задает или получает код номенклатуры.
        /// </summary>
        public Guid? GoodsItemID { get; set; }

        /// <summary>
        /// Получает идентификатор.
        /// </summary>
        public override Guid? GetId()
        {
            return GoodsItemID;
        }

        /// <summary>
        /// Задает или получает код измерения.
        /// </summary>
        [DisplayName("Измерение")]
        [Required]
        public byte? DimensionKindID { get; set; }

        /// <summary>
        /// Задает или получает код категории.
        /// </summary>
        [DisplayName("Категория")]
        [Required]
        public Guid? ItemCategoryID { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        [DisplayName("Название")]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        [DisplayName("Описание")]
        public string Description { get; set; }

        /// <summary>
        /// Задает или получает код товара.
        /// </summary>
        [DisplayName("Код товара")]
        public string UserCode { get; set; }

        /// <summary>
        /// Задает или получает Артикул.
        /// </summary>
        [DisplayName("Артикул")]
        public string Particular { get; set; }
    }
}