using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Core
{
    /// <summary>
    ///   Возникает когда пользователь пытается обратиться к неинициализированному сервису.
    /// </summary>
    public class ServiceIsNotAvailableException : Exception
    {
        /// <summary>
        ///   Инициализирует новый экземпляр класса <see cref="T:Romontinka.Server.Core.ServiceIsNotAvailableException" /> .
        /// </summary>
        public ServiceIsNotAvailableException(string serviceName)
        {
            ServiceName = serviceName;
        }

        /// <summary>
        ///   Выполняет инициализацию нового экземпляра класса <see cref="T:Romontinka.Server.Core.ServiceIsNotAvailableException" /> , используя указанное сообщение об ошибке.
        /// </summary>
        /// <param name="message"> Сообщение, описывающее ошибку. </param>
        /// <param name="serviceName"> Имя сервиса к которому пытались обратиться. </param>
        public ServiceIsNotAvailableException(string message, string serviceName)
            : base(message)
        {
            ServiceName = serviceName;
        }

        /// <summary>
        ///   Получает имя сервиса к которому пытались обратиться.
        /// </summary>
        public string ServiceName { get; private set; }
    }
}
