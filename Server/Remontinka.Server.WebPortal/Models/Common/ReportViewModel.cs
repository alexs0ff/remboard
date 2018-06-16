using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.XtraReports.UI;

namespace Remontinka.Server.WebPortal.Models.Common
{
    /// <summary>
    /// Модель для представления отчета.
    /// </summary>
    public class ReportViewModel
    {
        /// <summary>
        /// Задает или получает имя контроллера.
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Задает или получает имя действия.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Задает или получает имя действия экспорта данных.
        /// </summary>
        public string ExportActionName { get; set; }

        /// <summary>
        /// Задает или получает имя функции обновления параметров.
        /// </summary>
        public string UpdateParametersFunctionName { get; set; }

        /// <summary>
        /// Задает или получает наименование контрола отчетов.
        /// </summary>
        public string DocumentViewerName { get; set; }
        
        /// <summary>
        /// Задает или получает отчет.
        /// </summary>
        public XtraReport Report { get; set; }

        /// <summary>
        /// Параметры отчета.
        /// </summary>
        public ReportParametersModelBase ReportParameters { get; set; }
    }
}