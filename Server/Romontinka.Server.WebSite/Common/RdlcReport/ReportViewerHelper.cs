using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace Romontinka.Server.WebSite.Common.RdlcReport
{
    /// <summary>
    /// Методы для рендеринга отчетов.
    /// </summary>
    public static class ReportViewerHelper
    {
        private const string ReportTypeImage = "Image";

        private const string ReportTypeWord = "Word";

        private const string ReportTypeExcel = "Excel";

        private const string ReportTypePdf = "PDF";

        private const string DeviceInfoFormat =

           "<DeviceInfo>" +
           "  <OutputFormat>" + "{0}" + "</OutputFormat>" +
           "</DeviceInfo>";

        private const string ImageDeviceInfoFormat =

           "<DeviceInfo>" +
           "  <OutputFormat>Image</OutputFormat>" +
            "<StartPage>"+"{0}" +"</StartPage>"+
           "</DeviceInfo>";

        public static List<byte[]> RenderImage(LocalReport report)
        {
            var result = new List<byte[]>();

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            
            string mimeType;
            string encoding;
            string fileNameExtension;

            bool exception = false;
            int page = 1;
            do
            {
                try
                {
                    renderedBytes = report.Render(
                        ReportTypeImage,
                        string.Format(CultureInfo.InvariantCulture,ImageDeviceInfoFormat,page),
                        out mimeType,
                        out encoding,
                        out fileNameExtension,
                        out streams,
                        out warnings);

                    if (page%2==1)//четные убераем
                    {
                        result.Add(renderedBytes);    
                    }
                    
                }
                catch (LocalProcessingException)
                {
                    exception = true;
                }
                page++;

            } while (!exception);

            return result;
        }

        public static byte[] RenderReport(LocalReport report, ReportType format)
        {
            var type = string.Empty;

            switch (format)
            {
                case ReportType.Word:
                    type = ReportTypeWord;
                    break;
                case ReportType.Excel:
                    type = ReportTypeExcel;
                    break;
                case ReportType.Pdf:
                    type = ReportTypePdf;
                    break;
                case ReportType.Image:
                    type = ReportTypeImage;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("format");
            }

            

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            string reportType = type;
            string mimeType;
            string encoding;
            string fileNameExtension;

            renderedBytes = report.Render(
                reportType,
                string.Format(DeviceInfoFormat,type),
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return renderedBytes;

        }

    }
}