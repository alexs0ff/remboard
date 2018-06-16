using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol
{
    /// <summary>
    /// Визитор для создания самоподписанных сообщений.
    /// </summary>
    public abstract class MessageSignVisitorBase
    {
        /// <summary>
        /// Содержит сообщения которые необходимо "подписать".
        /// </summary>
        private readonly LinkedList<string> _valuesToSign = new LinkedList<string>();

        /// <summary>
        /// Добавляет значение для подписи.
        /// </summary>
        /// <param name="value">Значение.</param>
        internal void AddValue(string value)
        {
            _valuesToSign.AddLast(value);
        }

        /// <summary>
        /// Возвращает строку для подписи.
        /// </summary>
        /// <returns>Возвращаемая строка.</returns>
        public string GetRawDataToSign()
        {
            var result = new StringBuilder();
            foreach (var str in _valuesToSign)
            {
                result.Append(str);
            } //foreach
            return result.ToString();
        }

        /// <summary>
        /// Возвращает подписанное сообщение.
        /// </summary>
        /// <returns>Подпись.</returns>
        public abstract string CreateSign();
    }
}
