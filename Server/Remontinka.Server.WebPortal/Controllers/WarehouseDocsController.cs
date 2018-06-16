using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DevExpress.Web;
using DevExpress.Web.ASPxThemes;
using DevExpress.Web.Mvc;
using log4net;
using Remontinka.Server.WebPortal.Helpers;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.DataLayer.Entities.ReportItems;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер документов.
    /// </summary>
    [ExtendedAuthorize(Roles = UserRole.Admin)]
    public class WarehouseDocsController:BaseController
    {
        /// <summary>
        /// Текущий логер.
        /// </summary>
        protected static ILog _logger = LogManager.GetLogger("WarehouseDocsController");

        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "WarehouseDocs";

        /// <summary>
        /// Осуществляет проверку на статус обработки складского документа.
        /// </summary>
        /// <param name="docID">Код  складского документа.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult CheckDocIsProcessed(Guid? docID)
        {
            if (docID == null)
            {
                return JCrudError("не задан документ");

            } //if

            try
            {
                var token = GetToken();
                var result = RemontinkaServer.Instance.EntitiesFacade.WarehouseDocIsProcessed(token, docID);

                return JCrud(new JCrudBooleanDataResult { ResultState = CrudResultKind.Success, Data = result });

            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время получения статуса документа {0} произошла ошибка {1} {2} {3} {4}", docID, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка в при получении статуса {0}", ex.Message)));
            } //try
        }

        /// <summary>
        /// Запускает обработку приходной накладной.
        /// </summary>
        /// <param name="warehouseDocID">Код приходной накладной.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult StartProcessIncomingDoc(Guid? warehouseDocID)
        {
            return InnerStartProcessDocument(warehouseDocID,
                                             RemontinkaServer.Instance.EntitiesFacade.ProcessIncomingDocItems);
        }

        /// <summary>
        /// Запускает обработку документа о перемещении между складами.
        /// </summary>
        /// <param name="warehouseDocID">Код документа перемещения между складами.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult StartProcessTransferDoc(Guid? warehouseDocID)
        {
            return InnerStartProcessDocument(warehouseDocID,
                                             RemontinkaServer.Instance.EntitiesFacade.ProcessTransferDocItems);
        }

        /// <summary>
        /// Запускает обработку документа о списании со склада.
        /// </summary>
        /// <param name="warehouseDocID">Код документа о списании.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult StartProcessCancellationDoc(Guid? warehouseDocID)
        {
            return InnerStartProcessDocument(warehouseDocID,
                                             RemontinkaServer.Instance.EntitiesFacade.ProcessCancellationDocItems);
        }

        /// <summary>
        /// Внутряняя обработка складского документа.
        /// </summary>
        /// <param name="docID">Код документа.</param>
        /// <param name="funct">Функция обработки.</param>
        /// <returns>Результат.</returns>
        private JsonResult InnerStartProcessDocument(Guid? docID, Func<SecurityToken, Guid?, DateTime, DateTime, ProcessWarehouseDocResult> funct)
        {
            if (docID == null)
            {
                return Json(new JCrudErrorResult("не задан документ"));

            } //if

            try
            {
                var token = GetToken();

                if (RemontinkaServer.Instance.EntitiesFacade.WarehouseDocIsProcessed(token, docID))
                {
                    return Json(new JCrudResult
                    {
                        ResultState = CrudResultKind.Success
                    });
                }

                var result = funct(token, docID, DateTime.Today, DateTime.UtcNow);
                if (result.ProcessResult)
                {
                    return Json(new JCrudResult
                    {
                        ResultState = CrudResultKind.Success
                    });
                } //if

                return Json(new JCrudErrorResult(result.ErrorMessage));

            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время обработки документа склада {0} произошла ошибка {1} {2} {3} {4}",
                                    docID, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка в при обработке документа склада {0}", ex.Message)));
            } //try
        }


        /// <summary>
        /// Внутряняя обработка складского документа.
        /// </summary>
        /// <param name="docID">Код документа.</param>
        /// <param name="funct">Функция обработки.</param>
        /// <returns>Результат.</returns>
        private JsonResult InnerStartUnProcessDocument(Guid? docID, Func<SecurityToken, Guid?, ProcessWarehouseDocResult> funct)
        {
            if (docID == null)
            {
                return Json(new JCrudErrorResult("не задан документ"));

            } //if

            try
            {
                var token = GetToken();

                if (!RemontinkaServer.Instance.EntitiesFacade.WarehouseDocIsProcessed(token, docID))
                {
                    return JCrud(new JCrudResult
                    {
                        ResultState = CrudResultKind.Success
                    });
                }

                var result = funct(token, docID);
                if (result.ProcessResult)
                {
                    return JCrud(new JCrudResult
                    {
                        ResultState = CrudResultKind.Success
                    });
                } //if

                return JCrudError(result.ErrorMessage);

            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время отмены обработки документа склада {0} произошла ошибка {1} {2} {3} {4}",
                                    docID, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return JCrudError(string.Format("Произошла ошибка в при обработке документа склада {0}", ex.Message));
            } //try
        }

        /// <summary>
        /// Запускает отмену обработки приходной накладной.
        /// </summary>
        /// <param name="warehouseDocID">Код отменяемой приходной накладной.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult StartUnProcessIncomingDoc(Guid? warehouseDocID)
        {
            return InnerStartUnProcessDocument(warehouseDocID,
                                             RemontinkaServer.Instance.EntitiesFacade.UnProcessIncomingDocItems);
        }

        /// <summary>
        /// Запускает отмену обработки документа о списании.
        /// </summary>
        /// <param name="warehouseDocID">Код отменяемого документа о списании.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult StartUnProcessCancellationDoc(Guid? warehouseDocID)
        {
            return InnerStartUnProcessDocument(warehouseDocID,
                                             RemontinkaServer.Instance.EntitiesFacade.UnProcessCancellationDocItems);
        }

        /// <summary>
        /// Запускает отмену обработки документа о перемещении товаров.
        /// </summary>
        /// <param name="warehouseDocID">Код отменяемого документа о перемещении.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult StartUnProcessTransferDoc(Guid? warehouseDocID)
        {
            return InnerStartUnProcessDocument(warehouseDocID,
                                             RemontinkaServer.Instance.EntitiesFacade.UnProcessTransferDocItems);
        }

        /// <summary>
        /// Строка обработки запросов на отмену и проведение документов.
        /// </summary>
        public const string ProcessDocItemClickJS = @"
    function(s,e){{
        jCrudEngine.updateWarehouseDocStatus({0},'{1}','{2}');
    }}
    ";

        public static void SetUpUpdateDocStateButton(ButtonSettings buttonSettings, string documentName,string gridName,object dataItem,UrlHelper helper)
        {
            var id = (Guid)DataBinder.Eval(dataItem, documentName+"ID");
            buttonSettings.Name = "Process"+ documentName + "Button" + id.EscapeForHtml();
            buttonSettings.RenderMode = ButtonRenderMode.Link;
            var isProcessed = (bool)DataBinder.Eval(dataItem, "IsProcessed");
            var action = "StartProcess"+ documentName;
            if (isProcessed)
            {
                buttonSettings.Text = "Отменить";
                buttonSettings.Images.Image.IconID = IconID.ActionsCancel16x16;
                action = "StartUnProcess"+ documentName;
            }
            else
            {
                buttonSettings.Text = "Провести";
                buttonSettings.Images.Image.IconID = IconID.SaveSave16x16;
            }

            buttonSettings.ImagePosition = ImagePosition.Right;
            buttonSettings.ClientSideEvents.Click = string.Format(ProcessDocItemClickJS, gridName, id, helper.Action(action, Remontinka.Server.WebPortal.Controllers.WarehouseDocsController.ControllerName));
        }
    }
}