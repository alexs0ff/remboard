using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.CustomReport
{
    /// <summary>
    /// Модель для контроллера редактирования отчетов.
    /// </summary>
    public class CustomizeReportModel
    {
        /// <summary>
        /// Задает или получает код отчета.
        /// </summary>
        [DisplayName("Документ")]
        [UIHint("AjaxComboBox")]
        [AjaxComboBox("AjaxCustomReportComboBox")]
        public Guid? CustomReportID { get; set; }
    }
}