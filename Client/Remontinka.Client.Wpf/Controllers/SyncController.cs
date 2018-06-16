using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Remontinka.Client.Core;
using Remontinka.Client.Core.Models;
using Remontinka.Client.Wpf.Model;
using Remontinka.Client.Wpf.View;

namespace Remontinka.Client.Wpf.Controllers
{
    public class SyncController : BaseController
    {
        /// <summary>
        /// Задает или получает текущую модель.
        /// </summary>
        private SyncProcessModel _model;

        /// <summary>
        /// Содержит представление.
        /// </summary>
        private SyncView _view;

        /// <summary>
        /// Получает View для отображения на форме.
        /// </summary>
        /// <returns>View.</returns>
        public override UserControl GetView()
        {
            return _view;
        }

        /// <summary>
        /// Инициализация данных контроллера.
        /// </summary>
        public override void Initialize()
        {
            var model = ClientCore.Instance.SyncService.CurrentModel;
            if (model!=null)
            {
                _model = (SyncProcessModel)model.Model;
                _model.IsEnabaled = false;
            } //if
            else
            {
                _model = ClientCore.Instance.CreateInstance<SyncProcessModel>();
                
                _model.GetUsers.Title = "Получение пользователей";
                _model.Items.Add(_model.GetUsers);

                _model.GetUserBranches.Title = "Получение филиалов";
                _model.Items.Add(_model.GetUserBranches);

                _model.GetFinancialGroups.Title = "Получение фингрупп";
                _model.Items.Add(_model.GetFinancialGroups);

                _model.GetWarehouses.Title = "Получение складов";
                _model.Items.Add(_model.GetWarehouses);

                _model.GetGoodsItems.Title = "Получение номенклатуры";
                _model.Items.Add(_model.GetGoodsItems);

                _model.GetWarehouseItems.Title = "Получение остатков на складах";
                _model.Items.Add(_model.GetWarehouseItems);

                _model.GetOrderStatuses.Title = "Получение статусов заказов";
                _model.Items.Add(_model.GetOrderStatuses);

                _model.GetCustomReportItems.Title = "Получение отчетов";
                _model.Items.Add(_model.GetCustomReportItems);

                _model.UpdateRepairOrders.Title = "Синхронизация заказов";
                _model.Items.Add(_model.UpdateRepairOrders);

                _model.IsEnabaled = true;
            } //else
            
            _view = new SyncView();
            _view.DataContext = _model;
            
            ClientCore.Instance.SyncService.Error += SyncServiceError;
            ClientCore.Instance.SyncService.Info += SyncServiceInfo;
            ClientCore.Instance.SyncService.SyncProcessFinished+=SyncServiceOnSyncProcessFinished;
            ClientCore.Instance.SyncService.SyncItemDescriptionChanged+=SyncServiceOnSyncItemDescriptionChanged;
            ClientCore.Instance.SyncService.SyncItemStatusChangedEvent+=SyncServiceOnSyncItemStatusChangedEvent;

            _view.syncButton.Click += SyncButtonClick;
            _view.cancelButton.Click += (sender, args) => ArmController.Instance.ShowRepairOrderList();
        }

        /// <summary>
        /// Вызывается когда пользователь нажимает кнопку синхронизации.
        /// </summary>
        void SyncButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            _model.IsEnabaled = false;
            ArmController.Instance.Model.ShowMainMenu = false;
            var descriptor = new SyncModelDescriptor();
            descriptor.Model = _model;
            descriptor.Items.Add(new KeyValuePair<SyncItemModelKind, SyncItemContainer>(SyncItemModelKind.GetUsers,
                                                                                        new SyncItemContainer
                                                                                        {ItemModel = _model.GetUsers}));
            descriptor.Items.Add(new KeyValuePair<SyncItemModelKind, SyncItemContainer>(SyncItemModelKind.GetUserBranches,
                                                                                        new SyncItemContainer { ItemModel = _model.GetUserBranches }));
            descriptor.Items.Add(new KeyValuePair<SyncItemModelKind, SyncItemContainer>(SyncItemModelKind.GetFinancialGroups,
                                                                                        new SyncItemContainer { ItemModel = _model.GetFinancialGroups }));
            descriptor.Items.Add(new KeyValuePair<SyncItemModelKind, SyncItemContainer>(SyncItemModelKind.GetWarehouses,
                                                                                        new SyncItemContainer { ItemModel = _model.GetWarehouses }));
            descriptor.Items.Add(new KeyValuePair<SyncItemModelKind, SyncItemContainer>(SyncItemModelKind.GetGoodsItems,
                                                                                        new SyncItemContainer { ItemModel = _model.GetGoodsItems }));
            descriptor.Items.Add(new KeyValuePair<SyncItemModelKind, SyncItemContainer>(SyncItemModelKind.GetWarehouseItems,
                                                                                        new SyncItemContainer { ItemModel = _model.GetWarehouseItems }));

