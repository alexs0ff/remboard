using System;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Список роле для пользователей.
    /// </summary>
    public static class UserRole
    {
        /// <summary>
        /// Администраторская роль.
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// Менеджерская роль.
        /// </summary>
        public const string Manager = "Manager";

        /// <summary>
        /// Менеджер мастера ремонтника.
        /// </summary>
        public const string Engineer = "Engineer";

        /// <summary>
        /// Разделитель названий ролей.
        /// </summary>
        private const string RolesSeparator = ",";

        /// <summary>
        /// Получает строковой список ролей для пользователя.
        /// </summary>
        /// <param name="roles">Перечисление ролей.</param>
        /// <returns>Сформированная строка.</returns>
        public static string[] GetRolesArray(params string[] roles)
        {
            if (roles != null)
            {
                var copyArray = new string[roles.Length];
                Array.Copy(roles, copyArray, roles.Length);
                return copyArray;
            }
            return new string[0];
        }

        /// <summary>
        /// Получает список ролей для пользователя.
        /// </summary>
        /// <param name="roles">Перечисление ролей.</param>
        /// <returns>Сформированная строка.</returns>
        public static string GetRolesString(params string[] roles)
        {
            if (roles != null)
            {
                return string.Join(RolesSeparator, roles);
            }
            return string.Empty;
        }

        /// <summary>
        /// Разделяет роли из строки.
        /// </summary>
        /// <param name="roles">Строка с ролями.</param>
        /// <returns>Разделенные значения.</returns>
        public static string[] SplitRoles(string roles)
        {
            if (string.IsNullOrWhiteSpace(roles))
            {
                return new string[0];
            }

            return roles.Split(new[] {RolesSeparator}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}