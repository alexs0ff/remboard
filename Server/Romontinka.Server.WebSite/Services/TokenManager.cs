using System.Web;
using Remontinka.Server.WebPortal.Services;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;

namespace Romontinka.Server.WebSite.Services
{
    /// <summary>
    ///  Менеджер маркеров безопасности.
    /// </summary>
    public class TokenManager : ITokenManager
    {
        /// <summary>
        ///   Получает текущий токен безопасности.
        /// </summary>
        /// <returns> </returns>
        public SecurityToken GetCurrentToken()
        {
            return
                RemontinkaServer.Instance.GetService<IWebSiteSettingsService>()
                    .GetTokenForUser(HttpContext.Current.User.Identity.Name);
        }

        /// <summary>
        /// Получает текущий токен безопасности.
        /// </summary>
        /// <param name="entityName">Имя текущего принципиала.</param>
        /// <returns>Сформированый токен.</returns>
        public SecurityToken GetCurrentToken(string entityName)
        {
            return RemontinkaServer.Instance.SecurityService.GetToken(entityName);
        }
       
    }
}