using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol
{
    /// <summary>
    /// Информация по запросу.
    /// </summary>
    public sealed class MessageInfo
    {
        /// <summary>
        /// Задает или получает номер версии.
        /// </summary>
        public string Version { get; internal set; }

        /// <summary>
        /// Задает или получает тип сообщения.
        /// </summary>
        public MessageKind Kind { get; internal set; }
    }
}
