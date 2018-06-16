using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.UserGridForm
{
    /// <summary>
    /// Модель для грида пользователей.
    /// </summary>
    public class UserGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "UserID"; } }

        /// <summary>
        /// Получает роли в проекте.
        /// </summary>
        public ICollection<ProjectRole> ProjectRoles { get { return ProjectRoleSet.Roles; } }

        /// <summary>
        /// Задает или получает филиалы.
        /// </summary>
        public IQueryable<Branch> Branches { get; set; }
    }
}