using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Core
{
    /// <summary>
    /// Сервис электронной почты.
    /// Служит для отправки писем.
    /// </summary>
    public interface IMailingService
    {
        /// <summary>
        /// Получает email адрес для отправки информации.
        /// </summary>
        string InfoEmail { get; }

        /// <summary>
        /// Получает email адрес для отправки отзывов.
        /// </summary>
        string FeedbackEmail { get; }

        /// <summary>
        /// Отправлет сообщение на указаный адрес.
        /// </summary>
        /// <param name="recipient">Получатель или получатели.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="body">Тело сообщения.</param>
        void Send(string recipient, string title, string body);

        /// <summary>
        /// Отправлет сообщение на указаный адрес.
        /// </summary>
        /// <param name="recipient">Получатель или получатели.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="body">Тело сообщения.</param>
        /// <param name="attachment">Путь к файлу вложения.</param>
        void Send(string recipient, string title, string body, string attachment);

        void Send(string recipient, string title, string body, IEnumerable<string> attachments);

        /// <summary>
        /// Отправлет сообщение на указаный адрес.
        /// </summary>
        /// <param name="recipient">Получатель или получатели.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="body">Тело сообщения.</param>
        /// <param name="attachments">Список вложений.</param>
        void Send(string recipient, string title, string body, IList<KeyValuePair<string, byte[]>> attachments);
    }
}
