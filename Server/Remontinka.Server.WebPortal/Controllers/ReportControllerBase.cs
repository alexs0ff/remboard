using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpress.XtraReports.UI;
using log4net;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.Common;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Базовый класс для всех контроллеров отчетов.
    /// </summary>
    public abstract class ReportControllerBase<TReport, TReportAdapter, TReportParameters> : BaseController
        where TReportParameters : ReportParametersModelBase
        where TReport : XtraReport
        where TReportAdapter : ReportAdapterBase<TReport, TReportParameters>
    {
        /// <summary>
        /// Содержит адаптер данных.
        /// </summary> 
        private readonly ReportAdapterBase<TReport, TReportParameters> _dataAdapter;

        protected ReportControllerBase(ReportAdapterBase<TReport, TReportParameters> dataAdapter)
        {
            _dataAdapter = dataAdapter;
        }

        /// <summary>
        /// Возвращает имя контроллера.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetControllerName();

        /// <summary>
        /// Текущий логер.
        /// </summary>
        protected static ILog _logger = LogManager.GetLogger("ReportController");

        /// <summary>
        /// Метод указывающий необходимо ли использовать лайаут по-умолчанию.
        /// </summary>
        /// <returns>Признак использования лайаута по-умолчанию.</returns>
        public override bool UseDefaultLayout()
        {
            return false;
        }

        /// <summary>
        /// Создает представление для отображения отчета.
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportView()
        {
            ReportViewModel reportModel = null;
            try
            {
                reportModel = ProcessReport(null);
            }
            catch (Exception ex)
            {

                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время формирования отчета {0} произошла ошибка {1} {2} {3} {4}", GetType().Name, ex.Message, ex.GetType(), innerException, ex.StackTrace);
            }

            return View("devExpressReport",reportModel);
        }

        /// <summary>
        /// Создает колбек для отображения viwerа.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReport( TReportParameters parameters)
        {
            ReportViewModel reportModel = null;
            
            try
            {
                reportModel = ProcessReport(parameters);
            }
            catch (Exception ex)
            {

                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время получения данных отчета {0} произошла ошибка {1} {2} {3} {4}", GetType().Name, ex.Message, ex.GetType(), innerException, ex.StackTrace);
            }

            return View("report", reportModel);
        }

        /// <summary>
        /// Создает колбек для отображения viwerа.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetExport(TReportParameters parameters)
        {
            ReportViewModel reportModel = null;
            try
            {
                reportModel = ProcessReport(parameters);
            }
            catch (Exception ex)
            {

                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время получения экспорта отчета {0} произошла ошибка {1} {2} {3} {4}", GetType().Name, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return HttpNotFound();
            }
            
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(reportModel.Report);
        }

        /// <summary>
        /// Обрабоатывает параметры отчета и создает модель.
        /// </summary>
        /// <param name="parameters">Параметры.</param>
        /// <returns>Модель.</returns>
        private ReportViewModel ProcessReport(TReportParameters parameters)
        {
            var model = new ReportViewModel();
            var token = GetToken();
            model.ReportParameters = parameters;
            if (parameters == null)
            {
                model.ReportParameters = _dataAdapter.CreateReportParameters(token, ControllerContext);
            }

            model.ReportParameters.Token =token;

            var reportParameters = (TReportParameters)model.ReportParameters;

            model.ControllerName = GetControllerName();
            model.ActionName = "GetReport";
            model.ExportActionName = "GetExport";
            model.DocumentViewerName = model.ControllerName + "DocumentViewer";
            model.UpdateParametersFunctionName = model.ControllerName + "UpdateParameters";
            var report = _dataAdapter.CreateReport(token);
            model.Report = report;
            
            _dataAdapter.UpdateDataSource(token, report, reportParameters);

            return model;
        }
    }
}