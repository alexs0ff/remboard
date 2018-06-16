namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Тип графика событий.
    /// </summary>
    public class TimelineKind:EntityBase<long>
    {
        /// <summary>
        /// Задает или получает тип графика событий.
        /// </summary>
        public long? TimelineKindID { get; set; }

        /// <summary>
        /// Задает или получает название типа.
        /// </summary>
        public string Title { get; set; }
    }
}
