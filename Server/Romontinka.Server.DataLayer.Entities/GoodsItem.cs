using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Номенклатура товара.
    /// </summary>
    public class GoodsItem:EntityBase<Guid>
    {
        /// <summary>
        /// Задает или получает код номенклатуры.
        /// </summary>
        public Guid? GoodsItemID { get; set; }

        /// <summary>
        /// Задает или получает код измерения.
        /// </summary>
        public byte? DimensionKindID { get; set; }

        /// <summary>
        /// Задает или получает 
        /// </summary>
        public Guid? UserDomainID { get; set; }

        /// <summary>
        /// Задает или получает код категории.
        /// </summary>
        public Guid? ItemCategoryID { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Задает или получает код товара.
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// Задает или получает Артикул.
        /// </summary>
        public string Particular { get; set; }

        /// <summary>
        /// Задает или получает штрих код.
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (GoodsItem) entityBase;
            entity.BarCode = BarCode;
            entity.Description = Description;
            entity.DimensionKindID = DimensionKindID;
            entity.GoodsItemID = GoodsItemID;
            entity.ItemCategoryID = ItemCategoryID;
            entity.Particular = Particular;
            entity.Title = Title;
            entity.UserCode = UserCode;
            entity.UserDomainID = UserDomainID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return GoodsItemID;
        }
    }
}
