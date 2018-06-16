using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using Romontinka.Server.Core;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.Protocol;
using Romontinka.Server.Protocol.AuthMessages;
using Romontinka.Server.Protocol.SynchronizationMessages;
using log4net;

namespace Romontinka.Server.ProtocolServices
{
    /// <summary>
    /// Сервер протокола.
    /// </summary>
    public class ProtocolServer
    {
        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof (ProtocolServer));

        #region  Singleton

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:Romontinka.Server.ProtocolServices.ProtocolServer"/>.
        /// </summary>
        private ProtocolServer()
        {
            //TODO добавлять обработчики сообщений
            _methods.Add(
                new KeyValuePair<MessageKind, Func<ProtocolSerializer, string, string, string>>(
                    MessageKind.RegisterPublicKeyRequest, ProcessRegisterPublicKeyRequest));
            
            _methods.Add(
                new KeyValuePair<MessageKind, Func<ProtocolSerializer, string, string, string>>(
                    MessageKind.ProbeKeyActivationRequest, ProcessProbeKeyActivationRequest));

            _methods.Add(
                new KeyValuePair<MessageKind, Func<ProtocolSerializer, string, string, string>>(
                    MessageKind.GetDomainUsersRequest, ProcessGetDomainUsersRequest));

            _methods.Add(
                new KeyValuePair<MessageKind, Func<ProtocolSerializer, string, string, string>>(
                    MessageKind.GetUserBranchesRequest, ProcessGetUserBranchesRequest));
            
            _methods.Add(
                new KeyValuePair<MessageKind, Func<ProtocolSerializer, string, string, string>>(
                    MessageKind.GetFinancialGroupBranchesRequest, ProcessGetFinancialGroupBranchesRequest));

            _methods.Add(
                new KeyValuePair<MessageKind, Func<ProtocolSerializer, string, string, string>>(
                    MessageKind.GetWarehousesRequest, ProcessGetWarehousesRequest));

            _methods.Add(
                new KeyValuePair<MessageKind, Func<ProtocolSerializer, string, string, string>>(
                    MessageKind.GetGoodsItemRequest, ProcessGetGoodsItemRequest));

            _methods.Add(
                new KeyValuePair<MessageKind, Func<ProtocolSerializer, string, string, string>>(
                    MessageKind.GetWarehouseItemsRequest, ProcessGetWarehouseItemsRequest));

            _methods.Add(
                new KeyValuePair<MessageKind, Func<ProtocolSerializer, string, string, string>>(
                    MessageKind.GetOrderStatusesRequest, ProcessGetOrderStatusesRequest));
            _methods.Add(
                new KeyValuePair<MessageKind, Func<ProtocolSerializer, string, string, string>>(
                    MessageKind.GetServerRepairOrderHashesRequest, ProcessGetServerRepairOrderHashesRequest));

            _methods.Add(
                new KeyValuePair<MessageKind, Func<ProtocolSerializer, string, string, string>>(
                    MessageKind.GetRepairOrdersRequest, ProcessGetRepairOrdersRequest));

            _methods.Add(
                new KeyValuePair<MessageKind, Func<ProtocolSerializer, string, string, string>>(
                    MessageKind.SaveRepairOrderRequest, ProcessSaveRepairOrderRequest));
            
            _methods.Add(
                new KeyValuePair<MessageKind, Func<ProtocolSerializer, string, string, string>>(
                    MessageKind.GetCustomReportItemRequest, ProcessGetCustomReportItemRequest));
        }

        /// <summary>
        ///   Инстанс менеджера.
        /// </summary>
        private static volatile ProtocolServer _protocolService;

        /// <summary>
        ///   Объект синхронизации.
        /// </summary>
        private static readonly object _syncRoot = new object();

        /// <summary>
        ///   Получает инстанс менеджера.
        /// </summary>
        /// <returns> </returns>
        public static ProtocolServer Instance
        {
            get
            {
                if (_protocolService == null)
                {
                    lock (_syncRoot)
                    {
                        if (_protocolService == null)
                        {
                            _protocolService = new ProtocolServer();
                        }
                    }
                }

                return _protocolService;
            }
        }

        #endregion  Singleton

        /// <summary>
        /// Строка показывающая, что метод неопознан.
        /// </summary>
        private const string UnknownMethod = "Unknow mode";

        private readonly IDictionary<MessageKind, Func<ProtocolSerializer, string, string, string>> _methods =
            new Dictionary<MessageKind, Func<ProtocolSerializer, string, string, string>>();

        /// <summary>
        /// Обрабатывает запрос пришедший с арма и передает соответствующим службам.
        /// </summary>
        /// <param name="postData">Данные для обработки.</param>
        /// <param name="hostAddress">Адрес хоста клиента. </param>
        /// <returns>Сформированный ответ.</returns>
        public string ProcessRequest(string postData, string hostAddress)
        {
            var result = UnknownMethod;
            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);

            var kindInfo = serializer.GetMessageInfoOrNull(postData);

