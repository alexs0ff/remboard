using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romontinka.Server.Protocol;

namespace Remontinka.Client.Core.Services
{
    /// <summary>
    /// Реализация класса для подписи клиентских сообщений.
    /// </summary>
    internal class ClientMessageSignVisitor : MessageSignVisitorBase
    {
        private static readonly Encoding _encoding = Encoding.UTF8;

        /// <summary>
        /// Возвращает подписанное сообщение.
        /// </summary>
        /// <returns>Подпись.</returns>
        public override string CreateSign()
        {
            return ClientCore.Instance.AuthService.CreateSign(GetRawDataToSign(), _encoding);
        }
    }
}
