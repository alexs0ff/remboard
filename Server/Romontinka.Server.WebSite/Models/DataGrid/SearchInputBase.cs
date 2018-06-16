namespace Romontinka.Server.WebSite.Models.DataGrid
{
    /// <summary>
    /// Базовый класс для контролов поиска.
    /// </summary>
    public abstract class SearchInputBase
    {
        /// <summary>
        /// Задает или получает наименование контрола ввода.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Задает или получает идентификатор контрола ввода.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Задает или получает значение контрола ввода.
        /// </summary>
        public string Value { get; set; }
    }
}