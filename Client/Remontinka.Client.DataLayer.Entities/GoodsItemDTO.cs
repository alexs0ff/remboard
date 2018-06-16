namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// DTO объект для номенклатуры.
    /// </summary>
    public class GoodsItemDTO : GoodsItem
    {
        /// <summary>
        /// Задает или получает название категории.
        /// </summary>
        public string ItemCategoryTitle { get; set; }
    }
}
