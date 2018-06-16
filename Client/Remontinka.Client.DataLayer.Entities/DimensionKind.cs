namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Тип измерения.
    /// </summary>
    public class DimensionKind:EntityBase<byte>
    {
        /// <summary>
        /// Задает или получает код типа измерения.
        /// </summary>
        public long? DimensionKindID { get; set; }

        /// <summary>
        /// Задает или получает название типа измерения.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает короткое название.
        /// </summary>
        public string ShortTitle { get; set; }
    }
}
