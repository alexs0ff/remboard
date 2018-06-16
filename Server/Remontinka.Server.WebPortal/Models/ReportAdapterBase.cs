using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.DocumentServices.ServiceModel.DataContracts;
using DevExpress.XtraReports.UI;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Models
{
    /// <summary>
    /// Базовый класс для адаптера отчетов.
    /// </summary>
    public abstract class ReportAdapterBase<TReport,TReportParameters>
        where TReportParameters : ReportParametersModelBase
        where TReport: XtraReport
    {

        /// <summary>
        /// Создает отчет.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Отчет.</returns>
        public abstract TReport CreateReport(SecurityToken token);

        /// <summary>
        /// Создает параметры отчета.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="context">Контекст контроллера.</param>
        /// <returns>Созданный параметры.</returns>
        public abstract TReportParameters CreateReportParameters(SecurityToken token, ControllerContext context);

        /// <summary>
        /// Вызывается, когда необходимо обновление источника данных для отчета.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="report">Отчет.</param>
        /// <param name="reportParameters">Параметры отчета.</param>
        public abstract void UpdateDataSource(SecurityToken token, TReport report, TReportParameters reportParameters);

        /// <summary>
        /// Регистрация параметров периода.
        /// </summary>
        /// <param name="report">Отчет.</param>
        /// <param name="beginDate">Дата начала.</param>
        /// <param name="endDate">Дата окончания.</param>
        protected void SetPeriodParameters(XtraReport report, DateTime beginDate, DateTime endDate)
        {
            report.Parameters["BeginDate"].Value = beginDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            report.Parameters["EndDate"].Value = endDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
        }
    }
}