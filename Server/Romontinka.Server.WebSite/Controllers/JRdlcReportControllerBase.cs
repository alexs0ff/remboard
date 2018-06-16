using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using Romontinka.Server.Core.Security;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Common.RdlcReport;
using log4net;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Базовый класс для контроллеров создания отчетов.
    /// </summary>
    public abstract class JRdlcReportControllerBase<TParameters>:BaseController/*TODO: сделать асинхронным*/
        where TParameters : JReportParametersBaseModel
    {
        /// <summary>
        /// Текущий логер.
        /// </summary>
        protected static ILog _logger = LogManager.GetLogger("JRdlcReport");

        /// <summary>
        /// Задает или получает заголовок страницы отчета.
        /// </summary>
        protected string ReportTitle { get; set; }

        /// <summary>
        /// Задает или получает флаг указывающий на необходимость автоматической загрузки отчета.
        /// </summary>
        protected bool AutoLoad { get; set; }

        /// <summary>
        /// Начальная страница.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(TParameters parameters)
        {
            ViewBag.Title = ReportTitle;
            ViewBag.Parameters = parameters;
            ViewBag.AutoLoad = AutoLoad;
            return View(ReportView);
        }

        /// <summary>
        /// Название представляния отчета.
        /// </summary>
        private const string ReportView = "JReport";

        /// <summary>
        /// Создает модель для панели параметров формы отчета по умолчанию.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="urlParameters">Параметры с URL.</param>
        /// <returns>Созданная модель.</returns>
        public abstract TParameters CreateDefaultPanel(SecurityToken token, TParameters urlParameters);

        /// <summary>
        /// Должен переопределиться для регистрации данных для отчета.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="report">Создаваемый отчет.</param>
        /// <param name="input">Значения полученные с формы ввода.</param>
        /// <returns>Название файла.</returns>
        public abstract string RegisterData(SecurityToken token, LocalReport report, TParameters input);

        #region Report File

        /// <summary>
        /// Задает или получает название файла отчета.
        /// </summary>
        protected string ReportFile { get; set; }

        /// <summary>
        /// Создает полный путь к файлу отчета.
        /// </summary>
        /// <param name="reportFileName">Имя файла отчета.</param>
        /// <returns>Поный путь.</returns>
        private string CreateReportPath(string reportFileName)
        {
            return Server.MapPath(Path.Combine(ReportFolder, reportFileName));
        }

        /// <summary>
        /// Путь к файлам отчета.
        /// TODO: сделать в параметрах
        /// </summary>
        private const string ReportFolder = "~/Reports/";

        #endregion Report File
        
        /// <summary>
        /// Создает объект отчета и производит регистрацию данных.
        /// </summary>
        /// <param name="input">Параметры отчета.</param>
        /// <param name="securityToken">Текущий токен безопасности.</param>
        /// <returns>Созданый и инициализированный отчет с данными.</returns>
        private JReportResult CreateReport(TParameters input, SecurityToken securityToken)
        {
            var result = new JReportResult();
            result.Report = new LocalReport();

            result.Report.ReportPath = CreateReportPath(ReportFile);
            result.OutputFileName = RegisterData(securityToken, result.Report, input);

            return result;
        }
        
        /// <summary>
        /// Строка с текстом ошибки при создании отчета.
        /// </summary>
        private const string ErrorFileData = "Failed report render";

        #region Formats

        /// <summary>
        /// Mime type для wordовского документа.
        /// </summary>
        private const string WordMimeType = "application/msword";

        /// <summary>
        /// Mime type для excelовского документа.
        /// </summary>
        private const string ExcelMimeType = "application/vnd.ms-excel";

        /// <summary>
        /// Mime type для pdfовского документа.
        /// </summary>
        private const string PdfMimeType = "application/pdf";

        /// <summary>
        /// Создает отчет в определенном формате.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="input">Параметры.</param>
        /// <param name="mimeType">Тип контента.</param>
        /// <param name="reportType">Тип создаваемого документа.</param>
        /// <param name="extention">Расширение.</param>
        /// <returns>Результат выполенения.</returns>
        private FileContentResult InternalRender(SecurityToken token,TParameters input,string mimeType,ReportType reportType ,string extention)
        {
            byte[] data;
            string outPutFileName = "ошибка формирования";
            _logger.InfoFormat("Старт формирования докомента {0} для отчета {1} пользователем {2}", extention,
                               ReportFile, token.User.LoginName);
            try
            {
                var jresult = CreateReport(input, token);
                data = ReportViewerHelper.RenderReport(jresult.Report, reportType);
                outPutFileName = jresult.OutputFileName;
                _logger.InfoFormat("Документ для отчета {0}: {1} успешно сформирован пользователю {2}", jresult.OutputFileName,
                                   ReportFile,token.User.LoginName);

            }
            catch (Exception ex)
            {
                data = Encoding.UTF8.GetBytes(ErrorFileData);
                string message = string.Empty;
                if (ex.InnerException != null)
                {
                    message = ex.InnerException.Message;
                } //if
                _logger.ErrorFormat(
                    "Во время создания документа отчета пользователем {0} произошла ошибка: {1} {2} {3} {4} {5}",
                    token.User.LoginName, ReportFile, message, ex.GetType(), ex.Message, ex.StackTrace);
            }

            return File(data, mimeType, string.Concat(outPutFileName, ".", extention));
        }

        /// <summary>
        /// Создает отчет в виде документа word.
        /// </summary>
        /// <param name="input">Данные параметров.</param>
        /// <returns>Результат выполнения.</returns>
        public FileContentResult RenderWord(TParameters input)
        {
            return InternalRender(GetToken(), input, WordMimeType, ReportType.Word, "doc");
        }

        /// <summary>
        /// Создает отчет в виде документа Excel.
        /// </summary>
        /// <param name="input">Данные параметров.</param>
        /// <returns>Результат выполнения.</returns>
        public FileContentResult RenderExcel(TParameters input)
        {
            return InternalRender(GetToken(), input, ExcelMimeType, ReportType.Excel, "xls");
        }

        /// <summary>
        /// Создает отчет в виде документа Pdf.
        /// </summary>
        /// <param name="input">Данные параметров.</param>
        /// <returns>Результат выполнения.</returns>
        public FileContentResult RenderPdf(TParameters input)
        {
            return InternalRender(GetToken(), input, PdfMimeType, ReportType.Pdf, "pdf");
        }

        #endregion Formats

        #region Render

        /// <summary>
        /// Создает изображение для отображения отчета.
        /// </summary>
        /// <returns>Результат.</returns>
        public JsonResult RenderReport(TParameters input)
        {
            return Json(GetReportPng(input, GetToken()));
        }

        /// <summary>
        /// Создает изображение png представляющую отчет для отправки на страницу.
        /// </summary>
        /// <param name="input">Параметры отчета.</param>
        /// <param name="securityToken">Текущий токен безопасности.</param>
        /// <returns>Данные с изображением png Отчета.</returns>
        private JCrudResult GetReportPng(TParameters input, SecurityToken securityToken)
        {
            try
            {
                
                _logger.InfoFormat("Старт формирования отчета {0},{1} пользователем {2}", GetType(), ReportFile,
                                   securityToken.User.LoginName);
                var jresult = CreateReport(input, securityToken);

                var renderImages = ReportViewerHelper.RenderImage(jresult.Report);
                var result = new List<byte[]>();

                foreach (var renderImage in renderImages)
                {
                    using (var ms = new MemoryStream(renderImage))
                    {
                        var bmp = new Bitmap(ms);
                        using (var stream = new MemoryStream())
                        {
                            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                            stream.Close();

                            result.Add( stream.ToArray());
                        }
                    }
                }

                _logger.InfoFormat("Формирование отчета '{0}' {1} завершено для пользователя {2}",
                                   jresult.OutputFileName, GetType(), securityToken.User.LoginName);
                

                return new JReportDataResult
                {
                    ReportData = result.Select(Convert.ToBase64String).ToArray(), 
                               ResultState = CrudResultKind.Success
                           };
            }
            catch (Exception ex)
            {
                string message = string.Empty;
                if (ex.InnerException != null)
                {
                    message = ex.InnerException.Message +":";

                    if (ex.InnerException.InnerException!=null)
                    {
                        message += ":" + ex.InnerException.InnerException.Message;
                        _logger.ErrorFormat(ex.InnerException.InnerException.StackTrace);
                    }
                } //if
                _logger.ErrorFormat("Во время создания отчета произошла ошибка: {0} {1} {2} {3} {4} {5}",securityToken.User.LoginName,ReportFile, message, ex.GetType(), ex.Message, ex.StackTrace);
                return new JCrudErrorResult("Произошла ошибка во время создания отчета, если данная ошибка будет повторяться обратитесь в техподдержку");
            } //try
        }

        #endregion Render

        /// <summary>
        /// Название Parial View для панели с параметрами.
        /// </summary>
        private const string ParametersViewName = "parameters";

        /// <summary>
        /// Рендерит параметры для отчета.
        /// </summary>
        /// <returns>Результат выполнения.</returns>
        [ChildActionOnly]
        public ActionResult ParameterPanel(TParameters parameters)
        {
            var token = GetToken();
            TParameters panel = CreateDefaultPanel(token,parameters);
            
            ModelState.Clear();
            return PartialView(ParametersViewName, panel);
        }

        #region Misc

        /// <summary>
        /// Устанавливает значение для параметров периода.
        /// </summary>
        /// <param name="report">Отчет в котором заданы параметры начала и окончания периода в текстовом формате.</param>
        /// <param name="beginDate">Дата начала периода.</param>
        /// <param name="endDate">Дата окончания периода.</param>
        protected void SetPeriodParameters(LocalReport report, DateTime beginDate, DateTime endDate)
        {
            report.SetParameters(new ReportParameter("BeginDate", beginDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)));
            report.SetParameters(new ReportParameter("EndDate", endDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)));
        }

        #endregion Misc

    }
}