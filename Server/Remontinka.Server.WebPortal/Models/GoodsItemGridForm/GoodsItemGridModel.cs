using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.GoodsItemGridForm
{
    /// <summary>
    /// Модель данных для грида номенклатуры.
    /// </summary>
    public class GoodsItemGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName {
            get { return "GoodsItemID"; }
        }

        /// <summary>
        /// Получает типы измерений.
        /// </summary>
        public IEnumerable<DimensionKind> DimensionKinds { get { return DimensionKindSet.Kinds; } }

        /// <summary>
        /// Задает или получает категории номенклатуры.
        /// </summary>
        public IQueryable<ItemCategory> ItemCategories { get; set; }
    }
}