using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using Romontinka.Server.Core;
using log4net;
using Remontinka.Server.WebPortal.Services;
using Romontinka.Server.WebSite.Services;

namespace Remontinka.Server.WebPortal.Services
{
    /// <summary>
    /// Реализация сервиса для отправки email.
    /// </summary>
    public class MailingService : IMailingService
    {
        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(MailingService));

        /// <summary>
        /// Отправлет сообщение на указаный адрес.
        /// </summary>
        /// <param name="recipient">Получатель или получатели.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="body">Тело сообщения.</param>
        /// <param name="attachments">Список вложений.</param>
        public void Send(string recipient, string title, string body, IList<KeyValuePair<string, byte[]>> attachments)
        {
            try
            {
                _logger.InfoFormat("Старт отправки email сообщения \"{0}\" для \"{1}\" ", title, recipient);
                var smtpClient = new SmtpClient(MailingServiceConfiguration.Settings.Host,
                                                MailingServiceConfiguration.Settings.Port);
                smtpClient.EnableSsl = MailingServiceConfiguration.Settings.UseSsl;
                smtpClient.Credentials = new NetworkCredential(MailingServiceConfiguration.Settings.Login,
                                                               MailingServiceConfiguration.Settings.Password);
                var message = new MailMessage();

                message.IsBodyHtml = false;
                //message.BodyEncoding = Encoding.Default;

                message.To.Add(recipient);

                var messageFrom = new MailAddress(MailingServiceConfiguration.Settings.Login);
                message.From = messageFrom;

                message.Body = body;
                message.Subject = title;

                foreach (var attachment in attachments)
                {
                    var attach = AttachmentHelper.CreateAttachment(new MemoryStream(attachment.Value), attachment.Key,
                                                                   TransferEncoding.Base64);
                    message.Attachments.Add(attach);
                } //foreach

                smtpClient.Send(message);
                message.Dispose();
                smtpClient.Dispose();
                _logger.InfoFormat(
                            "Сообщение  \"{0}\" для \"{1}\" успешно отправлено.", title,
                            recipient);
            }
            catch (SmtpException ex)
            {
                string inner = string.Empty;
                if (ex.InnerException != null)
                {
                    inner = ex.InnerException.Message;
                }
                _logger.ErrorFormat(
                           "Во время отправки1 сообщения {0} \"{1}\" для \"{2}\" произошла ошибка {3} {4} |{5} |{6}", body,
                           title, recipient, ex.GetType(), ex.Message,
                           inner, ex.StackTrace);
            }
        }

        /// <summary>
        /// Отправлет сообщение на указаный адрес.
        /// </summary>
        /// <param name="recipient">Получатель или получатели.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="body">Тело сообщения.</param>
        /// <param name="attachments">Путь к файлам вложения.</param>
        public void Send(string recipient, string title, string body, IEnumerable<string> attachments)
        {
            var attachContents = new List<KeyValuePair<string, byte[]>>();
            foreach (string attachment in attachments)
            {
                if (File.Exists(attachment))
                {
                    attachContents.Add(new KeyValuePair<string, byte[]>(Path.GetFileName(attachment),
                                                                        File.ReadAllBytes(attachment)));
                } //if
            } //foreach

            Send(recipient, title, body, attachContents);
        }

        /// <summary>
        /// Отправлет сообщение на указаный адрес.
        /// </summary>
        /// <param name="recipient">Получатель или получатели.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="body">Тело сообщения.</param>
        /// <param name="attachment">Путь к файлу вложения.</param>
        public void Send(string recipient, string title, string body, string attachment)
        {
            Send(recipient, title, body, new[] { attachment });
        }

        /// <summary>
        /// Отправлет сообщение на указаный адрес.
        /// </summary>
        /// <param name="recipient">Получатель или получатели.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="body">Тело сообщения.</param>
        public void Send(string recipient, string title, string body)
        {
            Send(recipient, title, body, string.Empty);
        }

        /// <summary>
        /// Получает email адрес для отправки информации.
        /// </summary>
        public string InfoEmail
        {
            get { return MailingServiceConfiguration.Settings.InfoEmail; }
        }

        /// <summary>
        /// Получает email адрес для отправки отзывов.
        /// </summary>
        public string FeedbackEmail
        {
            get { return MailingServiceConfiguration.Settings.FeedbackEmail; }
        }
    }
}