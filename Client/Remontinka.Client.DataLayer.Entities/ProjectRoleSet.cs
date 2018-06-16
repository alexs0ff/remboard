using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Набор ролей в проекте.
    /// </summary>
    public static class ProjectRoleSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        static ProjectRoleSet()
        {
            Admin = new ProjectRole { ProjectRoleID = 1, Title = "Администратор" };
            Manager = new ProjectRole { ProjectRoleID = 2, Title = "Менеджер" };
            Engineer = new ProjectRole { ProjectRoleID = 3, Title = "Инженер" };

            Roles = new Collection<ProjectRole>(new[] { Admin, Manager, Engineer });
        }

        /// <summary>
        /// Получает Idшник минимальной роли.
        /// </summary>
        /// <returns>Id шник минимальной роли.</returns>
        public static long? GetMinimumRoleID()
        {
            return Manager.ProjectRoleID;
        }

        /// <summary>
        /// Проверяет входит ли указанная роль в остальные.
        /// </summary>
        /// <param name="projectRoleID">Код проверяемой роли.</param>
        /// <param name="roles">Список ролей для проверки.</param>
        /// <returns>Результат.</returns>
        public static bool UserHasRole(long? projectRoleID, params ProjectRole[] roles)
        {
            return roles.Any(i => i.ProjectRoleID == projectRoleID);
        }

        /// <summary>
        ///   Возвращает роль в проекте по его коду.
        /// </summary>
        /// <returns> Роль в проекте или null. </returns>
        public static ProjectRole GetKindByID(long? id)
        {
            return Roles.FirstOrDefault(a => a.ProjectRoleID == id);
        }

        /// <summary>
        ///   Список всех типов ролей.
        /// </summary>
        public static ICollection<ProjectRole> Roles { get; private set; }

        /// <summary>
        ///   Администраторская роль.
        /// </summary>
        public static ProjectRole Admin { get; private set; }

        /// <summary>
        /// Менеджерская роль.
        /// </summary>
        public static ProjectRole Manager { get; private set; }

        /// <summary>
        /// Инженерская роль.
        /// </summary>
        public static ProjectRole Engineer { get; private set; }
    }
}
