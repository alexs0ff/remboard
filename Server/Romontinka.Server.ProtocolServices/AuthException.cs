using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.ProtocolServices
{
    /// <summary>
    /// Ошибка аутентификации.
    /// </summary>
    public class AuthException:Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        public AuthException(string message) : base(message)
        {
        }
    }
}
