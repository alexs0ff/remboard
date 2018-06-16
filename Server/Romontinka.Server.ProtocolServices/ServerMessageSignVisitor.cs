using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romontinka.Server.Protocol;

namespace Romontinka.Server.ProtocolServices
{
    /// <summary>
    /// Визитор для проверки пришедших сообщений.
    /// </summary>
    internal class ServerMessageSignVisitor : MessageSignVisitorBase
    {
        /// <summary>
        /// Возвращает подписанное сообщение.
        /// </summary>
        /// <returns>Подпись.</returns>
        public override string CreateSign()
        {
            throw new NotImplementedException();
        }
    }
}
