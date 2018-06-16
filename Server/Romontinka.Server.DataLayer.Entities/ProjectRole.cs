namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Задает или получает роль в проекте.
    /// </summary>
    public class ProjectRole:EntityBase<byte>
    {
        /// <summary>
        /// Задает или получает код роли в проекте.
        /// </summary>
        public byte? ProjectRoleID { get; set; }

        /// <summary>
        /// Задает или получает код названия.
        /// </summary>
        public string Title { get; set; }
    }
}
