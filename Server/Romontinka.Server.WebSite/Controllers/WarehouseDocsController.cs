using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.DataLayer.Entities.ReportItems;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.Controls;
using Romontinka.Server.WebSite.Models.DataGrid;
using Romontinka.Server.WebSite.Models.WarehouseDocsForm;
using log4net;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер по управлению складскими документами.
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
        /// Получает содержимое страницы.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            /*Приходные накладные*/
            var model = new WarehouseDocsViewModel();
            model.IncomingDocGrid = new DataGridDescriptor();
            model.IncomingDocGrid.Name = IncomingDocController.ControllerName;
            model.IncomingDocGrid.Controller = IncomingDocController.ControllerName;
            model.IncomingDocGrid.SearchInputs.Add(new DateTimeSearchInput { Name = "Начало",Id = "IncomingDocBeginDate",DateTimeValue = Utils.GetFirstDayOfMonth(DateTime.Today)});
            model.IncomingDocGrid.SearchInputs.Add(new DateTimeSearchInput {Name = "Окончание", Id = "IncomingDocEndDate", DateTimeValue = DateTime.Today });
            model.IncomingDocGrid.SearchInputs.Add(new NewLineSearchInput());
            model.IncomingDocGrid.SearchInputs.Add(new ComboBoxSearchInput { Name = "Склад", ComboBoxModel = new AjaxComboBoxModel { Property = "IncomingDocWarehouseID", Controller = "AjaxWarehouseComboBox", FirstIsNull = true } });
            model.IncomingDocGrid.SearchInputs.Add(new TextSearchInput { Id = "IncomingDocName", Value = string.Empty, Name = "Имя" });

            model.IncomingDocGrid.Columns.Add(new TextGridColumn { Name = "Изменено", Id = "Creator" });
            model.IncomingDocGrid.Columns.Add(new TextGridColumn { Name = "Склад", Id = "WarehouseTitle" });
            model.IncomingDocGrid.Columns.Add(new TextGridColumn { Name = "Поставщик", Id = "ContractorLegalName" });
            model.IncomingDocGrid.Columns.Add(new TextGridColumn { Name = "Номер", Id = "DocNumber" });
            model.IncomingDocGrid.Columns.Add(new TextGridColumn { Name = "Дата", Id = "DocDate" });
            model.IncomingDocGrid.Columns.Add(new TextGridColumn { Name = "Описание", Id = "DocDescription" });

            model.IncomingDocGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание накладной", Height = 370, Width = 480 };
            model.IncomingDocGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 370, Width = 480 };
            model.IncomingDocGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить накладную ", DataId = "DocNumber" };
            model.IncomingDocGrid.ShowDetailsButtonColumn = new ShowDetailsButtonColumn { ToolTip = "Показать элементы накладной", CallFunctionName = "showIncomingDocItems" };

            model.IncomingDocGrid.AutoLoad = true;

            model.IncomingDocGrid.HasTableBorderedClass = true;
            model.IncomingDocGrid.HasTableStripedClass = false;

            /*Элементы приходной накладной*/

            model.IncomingDocItemsGrid = new DataGridDescriptor();
            model.IncomingDocItemsGrid.Name = IncomingDocItemController.ControllerName;
            model.IncomingDocItemsGrid.Controller = IncomingDocItemController.ControllerName;
            model.IncomingDocItemsGrid.SearchInputs.Add(new HiddenSearchInput { Id = "IncomingDocItemDocID" });
            model.IncomingDocItemsGrid.SearchInputs.Add(new TextSearchInput { Id = "IncomingDocItemName", Value = string.Empty, Name = "Наименование" });

            model.IncomingDocItemsGrid.Columns.Add(new TextGridColumn { Name = "Номенклатура", Id = "GoodsItemTitle" });
            model.IncomingDocItemsGrid.Columns.Add(new TextGridColumn { Name = "Количество", Id = "Total" });
            model.IncomingDocItemsGrid.Columns.Add(new TextGridColumn { Name = "Цена закупки", Id = "InitPrice" });
            model.IncomingDocItemsGrid.Columns.Add(new TextGridColumn { Name = "Нулевая цена", Id = "StartPrice" });
            model.IncomingDocItemsGrid.Columns.Add(new TextGridColumn { Name = "Ремонтная цена", Id = "RepairPrice" });
            model.IncomingDocItemsGrid.Columns.Add(new TextGridColumn { Name = "Цена продажи", Id = "SalePrice" });
            model.IncomingDocItemsGrid.Columns.Add(new TextGridColumn { Name = "Описание", Id = "Description" });

            model.IncomingDocItemsGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание элемента накладной", Height = 480, Width = 520 };
            model.IncomingDocItemsGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 480, Width = 520 };
            model.IncomingDocItemsGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить элемент ", DataId = "GoodsItemTitle" };

            model.IncomingDocItemsGrid.AutoLoad = false;

            model.IncomingDocItemsGrid.HasTableBorderedClass = true;
            model.IncomingDocItemsGrid.HasTableStripedClass = true;

            /*Документы о списании со склада*/
            model.CancellationDocGrid = new DataGridDescriptor();
            model.CancellationDocGrid.Name = CancellationDocController.ControllerName;
            model.CancellationDocGrid.Controller = CancellationDocController.ControllerName;
            model.CancellationDocGrid.SearchInputs.Add(new DateTimeSearchInput { Name = "Начало", Id = "CancellationDocBeginDate", DateTimeValue = Utils.GetFirstDayOfMonth(DateTime.Today) });
            model.CancellationDocGrid.SearchInputs.Add(new DateTimeSearchInput { Name = "Окончание", Id = "CancellationDocEndDate", DateTimeValue = DateTime.Today });
            model.CancellationDocGrid.SearchInputs.Add(new NewLineSearchInput());
            model.CancellationDocGrid.SearchInputs.Add(new ComboBoxSearchInput { Name = "Склад", ComboBoxModel = new AjaxComboBoxModel { Property = "CancellationDocWarehouseID", Controller = "AjaxWarehouseComboBox", FirstIsNull = true } });
            model.CancellationDocGrid.SearchInputs.Add(new TextSearchInput { Id = "CancellationDocName", Value = string.Empty, Name = "Имя" });

            model.CancellationDocGrid.Columns.Add(new TextGridColumn { Name = "Изменено", Id = "Creator" });
            model.CancellationDocGrid.Columns.Add(new TextGridColumn { Name = "Склад", Id = "WarehouseTitle" });
            model.CancellationDocGrid.Columns.Add(new TextGridColumn { Name = "Номер", Id = "DocNumber" });
            model.CancellationDocGrid.Columns.Add(new TextGridColumn { Name = "Дата", Id = "DocDate" });
            model.CancellationDocGrid.Columns.Add(new TextGridColumn { Name = "Описание", Id = "DocDescription" });

            model.CancellationDocGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание документа о списании", Height = 370, Width = 480 };
            model.CancellationDocGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 370, Width = 480 };
            model.CancellationDocGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить документ о списании ", DataId = "DocNumber" };
            model.CancellationDocGrid.ShowDetailsButtonColumn = new ShowDetailsButtonColumn { ToolTip = "Показать элементы документа о списании", CallFunctionName = "showCancellationDocItems" };

            model.CancellationDocGrid.AutoLoad = true;

            model.CancellationDocGrid.HasTableBorderedClass = true;
            model.CancellationDocGrid.HasTableStripedClass = false;

            /*Элементы документа о списании*/
            model.CancellationDocItemsGrid = new DataGridDescriptor();
            model.CancellationDocItemsGrid.Name = CancellationDocItemController.ControllerName;
            model.CancellationDocItemsGrid.Controller = CancellationDocItemController.ControllerName;
            model.CancellationDocItemsGrid.SearchInputs.Add(new HiddenSearchInput { Id = "CancellationDocItemDocID" });
            model.CancellationDocItemsGrid.SearchInputs.Add(new TextSearchInput { Id = "CancellationDocItemName", Value = string.Empty, Name = "Наименование" });

            model.CancellationDocItemsGrid.Columns.Add(new TextGridColumn { Name = "Номенклатура", Id = "GoodsItemTitle" });
            model.CancellationDocItemsGrid.Columns.Add(new TextGridColumn { Name = "Количество", Id = "Total" });
            model.CancellationDocItemsGrid.Columns.Add(new TextGridColumn { Name = "Описание", Id = "Description" });

            model.CancellationDocItemsGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание элемента документа", Height = 300, Width = 520 };
            model.CancellationDocItemsGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 300, Width = 520 };
            model.CancellationDocItemsGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить элемент документа ", DataId = "GoodsItemTitle" };

            model.CancellationDocItemsGrid.AutoLoad = false;

            model.CancellationDocItemsGrid.HasTableBorderedClass = true;
            model.CancellationDocItemsGrid.HasTableStripedClass = true;

            /*документы перемещения между складами*/

            model.TransferDocGrid = new DataGridDescriptor();
            model.TransferDocGrid.Name = TransferDocController.ControllerName;
            model.TransferDocGrid.Controller = TransferDocController.ControllerName;
            model.TransferDocGrid.SearchInputs.Add(new ComboBoxSearchInput { Name = "Со склада", ComboBoxModel = new AjaxComboBoxModel { Property = "TransferDocSenderWarehouseID", Controller = "AjaxWarehouseComboBox", FirstIsNull = true } });
            model.TransferDocGrid.SearchInputs.Add(new ComboBoxSearchInput { Name = "На склад", ComboBoxModel = new AjaxComboBoxModel { Property = "TransferDocRecipientWarehouseID", Controller = "AjaxWarehouseComboBox", FirstIsNull = true } });
            model.TransferDocGrid.SearchInputs.Add(new NewLineSearchInput());
            model.TransferDocGrid.SearchInputs.Add(new DateTimeSearchInput { Name = "Начало", Id = "TransferDocBeginDate", DateTimeValue = Utils.GetFirstDayOfMonth(DateTime.Today) });
            model.TransferDocGrid.SearchInputs.Add(new DateTimeSearchInput { Name = "Окончание", Id = "TransferDocEndDate", DateTimeValue = DateTime.Today });
            model.TransferDocGrid.SearchInputs.Add(new NewLineSearchInput());
            model.TransferDocGrid.SearchInputs.Add(new TextSearchInput { Id = "TransferDocName", Value = string.Empty, Name = "Имя" });

            model.TransferDocGrid.Columns.Add(new TextGridColumn { Name = "Изменено", Id = "Creator" });
            model.TransferDocGrid.Columns.Add(new TextGridColumn { Name = "Со склада", Id = "SenderWarehouseTitle" });
            model.TransferDocGrid.Columns.Add(new TextGridColumn { Name = "На склад", Id = "RecipientWarehouseTitle" });
            model.TransferDocGrid.Columns.Add(new TextGridColumn { Name = "Номер", Id = "DocNumber" });
            model.TransferDocGrid.Columns.Add(new TextGridColumn { Name = "Дата", Id = "DocDate" });
            model.TransferDocGrid.Columns.Add(new TextGridColumn { Name = "Описание", Id = "DocDescription" });

            model.TransferDocGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание документа о перемещении", Height = 370, Width = 480 };
            model.TransferDocGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 370, Width = 480 };
            model.TransferDocGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить документ о перемещении ", DataId = "DocNumber" };
            model.TransferDocGrid.ShowDetailsButtonColumn = new ShowDetailsButtonColumn { ToolTip = "Показать элементы документа о перемещении", CallFunctionName = "showTransferDocItems" };

            model.TransferDocGrid.AutoLoad = true;

            model.TransferDocGrid.HasTableBorderedClass = true;
            model.TransferDocGrid.HasTableStripedClass = false;

            /*Элементы документа о перемещении*/

            model.TransferDocItemsGrid = new DataGridDescriptor();
            model.TransferDocItemsGrid.Name = TransferDocItemController.ControllerName;
            model.TransferDocItemsGrid.Controller = TransferDocItemController.ControllerName;
            model.TransferDocItemsGrid.SearchInputs.Add(new HiddenSearchInput { Id = "TransferDocItemDocID" });
            model.TransferDocItemsGrid.SearchInputs.Add(new TextSearchInput { Id = "TransferDocItemName", Value = string.Empty, Name = "Наименование" });

            model.TransferDocItemsGrid.Columns.Add(new TextGridColumn { Name = "Номенклатура", Id = "GoodsItemTitle" });
            model.TransferDocItemsGrid.Columns.Add(new TextGridColumn { Name = "Количество", Id = "Total" });
            model.TransferDocItemsGrid.Columns.Add(new TextGridColumn { Name = "Описание", Id = "Description" });

            model.TransferDocItemsGrid.CreateButtonGrid = new CreateButtonGrid { Name = "Создание элемента документа", Height = 300, Width = 520 };
            model.TransferDocItemsGrid.EditButtonGridColumn = new EditButtonGridColumn { Height = 300, Width = 520 };
            model.TransferDocItemsGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить элемент документа ", DataId = "GoodsItemTitle" };

            model.TransferDocItemsGrid.AutoLoad = false;

            model.TransferDocItemsGrid.HasTableBorderedClass = true;
            model.TransferDocItemsGrid.HasTableStripedClass = true;

            return View(model);
        }

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
                return Json(new JCrudErrorResult("не задан документ"));

            } //if

            try
            {
                var token = GetToken();
                var result = RemontinkaServer.Instance.EntitiesFacade.WarehouseDocIsProcessed(token, docID);

                return Json(new JCrudBooleanDataResult {ResultState = CrudResultKind.Success, Data = result});

            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время получения статуса документа {0} произошла ошибка {1} {2} {3} {4}",docID, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка в при получении статуса {0}", ex.Message)));
            } //try
        }

        /// <summary>
        /// Запускает обработку приходной накладной.
        /// </summary>
        /// <param name="incomingDocID">Код приходной накладной.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult StartProcessIncomingDoc(Guid? incomingDocID)
        {
            return InnerStartProcessDocument(incomingDocID,
                                             RemontinkaServer.Instance.EntitiesFacade.ProcessIncomingDocItems);
        }

        /// <summary>
        /// Запускает обработку документа о перемещении между складами.
        /// </summary>
        /// <param name="transferDocID">Код документа перемещения между складами.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult StartProcessTransferDoc(Guid? transferDocID)
        {
            return InnerStartProcessDocument(transferDocID,
                                             RemontinkaServer.Instance.EntitiesFacade.ProcessTransferDocItems);
        }

        /// <summary>
        /// Запускает обработку документа о списании со склада.
        /// </summary>
        /// <param name="cancellationDocID">Код документа о списании.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult StartProcessCancellationDoc(Guid? cancellationDocID)
        {
            return InnerStartProcessDocument(cancellationDocID,
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
                    return Json(new JCrudResult
                    {
                        ResultState = CrudResultKind.Success
                    });
                }

                var result = funct(token, docID);
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

                _logger.ErrorFormat("Во время отмены обработки документа склада {0} произошла ошибка {1} {2} {3} {4}",
                                    docID, ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка в при обработке документа склада {0}", ex.Message)));
            } //try
        }

        /// <summary>
        /// Запускает отмену обработки приходной накладной.
        /// </summary>
        /// <param name="incomingDocID">Код отменяемой приходной накладной.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult StartUnProcessIncomingDoc(Guid? incomingDocID)
        {
            return InnerStartUnProcessDocument(incomingDocID,
                                             RemontinkaServer.Instance.EntitiesFacade.UnProcessIncomingDocItems);
        }

        /// <summary>
        /// Запускает отмену обработки документа о списании.
        /// </summary>
        /// <param name="cancellationDocID">Код отменяемого документа о списании.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult StartUnProcessCancellationDoc(Guid? cancellationDocID)
        {
            return InnerStartUnProcessDocument(cancellationDocID,
                                             RemontinkaServer.Instance.EntitiesFacade.UnProcessCancellationDocItems);
        }

        /// <summary>
        /// Запускает отмену обработки документа о перемещении товаров.
        /// </summary>
        /// <param name="transferDocID">Код отменяемого документа о перемещении.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult StartUnProcessTransferDoc(Guid? transferDocID)
        {
            return InnerStartUnProcessDocument(transferDocID,
                                             RemontinkaServer.Instance.EntitiesFacade.UnProcessTransferDocItems);
        }
    }
}