using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol
{
    /// <summary>
    /// Исключение при разборе сообщений протокола.
    /// </summary>
    public class MessageParseException:Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        /// <param name="name">Сообщение. </param>
        public MessageParseException(string message, string name) : base(message)
        {
            Name = name;
        }

        /// <summary>
        /// Получает название элемента.
        /// </summary>
        public string Name { get; private set; }
    }
}
