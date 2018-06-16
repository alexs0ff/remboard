using System;
using System.Web.Mvc;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.CustomReport;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер управления отчетами.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class CustomizeReportController : JCrudControllerBase<Guid, CustomReportItemModel, CustomReportItemModel, CustomReportItem, CustomReportItem>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "CustomizeReport";

        public ActionResult Index()
        {
            var model = new CustomizeReportModel();
            return View(model);
        }

        /// <summary>
        /// Инициализирует модель для создания формы новой сущности.
        /// </summary>
        ///<param name="token">Токен безопасности.</param>
        /// <returns>Инициализированная модель.</returns>
        protected override CustomReportItemModel InitCreateModel(SecurityToken token)
        {
            return new CustomReportItemModel();
        }

        private const string HtmlContentDefault = "Документ";

        /// <summary>
        /// Сохраняет модель при сохранении новой сущности.
        /// </summary>
        ///<param name="model">Модель для сохранения сущности.</param>
        ///<param name="token">Токен безопасности.</param>
        /// <returns>Сохраненная сущность.</returns>
        protected override CustomReportItem SaveCreateModel(CustomReportItemModel model, SecurityToken token)
        {
            var item = RemontinkaServer.Instance.EntitiesFacade.GetCustomReportItem(token,model.CustomReportItemID);
            if (item == null)
            {
                item = new CustomReportItem();
                item.HtmlContent = HtmlContentDefault;
            }
            item.CustomReportID = model.CustomReportItemID;
            item.DocumentKindID = model.DocumentKindID;
            item.Title = model.Title;

            RemontinkaServer.Instance.EntitiesFacade.SaveCustomReportItem(token, item);

            return item;
        }

        /// <summary>
        /// Сохраняет модель при редактировании существующей сущности.
        /// </summary>
        ///<param name="model">Модель для сохранения редактируемой сущности.</param>
        ///<param name="token">Токен безопасности.</param>
        /// <returns>Сохраненная сущность.</returns>
        protected override CustomReportItem SaveEditModel(CustomReportItemModel model, SecurityToken token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Инициализирует модель для редактирования формы существующей сущности.
        /// </summary>
        ///<param name="token">Токен безопасности.</param>
        ///<param name="id">Код сущности.</param>
        ///<returns>Инициализированная модель редактирования.</returns>
        protected override CustomReportItemModel InitEditModel(SecurityToken token, Guid? id)
        {
            var entity = RemontinkaServer.Instance.EntitiesFacade.GetCustomReportItem(token, id);

            var model = new CustomReportItemModel();

            model.CustomReportItemID = entity.CustomReportID;
            model.DocumentKindID = entity.DocumentKindID;
            model.Title = entity.Title;

            return model;
        }

        /// <summary>
        /// Удаляет сущность.
        /// </summary>
        /// <param name="token">Токен безопасности. </param>
        /// <param name="id">Код сущности для удаления.</param>
        protected override void DeleteEntity(SecurityToken token,Guid? id)
        {
            RemontinkaServer.Instance.EntitiesFacade.DeleteCustomReportItem(token,id);
        }

        /// <summary>
        /// Сохранение контента для отчета.
        /// </summary>
        /// <param name="model">Модель создания контента.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveReportContent(ReportContentModel model)
        {
            try
            {
                var token = GetToken();
                var item = RemontinkaServer.Instance.EntitiesFacade.GetCustomReportItem(token,model.CustomReportID);
                item.HtmlContent = model.HtmlContent;
                if (string.IsNullOrWhiteSpace(item.HtmlContent))
                {
                    item.HtmlContent = HtmlContentDefault;
                }
                RemontinkaServer.Instance.EntitiesFacade.SaveCustomReportItem(token, item);

                //Все нормально
                return
                    Json(new JCrudResult
                    {
                        ResultState = CrudResultKind.Success
                    });

            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время сохранения контента документа произошла ошибка {0} {1} {2} {3}",
                                    ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка {0}", ex.Message)));
            } //try
        }

        /// <summary>
        /// Получает контент отчета.
        /// </summary>
        /// <param name="reportID">Код отчета.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult GetReportContent(Guid? reportID)
        {
            try
            {
                var token = GetToken();
                var report = RemontinkaServer.Instance.EntitiesFacade.GetCustomReportItem(token, reportID);

                if (report==null)
                {
                    throw new Exception(string.Format("Отчет не найден {0}",reportID));
                }

                var model = new ReportContentModel();
                model.CustomReportID = reportID;
                model.HtmlContent = report.HtmlContent;

                return
                    Json(new JCrudItemResult<ReportContentModel>
                             {
                        ResultState = CrudResultKind.Success,
                        Item = model
                    });

            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat(
                    "Во время получения контента для отчета с id {0} произошла ошибка {1} {2} {3} {4}",
                    reportID, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка {0}", ex.Message)));
            } //try
        }
    }
}