using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Helpers;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.Controls;
using Romontinka.Server.WebSite.Models.DataGrid;
using Romontinka.Server.WebSite.Models.RepairOrderForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер для управления заказами.
    /// </summary>
    [ExtendedAuthorize]
    public class RepairOrderController : JGridControllerBase<Guid, RepairOrderGridItemModel, RepairOrderCreateModel, RepairOrderEditModel, RepairOrderSearchModel>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "RepairOrder";

        /// <summary>
        /// Инициализирует новый инстанс для контроллера данных грида.
        /// </summary>
        /// <param name="adapter">Адаптер даных.</param>
        public RepairOrderController(JGridDataAdapterBase<Guid, RepairOrderGridItemModel, RepairOrderCreateModel, RepairOrderEditModel, RepairOrderSearchModel> adapter) : base(adapter)
        {
        }

        /// <summary>
        /// Получает содержимое страницы.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = CreateRepairOrderViewModel(GetToken());
            ViewBag.Title = "Управление заказами";
            return View("RepairOrderGrid", model);
        }

        /// <summary>
        /// Создает модель данных для представления списка заказов.
        /// </summary>
        /// <returns>Модель данных.</returns>
        public static RepairOrderViewModel CreateRepairOrderViewModel(SecurityToken token)
        {
            var model = new RepairOrderViewModel();
            model.OrderGrid = new DataGridDescriptor();
            model.OrderGrid.BeforeGridUpdateJsFunctionName = "BeforeUpdateRepairOrderGrid";
            model.OrderGrid.Name = ControllerName;
            model.OrderGrid.Controller = ControllerName;
            model.OrderGrid.SearchInputs.Add(new ComboBoxSearchInput
                                             {
                                                 Name = "Фильтр",
                                                 ComboBoxModel =
                                                     new AjaxComboBoxModel
                                                     {
                                                         Property = "FilterID",
                                                         Controller = "AjaxRepairOrderFilterComboBox",
                                                         FirstIsNull = true
                                                     }
                                             });
            model.OrderGrid.SearchInputs.Add(new ComboBoxSearchInput
                                             {
                                                 Name = "Пользователь",
                                                 ComboBoxModel =
                                                     new AjaxComboBoxModel
                                                     {
                                                         Property = "UserID",
                                                         Controller = "AjaxUserComboBox",
                                                         FirstIsNull = true
                                                     }
                                             });
            model.OrderGrid.SearchInputs.Add(new ComboBoxSearchInput
                                             {
                                                 Name = "Статус",
                                                 ComboBoxModel = new AjaxComboBoxModel
                                                                 {
                                                                     Property = "OrderStatusID",
                                                                     Controller = "AjaxOrderStatus",
                                                                     FirstIsNull = true
                                                                 }
                                             });

            model.OrderGrid.SearchInputs.Add(new TextSearchInput {Id = "Name", Value = string.Empty, Name = "Имя"});
            model.OrderGrid.SearchInputs.Add(new HiddenSearchInput { Id = "CopyFromRepairOrderID" });

            model.OrderGrid.Columns.Add(new TextGridColumn {Name = "Номер", Id = "Number"});
            model.OrderGrid.Columns.Add(new TextGridColumn {Name = "Статус", Id = "StatusTitle"});
            model.OrderGrid.Columns.Add(new TextGridColumn {Name = "Дата", Id = "EventDate"});
            model.OrderGrid.Columns.Add(new TextGridColumn { Name = "Дата готовности", Id = "EventDateOfBeReady" });
            model.OrderGrid.Columns.Add(new TextGridColumn {Name = "Менеджер", Id = "ManagerFullName"});
            model.OrderGrid.Columns.Add(new TextGridColumn {Name = "Инженер", Id = "EngineerFullName"});
            model.OrderGrid.Columns.Add(new TextGridColumn {Name = "Клиент", Id = "ClientFullName"});
            model.OrderGrid.Columns.Add(new TextGridColumn {Name = "Устройство", Id = "DeviceTitle"});

            if (ProjectRoleSet.UserHasRole(token.User.ProjectRoleID, ProjectRoleSet.Engineer))
            {
                model.OrderGrid.Columns.Add(new TextGridColumn { Name = "Неисправности", Id = "Defect" });
            }
            else
            {
                model.OrderGrid.Columns.Add(new TextGridColumn { Name = "Суммы", Id = "Totals" });    
            }
            

            if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
            {
                model.OrderGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn { QuestionText = "Вы точно хотите удалить заказ ", DataId = "Number" };    
            }

            if (ProjectRoleSet.UserHasRole(token.User.ProjectRoleID, ProjectRoleSet.Admin, ProjectRoleSet.Manager))
            {
               
                model.OrderGrid.CreateButtonGrid = new CreateButtonGrid
                                                   {Name = "Создание заказа", FullScreen = true, NoDialogTitle = true};
            } //if
            
            model.OrderGrid.EditButtonGridColumn = new EditButtonGridColumn
                                                       {FullScreen = true, NoDialogTitle = true};
            
            model.OrderGrid.ShowDetailsButtonColumn = new ShowDetailsButtonColumn
                                                      {ToolTip = "Показать работы и запчасти", CallFunctionName = "showDetails"};
            model.OrderGrid.AutoLoad = true;

            model.OrderGrid.HasTableBorderedClass = true;
            model.OrderGrid.HasTableStripedClass = false;

            model.WorkItemsGrid = new DataGridDescriptor();
            model.WorkItemsGrid.Name = WorkItemController.ControllerName;
            model.WorkItemsGrid.Controller = WorkItemController.ControllerName;
            model.WorkItemsGrid.SearchInputs.Add(new HiddenSearchInput {Id = "WorkItemRepairOrderID"});
            model.WorkItemsGrid.SearchInputs.Add(new TextSearchInput
                                                 {Id = "WorkItemName", Value = string.Empty, Name = "Название"});

            model.WorkItemsGrid.Columns.Add(new TextGridColumn {Name = "Дата", Id = "WorkItemEventDate"});
            model.WorkItemsGrid.Columns.Add(new TextGridColumn {Name = "Описание", Id = "WorkItemTitle"});
            model.WorkItemsGrid.Columns.Add(new TextGridColumn {Name = "Инженер", Id = "WorkItemEngineerFullName"});
            model.WorkItemsGrid.Columns.Add(new TextGridColumn {Name = "Стоимость", Id = "WorkItemPrice"});

            model.WorkItemsGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn
                                                         {
                                                             QuestionText = "Вы точно хотите удалить работу ",
                                                             DataId = "WorkItemTitle"
                                                         };

            model.WorkItemsGrid.EditButtonGridColumn = new EditButtonGridColumn {Height = 400, Width = 500};
            model.WorkItemsGrid.CreateButtonGrid = new CreateButtonGrid
                                                   {Name = "Создание выполненной работы", Height = 400, Width = 500};

            model.WorkItemsGrid.AutoLoad = false;

            model.WorkItemsGrid.HasTableBorderedClass = true;
            model.WorkItemsGrid.HasTableStripedClass = true;

            model.DeviceItemsGrid = new DataGridDescriptor();
            model.DeviceItemsGrid.Name = DeviceItemController.ControllerName;
            model.DeviceItemsGrid.Controller = DeviceItemController.ControllerName;
            model.DeviceItemsGrid.SearchInputs.Add(new HiddenSearchInput {Id = "DeviceItemRepairOrderID"});
            model.DeviceItemsGrid.SearchInputs.Add(new TextSearchInput
                                                   {Id = "DeviceItemName", Value = string.Empty, Name = "Название"});

            model.DeviceItemsGrid.Columns.Add(new TextGridColumn {Name = "Описание", Id = "DeviceItemTitle"});
            model.DeviceItemsGrid.Columns.Add(new TextGridColumn {Name = "Количество", Id = "DeviceItemCount"});
            model.DeviceItemsGrid.Columns.Add(new TextGridColumn {Name = "Себестоимость", Id = "DeviceItemCostPrice"});
            model.DeviceItemsGrid.Columns.Add(new TextGridColumn {Name = "Стоимость", Id = "DeviceItemPrice"});

            model.DeviceItemsGrid.DeleteButtonGridColumn = new DeleteButtonGridColumn
                                                           {
                                                               QuestionText = "Вы точно хотите удалить запчасть ",
                                                               DataId = "DeviceItemTitle"
                                                           };

            model.DeviceItemsGrid.EditButtonGridColumn = new EditButtonGridColumn {Height = 450, Width = 550};
            model.DeviceItemsGrid.CreateButtonGrid = new CreateButtonGrid
                                                     {Name = "Создание запчасти", Height = 450, Width = 550};

            model.DeviceItemsGrid.AutoLoad = false;

            model.DeviceItemsGrid.HasTableBorderedClass = true;
            model.DeviceItemsGrid.HasTableStripedClass = true;

            model.Documents =
                RemontinkaServer.Instance.EntitiesFacade.GetCustomReportItems(token,
                    DocumentKindSet.OrderReportDocument.DocumentKindID).Select(i => new RepairOrderDocumentModel
                                                                                    {
                                                                                        CustomReportID = i.CustomReportID,
                                                                                        Title = i.Title
                                                                                    });

            model.DeviceTrademarkAutocompleteItems =
                RemontinkaServer.Instance.EntitiesFacade.GetAutocompleteItems(token,
                                                                              AutocompleteKindSet.DeviceTrademark.
                                                                                  AutocompleteKindID).Select(
                                                                                      i => i.Title);

            model.DeviceOptionsAutocompleteItems =
                RemontinkaServer.Instance.EntitiesFacade.GetAutocompleteItems(token,
                                                                              AutocompleteKindSet.DeviceOptions.
                                                                                  AutocompleteKindID).Select(
                                                                                      i => i.Title);
            model.DeviceAppearanceAutocompleteItems =
                RemontinkaServer.Instance.EntitiesFacade.GetAutocompleteItems(token,
                                                                              AutocompleteKindSet.DeviceAppearance.
                                                                                  AutocompleteKindID).Select(
                                                                                      i => i.Title);
            return model;
        }

        /// <summary>
        /// Содержит типы классов пунктов истории по заказу.
        /// </summary>
        private static readonly IDictionary<byte?,string> _timelineKindMap = new Dictionary<byte?, string>
                                                               {
                                                                   {TimelineKindSet.CommentAdded.TimelineKindID,"timeline-comment-added"},
                                                                   {TimelineKindSet.Completed.TimelineKindID,"timeline-completed"},
                                                                   {TimelineKindSet.DeviceItemAdded.TimelineKindID,"timeline-device-item-added"},
                                                                   {TimelineKindSet.WorkAdded.TimelineKindID,"timeline-work-item-added"},
                                                                   {TimelineKindSet.EngineerAssigned.TimelineKindID,"timeline-engineer-assigned"},
                                                                   {TimelineKindSet.ManagerAssigned.TimelineKindID,"timeline-manager-assigned"},
                                                                   {TimelineKindSet.StatusChanged.TimelineKindID,"timeline-status-changed"},
                                                               };

            /// <summary>
        /// Получение пунктов истории заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult GetRepairOrderTimelines(Guid? repairOrderID)
        {
            try
            {
                var items = RemontinkaServer.Instance.EntitiesFacade.GetOrderTimelines(GetToken(),repairOrderID).ToList().Select(i=>new RepairOrderTimelineItemModel
                                                                                                                               {
                                                                                                                                   EventDate = Utils.DateTimeToString(i.EventDateTime),
                                                                                                                                   Title = i.Title,
                                                                                                                                   TimelineKindClass = _timelineKindMap[i.TimelineKindID]
                                                                                                                               });

                //Все нормально
                return
                    Json(new JCrudItemsResult<RepairOrderTimelineItemModel>
                    {
                        ResultState = CrudResultKind.Success,
                        Items = items
                    });

            }
            catch (Exception ex)
            {
                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время получения истории по заказу {0} {1} {2} {3} {4}",repairOrderID,
                                    ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка {0}", ex.Message)));
            } //try
        }

        /// <summary>
        /// Добавляет комментарий к заказу.
        /// </summary>
        /// <param name="model">Модель комментария.</param>
        /// <returns>Результат.</returns>
        [HttpPost]
        public JsonResult AddRepairOrderComment(RepairOrderCommentModel model)
        {
             try
             {
                 RemontinkaServer.Instance.EntitiesFacade.AddRepairOrderComment(GetToken(), model.CommentRepairOrderID,
                                                                                model.CommentText);
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

                _logger.ErrorFormat("Во время создания комментария к заказу {0} {1} {2} {3} {4}",model.CommentRepairOrderID,
                                    ex.Message, ex.GetType(), innerException, ex.StackTrace);
                return Json(new JCrudErrorResult(string.Format("Произошла ошибка {0}", ex.Message)));
            } //try
        }
        
    }
}