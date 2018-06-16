using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Ошибка авторизации.
    /// </summary>
    public class ResponseAuthException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        public ResponseAuthException(string message) : base(message)
        {

        }
    }
}
