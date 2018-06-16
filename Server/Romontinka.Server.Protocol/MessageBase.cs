using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol
{
    /// <summary>
    /// Базовый класс для всех запросов.
    /// </summary>
    public abstract class MessageBase
    {
        /// <summary>
        /// Получает версию протокола.
        /// </summary>
        public string Version { get; internal set; }

        /// <summary>
        /// Задает или получает тип сообщения.
        /// </summary>
        public MessageKind Kind { get; protected set; }
    }
}
