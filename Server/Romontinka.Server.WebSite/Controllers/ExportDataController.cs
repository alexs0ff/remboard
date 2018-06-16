using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.Core;
using Romontinka.Server.Core.ServiceEntities;
using Romontinka.Server.Core.UnitOfWorks;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.SystemForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер экспорта для данных системы.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class ExportDataController : BaseAsyncController
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "ExportData";

        /// <summary>
        /// Mime type для csvовского документа.
        /// </summary>
        protected const string CsvMimeType = "text/csv";

        /// <summary>
        /// Наименование файла экспорта.
        /// </summary>
        private const string ExportFileName = "export.csv";

        /// <summary>
        /// Создает экспортируемый файл.
        /// </summary>
        /// <param name="model">Модель.</param>
        /// <returns>Асинхронный результат.</returns>
        public Task<FileContentResult> StartExport(ExportModel model)
        {
            var name = HttpContext.User.Identity.Name;
            return
                Task.Factory.StartNew(() => InternalExport(name, model)).ContinueWith(
                    t => File(t.Result, CsvMimeType, ExportFileName));
        }

        /// <summary>
        /// Содержит внутреннию логику экспорта.
        /// </summary>
        private byte[] InternalExport(string userName, ExportModel model)
        {
            var token = GetToken(userName);

            return RemontinkaServer.Instance.SystemService.Export(token, new ExportParams
                                                                         {
                                                                             BeginDate = model.BeginDate,
                                                                             EndDate = model.EndDate,
                                                                             Kind =
                                                                                 ExportKindSet.GetKindByID(model.Kind)
                                                                         });
        }
    }
}