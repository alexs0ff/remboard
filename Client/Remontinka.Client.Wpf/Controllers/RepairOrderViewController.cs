using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Remontinka.Client.Core;
using Remontinka.Client.Wpf.Model.Controls;
using Remontinka.Client.Wpf.Model.Items;
using Remontinka.Client.DataLayer.Entities;
using Remontinka.Client.Wpf.Controllers.Controls;
using Remontinka.Client.Wpf.Controllers.Items;
using Remontinka.Client.Wpf.Model;
using Remontinka.Client.Wpf.View;

namespace Remontinka.Client.Wpf.Controllers
{
    /// <summary>
    /// Контроллер для управления заказами.
    /// </summary>
    public class RepairOrderViewController : BaseController
    {
        /// <summary>
        /// Содержит текущее представление.
        /// </summary>
        private RepairOrderView _view;

        /// <summary>
        /// Содержит модель Представления.
        /// </summary>
        private RepairOrderViewModel _orderModel;

        /// <summary>
        /// Получает View для отображения на форме.
        /// </summary>
        /// <returns>View.</returns>
        public override UserControl GetView()
        {
            return _view;
        }

        #region Controls 

        /// <summary>
        /// Содержит значение фильтра.
        /// </summary>
        private RepairOrderFilterComboBoxController _filterBox;

        /// <summary>
        /// Содержит значение пользователя.
        /// </summary>
        private UserComboBoxController _userBox;

        /// <summary>
        /// Содержит значение статусов заказа.
        /// </summary>
        private OrderStatusController _statusBox;

        /// <summary>
        /// Содержит значение имени.
        /// </summary>
        private TextBoxController _nameBox;

        /// <summary>
        /// Контролер пагинатора.
        /// </summary>
        private PaginatorController _paginatorController;

        /// <summary>
        /// Содержит максимальное количество элементов на главном гриде.
        /// </summary>
        private const int ItemsPerPage = 25;

        /// <summary>
        /// Содержит максимально отображаемое количество страниц.
        /// </summary>
        private const int MaxPages = 10;

        #endregion Controls

        /// <summary>
        /// Инициализация данных контроллера.
        /// </summary>
        public override void Initialize()
        {
            _orderModel = ClientCore.Instance.CreateInstance<RepairOrderViewModel>();
            _orderModel.Orders = new ObservableCollection<RepairOrderItemModel>();
            _orderModel.CurrentWorkItems = new ObservableCollection<WorkItemModel>();
            _orderModel.CurrentDeviceItems = new ObservableCollection<DeviceItemModel>();
            _orderModel.CurrentOrderTimelineItems = new ObservableCollection<OrderTimelineModel>();
            _view = new RepairOrderView { DataContext = _orderModel };

            _paginatorController = new PaginatorController();
            _paginatorController.SetView(_view.paginatorPanel,MaxPages,ItemsPerPage);
            _paginatorController.PageChanged += PaginatorControllerPageChanged;

            _filterBox = new RepairOrderFilterComboBoxController();
            _filterBox.SetView(_view.filterBox, null, true, false);

            _userBox = new UserComboBoxController();
            _userBox.SetView(_view.userBox, null, true, true);

            _statusBox = new OrderStatusController();
            _statusBox.SetView(_view.statusBox, null, true, true);

            _nameBox = new TextBoxController();
            _nameBox.SetView(_view.nameBox);

            _view.listView.SelectionChanged += RepairOrderListSelectionChanged;
            _view.listView.MouseDoubleClick+=ListViewOnMouseDoubleClick;

            _view.editOrderItem.Click += (sender, args) => SelectedRepairOrderStartEdit();
            _view.deleteOrderItem.Click += (sender, args) => SelectedRepairOrderStartDelete();

            _view.createButton.Click += (sender, args) => RepairOrderStartCreate();

            _nameBox.Model.PressKey +=NameBoxOnPressKey;
            _view.updateButton.Click += (sender, args) => StartUpdateOrderList(1);

            _orderDataController = new RepairOrderDataController();
            _orderDataController.Initialize();
            _orderDataController.CreateModelSaved += OrderDataControllerCreateModelSaved;
            _orderDataController.EditModelSaved+=OrderDataControllerOnEditModelSaved;

            _workItemDataController = new WorkItemDataController();
            _workItemDataController.Initialize();
            _workItemDataController.CreateModelSaved+=WorkItemDataControllerOnCreateModelSaved;
            _workItemDataController.EditModelSaved+=WorkItemDataControllerOnEditModelSaved;
            _view.editWorkItem.Click += (sender, args) => SelectedWorkItemStartEdit();
            _view.createWorkButton.Click += (sender, args) => WorkItemStartCreate();

            _deviceItemDataController = new DeviceItemDataController();
            _deviceItemDataController.Initialize();
            _deviceItemDataController.CreateModelSaved += DeviceItemDataControllerOnCreateModelSaved;
            _deviceItemDataController.EditModelSaved+=DeviceItemDataControllerOnEditModelSaved;
            _view.editDeviceItem.Click += (sender, args) => SelectedDeviceItemStartEdit();
            _view.createDeviceButton.Click += (sender, args) => DeviceItemStartCreate();

            _commentDataController = new CommentDataController();
            _commentDataController.Initialize();
            _commentDataController.CreateModelSaved += CommentDataControllerOnCreateModelSaved;
            _view.createCommentButton.Click += (sender, args) => CommentStartCreate();

            StartUpdateOrderList(1);

            _customReportPreviewController = new CustomReportPreviewController();
            _customReportPreviewController.Initialize();

            StartPopulateReportList();

            if (ClientCore.Instance.AuthService.SecurityToken.User.ProjectRoleID == ProjectRoleSet.Engineer.ProjectRoleID)
            {
                _view.createButton.IsEnabled = false;

                foreach (var editFormControlModel in _orderDataController.GetEditFormControlModels())
                {
                    if (!StringComparer.OrdinalIgnoreCase.Equals(editFormControlModel.Id, "RepairOrderStatusID"))
                    {
                        editFormControlModel.ReadOnly = true;    
                    }
                    
                }

                
            }
        }

