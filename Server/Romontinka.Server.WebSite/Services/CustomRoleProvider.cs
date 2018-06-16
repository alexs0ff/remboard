using System;
using System.Web.Security;
using Romontinka.Server.Core;

namespace Romontinka.Server.WebSite.Services
{
    /// <summary>
    ///   Поставщик ролей для пользователя.
    /// </summary>
    public class CustomRoleProvider : RoleProvider
    {
        /// <summary>
        ///   Возвращает значение, указывающее связан ли указанный пользователь с указанной ролью для настроенного applicationName.
        /// </summary>
        /// <returns> Значение true, если указанный пользователь связан с указанной ролью для настроенного applicationName; в противном случае — false. </returns>
        /// <param name="username"> Имя пользователя для поиска. </param>
        /// <param name="roleName"> Роль, в которой следует выполнить поиск. </param>
        public override bool IsUserInRole(string username, string roleName)
        {
            return RemontinkaServer.Instance.SecurityService.IsUserInRole(username, roleName);
        }

        /// <summary>
        ///   Возвращает список ролей, с которыми связан указанный пользователей для настроенного applicationName.
        /// </summary>
        /// <returns> Массив строк, содержащий имена всех ролей, с которыми связан указанный пользователь для настроенного applicationName. </returns>
        /// <param name="username"> Имя пользователя, для которого нужно возвратить список ролей. </param>
        public override string[] GetRolesForUser(string username)
        {
            return RemontinkaServer.Instance.SecurityService.GetRolesForUser(username);
        }

        /// <summary>
        ///   Добавляет новую роль к источнику данных для настроенного приложения applicationName.
        /// </summary>
        /// <param name="roleName"> Имя создаваемой роли. </param>
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Удаляет роль из источника данных для настроенного приложения applicationName.
        /// </summary>
        /// <returns> Значение true, если роль успешно удалена; в противном случае — значение false. </returns>
        /// <param name="roleName"> Имя удаляемой роли. </param>
        /// <param name="throwOnPopulatedRole"> Если true, то выдайте исключение, если <paramref name="roleName" /> имеет один или более членов и не удаляйте <paramref
        ///    name="roleName" /> . </param>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Возвращает значение, указывающее, существует ли указанная роль в источнике данных ролей для настроенного applicationName.
        /// </summary>
        /// <returns> true, если имя роли уже существует в источнике данных для настроенного applicationName; в противном случае — false. </returns>
        /// <param name="roleName"> Имя роли, которую необходимо найти в источнике данных. </param>
        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Добавляет указанные имена пользователей к указанным ролям для установленного приложения applicationName.
        /// </summary>
        /// <param name="usernames"> Массив строк имен пользователей для добавления в указанные роли. </param>
        /// <param name="roleNames"> Массив строк имен пользователей, в который добавляются указанные имена пользователей. </param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Удаляет указанные имена пользователей из указанных ролей для установленного приложения applicationName.
        /// </summary>
        /// <param name="usernames"> Массив строк имен пользователей для удаления из указанных ролей. </param>
        /// <param name="roleNames"> Массив строк имен ролей, из которых необходимо удалить указанные имена пользователей. </param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Возвращает список пользователей, связанных с указанной ролью, для настроенного applicationName.
        /// </summary>
        /// <returns> Массив строк, содержащий имена всех пользователей, которые являются членами указанной роли для настроенного applicationName. </returns>
        /// <param name="roleName"> Имя роли, для которой необходимо возвратить список пользователей. </param>
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Возвращает список всех ролей для настроенного applicationName.
        /// </summary>
        /// <returns> Массив строк, содержащий имена всех ролей, сохраненных в источнике данных для настроенного applicationName. </returns>
        public override string[] GetAllRoles()
        {
            return RemontinkaServer.Instance.SecurityService.GetAllRoles();
        }

        /// <summary>
        ///   Возвращает массив имен пользователей в роли, у которых имена совпадают с указанными именами пользователей.
        /// </summary>
        /// <returns> Массив строк, содержащий имена всех пользователей, имена которых совпадают с <paramref name="usernameToMatch" /> , и которые являются членами указанной роли. </returns>
        /// <param name="roleName"> Роль, в которой следует выполнить поиск. </param>
        /// <param name="usernameToMatch"> Имя пользователя для поиска. </param>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Получает или задает имя приложения, для которого будут сохраняться и извлекаться сведения о роли.
        /// </summary>
        /// <returns> Имя приложения, для которого будут сохраняться и извлекаться сведения о роли. </returns>
        public override string ApplicationName { get; set; }
    }
}