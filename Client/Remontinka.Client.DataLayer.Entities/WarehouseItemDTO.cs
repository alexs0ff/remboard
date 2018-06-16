namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// DTO объект для элемента остатков на складе.
    /// </summary>
    public class WarehouseItemDTO : WarehouseItem
    {
        /// <summary>
        /// Задает или получает название связанной номенклатуры.
        /// </summary>
        public string GoodsItemTitle { get; set; }

        /// <summary>
        /// Задает или получает код измерения.
        /// </summary>
        public long? DimensionKindID { get; set; }
    }
}
