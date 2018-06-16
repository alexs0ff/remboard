namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Тип графика событий.
    /// </summary>
    public class TimelineKind:EntityBase<byte>
    {
        /// <summary>
        /// Задает или получает тип графика событий.
        /// </summary>
        public byte? TimelineKindID { get; set; }

        /// <summary>
        /// Задает или получает название типа.
        /// </summary>
        public string Title { get; set; }
    }
}
