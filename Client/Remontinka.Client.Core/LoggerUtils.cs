using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Расширения для логера.
    /// </summary>
    public static class LoggerUtils
    {
        /// <summary>
        /// Производит логирование исключения.
        /// </summary>
        /// <param name="log">Экземляр логгера.</param>
        /// <param name="exception">Логируемое исключение.</param>
        /// <param name="message">Доп сообщение об ошибке.</param>
        public static void LogError(this ILog log, Exception exception,string message)
        {
            if (log==null)
            {
                throw new ArgumentNullException("log");
            } //if

            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            } //if

            var inner = string.Empty;
            if (exception.InnerException != null)
            {
                inner = exception.InnerException.Message;
            } //if
            log.ErrorFormat("{0} {1} {2} {3} {4}", message, exception.GetType(), exception.Message, inner,
                            exception.StackTrace);
        }
    }
}
