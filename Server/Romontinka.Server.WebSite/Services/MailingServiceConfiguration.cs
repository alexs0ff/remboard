using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Services
{
    /// <summary>
    /// Конфигурация для сервиса email.
    /// </summary>
    public class MailingServiceConfiguration : ConfigurationSection
    {
        private static readonly MailingServiceConfiguration _settings =
            (MailingServiceConfiguration)ConfigurationManager.GetSection("MailingService");

        /// <summary>
        ///   Настройки
        /// </summary>
        public static MailingServiceConfiguration Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Хост mail сервера.
        /// </summary>
        [ConfigurationProperty("Host", IsRequired = true)]
        public string Host
        {
            get { return ((string)this["Host"]); }
        }

        /// <summary>
        /// Порт для хоста mail сервера.
        /// </summary>
        [ConfigurationProperty("Port", IsRequired = true)]
        public int Port
        {
            get { return ((int)this["Port"]); }
        }

        /// <summary>
        /// Пароль для mail сервера.
        /// </summary>
        [ConfigurationProperty("Password", IsRequired = true)]
        public string Password
        {
            get { return ((string)this["Password"]); }
        }

        /// <summary>
        /// Логин для mail сервера.
        /// </summary>
        [ConfigurationProperty("Login", IsRequired = true)]
        public string Login
        {
            get { return ((string)this["Login"]); }
        }

        /// <summary>
        /// Признак того, что необходимо использовать SSL соединение.
        /// </summary>
        [ConfigurationProperty("UseSsl", IsRequired = true)]
        public bool UseSsl
        {
            get { return ((bool)this["UseSsl"]); }
        }

        /// <summary>
        /// Получает email адрес на который отправляются информационные письма.
        /// </summary>
        [ConfigurationProperty("InfoEmail", IsRequired = true)]
        public string InfoEmail
        {
            get { return ((string)this["InfoEmail"]); }
        }

        /// <summary>
        /// Получает email адрес на который отправляются отзывы.
        /// </summary>
        [ConfigurationProperty("FeedbackEmail", IsRequired = true)]
        public string FeedbackEmail
        {
            get { return ((string)this["FeedbackEmail"]); }
        }
    }
}