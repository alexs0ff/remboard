namespace Remontinka.Server.WebPortal.Models.Common
{
    /// <summary>
    /// Запрос в необходимости авторизации.
    /// </summary>
    public class JCrudAuthorizeResult : JCrudResult
    {
        public JCrudAuthorizeResult(string authUrl)
        {
            ResultState = CrudResultKind.Authorize;
            AuthUrl = authUrl;
        }

        /// <summary>
        /// Задает или получает URL авторизации.
        /// </summary>
        public string AuthUrl { get; set; }
    }
}