        #region Custom Reports 

        private CustomReportPreviewController _customReportPreviewController;

        /// <summary>
        /// Наполняет контекстное меню отчетами.
        /// </summary>
        private void StartPopulateReportList()
        {
            SaveStartTask(
                source =>
                ClientCore.Instance.DataStore.GetCustomReportItems().Select(i => new {i.Title, i.CustomReportID}),
                items =>
                {
                    foreach (var customReportItem in items)
                    {
                        var customReportMenuItem = new MenuItem();
                        customReportMenuItem.Header = customReportItem.Title;
                        customReportMenuItem.DataContext = FormatUtils.StringToGuid(customReportItem.CustomReportID);
                        customReportMenuItem.Click += CustomReportMenuItemClick;
                        customReportMenuItem.Icon = new Image
                                            {
                                                Source =
                                                    new BitmapImage(new Uri(
                                                                        "/Remboard;component/Images/PrintView16.png",
                                                                        UriKind.Relative))
                                            };
                        _view.
                            repairOrderContextMenu.Items.Add(customReportMenuItem);
                    } //foreach

                }, null);
        }

        /// <summary>
        /// Вызывается когда пользователь выбирает отчет для агента.
        /// </summary>
        private void CustomReportMenuItemClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var menuItem = sender as MenuItem;

            if (menuItem != null)
            {
                var id = (Guid) menuItem.DataContext;
                var selectedItem = _view.listView.SelectedItem as RepairOrderItemModel;
                if (selectedItem != null && selectedItem.Id != null)
                {
                    _customReportPreviewController.ShowReport(id, selectedItem.Id);
                }
            } //if
        }

        #endregion Custom Reports

        /// <summary>
        /// Вызывается при успешном редактировании заказа.
        /// </summary>
        private void OrderDataControllerOnEditModelSaved(object sender, ModelSavedEventArgs<RepairOrderEditModel, Guid> modelSavedEventArgs)
        {
            var item =_orderModel.Orders.FirstOrDefault(i => i.Id == modelSavedEventArgs.SavedModel.Id);

            if (item!=null)
            {
                SaveStartTask(source => ClientCore.Instance.DataStore.GetRepairOrderDTO(item.Id), dto =>
                                                                                                  {
                                                                                                      var index = _orderModel.Orders.IndexOf(item);
                                                                                                      if (index>=0)
                                                                                                      {
                                                                                                          _orderModel.Orders.Remove(item);
                                                                                                          _orderModel.Orders.Insert(index, new RepairOrderItemModel(dto));
                                                                                                      } //if
                                                                                                  }, null);
            } //if
        }

