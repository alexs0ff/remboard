using System;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Номенклатура товара.
    /// </summary>
    public class GoodsItem:EntityBase<Guid>
    {
        private string _goodsItemID;

        /// <summary>
        /// Задает или получает код номенклатуры.
        /// </summary>
        public string GoodsItemID
        {
            get { return _goodsItemID; }
            set { FormatUtils.ExchangeFields(ref _goodsItemIDGuid, ref _goodsItemID, value); }
        }

        private Guid? _goodsItemIDGuid;

        /// <summary>
        /// Задает или получает код номенклатуры.
        /// </summary>
        public Guid? GoodsItemIDGuid
        {
            get { return _goodsItemIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _goodsItemIDGuid, ref _goodsItemID, value);
            }
        }

        /// <summary>
        /// Задает или получает код измерения.
        /// </summary>
        public long? DimensionKindID { get; set; }

        private string _itemCategoryID;

        /// <summary>
        /// Задает или получает код категории.
        /// </summary>
        public string ItemCategoryID
        {
            get { return _itemCategoryID; }
            set { FormatUtils.ExchangeFields(ref _itemCategoryIDGuid, ref _itemCategoryID, value); }
        }

        private Guid? _itemCategoryIDGuid;

        /// <summary>
        /// Задает или получает код категории.
        /// </summary>
        public Guid? ItemCategoryIDGuid
        {
            get { return _itemCategoryIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _itemCategoryIDGuid, ref _itemCategoryID,value);
            }
        }

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
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return GoodsItemIDGuid;
        }
    }
}
