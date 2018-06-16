using System;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Элемент остатка на складе.
    /// </summary>
    public class WarehouseItem:EntityBase<Guid>
    {
        private string _warehouseItemID;

        /// <summary>
        /// Задает или получает код элемента остатка на складе.
        /// </summary>
        public string WarehouseItemID
        {
            get { return _warehouseItemID; }
            set { FormatUtils.ExchangeFields(ref _warehouseItemIDGuid, ref _warehouseItemID, value); }
        }

        private Guid? _warehouseItemIDGuid;

        /// <summary>
        /// Задает или получает код элемента остатка на складе.
        /// </summary>
        public Guid? WarehouseItemIDGuid
        {
            get { return _warehouseItemIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _warehouseItemIDGuid, ref _warehouseItemID,value);
            }
        }

        private string _warehouseID;

        /// <summary>
        /// Задает или получает код склада.
        /// </summary>
        public string WarehouseID
        {
            get { return _warehouseID; }
            set { FormatUtils.ExchangeFields(ref _warehouseIDGuid, ref _warehouseID, value); }
        }

        private Guid? _warehouseIDGuid;

        /// <summary>
        /// Задает или получает код склада.
        /// </summary>
        public Guid? WarehouseIDGuid
        {
            get { return _warehouseIDGuid; }
            set
            {
                FormatUtils.ExchangeFields(ref _warehouseIDGuid, ref _warehouseID, value);
            }
        }

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
                FormatUtils.ExchangeFields(ref _goodsItemIDGuid, ref _goodsItemID,value);
                
            }
        }

        /// <summary>
        /// Задает или получает количество.
        /// </summary>
        public double Total { get; set; }

        /// <summary>
        /// Задает или получает нулевую цену.
        /// </summary>
        public double StartPrice { get; set; }

        /// <summary>
        /// Задает или получает ремонтную цену.
        /// </summary>
        public double RepairPrice { get; set; }

        /// <summary>
        /// Задает или получает цену продажи.
        /// </summary>
        public double SalePrice { get; set; }

        /// <summary>
        ///   Копирует поля указанного объекта в другой объект.
        /// </summary>
        /// <param name="entityBase"> Объект для копирования. </param>
        public override void CopyTo(EntityBase<Guid> entityBase)
        {
            var entity = (WarehouseItem) entityBase;
            entity.GoodsItemID = GoodsItemID;
            entity.RepairPrice = RepairPrice;
            entity.SalePrice = SalePrice;
            entity.StartPrice = StartPrice;
            entity.Total = Total;
            entity.WarehouseID = WarehouseID;
            entity.WarehouseItemID = WarehouseItemID;
        }

        /// <summary>
        ///   Функция для получения идентификатора сущности.
        /// </summary>
        /// <returns>Значение идентификатора. </returns>
        public override Guid? GetId()
        {
            return WarehouseItemIDGuid;
        }
    }
}