            if (kindInfo==null)
            {
                _logger.ErrorFormat("Получен неизвестный метод с клиента \"{0}\" сообщение = \"{1}\"",
                                           hostAddress, postData);
                return result;
            } //if

            if (!_methods.ContainsKey(kindInfo.Kind))
            {
                _logger.ErrorFormat("Нет обработчика для метода {0} с клиента \"{1}\" сообщение = \"{2}\"",kindInfo.Kind,
                                           hostAddress, postData);
                return result;
            } //if

            try
            {
                var method = _methods[kindInfo.Kind];
                result = method(serializer, postData, hostAddress);
            }
            catch (AuthException ex)
            {
                _logger.ErrorFormat(
                    "Во время обработки подписанного запроса с клинета {0}:{1} была получена ошибка [{2} {3} {4}] сообщение \"{5}\"", kindInfo.Kind,
                    hostAddress, ex.Message, ex.GetType(), ex.StackTrace, postData);
                result = serializer.Serialize(new AuthErrorResponse {Message = ex.Message});
            }
            catch (Exception ex)
            {
                var inner = string.Empty;
                if (ex.InnerException!=null)
                {
                    inner = ex.InnerException.Message;
                }
                _logger.ErrorFormat(
                    "Во время обработки запроса с клинета {0}:{1} была получена ошибка [{2} {3} {4} {5}] сообщение \"{6}\"", kindInfo.Kind,
                    hostAddress, ex.Message, ex.GetType(), ex.StackTrace, inner,postData);
                result = serializer.Serialize(new ErrorResponse { Description = ex.Message });

            } //try