        /// <summary>
        /// Вызывается когда пользователь сохраняет новый заказ. 
        /// </summary>
        void OrderDataControllerCreateModelSaved(object sender, ModelSavedEventArgs<RepairOrderCreateModel, Guid> e)
        {
            _orderModel.Orders.Add(new RepairOrderItemModel(ClientCore.Instance.DataStore.GetRepairOrderDTO(e.SavedModel.Id)));
        }

        /// <summary>
        /// Вызывается когда пользователь хочет изменить страницу.
        /// </summary>
        void PaginatorControllerPageChanged(object sender, PageChangedEventArgs e)
        {
            StartUpdateOrderList(e.Page);
        }

        /// <summary>
        /// Вызывается во время изменения выбора заказа.
        /// </summary>
        void RepairOrderListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StartUpdateChildLists();
        }

        private void NameBoxOnPressKey(object sender, PressKeyEventArgs pressKeyEventArgs)
        {
            if (pressKeyEventArgs.Key == Key.Enter)
            {
                StartUpdateOrderList(1);
            }
        }

        /// <summary>
        /// Вызывается при двойном клике по списку заказов.
        /// </summary>
        private void ListViewOnMouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
           SelectedRepairOrderStartEdit();
        }

        /// <summary>
        /// Завершает действие контроллера, освобождая его ресурсы.
        /// </summary>
        public override void Terminate()
        {
            
        }

        #region Work Items

        /// <summary>
        /// Вызывается при успешном сохранении работы.
        /// </summary>
        private void WorkItemDataControllerOnEditModelSaved(object sender, ModelSavedEventArgs<WorkItemEditModel, Guid> modelSavedEventArgs)
        {
            var item = _orderModel.CurrentWorkItems.FirstOrDefault(i => i.Id == modelSavedEventArgs.SavedModel.Id);

            if (item != null)
            {
                SaveStartTask(source => ClientCore.Instance.DataStore.GetWorkItem(item.Id), dto =>
                {
                    var index = _orderModel.CurrentWorkItems.IndexOf(item);
                    if (index >= 0)
                    {
                        _orderModel.CurrentWorkItems.Remove(item);
                        _orderModel.CurrentWorkItems.Insert(index, new WorkItemModel(dto));
                    } //if
                }, null);
            } //if
        }

        /// <summary>
        /// Содержит контроллер управления работами.
        /// </summary>
        private WorkItemDataController _workItemDataController;

        /// <summary>
        /// Вызывается при создании работы.
        /// </summary>
        private void WorkItemDataControllerOnCreateModelSaved(object sender, ModelSavedEventArgs<WorkItemEditModel, Guid> modelSavedEventArgs)
        {
            _orderModel.CurrentWorkItems.Add(new WorkItemModel( ClientCore.Instance.DataStore.GetWorkItem(modelSavedEventArgs.SavedModel.Id)));
            StartUpdateOrderTimelineList(modelSavedEventArgs.SavedModel.RepairOrderID);
        }
       
        /// <summary>
        /// Запускает на исполнение создание нового заказа.
        /// </summary>
        private void WorkItemStartCreate()
        {
             var selectedItem = _view.listView.SelectedItem as RepairOrderItemModel;
             if (selectedItem != null && selectedItem.Id != null)
             {
                 _workItemDataController.StartCreateModel(selectedItem.Id);
             }
        }

        /// <summary>
        /// Запускает на редактирование выбранный заказ.
        /// </summary>
        private void SelectedWorkItemStartEdit()
        {
            var selectedItem = _view.workListView.SelectedItem as WorkItemModel;
            if (selectedItem != null && selectedItem.Id != null)
            {
                _workItemDataController.StartEditModel(selectedItem.Id.Value, null);
            } //if
        }

        private CancellationTokenSource _updateWorkListSource;

        /// <summary>
        /// Страт процесса обновления данных выполненных работ.
        /// </summary>
        private void StartUpdateWorkList(Guid? repairOrderID)
        {
            _orderModel.CurrentWorkItems.Clear();

            if (_updateWorkListSource != null)
            {
                _updateWorkListSource.Cancel();
            } //if

            _updateWorkListSource =
                SaveStartTask(source => GetWorkItems(repairOrderID), PupulateWorkItemList,
                              null);
        }

        /// <summary>
        /// Заполняет пунктами выполенных работ.
        /// </summary>
        /// <param name="workItems">Полученные пункты.</param>
        private void PupulateWorkItemList(IEnumerable<WorkItemDTO> workItems)
        {
            _orderModel.CurrentWorkItems.Clear();

            foreach (var repairOrderDTO in workItems)
            {
                _orderModel.CurrentWorkItems.Add(new WorkItemModel(repairOrderDTO));
            } //foreach
        }

        /// <summary>
        /// Получает связанные пункты работ.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список пунктов работ.</returns>
        private IEnumerable<WorkItemDTO> GetWorkItems(Guid? repairOrderID)
        {
            return
                ClientCore.Instance.DataStore.GetWorkItemDtos(repairOrderID);
        }

        #endregion Work Items

        #region Order TimeLine

        /// <summary>
        /// Содержит контроллер управления комментариями.
        /// </summary>
        private CommentDataController _commentDataController;

        /// <summary>
        /// Вызывается при создании комментария.
        /// </summary>
        private void CommentDataControllerOnCreateModelSaved(object sender, ModelSavedEventArgs<CommentCreateModel, Guid> modelSavedEventArgs)
        {
            StartUpdateOrderTimelineList(modelSavedEventArgs.SavedModel.RepairOrderID);
        }

        /// <summary>
        /// Запускает на исполнение создание нового комментария.
        /// </summary>
        private void CommentStartCreate()
        {
            var selectedItem = _view.listView.SelectedItem as RepairOrderItemModel;
            if (selectedItem != null && selectedItem.Id != null)
            {
                _commentDataController.StartCreateModel(selectedItem.Id);
            }
        }

        private CancellationTokenSource _updateOrderTimelineListSource;

        /// <summary>
        /// Страт процесса обновления данных событий заказа.
        /// </summary>
        private void StartUpdateOrderTimelineList(Guid? repairOrderID)
        {
            _orderModel.CurrentOrderTimelineItems.Clear();

            if (_updateOrderTimelineListSource != null)
            {
                _updateOrderTimelineListSource.Cancel();
            } //if

            _updateOrderTimelineListSource =
                SaveStartTask(source => GetOrderTimelineItems(repairOrderID), PupulateOrderTimelineItemList,
                              null);
        }

        /// <summary>
        /// Получает связанные пункты событий заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список событий заказа.</returns>
        private IEnumerable<OrderTimeline> GetOrderTimelineItems(Guid? repairOrderID)
        {
            return
                ClientCore.Instance.DataStore.GetOrderTimelines(repairOrderID);
        }

        /// <summary>
        /// Заполняет пунктами выполенных событий заказа.
        /// </summary>
        /// <param name="workItems">Полученные пункты.</param>
        private void PupulateOrderTimelineItemList(IEnumerable<OrderTimeline> workItems)
        {
            _orderModel.CurrentOrderTimelineItems.Clear();

            foreach (var orderTimeline in workItems)
            {
                _orderModel.CurrentOrderTimelineItems.Add(new OrderTimelineModel(orderTimeline));
            } //foreach
        }

        #endregion Order TimeLine
      
        #region Device Items

        /// <summary>
        /// Вызывается при успешном сохранении работы.
        /// </summary>
        private void DeviceItemDataControllerOnEditModelSaved(object sender, ModelSavedEventArgs<DeviceItemEditModel, Guid> modelSavedEventArgs)
        {
            var item = _orderModel.CurrentDeviceItems.FirstOrDefault(i => i.Id == modelSavedEventArgs.SavedModel.Id);

            if (item != null)
            {
                SaveStartTask(source => ClientCore.Instance.DataStore.GetDeviceItem(item.Id), dto =>
                {
                    var index = _orderModel.CurrentDeviceItems.IndexOf(item);
                    if (index >= 0)
                    {
                        _orderModel.CurrentDeviceItems.Remove(item);
                        _orderModel.CurrentDeviceItems.Insert(index, new DeviceItemModel(dto));
                    } //if
                }, null);
            } //if
        }

        /// <summary>
        /// Содержит контроллер управления запчастями.
        /// </summary>
        private DeviceItemDataController _deviceItemDataController;

        /// <summary>
        /// Вызывается при создании запчасти.
        /// </summary>
        private void DeviceItemDataControllerOnCreateModelSaved(object sender, ModelSavedEventArgs<DeviceItemEditModel, Guid> modelSavedEventArgs)
        {
            _orderModel.CurrentDeviceItems.Add(new DeviceItemModel(ClientCore.Instance.DataStore.GetDeviceItem(modelSavedEventArgs.SavedModel.Id)));
            StartUpdateOrderTimelineList(modelSavedEventArgs.SavedModel.RepairOrderID);
        }

        /// <summary>
        /// Запускает на исполнение создание нового запчасти.
        /// </summary>
        private void DeviceItemStartCreate()
        {
            var selectedItem = _view.listView.SelectedItem as RepairOrderItemModel;
            if (selectedItem != null && selectedItem.Id != null)
            {
                _deviceItemDataController.StartCreateModel(selectedItem.Id);
            }
        }

        /// <summary>
        /// Запускает на редактирование выбранный запчасти.
        /// </summary>
        private void SelectedDeviceItemStartEdit()
        {
            var selectedItem = _view.deviceListView.SelectedItem as DeviceItemModel;
            if (selectedItem != null && selectedItem.Id != null)
            {
                _deviceItemDataController.StartEditModel(selectedItem.Id.Value, null);
            } //if
        }

        private CancellationTokenSource _updateDeviceListSource;

        /// <summary>
        /// Страт процесса обновления данных установленных запчастей.
        /// </summary>
        private void StartUpdateDeviceList(Guid? repairOrderID)
        {
            _orderModel.CurrentDeviceItems.Clear();

            if (_updateDeviceListSource != null)
            {
                _updateDeviceListSource.Cancel();
            } //if

            _updateDeviceListSource =
                SaveStartTask(source => GetDeviceItems(repairOrderID), PupulateDeviceItemList,
                              null);
        }

        /// <summary>
        /// Получает связанные пункты работ.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список пунктов установленных запчастей.</returns>
        private IEnumerable<DeviceItem> GetDeviceItems(Guid? repairOrderID)
        {
            return
                ClientCore.Instance.DataStore.GetDeviceItems(repairOrderID);
        }

        /// <summary>
        /// Заполняет пунктами установленных запчастей.
        /// </summary>
        /// <param name="workItems">Полученные пункты.</param>
        private void PupulateDeviceItemList(IEnumerable<DeviceItem> workItems)
        {
            _orderModel.CurrentDeviceItems.Clear();

            foreach (var repairOrderDTO in workItems)
            {
                _orderModel.CurrentDeviceItems.Add(new DeviceItemModel(repairOrderDTO));
            } //foreach
        }

        #endregion Device Items

        #region Repair Orders

        /// <summary>
        /// Запускает на исполнение создание нового заказа.
        /// </summary>
        private void RepairOrderStartCreate()
        {
            _orderDataController.StartCreateModel(null);
        }

        private RepairOrderDataController _orderDataController;

        /// <summary>
        /// Запускает на редактирование выбранный заказ.
        /// </summary>
        private void SelectedRepairOrderStartEdit()
        {
            var selectedItem = _view.listView.SelectedItem as RepairOrderItemModel;
            if (selectedItem!=null && selectedItem.Id!=null)
            {
                _orderDataController.StartEditModel(selectedItem.Id.Value, null);
            } //if
        }

        /// <summary>
        /// Запускает на редактирование выбранный заказ.
        /// </summary>
        private void SelectedRepairOrderStartDelete()
        {
            var selectedItem = _view.listView.SelectedItem as RepairOrderItemModel;
            if (selectedItem != null && selectedItem.Id != null)
            {
                try
                {
                    if (ClientCore.Instance.AuthService.SecurityToken.User.ProjectRoleID==ProjectRoleSet.Admin.ProjectRoleID)
                    {
                        _orderDataController.DeleteEntity(ClientCore.Instance.AuthService.SecurityToken, selectedItem.Id.Value);
                        _orderModel.Orders.Remove(selectedItem);    
                    } //if
                }
                catch (Exception ex)
                {
                    ClientCore.Instance.UserNotifier.ShowMessage("Ошибка удаления",ex.Message);
                    
                } //try
                
            } //if
        }

        /// <summary>
        /// Стратует обновление всех зависимых списков.
        /// </summary>
        private void StartUpdateChildLists()
        {
            var selectedItem = _view.listView.SelectedItem as RepairOrderItemModel;
            if (selectedItem!=null)
            {
                StartUpdateWorkList(selectedItem.Id);
                StartUpdateDeviceList(selectedItem.Id);
                StartUpdateOrderTimelineList(selectedItem.Id);
            }
        }

        private CancellationTokenSource _updateOrderListSource;

        /// <summary>
        /// Страт процесса обновления данных заказов.
        /// </summary>
        private void StartUpdateOrderList(int page)
        {
            //ощищаем связанные элементы
            _orderModel.CurrentWorkItems.Clear();
            _orderModel.CurrentDeviceItems.Clear();
            _orderModel.CurrentOrderTimelineItems.Clear();

            var filterId = _filterBox.SelectedValue;
            var orderStatusId = _statusBox.SelectedValue;
            var token = ClientCore.Instance.AuthService.SecurityToken;
            var userId = _userBox.SelectedValue;
            var name = _nameBox.Model.Value;

            if (_updateOrderListSource != null)
            {
                _updateOrderListSource.Cancel();
            } //if

            _updateOrderListSource =
                SaveStartTask(source => GetRepairOrders(page, filterId, orderStatusId, userId, name, token), PupulateOrderList,
                              null);
        }

        /// <summary>
        /// Выполняет процедуру наполнения списка заказов.
        /// </summary>
        private void PupulateOrderList(RepairOrderResult result)
        {
            _orderModel.Orders.Clear();

            foreach (var repairOrderDTO in result.RepairOrderDtos)
            {
                _orderModel.Orders.Add(new RepairOrderItemModel(repairOrderDTO));
            } //foreach

            _paginatorController.SetPages(result.TotalCount, result.Page);
        }

        /// <summary>
        /// Результат получения списка заказов.
        /// </summary>
        private class RepairOrderResult
        {
            /// <summary>
            /// Задает или получает текущую страницу.
            /// </summary>
            public int Page { get; set; }

            /// <summary>
            /// Задает или получает общее количество элементов.
            /// </summary>
            public int TotalCount { get; set; }

            /// <summary>
            /// Задает или получает список заказов.
            /// </summary>
            public IEnumerable<RepairOrderDTO> RepairOrderDtos { get; set; }
        }

        /// <summary>
        /// Выполняет процедуру получения значений заказов из базы.
        /// </summary>
        private RepairOrderResult GetRepairOrders(int page,int? filterID, Guid? orderStatusID, Guid? userID,
                                                            string name, SecurityToken token)
        {
            int totalCount;
            var result = new RepairOrderResult();
            result.Page = page;
            if (filterID == OrderSearchSet.All.Key || filterID == null)
            {
                if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
                {
                    result.RepairOrderDtos = ClientCore.Instance.DataStore.GetRepairOrders(orderStatusID, null, name, page,
                        ItemsPerPage,
                                                                      out totalCount);
                    result.TotalCount = totalCount;
                    return result;

                } //if

                result.RepairOrderDtos =
                    ClientCore.Instance.DataStore.GetRepairOrdersUserBranch(orderStatusID, null,
                                                                            token.UserID, name, page,
                                                                            ItemsPerPage, out totalCount);
                result.TotalCount = totalCount;
                return result;

            } //if

            if (filterID == OrderSearchSet.CurrentUser.Key)
            {
                result.RepairOrderDtos =
                    ClientCore.Instance.DataStore.GetRepairOrdersUser(orderStatusID, null,
                                                                      token.UserID, name,
                                                                      page, ItemsPerPage,
                                                                      out totalCount);
                result.TotalCount = totalCount;
                return result;
            } //if

            if (filterID == OrderSearchSet.OnlyUrgents.Key)
            {
                if (token.User.ProjectRoleID == ProjectRoleSet.Admin.ProjectRoleID)
                {
                    result.RepairOrderDtos =
                        ClientCore.Instance.DataStore.GetRepairOrders(orderStatusID, true, name,
                        page, ItemsPerPage,
                                                                      out totalCount);

                    result.TotalCount = totalCount;
                    return result;
                } //if

                result.RepairOrderDtos =
                    ClientCore.Instance.DataStore.GetRepairOrdersUserBranch(orderStatusID, true,
                                                                            token.UserID, name, page,
                                                                            ItemsPerPage,
                                                                            out totalCount);

                result.TotalCount = totalCount;
                return result;
            } //if

            if (filterID == OrderSearchSet.SpecificUser.Key)
            {
                result.RepairOrderDtos =
                    ClientCore.Instance.DataStore.GetRepairOrdersUser(orderStatusID, null, userID, name,
                                                                      page, ItemsPerPage,
                                                                      out totalCount);
                result.TotalCount = totalCount;
                return result;
            }

            result.RepairOrderDtos = new RepairOrderDTO[0];
            return result;
        }

        #endregion Repair Orders

    }
}
