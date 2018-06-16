using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Updater
{
    /// <summary>
    /// EventArgs для ответа по информации с закаченного файла.
    /// </summary>
    public class ResponseInfoEventArgs : EventArgs
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ResponseInfoEventArgs"/>.
        /// </summary>
        public ResponseInfoEventArgs(string statusDescription, long contentLength, bool hasError)
        {
            StatusDescription = statusDescription;
            ContentLength = contentLength;
            HasError = hasError;
        }

        /// <summary>
        /// Получает описание статуса.
        /// </summary>
        public string StatusDescription { get; private set; }

        /// <summary>
        /// Получает длину контента.
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>
        /// Получает признак наличия ошибки
        /// </summary>
        public bool HasError { get; private set; }
    }
}