            return result;
        }

        /// <summary>
        /// Создавет ответ на ошибку.
        /// </summary>
        /// <param name="serializer">Текущие сериалайзер.</param>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <returns>Ответ.</returns>
        private string CreateErrorResponse(ProtocolSerializer serializer, string message)
        {
            var error = new ErrorResponse();
            error.Description = message;
            return serializer.Serialize(error);
        }

        /// <summary>
        /// Содержит кодировку подписаных сообщений.
        /// </summary>
        private readonly Encoding _signEncoding = Encoding.UTF8;

        /// <summary>
        /// Производит проверку подписанного сообщения.
        /// </summary>
        /// <param name="signedRequest">Подписанное сообщение.</param>
        /// <returns>Домен пользователя.</returns>
        private Guid? CheckSignedMessage(SignedRequestBase signedRequest)
        {
            var key = RemontinkaServer.Instance.DataStore.GetCurrentPublicKey(signedRequest.UserID);
            if (key == null)
            {
                _logger.ErrorFormat("Для пользователя {0} отсутствует публичный ключ", signedRequest.UserID);
                throw new AuthException("Для пользователя отсутствует публичный ключ");
            } //if

            if (key.IsRevoked)
            {
                _logger.ErrorFormat("Для пользователя {0} публичный ключ был отозван {1}", signedRequest.UserID, key.Number);
                throw new AuthException("Для пользователя отозванный публичный ключ");
            } //if

            var data = signedRequest.GetDataForSign(new ServerMessageSignVisitor());
            if (!RemontinkaServer.Instance.CryptoService.Verify(key.PublicKeyData, data, signedRequest.SignData, _signEncoding))
            {
                _logger.InfoFormat("Сообщение {0} не прошло проверку у пользователя {1} с ключем  {2}", signedRequest.Kind, signedRequest.UserID, key.Number);
                throw new AuthException("Сообщение не прошло проверку");
            }

            return RemontinkaServer.Instance.DataStore.GetUserDomainByUserID(signedRequest.UserID);
        }

        #region RegisterPublicKeyRequest 

        /// <summary>
        /// Обработчик запроса на регистрацию ключей.
        /// </summary>
        /// <param name="serializer">Сериализатор протокола.</param>
        /// <param name="postData">Данные запроса.</param>
        /// <param name="hostAddress">Адрес клиента.</param>
        /// <returns>Результат.</returns>
        private string ProcessRegisterPublicKeyRequest(ProtocolSerializer serializer,string postData, string hostAddress)
        {
            var request = serializer.DeserializeRegisterPublicKeyRequest(postData);
            _logger.InfoFormat("Получен запрос на регистрацию нового ключа для пользователя {0} с клиента {1}",
                               request.UserLogin, hostAddress);
            var entity = new UserPublicKeyRequest();

            var user = RemontinkaServer.Instance.DataStore.GetUser(request.UserLogin);

            if (user == null)
            {
                _logger.ErrorFormat("Нет пользователя для запроса по публичному ключю {0} с клиента {1}",request.UserLogin,hostAddress);

                return CreateErrorResponse(serializer, "Нет такого пользователя");
            } //if

            entity.ClientIdentifier = hostAddress;
            entity.KeyNotes = request.KeyNotes;
            entity.PublicKeyData = request.PublicKeyData;
            entity.UserID = user.UserID;
            entity.EventDate = request.EventDate;

            if (request.ClientUserDomainID!=null)
            {
                if (request.ClientUserDomainID!=user.UserDomainID)
                {
                    _logger.ErrorFormat("В запросе на регистрацию нового ключа не совпадают домены {0} {1}",
                                        request.ClientUserDomainID, user.UserDomainID);
                    return CreateErrorResponse(serializer, "Домены не совпадают");
                } //if
            } //if

            var number =
                RemontinkaServer.Instance.DataStore.GetNewOrderNumber(user.UserDomainID).ToString(
                    CultureInfo.InvariantCulture);
            entity.Number = string.Format(NewPublicKeyNumberFormat, number);

            RemontinkaServer.Instance.DataStore.SaveUserPublicKeyRequest(entity);

            var response = new RegisterPublicKeyResponse();
            response.Number = entity.Number;
            response.UserDomainID = user.UserDomainID;

            return serializer.Serialize(response);
        }

        /// <summary>
        /// Содержит формат для номеров клиентских ключей.
        /// </summary>
        private const string NewPublicKeyNumberFormat = "U{0}";

        #endregion RegisterPublicKeyRequest

        #region ProbeKeyActivationRequest

         /// <summary>
        /// Обработчик запроса на получению информации по регистрации публичных ключей.
        /// </summary>
        /// <param name="serializer">Сериализатор протокола.</param>
        /// <param name="postData">Данные запроса.</param>
        /// <param name="hostAddress">Адрес клиента.</param>
        /// <returns>Результат.</returns>
        private string ProcessProbeKeyActivationRequest(ProtocolSerializer serializer, string postData, string hostAddress)
         {
             var request = serializer.DeserializeProbeKeyActivationRequest(postData);
             _logger.InfoFormat("Получен запрос на проверки регистрации ключа {0} для домена {1} с клиента {2}",
                                request.KeyNumber,
                                request.UserDomainID, hostAddress);

             var response = new ProbeKeyActivationResponse();

             response.IsExists = true;
             var currentKey = RemontinkaServer.Instance.DataStore.GetPublicKey(request.UserDomainID,request.KeyNumber);
             if (currentKey!=null)
             {
                 currentKey.IsRevoked = currentKey.IsRevoked;
                 response.UserID = currentKey.UserID;

                 currentKey = RemontinkaServer.Instance.DataStore.GetCurrentPublicKey(currentKey.UserID);

                 if (currentKey!=null)
                 {
                     currentKey.IsRevoked = currentKey.IsRevoked;
                 } //if
                 else
                 {
                     response.IsExpired = true;
                 } //else
             } //
             else
             {
                 var keyRequest = RemontinkaServer.Instance.DataStore.GetUserPublicKeyRequest(request.UserDomainID, request.KeyNumber);
                 if (keyRequest != null)
                 {
                     response.IsNotAccepted = true;
                 } //if
                 else
                 {
                     response.IsExists = false;
                 } //else
             } //if

            return serializer.Serialize(response);
         }

        #endregion ProbeKeyActivationRequest

        #region GetDomainUsersRequest

         /// <summary>
        /// Обработчик запроса на получение данных по пользователям домена.
        /// </summary>
        /// <param name="serializer">Сериализатор протокола.</param>
        /// <param name="postData">Данные запроса.</param>
        /// <param name="hostAddress">Адрес клиента.</param>
        /// <returns>Результат.</returns>
        private string ProcessGetDomainUsersRequest(ProtocolSerializer serializer, string postData, string hostAddress)
        {
            var request = serializer.DeserializeGetDomainUsersRequest(postData);
             _logger.InfoFormat(
                 "Получен запрос на получение информации по пользователям от пользователя {0} с клиента {1}",
                 request.UserID, hostAddress);

            var userDomainID= CheckSignedMessage(request);

            var users = RemontinkaServer.Instance.DataStore.GetUsers(userDomainID);
             var response = new GetDomainUsersResponse();
             foreach (var user in users)
             {
                 response.Users.Add(new DomainUserDTO
                                    {
                                        Email = user.Email,
                                        FirstName = user.FirstName,
                                        LastName = user.LastName,
                                        MiddleName = user.MiddleName,
                                        Phone = user.Phone,
                                        ProjectRoleID = user.ProjectRoleID,
                                        UserID = user.UserID,
                                        LoginName = user.LoginName
                                    });
             } //foreach

             return serializer.Serialize(response);
        }

        #endregion GetDomainUsersRequest

        #region GetUserBranchesRequest

        /// <summary>
        /// Обработчик запроса на получение филиалов и их связей с пользователями.
        /// </summary>
        /// <param name="serializer">Сериализатор протокола.</param>
        /// <param name="postData">Данные запроса.</param>
        /// <param name="hostAddress">Адрес клиента.</param>
        /// <returns>Результат.</returns>
        private string ProcessGetUserBranchesRequest(ProtocolSerializer serializer, string postData, string hostAddress)
        {
            var request = serializer.DeserializeGetUserBranchesRequest(postData);
            _logger.InfoFormat(
                "Получен запрос на получение информации по филиалам и их связей с пользователями от пользователя {0} с клиента {1}",
                request.UserID, hostAddress);

            var userDomainID = CheckSignedMessage(request);

            var branches = RemontinkaServer.Instance.DataStore.GetBranches(userDomainID);
            var response = new GetUserBranchesResponse();
            foreach (var branch in branches)
            {
                response.Branches.Add(new BranchDTO
                                      {
                                          Address = branch.Address,
                                          BranchID = branch.BranchID,
                                          LegalName = branch.LegalName,
                                          Title = branch.Title

                                      });
            } //foreach

            var items = RemontinkaServer.Instance.DataStore.GetUserBranchMapItems(userDomainID);

            foreach (var userBranchMapItem in items)
            {
                response.UserBranchMapItems.Add(new Protocol.SynchronizationMessages.UserBranchMapItemDTO
                                                {
                                                    BranchID = userBranchMapItem.BranchID,
                                                    EventDate = userBranchMapItem.EventDate,
                                                    UserBranchMapID = userBranchMapItem.UserBranchMapID,
                                                    UserID = userBranchMapItem.UserID
                                                });
            } //foreach

            return serializer.Serialize(response);
        }

        #endregion GetUserBranchesRequest

        #region GetFinancialGroupBranchesRequest

        /// <summary>
        /// Обработчик запроса на получение филиалов и их связей с пользователями.
        /// </summary>
        /// <param name="serializer">Сериализатор протокола.</param>
        /// <param name="postData">Данные запроса.</param>
        /// <param name="hostAddress">Адрес клиента.</param>
        /// <returns>Результат.</returns>
        private string ProcessGetFinancialGroupBranchesRequest(ProtocolSerializer serializer, string postData, string hostAddress)
        {
            var request = serializer.DeserializeGetFinancialGroupBranchesRequest(postData);
            _logger.InfoFormat(
                "Получен запрос на получение информации по фингруппам и их связей с филиалами от пользователя {0} с клиента {1}",
                request.UserID, hostAddress);

            var userDomainID = CheckSignedMessage(request);

            var branches = RemontinkaServer.Instance.DataStore.GetFinancialGroupItems(userDomainID);
            var response = new GetFinancialGroupBranchesResponse();
            foreach (var branch in branches)
            {
                response.FinancialGroupItems.Add(new FinancialGroupItemDTO
                {
                    Trademark = branch.Trademark,
                    FinancialGroupID = branch.FinancialGroupID,
                    LegalName = branch.LegalName,
                    Title = branch.Title

                });
            } //foreach

            var items = RemontinkaServer.Instance.DataStore.GetFinancialGroupBranchMapItems(userDomainID);

            foreach (var userBranchMapItem in items)
            {
                response.FinancialGroupBranchMapItems.Add(new Protocol.SynchronizationMessages.FinancialGroupBranchMapItemDTO
                {
                    BranchID = userBranchMapItem.BranchID,
                    FinancialGroupID = userBranchMapItem.FinancialGroupID,
                    FinancialGroupBranchMapID = userBranchMapItem.FinancialGroupBranchMapID,
                });
            } //foreach

            return serializer.Serialize(response);
        }

        #endregion GetFinancialGroupBranchesRequest

        #region GetWarehousesRequest

        /// <summary>
        /// Обработчик запроса на получение складов и их связей с фингруппами.
        /// </summary>
        /// <param name="serializer">Сериализатор протокола.</param>
        /// <param name="postData">Данные запроса.</param>
        /// <param name="hostAddress">Адрес клиента.</param>
        /// <returns>Результат.</returns>
        private string ProcessGetWarehousesRequest(ProtocolSerializer serializer, string postData, string hostAddress)
        {
            var request = serializer.DeserializeGetWarehousesRequest(postData);
            _logger.InfoFormat(
                "Получен запрос на получение информации по складам и их связей с фингруппами от пользователя {0} с клиента {1}",
                request.UserID, hostAddress);

            var userDomainID = CheckSignedMessage(request);

            var warehouses = RemontinkaServer.Instance.DataStore.GetWarehouses(userDomainID);
            var response = new GetWarehousesResponse();
            foreach (var warehouse in warehouses)
            {
                response.Warehouses.Add(new WarehouseDTO
                {
                    WarehouseID = warehouse.WarehouseID,
                    Title = warehouse.Title

                });
            } //foreach

            var items = RemontinkaServer.Instance.DataStore.GetFinancialGroupWarehouseMapItems(userDomainID);

            foreach (var groupWarehouseMapItem in items)
            {
                response.MapItems.Add(new Protocol.SynchronizationMessages.FinancialGroupWarehouseMapItemDTO
                {
                    WarehouseID = groupWarehouseMapItem.WarehouseID,
                    FinancialGroupID = groupWarehouseMapItem.FinancialGroupID,
                    FinancialGroupWarehouseMapID = groupWarehouseMapItem.FinancialGroupWarehouseMapID,
                });
            } //foreach

            return serializer.Serialize(response);
        }

        #endregion GetWarehousesRequest

        #region GetGoodsItemRequest

        /// <summary>
        /// Обработчик запроса на получение категорий и номенклатур товаров.
        /// </summary>
        /// <param name="serializer">Сериализатор протокола.</param>
        /// <param name="postData">Данные запроса.</param>
        /// <param name="hostAddress">Адрес клиента.</param>
        /// <returns>Результат.</returns>
        private string ProcessGetGoodsItemRequest(ProtocolSerializer serializer, string postData, string hostAddress)
        {
            var request = serializer.DeserializeGetGoodsItemRequest(postData);
            _logger.InfoFormat(
                "Получен запрос на получение информации по категориям и номенклатур от пользователя {0} с клиента {1}",
                request.UserID, hostAddress);

            var userDomainID = CheckSignedMessage(request);

            var goodsItems = RemontinkaServer.Instance.DataStore.GetGoodsItems(userDomainID);
            var response = new GetGoodsItemResponse();
            foreach (var item in goodsItems)
            {
                response.GoodsItems.Add(new Protocol.SynchronizationMessages.GoodsItemDTO
                {
                    BarCode = item.BarCode,
                    Description = item.Description,
                    DimensionKindID = item.DimensionKindID,
                    GoodsItemID = item.GoodsItemID,
                    ItemCategoryID = item.ItemCategoryID,
                    Particular = item.Particular,
                    Title = item.Title,
                    UserCode = item.UserCode

                });
            } //foreach

            var items = RemontinkaServer.Instance.DataStore.GetItemCategories(userDomainID);

            foreach (var item in items)
            {
                response.ItemCategories.Add(new ItemCategoryDTO
                {
                    ItemCategoryID = item.ItemCategoryID,
                    Title = item.Title
                });
            } //foreach

            return serializer.Serialize(response);
        }

        #endregion GetGoodsItemRequest

        #region GetWarehouseItemsRequest

        /// <summary>
        /// Обработчик запроса на получение остатков на складах.
        /// </summary>
        /// <param name="serializer">Сериализатор протокола.</param>
        /// <param name="postData">Данные запроса.</param>
        /// <param name="hostAddress">Адрес клиента.</param>
        /// <returns>Результат.</returns>
        private string ProcessGetWarehouseItemsRequest(ProtocolSerializer serializer, string postData, string hostAddress)
        {
            var request = serializer.DeserializeGetWarehouseItemsRequest(postData);
            _logger.InfoFormat(
                "Получен запрос на получение информации остаткам на складах от пользователя {0} с клиента {1}",
                request.UserID, hostAddress);

            var userDomainID = CheckSignedMessage(request);

            var warehouseItems = RemontinkaServer.Instance.DataStore.GetWarehouseItems(userDomainID);
            var response = new GetWarehouseItemsResponse();
            foreach (var warehouse in warehouseItems)
            {
                response.WarehouseItems.Add(new Protocol.SynchronizationMessages.WarehouseItemDTO
                {
                    WarehouseID = warehouse.WarehouseID,
                    GoodsItemID = warehouse.WarehouseID,
                    RepairPrice = warehouse.RepairPrice,
                    SalePrice = warehouse.SalePrice,
                    StartPrice = warehouse.StartPrice,
                    Total = warehouse.StartPrice,
                    WarehouseItemID = warehouse.WarehouseItemID
                });
            } //foreach
            
            return serializer.Serialize(response);
        }

        #endregion GetWarehouseItemsRequest

        #region GetOrderStatusesRequest

        /// <summary>
        /// Обработчик запроса на получение статусов заказа.
        /// </summary>
        /// <param name="serializer">Сериализатор протокола.</param>
        /// <param name="postData">Данные запроса.</param>
        /// <param name="hostAddress">Адрес клиента.</param>
        /// <returns>Результат.</returns>
        private string ProcessGetOrderStatusesRequest(ProtocolSerializer serializer, string postData, string hostAddress)
        {
            var request = serializer.DeserializeGetOrderStatusesRequest(postData);
            _logger.InfoFormat(
                "Получен запрос на получение информации по статусам заказов от пользователя {0} с клиента {1}",
                request.UserID, hostAddress);

            var userDomainID = CheckSignedMessage(request);

            var statuses = RemontinkaServer.Instance.DataStore.GetOrderStatuses(userDomainID);
            var response = new GetOrderStatusesResponse();
            foreach (var status in statuses)
            {
                response.OrderStatuses.Add(new OrderStatusDTO
                {
                    OrderStatusID = status.OrderStatusID,
                    Title = status.Title,
                    StatusKindID = status.StatusKindID

                });
            } //foreach

            var orderKinds = RemontinkaServer.Instance.DataStore.GetOrderKinds(userDomainID);

            foreach (var orderKind in orderKinds)
            {
                response.OrderKinds.Add(new OrderKindDTO
                {
                    OrderKindID = orderKind.OrderKindID,
                    Title = orderKind.Title
                });
            } //foreach

            return serializer.Serialize(response);
        }

        #endregion GetOrderStatusesRequest

        #region GetServerRepairOrderHashesRequest

        /// <summary>
        /// Обработчик запроса на получение хэшей заказов.
        /// </summary>
        /// <param name="serializer">Сериализатор протокола.</param>
        /// <param name="postData">Данные запроса.</param>
        /// <param name="hostAddress">Адрес клиента.</param>
        /// <returns>Результат.</returns>
        private string ProcessGetServerRepairOrderHashesRequest(ProtocolSerializer serializer, string postData, string hostAddress)
        {
            var request = serializer.DeserializeGetServerRepairOrderHashesRequest(postData);
            _logger.InfoFormat(
                "Получен запрос на получение информации по хэшам заказов от пользователя {0} с клиента {1}",
                request.UserID, hostAddress);

            var userDomainID = CheckSignedMessage(request);

            var response = new GetServerRepairOrderHashesResponse();

            int total;

            var hashes = RemontinkaServer.Instance.DataStore.GetRepairOrderHashes(userDomainID,
                                                                                  request.LastRepairOrderID, out total).ToList();

            response.TotalCount = total;

            foreach (var repairOrderHash in hashes)
            {
                var item = new RepairOrderServerHashDTO
                           {
                               DataHash = repairOrderHash.DataHash,
                               OrderTimelinesCount = repairOrderHash.OrderTimelinesCount,
                               RepairOrderID = repairOrderHash.RepairOrderID
                           };

                response.RepairOrderServerHashes.Add(item);

                foreach (var deviceItemHash in repairOrderHash.DeviceItemHashes)
                {
                    item.DeviceItems.Add(new DeviceItemServerHashDTO
                                         {
                                             DeviceItemID = deviceItemHash.DeviceItemID,
                                             DataHash = deviceItemHash.DataHash
                                         });
                } //foreach

                foreach (var workItemHash in repairOrderHash.WorkItemHashes)
                {
                    item.WorkItems.Add(new WorkItemServerHashDTO
                    {
                        WorkItemID = workItemHash.WorkItemID,

                        DataHash = workItemHash.DataHash
                    });
                } //foreach
            } //foreach


            return serializer.Serialize(response);
        }

        #endregion GetServerRepairOrderHashesRequest

        #region GetRepairOrdersRequest

         /// <summary>
        /// Обработчик запроса на получение заказов.
        /// </summary>
        /// <param name="serializer">Сериализатор протокола.</param>
        /// <param name="postData">Данные запроса.</param>
        /// <param name="hostAddress">Адрес клиента.</param>
        /// <returns>Результат.</returns>
        private string ProcessGetRepairOrdersRequest(ProtocolSerializer serializer, string postData, string hostAddress)
        {
            var request = serializer.DeserializeGetRepairOrdersRequest(postData);
            _logger.InfoFormat(
                "Получен запрос на получение информации по заказам от пользователя {0} с клиента {1}",
                request.UserID, hostAddress);

            var userDomainID = CheckSignedMessage(request);

            var response = new GetRepairOrdersResponse();

             foreach (var repairOrderId in request.RepairOrderIds)
             {
                 var repairOrder = RemontinkaServer.Instance.DataStore.GetRepairOrderLight(repairOrderId, userDomainID);
                 if (repairOrder!=null)
                 {
                     var orderDto = new Protocol.SynchronizationMessages.RepairOrderDTO
                                    {
                                        BranchID = repairOrder.BranchID,
                                        CallEventDate = repairOrder.CallEventDate,
                                        ClientAddress = repairOrder.ClientAddress,
                                        ClientEmail = repairOrder.ClientEmail,
                                        EventDate = repairOrder.EventDate,
                                        Number = repairOrder.Number,
                                        RepairOrderID = repairOrder.RepairOrderID,
                                        OrderKindID = repairOrder.OrderKindID,
                                        OrderStatusID = repairOrder.OrderStatusID,
                                        DeviceSN = repairOrder.DeviceSN,
                                        DeviceTitle = repairOrder.DeviceTitle,
                                        ClientFullName = repairOrder.ClientFullName,
                                        ClientPhone = repairOrder.ClientPhone,
                                        DateOfBeReady = repairOrder.DateOfBeReady,
                                        Defect = repairOrder.Defect,
                                        DeviceAppearance = repairOrder.DeviceAppearance,
                                        DeviceModel = repairOrder.DeviceModel,
                                        DeviceTrademark = repairOrder.DeviceTrademark,
                                        EngineerID = repairOrder.EngineerID,
                                        GuidePrice = repairOrder.GuidePrice,
                                        IsUrgent = repairOrder.IsUrgent,
                                        IssueDate = repairOrder.IssueDate,
                                        IssuerID = repairOrder.IssuerID,
                                        ManagerID = repairOrder.ManagerID,
                                        Notes = repairOrder.Notes,
                                        Options = repairOrder.Options,
                                        PrePayment = repairOrder.PrePayment,
                                        Recommendation = repairOrder.Recommendation,
                                        WarrantyTo = repairOrder.WarrantyTo
                                    };

                     response.RepairOrders.Add(orderDto);

                     var workItems = RemontinkaServer.Instance.DataStore.GetWorkItems(repairOrderId);

                     foreach (var item in workItems)
                     {
                         orderDto.WorkItems.Add(new Protocol.SynchronizationMessages.WorkItemDTO
                                                {
                                                    EventDate = item.EventDate,
                                                    Price = item.Price,
                                                    RepairOrderID = item.RepairOrderID,
                                                    Title = item.Title,
                                                    UserID = item.UserID,
                                                    WorkItemID = item.WorkItemID
                                                });
                     } //foreach

                     var deviceItems = RemontinkaServer.Instance.DataStore.GetDeviceItems(repairOrderId);

                     foreach (var item in deviceItems)
                     {
                         orderDto.DeviceItems.Add(new Protocol.SynchronizationMessages.DeviceItemDTO
                         {
                             EventDate = item.EventDate,
                             Price = item.Price,
                             RepairOrderID = item.RepairOrderID,
                             Title = item.Title,
                             UserID = item.UserID,
                             CostPrice = item.CostPrice,
                             Count = item.Count,
                             DeviceItemID = item.DeviceItemID,
                             WarehouseItemID = item.WarehouseItemID
                         });
                     } //foreach

                     var orderLines = RemontinkaServer.Instance.DataStore.GetOrderTimelines(repairOrderId);
                     foreach (var item in orderLines)
                     {
                         orderDto.OrderTimelines.Add(new OrderTimelineDTO
                         {
                             
                             RepairOrderID = item.RepairOrderID,
                             Title = item.Title,
                             EventDateTime = item.EventDateTime,
                             OrderTimelineID = item.OrderTimelineID,
                             TimelineKindID = item.TimelineKindID
                         });
                         
                     } //foreach

                 } //if
             } //foreach

            return serializer.Serialize(response);
        }

        #endregion GetRepairOrdersRequest

        #region SaveRepairOrderRequest

         /// <summary>
        /// Обработчик запроса на сохранение заказа.
        /// </summary>
        /// <param name="serializer">Сериализатор протокола.</param>
        /// <param name="postData">Данные запроса.</param>
        /// <param name="hostAddress">Адрес клиента.</param>
        /// <returns>Результат.</returns>
        private string ProcessSaveRepairOrderRequest(ProtocolSerializer serializer, string postData, string hostAddress)
        {
            var request = serializer.DeserializeSaveRepairOrderRequest(postData);
            _logger.InfoFormat(
                "Получен запрос сохранение заказа от пользователя {0} с клиента {1}",
                request.UserID, hostAddress);

            var userDomainID = CheckSignedMessage(request);

            var response = new SaveRepairOrderResponse();

             Guid? orderId = null;
             if (request.RepairOrder!=null)
             {
                 orderId = request.RepairOrder.RepairOrderID;
             } //if

             var savedOrder = RemontinkaServer.Instance.DataStore.GetRepairOrderLight(orderId, userDomainID);

             var user = RemontinkaServer.Instance.DataStore.GetUser(request.UserID, userDomainID);
             if (savedOrder!=null)
             {
                 if (!RemontinkaServer.Instance.EntitiesFacade.UserHasAccessToRepairOrder(user.UserID, savedOrder,
                                                                                     user.ProjectRoleID))
                 {
                     _logger.InfoFormat("Пользователь {0} не имеет доступа к заказу {1} отменяем синхронизацию", user.LoginName, savedOrder.RepairOrderID);
                     response.Success = false;
                     orderId = null;
                 }

             } //if

             if (orderId != null)
             {
                 Guid? managerID = null;
                 Guid? engineerID = null;

                 if (request.RepairOrder.ManagerID!=null)
                 {
                     managerID = request.RepairOrder.ManagerID;
                     if (RemontinkaServer.Instance.DataStore.GetUser(managerID, userDomainID)==null)
                     {
                         managerID = user.UserID;
                     }
                 }

                 if (request.RepairOrder.EngineerID != null)
                 {
                     engineerID = request.RepairOrder.EngineerID;
                     if (RemontinkaServer.Instance.DataStore.GetUser(engineerID, userDomainID) == null)
                     {
                         engineerID = user.UserID;
                     }
                 }

                 RemontinkaServer.Instance.DataStore.SaveRepairOrder(new RepairOrder
                                                                     {
                                                                         BranchID = request.RepairOrder.BranchID,
                                                                         CallEventDate = request.RepairOrder.CallEventDate,
                                                                         ClientAddress = request.RepairOrder.ClientAddress,
                                                                         ClientEmail = request.RepairOrder.ClientEmail,
                                                                         EventDate = request.RepairOrder.EventDate,
                                                                         Number = request.RepairOrder.Number,
                                                                         RepairOrderID = request.RepairOrder.RepairOrderID,
                                                                         OrderKindID = request.RepairOrder.OrderKindID,
                                                                         OrderStatusID = request.RepairOrder.OrderStatusID,
                                                                         DeviceSN = request.RepairOrder.DeviceSN,
                                                                         DeviceTitle = request.RepairOrder.DeviceTitle,
                                                                         ClientFullName = request.RepairOrder.ClientFullName,
                                                                         ClientPhone = request.RepairOrder.ClientPhone,
                                                                         DateOfBeReady = request.RepairOrder.DateOfBeReady,
                                                                         Defect = request.RepairOrder.Defect,
                                                                         DeviceAppearance = request.RepairOrder.DeviceAppearance,
                                                                         DeviceModel = request.RepairOrder.DeviceModel,
                                                                         DeviceTrademark = request.RepairOrder.DeviceTrademark,
                                                                         EngineerID = engineerID,
                                                                         GuidePrice = request.RepairOrder.GuidePrice,
                                                                         IsUrgent = request.RepairOrder.IsUrgent,
                                                                         IssueDate = request.RepairOrder.IssueDate,
                                                                         IssuerID = request.RepairOrder.IssuerID,
                                                                         ManagerID = managerID,
                                                                         Notes = request.RepairOrder.Notes,
                                                                         Options = request.RepairOrder.Options,
                                                                         PrePayment = request.RepairOrder.PrePayment,
                                                                         Recommendation = request.RepairOrder.Recommendation,
                                                                         WarrantyTo = request.RepairOrder.WarrantyTo,
                                                                         UserDomainID = userDomainID
                                                                     });

                 foreach (var workItemDTO in request.RepairOrder.WorkItems)
                 {
                    //TODO сохраняем заметки, т.к. на клиенте пока не реализованы.
                     var storedItem = RemontinkaServer.Instance.DataStore.GetWorkItem(workItemDTO.WorkItemID);
                     var notes = string.Empty;

                     if (storedItem != null && (!string.IsNullOrWhiteSpace(storedItem.Notes)))
                     {
                         notes = storedItem.Notes;
                     }

                     RemontinkaServer.Instance.DataStore.SaveWorkItem(new WorkItem
                                                                      {
                                                                          EventDate = workItemDTO.EventDate,
                                                                          Price = workItemDTO.Price,
                                                                          RepairOrderID = request.RepairOrder.RepairOrderID,
                                                                          Title = workItemDTO.Title,
                                                                          UserID = workItemDTO.UserID,
                                                                          WorkItemID = workItemDTO.WorkItemID,
                                                                          Notes = notes
                     });
                 } //foreach

                 foreach (var deviceItemDTO in request.RepairOrder.DeviceItems)
                 {
                     RemontinkaServer.Instance.DataStore.SaveDeviceItem(new DeviceItem
                     {
                         EventDate = deviceItemDTO.EventDate,
                         Price = deviceItemDTO.Price,
                         RepairOrderID = request.RepairOrder.RepairOrderID,
                         Title = deviceItemDTO.Title,
                         UserID = deviceItemDTO.UserID,
                         CostPrice = deviceItemDTO.CostPrice,
                         Count = deviceItemDTO.Count,
                         DeviceItemID = deviceItemDTO.DeviceItemID,
                         WarehouseItemID = deviceItemDTO.WarehouseItemID
                     });
                 } //foreach

                 foreach (var orderTimelineDTO in request.RepairOrder.OrderTimelines)
                 {
                     if (!RemontinkaServer.Instance.DataStore.OrderTimeLineExists(orderTimelineDTO.OrderTimelineID))
                     {
                         RemontinkaServer.Instance.DataStore.SaveOrderTimeline(new OrderTimeline
                                                                               {
                                                                                   EventDateTime = orderTimelineDTO.EventDateTime,
                                                                                   OrderTimelineID = orderTimelineDTO.OrderTimelineID,
                                                                                   RepairOrderID = request.RepairOrder.RepairOrderID,
                                                                                   TimelineKindID = orderTimelineDTO.TimelineKindID,
                                                                                   Title = orderTimelineDTO.Title
                                                                               });
                     } //if
                 } //foreach

                 response.Success = true;
             } //if
             else
             {
                 _logger.InfoFormat("Заказ для синхронизации не найден {0}",request.UserID);
                 response.Success = false;
             } //else

            return serializer.Serialize(response);
        }

        #endregion SaveRepairOrderRequest

        #region GetCustomReportItemRequest

        /// <summary>
        /// Обработчик запроса на получение пользовательских отчетов.
        /// </summary>
        /// <param name="serializer">Сериализатор протокола.</param>
        /// <param name="postData">Данные запроса.</param>
        /// <param name="hostAddress">Адрес клиента.</param>
        /// <returns>Результат.</returns>
        private string ProcessGetCustomReportItemRequest(ProtocolSerializer serializer, string postData, string hostAddress)
        {
            var request = serializer.DeserializeGetCustomReportItemRequest(postData);
            _logger.InfoFormat(
                "Получен запрос на получение информации по пользовательским отчетам от пользователя {0} с клиента {1}",
                request.UserID, hostAddress);

            var userDomainID = CheckSignedMessage(request);

            var items = RemontinkaServer.Instance.DataStore.GetCustomReportItems(userDomainID);
            var response = new GetCustomReportItemResponse();
            foreach (var reportItem in items)
            {
                response.CustomReportItems.Add(new CustomReportItemDTO
                {
                    CustomReportID = reportItem.CustomReportID,
                    HtmlContent = reportItem.HtmlContent,
                    Title = reportItem.Title,
                    DocumentKindID = reportItem.DocumentKindID
                });
            } //foreach

            return serializer.Serialize(response);
        }

        #endregion GetCustomReportItemRequest
    }
}