            descriptor.Items.Add(new KeyValuePair<SyncItemModelKind, SyncItemContainer>(SyncItemModelKind.GetOrderStatuses,
                                                                                        new SyncItemContainer { ItemModel = _model.GetOrderStatuses }));
            descriptor.Items.Add(new KeyValuePair<SyncItemModelKind, SyncItemContainer>(SyncItemModelKind.GetCustomReports,
                                                                                        new SyncItemContainer { ItemModel = _model.GetCustomReportItems }));

            descriptor.Items.Add(new KeyValuePair<SyncItemModelKind, SyncItemContainer>(SyncItemModelKind.UpdateRepairOrders,
                                                                                        new SyncItemContainer { ItemModel = _model.UpdateRepairOrders }));
            
            
            ClientCore.Instance.SyncService.StartProcess(descriptor);
        }

        /// <summary>
        /// Вызывается при смене описания в пункте синхронизации.
        /// </summary>
        private void SyncServiceOnSyncItemStatusChangedEvent(object sender, SyncItemStatusChangedEventArgs syncItemStatusChangedEventArgs)
        {
            StartSaveInvoke(() =>
                            {
                                var item = (SyncItem) syncItemStatusChangedEventArgs.ItemContainer.ItemModel;
                                item.Status = syncItemStatusChangedEventArgs.NewStatus;
                            });
        }

        /// <summary>
        /// Вызывается при смене статуса в пункте синхронизации.
        /// </summary>
        private void SyncServiceOnSyncItemDescriptionChanged(object sender, SyncItemDescriptionChangedEventArgs syncItemDescriptionChangedEventArgs)
        {
            StartSaveInvoke(() =>
            {
                var item = (SyncItem)syncItemDescriptionChangedEventArgs.ItemContainer.ItemModel;
                item.Description = syncItemDescriptionChangedEventArgs.NewDescription;
            });
        }

        /// <summary>
        /// Вызывается при завершении работы сервиса синхронизации.
        /// </summary>
        private void SyncServiceOnSyncProcessFinished(object sender, SyncProcessFinishedEventArgs syncProcessFinishedEventArgs)
        {
            StartSaveInvoke(() =>
            {
                var item = (SyncProcessModel)syncProcessFinishedEventArgs.ModelDescriptor.Model;
                item.IsEnabaled = true;
                ArmController.Instance.Model.ShowMainMenu = true;
                _view.syncButton.Click -= SyncButtonClick;
                _view.syncButton.Content = "Начать";
                _view.syncButton.Click += (o, args) => ArmController.Instance.ShowRepairOrderList();
            });
        }

        /// <summary>
        /// Вызывается при получении новой информации с сервиса синхронизации.
        /// </summary>
        void SyncServiceInfo(object sender, InfoEventArgs e)
        {
            var model = _model;
            if (model != null)
            {
                StartSaveInvoke(() =>
                                {
                                    model.InfoText = e.InfoText;
                                });
            } //if
        }

        /// <summary>
        /// Вызывается при ошибке в сервисе синхронизации.
        /// </summary>
        void SyncServiceError(object sender, ErrorEventArgs e)
        {
            var model = _model;
            if (model != null)
            {
                StartSaveInvoke(() =>
                                {
                                    model.ErrorText = e.Message;
                                });
            }
        }

        /// <summary>
        /// Завершает действие контроллера, освобождая его ресурсы.
        /// </summary>
        public override void Terminate()
        {
            ClientCore.Instance.SyncService.Error -= SyncServiceError;
            ClientCore.Instance.SyncService.Info -= SyncServiceInfo;
            ClientCore.Instance.SyncService.SyncProcessFinished -= SyncServiceOnSyncProcessFinished;
            ClientCore.Instance.SyncService.SyncItemDescriptionChanged -= SyncServiceOnSyncItemDescriptionChanged;
            ClientCore.Instance.SyncService.SyncItemStatusChangedEvent -= SyncServiceOnSyncItemStatusChangedEvent;
        }

        /// <summary>
        /// Вызывается для определения возможности завершения работы.
        /// Вызывается системой для проверки можно ли закрыть текущее представление и отобразить другое.
        /// </summary>
        /// <returns>Признак завершения.</returns>
        public override bool CanTerminate()
        {
            return ClientCore.Instance.SyncService.CurrentModel == null;
        }
    }
}
