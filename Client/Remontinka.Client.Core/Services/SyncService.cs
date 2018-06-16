using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Remontinka.Client.Core.Models;
using Remontinka.Client.DataLayer.Entities;
using Romontinka.Server.Protocol.SynchronizationMessages;
using log4net;
using RepairOrderDTO = Romontinka.Server.Protocol.SynchronizationMessages.RepairOrderDTO;

namespace Remontinka.Client.Core.Services
{
    /// <summary>
    /// Реализация сервиса синхронизации.
    /// </summary>
    public class SyncService : ISyncService
    {
        #region Common 

        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SyncService));

        /// <summary>
        /// Событие изменения статуса.
        /// </summary>
        public event EventHandler<SyncItemStatusChangedEventArgs> SyncItemStatusChangedEvent;

        /// <summary>
        /// Вызывает событие изменения статуса.
        /// </summary>
        /// <param name="newStatus">Новый статус.</param>
        /// <param name="container">Контейнер пункта.</param>
        private void RiseSyncItemStatusChangedEvent(SyncItemStatus newStatus, SyncItemContainer container)
        {
            EventHandler<SyncItemStatusChangedEventArgs> handler = SyncItemStatusChangedEvent;
            if (handler != null)
            {
                handler(this, new SyncItemStatusChangedEventArgs(newStatus,container));
            }
        }

        /// <summary>
        /// Происходит по завершению обработки процесса синхронизации.
        /// </summary>
        public event EventHandler<SyncProcessFinishedEventArgs> SyncProcessFinished;

        /// <summary>
        /// Вызывает событие завершения обработки процесса синхронизации.
        /// </summary>
        /// <param name="model">Модель.</param>
        private void RiseSyncProcessFinished(SyncModelDescriptor model)
        {
            EventHandler<SyncProcessFinishedEventArgs> handler = SyncProcessFinished;
            if (handler != null)
            {
                handler(this, new SyncProcessFinishedEventArgs(model));
            }
        }

        /// <summary>
        /// Происходит в момент наличия информации об ошибки.
        /// </summary>
        public event EventHandler<ErrorEventArgs> Error;

        /// <summary>
        /// Вызывает событие наличия информации об ошибке.
        /// </summary>
        /// <param name="errorText">Текст информации.</param>
        private void RiseError(string errorText)
        {
            EventHandler<ErrorEventArgs> handler = Error;
            if (handler != null)
            {
                handler(this, new ErrorEventArgs(errorText));
            }
        }

        /// <summary>
        /// Происходит в момент наличия дополнительной информации.
        /// </summary>
        public event EventHandler<InfoEventArgs> Info;

        /// <summary>
        /// Вызывает событие наличия дополнительной информации.
        /// </summary>
        /// <param name="infoText">Текст информации.</param>
        private void RiseInfo(string infoText)
        {
            EventHandler<InfoEventArgs> handler = Info;
            if (handler != null)
            {
                handler(this, new InfoEventArgs(infoText));
            }
        }

        /// <summary>
        /// Происходит во время смены описания для пункта синхронизации.
        /// </summary>
        public event EventHandler<SyncItemDescriptionChangedEventArgs> SyncItemDescriptionChanged;

        /// <summary>
        /// Вызывает событие смены описания пункта синхронизации.
        /// </summary>
        /// <param name="newDescription">Новое описание.</param>
        /// <param name="container">Контейнер.</param>
        private void RiseSyncItemDescriptionChanged(string newDescription, SyncItemContainer container)
        {
            EventHandler<SyncItemDescriptionChangedEventArgs> handler = SyncItemDescriptionChanged;
            if (handler != null)
            {
                handler(this, new SyncItemDescriptionChangedEventArgs(newDescription,container));
            }
        }

        /// <summary>
        /// Получает текущую модель.
        /// </summary>
        public SyncModelDescriptor CurrentModel { get; private set; }

        #endregion Common

        /// <summary>
        /// Стартует процесс синхронизации.
        /// </summary>
        public void StartProcess(SyncModelDescriptor model)
        {
            _logger.InfoFormat("Старт процесса синхронизации");
            if (CurrentModel!=null)
            {
                _logger.ErrorFormat("Пердыдущий процесс синхронизации еще не завершен.");
                return;
            } //if

            CurrentModel = model;
            ThreadPool.QueueUserWorkItem(state => Process());
        }

        /// <summary>
        /// Обработчик процесса
        /// </summary>
        private void Process()
        {
            var item = new SyncOperation();
            try
            {
                //TODO бэкап базы, если будет ошибка
                item.UserIDGuid = ClientCore.Instance.AuthService.AuthToken.UserID;
                item.IsSuccessBoolean = false;
                item.OperationBeginTimeDateTime = DateTime.Now;
                item.Comment = "Не завершенный процесс";
                ClientCore.Instance.DataStore.SaveSyncOperation(item);

                ExecItem(ProcessUsers, CurrentModel.Items[SyncItemModelKind.GetUsers]);
                ExecItem(ProcessUserBranches, CurrentModel.Items[SyncItemModelKind.GetUserBranches]);
                ExecItem(ProcessFinancialGroups, CurrentModel.Items[SyncItemModelKind.GetFinancialGroups]);
                ExecItem(ProcessWarehouses, CurrentModel.Items[SyncItemModelKind.GetWarehouses]);
                ExecItem(ProcessGoodsItems, CurrentModel.Items[SyncItemModelKind.GetGoodsItems]);
                ExecItem(ProcessWarehouseItems, CurrentModel.Items[SyncItemModelKind.GetWarehouseItems]);
                ExecItem(ProcessGetOrderStatuses, CurrentModel.Items[SyncItemModelKind.GetOrderStatuses]);
                ExecItem(ProcessGetCustomReportItems, CurrentModel.Items[SyncItemModelKind.GetCustomReports]);
                ExecItem(ProcessUpdateRepairOrders, CurrentModel.Items[SyncItemModelKind.UpdateRepairOrders]);
                
                item.Comment = "Успех";
                item.IsSuccessBoolean = true;

                ClientCore.Instance.AuthService.ReloadCurrentUserData();

                RiseInfo("Синхронизация завершена");
            }
            catch (ResponseAuthException ex)
            {
                _logger.LogError(ex, "Произошла ошибка авторизации при синхронизации данных с сервером");
                RiseError(string.Format("Произошла ошибка авторизации {0}", ex.Message));
                item.Comment = ex.Message;
            } //try
            catch (Exception ex)
            {
                RiseError(string.Format("Произошла ошибка при синхронизации {0}", ex.Message));
                _logger.LogError(ex, "Произошла ошибка при синхронизации данных с сервером");
                item.Comment = ex.Message;
            }
            finally
            {
                item.OperationEndTimeDateTime = DateTime.Now;
                ClientCore.Instance.DataStore.SaveSyncOperation(item);
            }

            RiseSyncProcessFinished(CurrentModel);
            CurrentModel = null;
        }

        /// <summary>
        /// Исполняет методы синхронизации с присвоением определенных статусов модели элементов.
        /// </summary>
        /// <param name="method">Метод для исполнения.</param>
        /// <param name="item">Модель пункта исполнения.</param>
        private void ExecItem(Action method, SyncItemContainer item)
        {
            try
            {
                RiseSyncItemStatusChangedEvent(SyncItemStatus.Processing, item);
                method();
                RiseSyncItemStatusChangedEvent(SyncItemStatus.Success, item);
            }
            catch (Exception)
            {
                RiseSyncItemStatusChangedEvent(SyncItemStatus.Failed, item);
                throw;
            } //try
        }

        #region GetUsers 

        /// <summary>
        /// Обработка обновлений пользователей.
        /// </summary>
        private void ProcessUsers()
        {

            _logger.InfoFormat("Старт синхронизации пользователей.");
            RiseInfo("Отправка запроса на получение пользователей домена");
            RiseSyncItemDescriptionChanged("Получение данных", CurrentModel.Items[SyncItemModelKind.GetUsers]);

            var response =
                ClientCore.Instance.WebClient.GetDomainUsers(new GetDomainUsersRequest
                                                             {UserID = ClientCore.Instance.AuthService.AuthToken.UserID});
            RiseInfo( string.Format("Получено {0} пользователей",response.Users.Count));
            foreach (var domainUserDTO in response.Users)
            {
                _logger.InfoFormat("Обработка полученного пользователя {0}",domainUserDTO.LoginName);
                
                RiseSyncItemDescriptionChanged(domainUserDTO.LoginName,CurrentModel.Items[SyncItemModelKind.GetUsers]);

                var user = ClientCore.Instance.DataStore.GetUser(domainUserDTO.LoginName);//Если пользователь не активированный.
                if (user==null)
                {
                    user = new User();
                    user.UserIDGuid = domainUserDTO.UserID;
                    user.PasswordHash = string.Empty;
                } //if

                user.Email = domainUserDTO.Email;
                user.FirstName = domainUserDTO.FirstName;
                user.LastName = domainUserDTO.LastName;
                user.LoginName = domainUserDTO.LoginName;
                user.MiddleName = domainUserDTO.MiddleName;
                user.Phone = domainUserDTO.Phone;
                user.ProjectRoleID = domainUserDTO.ProjectRoleID;
                
                user.DomainIDGuid = ClientCore.Instance.AuthService.AuthToken.UserDomainID;

                ClientCore.Instance.DataStore.SaveUser(user);
            } //foreach

            RiseSyncItemDescriptionChanged(string.Format("Обработано {0} пользователей", response.Users.Count), CurrentModel.Items[SyncItemModelKind.GetUsers]);
            _logger.InfoFormat("Синхронизация пользователей завершилась с успехом для {0} пользователей",
                               response.Users.Count);
        }

        #endregion GetUsers

        #region GetUserBranches

        /// <summary>
        /// Обработка обновлений филиалов и связей с пользователями.
        /// </summary>
        private void ProcessUserBranches()
        {
            _logger.InfoFormat("Старт синхронизации филиалов.");
            RiseInfo("Отправка запроса на получение филиалов домена");
            RiseSyncItemDescriptionChanged("Получение данных", CurrentModel.Items[SyncItemModelKind.GetUserBranches]);

            var response =
                ClientCore.Instance.WebClient.GetUserBranches(new GetUserBranchesRequest { UserID = ClientCore.Instance.AuthService.AuthToken.UserID });
            RiseInfo(string.Format("Получено {0} филиалов", response.Branches.Count));
            
            ClientCore.Instance.DataStore.DeleteAllBranches();

            foreach (var branch in response.Branches)
            {
                _logger.InfoFormat("Обработка полученного филиала {0}", branch.Title);

                RiseSyncItemDescriptionChanged(branch.Title, CurrentModel.Items[SyncItemModelKind.GetUserBranches]);

                var item = new Branch();

                item.Address = branch.Address;
                item.BranchIDGuid = branch.BranchID;
                item.LegalName = branch.LegalName;
                item.Title = branch.Title;

                ClientCore.Instance.DataStore.SaveBranch(item);
            } //foreach

            RiseSyncItemDescriptionChanged("Обработка связей", CurrentModel.Items[SyncItemModelKind.GetUserBranches]);
            ClientCore.Instance.DataStore.DeleteAllUserBranchMapItems();

            foreach (var mapItem in response.UserBranchMapItems)
            {
                _logger.InfoFormat("Обработка полученной связи филиала {0} и пользователя {1}", mapItem.BranchID, mapItem.UserID);

                

                var item = new UserBranchMapItem();

                item.EventDateDateTime = mapItem.EventDate??DateTime.Today;
                item.BranchIDGuid = mapItem.BranchID;
                item.UserIDGuid = mapItem.UserID;
                item.UserBranchMapIDGuid = mapItem.UserBranchMapID;

                ClientCore.Instance.DataStore.SaveUserBranchMapItem(item);
            } //foreach

            RiseSyncItemDescriptionChanged(string.Format("Обработано {0} филиалов и {1} связей", response.Branches.Count, response.UserBranchMapItems.Count), CurrentModel.Items[SyncItemModelKind.GetUserBranches]);
            _logger.InfoFormat("Синхронизация пользователей завершилась с успехом для {0} филиалов и {1} связей",
                               response.Branches.Count,response.UserBranchMapItems.Count);
        }

        #endregion GetUserBranches

        #region GetFinancialGroups

        /// <summary>
        /// Обработка обновлений фингрупп и связей с филиалами.
        /// </summary>
        private void ProcessFinancialGroups()
        {
            _logger.InfoFormat("Старт синхронизации фингрупп.");
            var curItem =CurrentModel.Items[SyncItemModelKind.GetFinancialGroups];

            RiseInfo("Отправка запроса на получение фингрупп домена");
            RiseSyncItemDescriptionChanged("Получение данных", CurrentModel.Items[SyncItemModelKind.GetFinancialGroups]);

            var response =
                ClientCore.Instance.WebClient.GetFinancialGroups(new GetFinancialGroupBranchesRequest { UserID = ClientCore.Instance.AuthService.AuthToken.UserID });
            RiseInfo(string.Format("Получено {0} фингрупп", response.FinancialGroupItems.Count));

            ClientCore.Instance.DataStore.DeleteAllFinancialGroupItems();

            foreach (var financialGroupItem in response.FinancialGroupItems)
            {
                _logger.InfoFormat("Обработка полученной фингруппы {0}", financialGroupItem.Title);

                RiseSyncItemDescriptionChanged(financialGroupItem.Title, curItem);

                var item = new FinancialGroupItem();

                item.FinancialGroupIDGuid = financialGroupItem.FinancialGroupID;
                item.Trademark = financialGroupItem.Trademark;
                item.LegalName = financialGroupItem.LegalName;
                item.Title = financialGroupItem.Title;

                ClientCore.Instance.DataStore.SaveFinancialGroupItem(item);
            } //foreach

            RiseSyncItemDescriptionChanged("Обработка связей", curItem);
            ClientCore.Instance.DataStore.DeleteAllFinancialGroupBranchMapItems();

            foreach (var mapItem in response.FinancialGroupBranchMapItems)
            {
                _logger.InfoFormat("Обработка полученной связи фингруппы {0} и филиала {1}", mapItem.BranchID, mapItem.BranchID);

                var item = new FinancialGroupBranchMapItem();
                item.BranchIDGuid = mapItem.BranchID;
                item.FinancialGroupBranchMapIDGuid = mapItem.FinancialGroupBranchMapID;
                item.FinancialGroupIDGuid = mapItem.FinancialGroupID;

                ClientCore.Instance.DataStore.SaveFinancialGroupMapBranchItem(item);
            } //foreach

            RiseSyncItemDescriptionChanged(string.Format("Обработано {0} фигрупп и {1} связей", response.FinancialGroupItems.Count, response.FinancialGroupBranchMapItems.Count), curItem);
            _logger.InfoFormat("Синхронизация фингрупп завершилась с успехом для {0} филиалов и {1} связей",
                               response.FinancialGroupItems.Count, response.FinancialGroupBranchMapItems.Count);
        }


        #endregion GetFinancialGroups

        #region GetWarehouses

        /// <summary>
        /// Обработка обновлений складов и их связей с фингруппами.
        /// </summary>
        private void ProcessWarehouses()
        {
            _logger.InfoFormat("Старт синхронизации складов.");
            var curItem = CurrentModel.Items[SyncItemModelKind.GetWarehouses];

            RiseInfo("Отправка запроса на получение складов домена");
            RiseSyncItemDescriptionChanged("Получение данных", curItem);

            var response =
                ClientCore.Instance.WebClient.GetWarehouses(new GetWarehousesRequest { UserID = ClientCore.Instance.AuthService.AuthToken.UserID });
            RiseInfo(string.Format("Получено {0} складов", response.Warehouses.Count));

            ClientCore.Instance.DataStore.DeleteAllWarehouses();

            foreach (var warehouse in response.Warehouses)
            {
                _logger.InfoFormat("Обработка полученного склада {0}", warehouse.Title);

                RiseSyncItemDescriptionChanged(warehouse.Title, curItem);

                var item = new Warehouse();

                item.WarehouseIDGuid = warehouse.WarehouseID;
                item.Title = warehouse.Title;

                ClientCore.Instance.DataStore.SaveWarehouse(item);
            } //foreach

            RiseSyncItemDescriptionChanged("Обработка связей", curItem);
            ClientCore.Instance.DataStore.DeleteAllFinancialGroupMapWarehouseItems();

            foreach (var mapItem in response.MapItems)
            {
                _logger.InfoFormat("Обработка полученной связи склада {0} и фингруппы {1}", mapItem.WarehouseID, mapItem.FinancialGroupID);

                var item = new FinancialGroupWarehouseMapItem();
                item.WarehouseIDGuid = mapItem.WarehouseID;
                item.FinancialGroupWarehouseMapIDGuid = mapItem.FinancialGroupWarehouseMapID;
                item.FinancialGroupIDGuid = mapItem.FinancialGroupID;

                ClientCore.Instance.DataStore.SaveFinancialGroupWarehouseItem(item);
            } //foreach

            RiseSyncItemDescriptionChanged(string.Format("Обработано {0} складов и {1} связей", response.Warehouses.Count, response.MapItems.Count), curItem);
            _logger.InfoFormat("Синхронизация складов завершилась с успехом для {0} скадов и {1} связей",
                               response.Warehouses.Count, response.MapItems.Count);
        }

        #endregion GetWarehouses

        #region GetGoodsItems

        /// <summary>
        /// Обработка обновлений номенклатуры и категорий товаров.
        /// </summary>
        private void ProcessGoodsItems()
        {
            _logger.InfoFormat("Старт синхронизации номенклатуры.");
            var curItem = CurrentModel.Items[SyncItemModelKind.GetGoodsItems];

            RiseInfo("Отправка запроса на получение номенклатуры домена");
            RiseSyncItemDescriptionChanged("Получение данных", curItem);

            var response =
                ClientCore.Instance.WebClient.GetGoodsItems(new GetGoodsItemRequest { UserID = ClientCore.Instance.AuthService.AuthToken.UserID });
            RiseInfo(string.Format("Получено {0} номенклатур", response.GoodsItems.Count));

            ClientCore.Instance.DataStore.DeleteAllGoodsItems();

            foreach (var goodsItem in response.GoodsItems)
            {
                _logger.InfoFormat("Обработка полученной номенклатуры {0}", goodsItem.Title);

                RiseSyncItemDescriptionChanged(goodsItem.Title, curItem);

                var item = new GoodsItem();

                item.GoodsItemIDGuid = goodsItem.GoodsItemID;
                item.Title = goodsItem.Title;
                item.BarCode = goodsItem.BarCode;
                item.Description = goodsItem.Description;
                item.DimensionKindID = goodsItem.DimensionKindID;
                item.GoodsItemIDGuid = goodsItem.GoodsItemID;
                item.ItemCategoryIDGuid = goodsItem.ItemCategoryID;
                item.Particular = goodsItem.Particular;
                item.Title = goodsItem.Title;
                item.UserCode = goodsItem.UserCode;

                ClientCore.Instance.DataStore.SaveGoodsItem(item);
            } //foreach

            RiseSyncItemDescriptionChanged("Обработка категорий", curItem);
            ClientCore.Instance.DataStore.DeleteAllItemCategories();

            foreach (var itemCategory in response.ItemCategories)
            {
                _logger.InfoFormat("Обработка полученной категории товара {0}", itemCategory.Title);

                var item = new ItemCategory();
                item.ItemCategoryIDGuid = itemCategory.ItemCategoryID;
                item.Title = itemCategory.Title;

                ClientCore.Instance.DataStore.SaveItemCategory(item);
            } //foreach

            RiseSyncItemDescriptionChanged(string.Format("Обработано {0} номенклатур и {1} категорий", response.GoodsItems.Count, response.ItemCategories.Count), curItem);
            _logger.InfoFormat("Синхронизация номенклатур и категорий завершилась с успехом для {0} номенклатур и {1} категорий",
                               response.GoodsItems.Count, response.ItemCategories.Count);
        }

        #endregion GetGoodsItems

        #region GetWarehouseItems

        /// <summary>
        /// Обработка обновлений остатков на складе.
        /// </summary>
        private void ProcessWarehouseItems()
        {
            _logger.InfoFormat("Старт синхронизации остатков на складе.");
            var curItem = CurrentModel.Items[SyncItemModelKind.GetWarehouseItems];

            RiseInfo("Отправка запроса на получение остатков на складе");
            RiseSyncItemDescriptionChanged("Получение данных", curItem);

            var response =
                ClientCore.Instance.WebClient.GetWarehouseItems(new GetWarehouseItemsRequest { UserID = ClientCore.Instance.AuthService.AuthToken.UserID });
            RiseInfo(string.Format("Получено {0} остатков", response.WarehouseItems.Count));

            ClientCore.Instance.DataStore.DeleteAllWarehouseItems();

            foreach (var warehouseItem in response.WarehouseItems)
            {
                _logger.InfoFormat("Обработка полученной номенклатуры {0}", warehouseItem.Total);

                var item = new WarehouseItem();

                item.GoodsItemIDGuid = warehouseItem.GoodsItemID;
                item.RepairPrice = (double)warehouseItem.RepairPrice;
                item.SalePrice = (double)warehouseItem.SalePrice;
                item.StartPrice = (double)warehouseItem.StartPrice;
                item.Total = (double)warehouseItem.Total;
                item.WarehouseIDGuid = warehouseItem.WarehouseID;
                item.WarehouseItemIDGuid = warehouseItem.WarehouseItemID;

                ClientCore.Instance.DataStore.SaveWarehouseItem(item);
            } //foreach

            RiseSyncItemDescriptionChanged(string.Format("Обработано {0} остатков", response.WarehouseItems.Count), curItem);
            _logger.InfoFormat("Синхронизация остатков на складе успешно завершилась для {0} остатков",
                               response.WarehouseItems.Count);
        }

        #endregion GetWarehouseItems

        #region GetOrderStatuses

        /// <summary>
        /// Обработка обновлений статусов заказов.
        /// </summary>
        private void ProcessGetOrderStatuses()
        {
            _logger.InfoFormat("Старт синхронизации статусов заказов.");
            var curItem = CurrentModel.Items[SyncItemModelKind.GetOrderStatuses];

            RiseInfo("Отправка запроса на получение статусов заказов домена");
            RiseSyncItemDescriptionChanged("Получение данных", curItem);

            var response =
                ClientCore.Instance.WebClient.GetOrderStatuses(new GetOrderStatusesRequest { UserID = ClientCore.Instance.AuthService.AuthToken.UserID });
            RiseInfo(string.Format("Получено {0} статусов заказов", response.OrderStatuses.Count));

            ClientCore.Instance.DataStore.DeleteAllOrderKinds();

            foreach (var orderKind in response.OrderKinds)
            {
                _logger.InfoFormat("Обработка полученного типа заказа {0}", orderKind.Title);

                RiseSyncItemDescriptionChanged(orderKind.Title, curItem);

                var item = new OrderKind();
                item.OrderKindIDGuid = orderKind.OrderKindID;
                item.Title = orderKind.Title;
                ClientCore.Instance.DataStore.SaveOrderKind(item);
            } //foreach

            RiseSyncItemDescriptionChanged("Обработка статусов", curItem);
            ClientCore.Instance.DataStore.DeleteAllOrderStatuses();

            foreach (var orderStatus in response.OrderStatuses)
            {
                _logger.InfoFormat("Обработка полученного статуса заказа {0}", orderStatus.Title);

                var item = new OrderStatus();
                item.OrderStatusIDGuid = orderStatus.OrderStatusID;
                item.Title = orderStatus.Title;
                item.StatusKindID = orderStatus.StatusKindID;

                ClientCore.Instance.DataStore.SaveOrderStatus(item);
            } //foreach

            RiseSyncItemDescriptionChanged(string.Format("Обработано {0} статусов и {1} типов заказов", response.OrderStatuses.Count, response.OrderKinds.Count), curItem);
            _logger.InfoFormat("Синхронизация статусов и типов товаров завершилась с успехом для {0} статусов и {1} типов",
                               response.OrderStatuses.Count, response.OrderKinds.Count);
        }

        #endregion GetOrderStatuses

        #region UpdateRepairOrders

        /// <summary>
        /// Максимальное количество сохраняемых Guidов.
        /// </summary>
        private const int MaxOrdersIds = 200;

        /// <summary>
        /// Обработка обновлений заказов.
        /// </summary>
        private void ProcessUpdateRepairOrders()
        {
            _logger.InfoFormat("Старт синхронизации заказов.");
            var curItem = CurrentModel.Items[SyncItemModelKind.UpdateRepairOrders];

            RiseInfo("Отправка запроса на получение хэшей заказов домена");
            RiseSyncItemDescriptionChanged("Получение хэшей заказов", curItem);

            var processedOrders = new List<RepairOrderServerHashDTO>();//Обработанные серверные данные

            int currentProcessed;
            int totalProcessed=0;
            int totalCount = 0;
            var process = true;
            Guid? lastOrderId = null;
            int processedIndex = 0;
            int localIndex = 0;

            var saveFromServerList = new List<RepairOrderServerHashDTO>();//серверные хэши для обновления данных с сервера.
            var saveToServerList = new List<ItemsContainer>();//Список контейнеров заказов для отправки данных на сервер.

            do
            {
                RiseSyncItemDescriptionChanged("Запрос на сервер по заказам", curItem);
                RiseInfo("Получение хэшей заказов с сервера");

                var orderResponse =
                ClientCore.Instance.WebClient.GetServerRepairOrderHashes(new GetServerRepairOrderHashesRequest
                {
                    LastRepairOrderID = lastOrderId,
                    UserID =
                        ClientCore.Instance.AuthService.AuthToken.
                        UserID
                });

                if (totalCount==0)
                {
                    totalCount = orderResponse.TotalCount;
                } //if

                RiseInfo(string.Format("Получено {0} серверных хэшей",orderResponse.RepairOrderServerHashes.Count));
                
                currentProcessed = 0;

                foreach (var item in orderResponse.RepairOrderServerHashes)
                {
                    if (processedOrders.All(i => i.RepairOrderID != item.RepairOrderID))
                    {
                        currentProcessed++;
                        totalProcessed++;
                        processedOrders.Add(item);
                        lastOrderId = item.RepairOrderID;
                    } //if    
                } //foreach

                _logger.InfoFormat("Для обработки получено {0} хэшей", currentProcessed);
                
                saveFromServerList.Clear();
                saveToServerList.Clear();

                if (currentProcessed>0)
                {
                    for (int i = 0; i < currentProcessed; i++)
                    {
                        var serverHash = processedOrders[processedOrders.Count - (i+1)];

                        var container = new ItemsContainer();
                        var result = CompareServerOrder(serverHash, container);
                        processedIndex++;

                        RiseSyncItemDescriptionChanged(string.Format("Сравнение {0} из {1} заказа", processedIndex, totalCount), curItem);

                        _logger.InfoFormat("Результат сравнения заказа {0}- {1}", serverHash.RepairOrderID, result);

                        if (result == HashesCompareResult.ServerChanged)
                        {
                            saveFromServerList.Add(serverHash);
                        } //if
                        else if(result == HashesCompareResult.ClientChanged)
                        {
                            saveToServerList.Add(container);
                        } //else

                    } //for
                } //if

                _logger.InfoFormat("Обработка списка сохранения на клиент {0}", saveFromServerList.Count);
                RiseInfo("Обработка для измененных на сервере заказов");

                if (saveFromServerList.Any())
                {
                    var getOrdersRequest = new GetRepairOrdersRequest
                    {
                        UserID = ClientCore.Instance.AuthService.AuthToken.
                            UserID
                    };
                    getOrdersRequest.RepairOrderIds.AddRange(saveFromServerList.Select(i=>i.RepairOrderID));


                    RiseSyncItemDescriptionChanged(string.Format("Запрос на сервер {0} заказов", getOrdersRequest.RepairOrderIds.Count), curItem);

                    var response = ClientCore.Instance.WebClient.GetRepairOrders(getOrdersRequest);
                    _logger.InfoFormat("Получено с сервера заказов {0}", response.RepairOrders.Count);
                    

                    foreach (var repairOrderDTO in response.RepairOrders)
                    {
                        _logger.InfoFormat("Сохранение заказа с сервера {0} ",repairOrderDTO.RepairOrderID);
                        localIndex++;
                        RiseSyncItemDescriptionChanged(string.Format("Сохранение серверного заказа {0} ({1} из {2})", repairOrderDTO.Number, localIndex, totalCount), curItem);

                        SaveRepairOrder(repairOrderDTO);
                        SaveRepairOrderServerHash(
                            saveFromServerList.FirstOrDefault(i => i.RepairOrderID == repairOrderDTO.RepairOrderID));

                    } //foreach
                } //if

                _logger.InfoFormat("Обработка списка заказов для отправки на сервер в количестве {0}", saveToServerList.Count);

                RiseInfo("Обработка для измененных на клиенте заказов");

                foreach (var itemsContainer in saveToServerList)
                {
                    RiseSyncItemDescriptionChanged(string.Format("Отправка на сервер заказа номер {0}", itemsContainer.ClientOrder.Number), curItem);
                    var request = new SaveRepairOrderRequest
                                  {
                                      UserID = ClientCore.Instance.AuthService.AuthToken.
                                          UserID
                                  };
                    request.RepairOrder = GetRepairOrder(itemsContainer.ClientOrder, itemsContainer.ClientWorkItems, itemsContainer.ClientDeviceItems, itemsContainer.ClientTimelines);
                    var
                    response = ClientCore.Instance.WebClient.SaveRepairOrder(request);

                    if (response.Success)
                    {
                        SaveRepairOrderServerHash(itemsContainer.RepairOrderServerHashDTO);
                    } //if
                } //foreach

                if (totalCount <= totalProcessed || currentProcessed == 0)
                {
                    process = false;
                } //if
                
                if (process)
                {
                    //Освобождаем память
                    if (processedOrders.Count>MaxOrdersIds)
                    {
                        var newList = new List<RepairOrderServerHashDTO>();
                        for (int i = (MaxOrdersIds / 2); i < processedOrders.Count; i++)
                        {
                            newList.Add(processedOrders[i]);
                        } //for
                        processedOrders = newList;
                    } //if
                } //if

            } while (process);

            _logger.InfoFormat("Старт обработки заказов созданных на клиенте");
            
            localIndex = 0;
            
            RiseInfo("Обработка новых заказов");

            var newClientOrders = ClientCore.Instance.DataStore.GetNewRepairOrderIds().ToList();

            foreach (var newRepairOrderId in newClientOrders)
            {
                RiseSyncItemDescriptionChanged(string.Format("Отправка нового заказа {0} из {1}", localIndex, newClientOrders.Count), curItem);
                _logger.InfoFormat("Старт обработки нового заказа {0}", newRepairOrderId);

                var order = ClientCore.Instance.DataStore.GetRepairOrder(newRepairOrderId);
                var devices = ClientCore.Instance.DataStore.GetDeviceItems(newRepairOrderId).ToList();
                var works = ClientCore.Instance.DataStore.GetWorkItems(newRepairOrderId).ToList();
                var timelines = ClientCore.Instance.DataStore.GetOrderTimelines(newRepairOrderId);
                var orderToServer = GetRepairOrder(order, works, devices, timelines);

                var request = new SaveRepairOrderRequest
                {
                    UserID = ClientCore.Instance.AuthService.AuthToken.
                        UserID
                };
                request.RepairOrder = orderToServer;
                var
                response = ClientCore.Instance.WebClient.SaveRepairOrder(request);

                if (response.Success)
                {
                    var serverHash = GetHashFromClientOrder(order, works, devices);
                    
                    SaveRepairOrderServerHash(serverHash);
                } //if

            } //foreach

            RiseSyncItemDescriptionChanged(string.Format("Обработано {0} заказов с сервера и {1} созданных заказов на клиенте", totalProcessed, newClientOrders.Count), curItem);
            _logger.InfoFormat("Синхронизация статусов и типов товаров завершилась с успехом для {0} заказов сервера и {1} заказов на клиенте",
                             totalProcessed, newClientOrders.Count);
        }

        /// <summary>
        /// Создает DTO объект заказа для отправки на сервер из данных в контейнере.
        /// </summary>
        /// <returns>DTO объект для отправки на сервер.</returns>
        private RepairOrderDTO GetRepairOrder(RepairOrder order, IEnumerable<WorkItem> works, IEnumerable<DeviceItem> devices, IEnumerable<OrderTimeline> timeLines)
        {
            
            var result = new RepairOrderDTO
                         {
                             BranchID = order.BranchIDGuid,
                             CallEventDate = order.CallEventDateDateTime,
                             ClientAddress = order.ClientAddress,
                             ClientEmail = order.ClientEmail,
                             EventDate = order.EventDateDateTime,
                             Number = order.Number,
                             RepairOrderID = order.RepairOrderIDGuid,
                             OrderKindID = order.OrderKindIDGuid,
                             OrderStatusID = order.OrderStatusIDGuid,
                             DeviceSN = order.DeviceSN,
                             DeviceTitle = order.DeviceTitle,
                             ClientFullName = order.ClientFullName,
                             ClientPhone = order.ClientPhone,
                             DateOfBeReady = order.DateOfBeReadyDateTime,
                             Defect = order.Defect,
                             DeviceAppearance = order.DeviceAppearance,
                             DeviceModel = order.DeviceModel,
                             DeviceTrademark = order.DeviceTrademark,
                             EngineerID = order.EngineerIDGuid,
                             GuidePrice = (decimal?)order.GuidePrice,
                             IsUrgent = order.IsUrgentBoolean,
                             IssueDate = order.IssueDateDateTime,
                             IssuerID = order.IssuerIDGuid,
                             ManagerID = order.ManagerIDGuid,
                             Notes = order.Notes,
                             Options = order.Options,
                             PrePayment = (decimal?)order.PrePayment,
                             Recommendation = order.Recommendation,
                             WarrantyTo = order.WarrantyToDateTime,
                         };


            foreach (var clientDeviceItem in devices)
            {
                result.DeviceItems.Add(new DeviceItemDTO
                                       {
                                           CostPrice = (decimal)clientDeviceItem.CostPrice,
                                           Count = (decimal)clientDeviceItem.Count,
                                           DeviceItemID = clientDeviceItem.DeviceItemIDGuid,
                                           EventDate = clientDeviceItem.EventDateDateTime,
                                           Price = (decimal)clientDeviceItem.Price,
                                           RepairOrderID = clientDeviceItem.RepairOrderIDGuid,
                                           Title = clientDeviceItem.Title,
                                           UserID = clientDeviceItem.UserIDGuid,
                                           WarehouseItemID = clientDeviceItem.WarehouseItemIDGuid
                                       });
            } //foreach

            foreach (var clientTimeline in timeLines)
            {
                result.OrderTimelines.Add(new OrderTimelineDTO
                                          {
                                              EventDateTime = clientTimeline.EventDateTimeDateTime,
                                              OrderTimelineID = clientTimeline.OrderTimelineIDGuid,
                                              RepairOrderID = clientTimeline.RepairOrderIDGuid,
                                              TimelineKindID = (byte?)clientTimeline.TimelineKindID,
                                              Title = clientTimeline.Title,
                                          });
            } //foreach

            foreach (var clientWorkItem in works)
            {
                result.WorkItems.Add(new Romontinka.Server.Protocol.SynchronizationMessages.WorkItemDTO
                                     {
                                         EventDate = clientWorkItem.EventDateDateTime,
                                         Price = (decimal)clientWorkItem.Price,
                                         RepairOrderID = clientWorkItem.RepairOrderIDGuid,
                                         Title = clientWorkItem.Title,
                                         UserID = clientWorkItem.UserIDGuid,
                                         WorkItemID = clientWorkItem.WorkItemIDGuid
                                     });
            } //foreach

            return result;
        }

        /// <summary>
        /// Сохраняет локально объект заказа на клиенте.
        /// </summary>
        /// <param name="repairOrderDTO">Заказ полученный с сервера.</param>
        private void SaveRepairOrder(RepairOrderDTO repairOrderDTO)
        {
            ClientCore.Instance.DataStore.SaveRepairOrder(
                new RepairOrder
                {
                    BranchIDGuid = repairOrderDTO.BranchID,
                    CallEventDateDateTime = repairOrderDTO.CallEventDate,
                    ClientAddress = repairOrderDTO.ClientAddress,
                    ClientEmail = repairOrderDTO.ClientEmail,
                    EventDateDateTime = repairOrderDTO.EventDate,
                    Number = repairOrderDTO.Number,
                    RepairOrderIDGuid = repairOrderDTO.RepairOrderID,
                    OrderKindIDGuid = repairOrderDTO.OrderKindID,
                    OrderStatusIDGuid = repairOrderDTO.OrderStatusID,
                    DeviceSN = repairOrderDTO.DeviceSN,
                    DeviceTitle = repairOrderDTO.DeviceTitle,
                    ClientFullName = repairOrderDTO.ClientFullName,
                    ClientPhone = repairOrderDTO.ClientPhone,
                    DateOfBeReadyDateTime = repairOrderDTO.DateOfBeReady,
                    Defect = repairOrderDTO.Defect,
                    DeviceAppearance = repairOrderDTO.DeviceAppearance,
                    DeviceModel = repairOrderDTO.DeviceModel,
                    DeviceTrademark = repairOrderDTO.DeviceTrademark,
                    EngineerIDGuid = repairOrderDTO.EngineerID,
                    GuidePrice = (double?)repairOrderDTO.GuidePrice, 
                    IsUrgentBoolean = repairOrderDTO.IsUrgent,
                    IssueDateDateTime = repairOrderDTO.IssueDate,
                    IssuerIDGuid = repairOrderDTO.IssuerID,
                    ManagerIDGuid = repairOrderDTO.ManagerID,
                    Notes = repairOrderDTO.Notes,
                    Options = repairOrderDTO.Options,
                    PrePayment = (double?)repairOrderDTO.PrePayment,
                    Recommendation = repairOrderDTO.Recommendation,
                    WarrantyToDateTime = repairOrderDTO.WarrantyTo,
                }
                );

            ClientCore.Instance.DataStore.DeleteAllOrderTimelines(repairOrderDTO.RepairOrderID);

            foreach (var orderTimelineDTO in repairOrderDTO.OrderTimelines)
            {
                ClientCore.Instance.DataStore.SaveOrderTimeline(new OrderTimeline
                                                                {
                                                                    EventDateTimeDateTime = orderTimelineDTO.EventDateTime,
                                                                    OrderTimelineIDGuid = orderTimelineDTO.OrderTimelineID,
                                                                    RepairOrderIDGuid = orderTimelineDTO.RepairOrderID,
                                                                    TimelineKindID = orderTimelineDTO.TimelineKindID,
                                                                    Title = orderTimelineDTO.Title
                                                                });
            } //foreach

            ClientCore.Instance.DataStore.DeleteAllWorkItems(repairOrderDTO.RepairOrderID);

            foreach (var workItemDTO in repairOrderDTO.WorkItems)
            {
                ClientCore.Instance.DataStore.SaveWorkItem(new WorkItem
                {
                   EventDateDateTime = workItemDTO.EventDate,
                   Price = (double)workItemDTO.Price,
                   Title = workItemDTO.Title,
                   RepairOrderIDGuid = workItemDTO.RepairOrderID,
                   UserIDGuid = workItemDTO.UserID,
                   WorkItemIDGuid = workItemDTO.WorkItemID
                });
            } //foreach

            ClientCore.Instance.DataStore.DeleteAllDeviceItems(repairOrderDTO.RepairOrderID);

            foreach (var deviceItemDTO in repairOrderDTO.DeviceItems)
            {
                ClientCore.Instance.DataStore.SaveDeviceItem(new DeviceItem
                {
                    EventDateDateTime = deviceItemDTO.EventDate,
                    Price = (double)deviceItemDTO.Price,
                    RepairOrderIDGuid = deviceItemDTO.RepairOrderID,
                    Title = deviceItemDTO.Title,
                    UserIDGuid = deviceItemDTO.UserID,
                    CostPrice = (double)deviceItemDTO.CostPrice,
                    Count = (double)deviceItemDTO.Count,
                    DeviceItemIDGuid = deviceItemDTO.DeviceItemID,
                    WarehouseItemIDGuid = deviceItemDTO.WarehouseItemID
                });
            } //foreach
        }

        /// <summary>
        /// Контейнер элементов.
        /// </summary>
        private class ItemsContainer
        {
            /// <summary>
            /// Задает или получает заказ сохраненный на клиенте.
            /// </summary>
            public RepairOrder ClientOrder { get; set; }

            /// <summary>
            /// Задает или получает пункты выполненных работ сохраненные на клиенте.
            /// </summary>
            public IList<WorkItem> ClientWorkItems { get; set; }

            /// <summary>
            /// Задает или получает пункты установленных запчастей сохраненных на клиенте.
            /// </summary>
            public IList<DeviceItem> ClientDeviceItems { get; set; }

            /// <summary>
            /// Задает или получает пункты графиков заказа сохраненных на клиенте.
            /// </summary>
            public IList<OrderTimeline> ClientTimelines { get; set; }

            /// <summary>
            /// Задает или получает сформированный серверный хэш из клиентских данных.
            /// </summary>
            public RepairOrderServerHashDTO RepairOrderServerHashDTO { get; set; }
        }

        /// <summary>
        /// Результат сравнения хэшей.
        /// </summary>
        private enum HashesCompareResult
        {
            /// <summary>
            /// Эквивалентные хэши.
            /// </summary>
            Equal,

            /// <summary>
            /// Изменения на сервере.
            /// </summary>
            ServerChanged,

            /// <summary>
            /// Изменения на клиенте.
            /// </summary>
            ClientChanged
        }

        /// <summary>
        /// Сравнивает серверный хэш с существующим.
        /// </summary>
        /// <param name="repairOrderServerHash">серверный хэш для сравнения.</param>
        /// <param name="container">Контейнер для передачи полученных данных.</param>
        /// <returns>1 - на сервере были изменения. 0 - Изменений на сервере и клиенте нет. -1 - на клиенте были изменения. </returns>
        private HashesCompareResult CompareServerOrder(RepairOrderServerHashDTO repairOrderServerHash, ItemsContainer container)
        {
           //сравниваем сначала предыдущие сохранения.

            var clientOrderHash =
                ClientCore.Instance.DataStore.GetRepairOrderServerHashItem(repairOrderServerHash.RepairOrderID);
            if (clientOrderHash==null) // если ранее не было синхронизации по этому заказу, тогда думаем, что на сервере новый заказ.
            {
                return HashesCompareResult.ServerChanged;
            } //if

            if ((!StringComparer.OrdinalIgnoreCase.Equals(clientOrderHash.DataHash,repairOrderServerHash.DataHash)) ||  clientOrderHash.OrderTimelinesCount!=repairOrderServerHash.OrderTimelinesCount) //сравниваем изменения по данным
            {
                return HashesCompareResult.ServerChanged;
            } //if

            var clientDeviceHashes =
                ClientCore.Instance.DataStore.GetDeviceItemServerHashItems(repairOrderServerHash.RepairOrderID).ToList();
            if (clientDeviceHashes.Count!=repairOrderServerHash.DeviceItems.Count)//Сравниваем количество устройств
            {
                return HashesCompareResult.ServerChanged;
            } //if

            if (!clientDeviceHashes.All(i => repairOrderServerHash.DeviceItems.Any(ii => ii.DeviceItemID == i.DeviceItemServerHashIDGuid && StringComparer.OrdinalIgnoreCase.Equals(i.DataHash,ii.DataHash))))
            {
                return HashesCompareResult.ServerChanged;
            } //if

            var clientWorkHashes =
                ClientCore.Instance.DataStore.GetWorkItemServerHashItems(repairOrderServerHash.RepairOrderID).ToList();

            if (clientWorkHashes.Count != repairOrderServerHash.WorkItems.Count)//Сравниваем количество произведенных работ
            {
                return HashesCompareResult.ServerChanged;
            } //if

            if (!clientWorkHashes.All(i => repairOrderServerHash.WorkItems.Any(ii => ii.WorkItemID == i.WorkItemServerHashIDGuid && StringComparer.OrdinalIgnoreCase.Equals(i.DataHash, ii.DataHash))))
            {
                return HashesCompareResult.ServerChanged;
            } //if
            
            //создаем клиентские хэши

            container.ClientOrder = ClientCore.Instance.DataStore.GetRepairOrder(repairOrderServerHash.RepairOrderID);
            if (container.ClientOrder==null)
            {
                return HashesCompareResult.ServerChanged;
            } //if

            //Получаем данные
            var order = container.ClientOrder;
            var repairOrderId = container.ClientOrder.RepairOrderIDGuid;
            var workItems = ClientCore.Instance.DataStore.GetWorkItems(repairOrderId).ToList();
            var deviceItems = ClientCore.Instance.DataStore.GetDeviceItems(repairOrderId).ToList();
            //Вычисляем хэши
            var serverHash = GetHashFromClientOrder(order, workItems, deviceItems);

            container.RepairOrderServerHashDTO = serverHash;
            container.ClientTimelines =
                ClientCore.Instance.DataStore.GetOrderTimelines(repairOrderId).ToList();
            container.ClientDeviceItems = deviceItems;
            container.ClientWorkItems = workItems;

            //сравнение заказа
            if (!StringComparer.OrdinalIgnoreCase.Equals(container.RepairOrderServerHashDTO.DataHash, repairOrderServerHash.DataHash) || container.RepairOrderServerHashDTO.OrderTimelinesCount != repairOrderServerHash.OrderTimelinesCount)
            {
                return HashesCompareResult.ClientChanged;
            } //if

            //сравнение количеств работ и запчастей

            if (container.RepairOrderServerHashDTO.WorkItems.Count != repairOrderServerHash.WorkItems.Count || container.RepairOrderServerHashDTO.DeviceItems.Count != repairOrderServerHash.DeviceItems.Count)
            {
                return HashesCompareResult.ClientChanged;
            } //if

            if (!container.RepairOrderServerHashDTO.WorkItems.All(i => repairOrderServerHash.WorkItems.Any(ii=>i.WorkItemID ==ii.WorkItemID && StringComparer.OrdinalIgnoreCase.Equals(i.DataHash,ii.DataHash))))
            {
                return HashesCompareResult.ClientChanged;
            } //if

            if (!container.RepairOrderServerHashDTO.DeviceItems.All(i => repairOrderServerHash.DeviceItems.Any(ii => i.DeviceItemID == ii.DeviceItemID && StringComparer.OrdinalIgnoreCase.Equals(i.DataHash, ii.DataHash))))
            {
                return HashesCompareResult.ClientChanged;
            } //if

            return HashesCompareResult.Equal;
        }

        /// <summary>
        /// Получает объект серверного хэша заказа из клиентских данных.
        /// </summary>
        /// <param name="order">Клиентский заказ.</param>
        /// <param name="workItems">Список проделанных работ.</param>
        /// <param name="deviceItems">Список установленных запчастей.</param>
        /// <returns>Хэш заказа.</returns>
        private RepairOrderServerHashDTO GetHashFromClientOrder(RepairOrder order, IEnumerable<WorkItem> workItems, IEnumerable<DeviceItem> deviceItems)
        {
            var serverHash = new RepairOrderServerHashDTO();
            serverHash.RepairOrderID = order.RepairOrderIDGuid;
            serverHash.DataHash = GetRepairOrderHash(order);
            serverHash.OrderTimelinesCount =
                ClientCore.Instance.DataStore.GetOrderTimelineCount(order.RepairOrderIDGuid);

            foreach (var clientWorkItem in workItems)
            {
                serverHash.WorkItems.Add(new WorkItemServerHashDTO
                {
                    DataHash = GetWorkItemHash(clientWorkItem),
                    WorkItemID = clientWorkItem.WorkItemIDGuid
                });
            } //foreach

            foreach (var clientDeviceItem in deviceItems)
            {
                serverHash.DeviceItems.Add(new DeviceItemServerHashDTO
                {
                    DataHash = GetDeviceItemHash(clientDeviceItem),
                    DeviceItemID = clientDeviceItem.DeviceItemIDGuid
                });
            } //foreach

            return serverHash;
        }

        /// <summary>
        /// Сохраняет полученные хэши с сервера локально.
        /// </summary>
        /// <param name="serverHashes">Серверные хэши.</param>
        private void SaveRepairOrderServerHash(RepairOrderServerHashDTO serverHashes)
        {
            _logger.InfoFormat("Сохраняем локально серверный хэш заказа {0}",serverHashes.RepairOrderID);

            ClientCore.Instance.DataStore.SaveRepairOrderServerHashItem(new RepairOrderServerHashItem
                                                                        {
                                                                            DataHash = serverHashes.DataHash,
                                                                            OrderTimelinesCount = serverHashes.OrderTimelinesCount,
                                                                            RepairOrderServerHashIDGuid = serverHashes.RepairOrderID
                                                                        });
            ClientCore.Instance.DataStore.DeleteAllWorkItemServerHashItems(serverHashes.RepairOrderID);

            foreach (var workItemServerHashDTO in serverHashes.WorkItems)
            {
                ClientCore.Instance.DataStore.SaveWorkItemServerHashItem(new WorkItemServerHashItem
                                                                         {
                                                                             DataHash = workItemServerHashDTO.DataHash,
                                                                             RepairOrderServerHashIDGuid = serverHashes.RepairOrderID,
                                                                             WorkItemServerHashIDGuid = workItemServerHashDTO.WorkItemID
                                                                         });
            } //foreach

            ClientCore.Instance.DataStore.DeleteAllDeviceItemServerHashItems(serverHashes.RepairOrderID);

            foreach (var deviceItemServerHashDTO in serverHashes.DeviceItems)
            {
                ClientCore.Instance.DataStore.SaveDeviceItemServerHashItem(new DeviceItemServerHashItem
                {
                    DataHash = deviceItemServerHashDTO.DataHash,
                    RepairOrderServerHashIDGuid = serverHashes.RepairOrderID,
                    DeviceItemServerHashIDGuid = deviceItemServerHashDTO.DeviceItemID
                });
            } //foreach
        }

        /// <summary>
        /// Получает хэш выполненной работы.
        /// </summary>
        /// <param name="workItem">Выполненная работа с которой необходимо вычислить хэш.</param>
        /// <returns>Хэш.</returns>
        private string GetWorkItemHash(WorkItem workItem)
        {
            var builder = new StringBuilder();
            builder.Append(GuidToString(workItem.WorkItemIDGuid));
            builder.Append(GuidToString(workItem.UserIDGuid));
            builder.Append(workItem.Title);
            builder.Append(DateTimeToString(workItem.EventDateDateTime));
            builder.Append(DoubleToString(workItem.Price));
            builder.Append(GuidToString(workItem.RepairOrderIDGuid));

            return ClientCore.Instance.CryptoService.CreateMd5Hash(builder.ToString(), _encoding);
        }

        /// <summary>
        /// Получает хэш установленной запчасти.
        /// </summary>
        /// <param name="deviceItem">Установленная запчасть с которой необходимо вычислить хэш.</param>
        /// <returns>Хэш.</returns>
        private string GetDeviceItemHash(DeviceItem deviceItem)
        {
            var builder = new StringBuilder();
            builder.Append(GuidToString(deviceItem.DeviceItemIDGuid));
            builder.Append(GuidToString(deviceItem.UserIDGuid));
            builder.Append(deviceItem.Title);
            builder.Append(DoubleToString(deviceItem.Count));
            builder.Append(DoubleToString(deviceItem.CostPrice));
            builder.Append(DoubleToString(deviceItem.Price));
            builder.Append(GuidToString(deviceItem.RepairOrderIDGuid));
            builder.Append(DateTimeToString(deviceItem.EventDateDateTime));
            builder.Append(GuidToString(deviceItem.WarehouseItemIDGuid));
            var rawStr = builder.ToString();
            return ClientCore.Instance.CryptoService.CreateMd5Hash(rawStr, _encoding);
        }

        /// <summary>
        /// Получает хэш заказа.
        /// </summary>
        /// <param name="repairOrder">Заказ с которого необходимо вычислить хэш.</param>
        /// <returns>Хэш.</returns>
        private string GetRepairOrderHash(RepairOrder repairOrder)
        {
            var builder = new StringBuilder();
            builder.Append(GuidToString(repairOrder.RepairOrderIDGuid));
            builder.Append(GuidToString(repairOrder.IssuerIDGuid));
            builder.Append(GuidToString(repairOrder.OrderStatusIDGuid));
            builder.Append(GuidToString(repairOrder.EngineerIDGuid));
            builder.Append(GuidToString(repairOrder.ManagerIDGuid));
            builder.Append(GuidToString(repairOrder.OrderKindIDGuid));
            builder.Append(DateTimeToString(repairOrder.EventDateDateTime));
            builder.Append(repairOrder.Number);
            builder.Append(repairOrder.ClientFullName);
            builder.Append(repairOrder.ClientAddress);
            builder.Append(repairOrder.ClientPhone);
            builder.Append(repairOrder.ClientEmail);
            builder.Append(repairOrder.DeviceTitle);
            builder.Append(repairOrder.DeviceSN);
            builder.Append(repairOrder.DeviceTrademark);
            builder.Append(repairOrder.DeviceModel);
            builder.Append(repairOrder.Defect);
            builder.Append(repairOrder.Options);
            builder.Append(repairOrder.DeviceAppearance);
            builder.Append(repairOrder.Notes);
            builder.Append(DateTimeToString(repairOrder.CallEventDateDateTime));
            builder.Append(DateTimeToString(repairOrder.DateOfBeReadyDateTime));
            builder.Append(DoubleToString(repairOrder.GuidePrice));
            builder.Append(DoubleToString(repairOrder.PrePayment));
            builder.Append(BooleanToString(repairOrder.IsUrgentBoolean));
            builder.Append(repairOrder.Recommendation);
            builder.Append(DateTimeToString(repairOrder.IssueDateDateTime));
            builder.Append(DateTimeToString(repairOrder.WarrantyToDateTime));
            builder.Append(GuidToString(repairOrder.BranchIDGuid));
            var rawStr = builder.ToString();
            return ClientCore.Instance.CryptoService.CreateMd5Hash(rawStr, _encoding);
        }

        /// <summary>
        /// Содержит кодировку данных.
        /// </summary>
        private readonly Encoding _encoding = Encoding.GetEncoding(1251);

        private string GuidToString(Guid? value)
        {
            if (value==null)
            {
                return string.Empty;
            } //if
            return value.Value.ToString().ToUpper();
        }

        private string BooleanToString(bool? value)
        {
            if (value == null)
            {
                return string.Empty;
            } //if
            return value.Value?"1":"0";
        }

        private string DoubleToString(double? value)
        {
            if (value == null)
            {
                return string.Empty;
            } //if
            return ((decimal) value.Value).ToString("0.00", CultureInfo.InvariantCulture).Replace(",", ".");
        }

        private string DateTimeToString(DateTime? value)
        {
            if (value == null)
            {
                return string.Empty;
            } //if
            return value.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #endregion UpdateRepairOrders

        #region CustomReportItem

        /// <summary>
        /// Обработка обновлений ответов.
        /// </summary>
        private void ProcessGetCustomReportItems()
        {
            _logger.InfoFormat("Старт синхронизации отчетов.");
            var curItem = CurrentModel.Items[SyncItemModelKind.GetCustomReports];

            RiseInfo("Отправка запроса на получение отчетов домена");
            RiseSyncItemDescriptionChanged("Получение данных", curItem);

            var response =
                ClientCore.Instance.WebClient.GetCustomReportItems(new GetCustomReportItemRequest { UserID = ClientCore.Instance.AuthService.AuthToken.UserID });
            RiseInfo(string.Format("Получено {0} отчетов", response.CustomReportItems.Count));

            ClientCore.Instance.DataStore.DeleteAllCustomReportItems();

            foreach (var reportItem in response.CustomReportItems)
            {
                _logger.InfoFormat("Обработка полученного отчета {0}", reportItem.Title);

                RiseSyncItemDescriptionChanged(reportItem.Title, curItem);

                var item = new CustomReportItem();
                item.CustomReportIDGuid = reportItem.CustomReportID;
                item.DocumentKindID = reportItem.DocumentKindID;
                item.HtmlContent = reportItem.HtmlContent;
                item.Title = reportItem.Title;

                ClientCore.Instance.DataStore.SaveCustomReportItem(item);
            } //foreach

            RiseSyncItemDescriptionChanged(string.Format("Обработано {0} отчетов", response.CustomReportItems.Count), curItem);
            _logger.InfoFormat("Синхронизация складов завершилась с успехом для {0} отчетов",
                               response.CustomReportItems.Count);
        }

        #endregion CustomReportItem

    }
}
