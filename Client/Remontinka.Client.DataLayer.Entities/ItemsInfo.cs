namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Информация по пунктам.
    /// </summary>
    public class ItemsInfo
    {
        /// <summary>
        /// Задает или получает количество пунктов.
        /// </summary>
        public double? Count { get; set; }
        
        /// <summary>
        /// Задает или получает сумму количества пунктов.
        /// </summary>
        public double? SumCount { get; set; }

        /// <summary>
        /// Задает или получает сумму пунктов.
        /// </summary>
        public double? Amount { get; set; }

        /// <summary>
        /// Получает сумму всех пунктов умноженных на количество.
        /// </summary>
        public double? TotalAmount { get; set; }
    }
}
