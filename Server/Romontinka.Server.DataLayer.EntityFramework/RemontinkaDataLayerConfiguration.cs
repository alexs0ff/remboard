using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.EntityFramework
{
    /// <summary>
    /// Конфигурация уровня данных.
    /// </summary>
    public class RemontinkaDataLayerConfiguration : ConfigurationSection
    {
        /// <summary>
        /// Настройки данных.
        /// </summary>
        private static readonly RemontinkaDataLayerConfiguration _settings =
            (RemontinkaDataLayerConfiguration)ConfigurationManager.GetSection("RemontinkaDataLayer");

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:Romontinka.Server.DataLayer.EntityFramework.RemontinkaDataLayerConfiguration"/>. 
        /// </summary>
        private RemontinkaDataLayerConfiguration()
        {
        }

        /// <summary>
        /// Настройки.
        /// </summary>
        public static RemontinkaDataLayerConfiguration Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Строка подключения к провайдеру данных для EF.
        /// </summary>
        [ConfigurationProperty("EFConnectionString", IsRequired = true)]
        public string EFConnectionString
        {
            get { return ((string)_settings["EFConnectionString"]); }
        }

        /// <summary>
        /// Имя поставщика услуг для EF.
        /// </summary>
        [ConfigurationProperty("EFProviderName", IsRequired = true)]
        public string EFProviderName
        {
            get { return ((string)_settings["EFProviderName"]); }
        }

        /// <summary>
        /// Метаданные для услуг для EF.
        /// </summary>
        [ConfigurationProperty("EFMetadata", IsRequired = true)]
        public string EFMetadata
        {
            get { return ((string)_settings["EFMetadata"]); }
        }
    }
}
