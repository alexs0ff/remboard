using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.SystemForm
{
    /// <summary>
    /// Модель окна системных настроек.
    /// </summary>
    public class SystemModel
    {
        /// <summary>
        /// Задает или получает данные о регистрационном домене.
        /// </summary>
        public RegistrationInfoModel RegistrationInfoModel { get; set; }

        /// <summary>
        /// Задает или получает модель экспорта данных.
        /// </summary>
        public ExportModel ExportModel { get; set; }
    }
}