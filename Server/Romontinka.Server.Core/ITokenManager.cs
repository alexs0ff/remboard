using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romontinka.Server.Core.Security;

namespace Romontinka.Server.Core
{
    /// <summary>
    ///   Менеджер авторизации, создает и получает информацию о текущем токене.
    /// </summary>
    public interface ITokenManager
    {
        /// <summary>
        ///   Получает текущий токен безопасности.
        /// </summary>
        /// <returns> </returns>
        SecurityToken GetCurrentToken();

        /// <summary>
        /// Получает текущий токен безопасности.
        /// </summary>
        /// <param name="entityName">Имя текущего принципиала.</param>
        /// <returns>Сформированый токен.</returns>
        SecurityToken GetCurrentToken(string entityName);
    }
}
