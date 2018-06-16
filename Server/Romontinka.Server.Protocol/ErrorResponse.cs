using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol
{
    /// <summary>
    /// Ответ с ошибкой.
    /// </summary>
    public class ErrorResponse : MessageBase
    {
        public ErrorResponse()
        {
            Kind = MessageKind.ErrorResponse;
        }

        /// <summary>
        /// Задает или получает описание ошибки.
        /// </summary>
        public string Description { get; set; }
    }
}
