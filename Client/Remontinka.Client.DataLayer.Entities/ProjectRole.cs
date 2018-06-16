using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Задает или получает роль в проекте.
    /// </summary>
    public class ProjectRole : EntityBase<long>
    {
        /// <summary>
        /// Задает или получает код роли в проекте.
        /// </summary>
        public long? ProjectRoleID { get; set; }

        /// <summary>
        /// Задает или получает код названия.
        /// </summary>
        public string Title { get; set; }
    }
}
