using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.SystemForm
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
        
    }
}