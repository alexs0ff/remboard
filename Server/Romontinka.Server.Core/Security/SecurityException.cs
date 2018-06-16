using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Core.Security
{
    /// <summary>
    ///   Исключение возникающее при ошибках с безопасностью.
    /// </summary>
    public class SecurityException : Exception
    {
        /// <summary>
        ///   Инициализирует новый экземпляр класса <see cref="SecurityException" /> .
        /// </summary>
        public SecurityException()
        {
        }

        /// <summary>
        ///   Выполняет инициализацию нового экземпляра класса <see cref="SecurityException" /> , используя указанное сообщение об ошибке.
        /// </summary>
        /// <param name="message"> Сообщение, описывающее ошибку. </param>
        public SecurityException(string message)
            : base(message)
        {
        }
    }
}
