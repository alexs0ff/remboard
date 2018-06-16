using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Romontinka.Server.Protocol;
using Romontinka.Server.Protocol.AuthMessages;
using Romontinka.Server.Protocol.SynchronizationMessages;

namespace Remontinka.Client.Core.Services
{
    /// <summary>
    /// Реализация сервиса доступа через http к серверу.
    /// </summary>
    public sealed class WebClientService : IWebClient
    {
        #region Common 
        
        /// <summary>
        /// Содержит адрес к обработчику.
        /// </summary>
        //private const string Url = "http://185.158.153.185/protocol.pcl"; //Для боевого режима
        private const string Url = "http://remboard.ru/protocol.pcl"; //Для боевого режима
        //private const string Url = "http://localhost:36785/protocol.pcl"; //Для тестового режима

        /// <summary>
        /// Содержит кодировку протокола.
        /// </summary>
        private readonly Encoding _protocolEncoding = Encoding.UTF8;

        /// <summary>
        /// Содержит период ожидания ответов на запрос.
        /// </summary>
        private readonly TimeSpan _timeSpan = TimeSpan.FromSeconds(60);

        /// <summary>
        /// Отправляет и получает xml запросы к серверу.
        /// </summary>
        /// <param name="xml">Xml запрос.</param>
        /// <returns>Xml ответ.</returns>
        private string SendRawXml(string xml)
        {
            var webRequest = (HttpWebRequest) WebRequest.Create(Url);
            webRequest.KeepAlive = false;
            webRequest.Method = "POST";
            webRequest.Timeout = (int) _timeSpan.TotalMilliseconds;
            webRequest.ReadWriteTimeout = (int) _timeSpan.TotalMilliseconds;
            var stream = webRequest.GetRequestStream();
            var writer = new StreamWriter(stream, _protocolEncoding);
            writer.Write(xml);
            writer.Flush();
            stream.Close();

            string result = string.Empty;

            var response = (HttpWebResponse) webRequest.GetResponse();
            stream = response.GetResponseStream();
            if (stream != null)
            {
                var reader = new StreamReader(stream, _protocolEncoding);
                result = reader.ReadToEnd();
                stream.Close();
            }
            response.Close();

            return result;
        }

        /// <summary>
        /// Осуществляет отправку данных на сервер и получение ответа.
        /// </summary>
        /// <typeparam name="TResponse">Ответ.</typeparam>
        /// <param name="serializeFunct">Вызов функции сериализации..</param>
        /// <param name="deserializeFunct">Вызов функции десериализации.</param>
        /// <returns>Полученный ответ.</returns>
        private TResponse SendData<TResponse>(Func<ProtocolSerializer, string> serializeFunct, Func<string, ProtocolSerializer, TResponse> deserializeFunct)
            where TResponse : MessageBase
        {
            
            var serializer = new ProtocolSerializer(ProtocolVersion.Version10);

            var rawRequest = serializeFunct(serializer);

            var rawResponse = SendRawXml(rawRequest);

            var info = serializer.GetMessageInfoOrNull(rawResponse);

            if (info == null)
            {
                throw new Exception("Нет ответа от сервера");
            } //if

            if (info.Kind == MessageKind.ErrorResponse)
            {
                var err = serializer.DeserializeErrorResponse(rawResponse);
                throw new ResponseErrorException(err.Description);
            } //if

            if (info.Kind == MessageKind.AuthErrorResponse)
            {
                var err = serializer.DeserializeAuthErrorResponse(rawResponse);
                throw new ResponseErrorException(err.Message);
            } //if

            return deserializeFunct(rawResponse, serializer);
        }

        /// <summary>
        /// Подписывает запрос клиентским сертификатом.
        /// </summary>
        /// <param name="request">Запрос для подписи.</param>
        private void SignRequest(SignedRequestBase request)
        {
            request.Sign(new ClientMessageSignVisitor());
        }

        #endregion Common

        /// <summary>
        /// Отправляет запрос на регистрацию публичного ключа.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        public RegisterPublicKeyResponse RegisterPublicKey(RegisterPublicKeyRequest request)
        {
            return SendData(serializer => serializer.Serialize(request),
                                                       (s, serializer) =>
                                                       serializer.DeserializeRegisterPublicKeyResponse(s));
        }

        /// <summary>
        /// Отправляет запрос на получение информации по ключам.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        public ProbeKeyActivationResponse ProbeKeyActivation(ProbeKeyActivationRequest request)
        {
            return SendData(serializer => serializer.Serialize(request),
                                                       (s, serializer) =>
                                                       serializer.DeserializeProbeKeyActivationResponse(s));
        }

        /// <summary>
        /// Отправляет запрос на получение информации по пользователям домена.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        public GetDomainUsersResponse GetDomainUsers(GetDomainUsersRequest request)
        {
            SignRequest(request);
            return SendData(serializer => serializer.Serialize(request),
                                                       (s, serializer) =>
                                                       serializer.DeserializeGetDomainUsersResponse(s));
        }

        /// <summary>
        /// Отправляет запрос на получение информации по филиалам и их связей с пользователями.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        public GetUserBranchesResponse GetUserBranches(GetUserBranchesRequest request)
        {
            SignRequest(request);
            return SendData(serializer => serializer.Serialize(request),
                                                       (s, serializer) =>
                                                       serializer.DeserializeGetUserBranchesResponse(s));
        }

        /// <summary>
        /// Отправляет запрос на получение информации по фингруппам и их связей с филиалами.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        public GetFinancialGroupBranchesResponse GetFinancialGroups(GetFinancialGroupBranchesRequest request)
        {
            SignRequest(request);
            return SendData(serializer => serializer.Serialize(request),
                                                       (s, serializer) =>
                                                       serializer.DeserializeGetFinancialGroupBranchesResponse(s));
        }

        /// <summary>
        /// Отправляет запрос на получение информации по складам и их связей с фингруппами.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        public GetWarehousesResponse GetWarehouses(GetWarehousesRequest request)
        {
            SignRequest(request);
            return SendData(serializer => serializer.Serialize(request),
                                                       (s, serializer) =>
                                                       serializer.DeserializeGetWarehousesResponse(s));
        }

        /// <summary>
        /// Отправляет запрос на получение информации по номенклатурам и категориям товаров.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        public GetGoodsItemResponse GetGoodsItems(GetGoodsItemRequest request)
        {
            SignRequest(request);
            return SendData(serializer => serializer.Serialize(request),
                                                       (s, serializer) =>
                                                       serializer.DeserializeGetGoodsItemResponse(s));
        }


        /// <summary>
        /// Отправляет запрос на получение информации по остаткам на складе.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        public GetWarehouseItemsResponse GetWarehouseItems(GetWarehouseItemsRequest request)
        {
            SignRequest(request);
            return SendData(serializer => serializer.Serialize(request),
                                                       (s, serializer) =>
                                                       serializer.DeserializeGetWarehouseItemsResponse(s));
        }


        /// <summary>
        /// Отправляет запрос на получение информации по остаткам на складе.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        public GetOrderStatusesResponse GetOrderStatuses(GetOrderStatusesRequest request)
        {
            SignRequest(request);
            return SendData(serializer => serializer.Serialize(request),
                                                       (s, serializer) =>
                                                       serializer.DeserializeGetOrderStatusesResponse(s));
        }

        /// <summary>
        /// Отправляет запрос на получение серверных хэшей заказов.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        public GetServerRepairOrderHashesResponse GetServerRepairOrderHashes(GetServerRepairOrderHashesRequest request)
        {
            SignRequest(request);
            return SendData(serializer => serializer.Serialize(request),
                                                       (s, serializer) =>
                                                       serializer.DeserializeGetServerRepairOrderHashesResponse(s));
        }

        /// <summary>
        /// Отправляет запрос на получение серверных заказов.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        public GetRepairOrdersResponse GetRepairOrders(GetRepairOrdersRequest request)
        {
            SignRequest(request);
            return SendData(serializer => serializer.Serialize(request),
                                                       (s, serializer) =>
                                                       serializer.DeserializeGetRepairOrdersResponse(s));
        }

        /// <summary>
        /// Отправляет запрос на сохранение заказа на сервере.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        public SaveRepairOrderResponse SaveRepairOrder(SaveRepairOrderRequest request)
        {
            SignRequest(request);
            return SendData(serializer => serializer.Serialize(request),
                                                       (s, serializer) =>
                                                       serializer.DeserializeSaveRepairOrderResponse(s));
        }

        /// <summary>
        /// Отправляет запрос на получение информации по пользовательским отчетам.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        public GetCustomReportItemResponse GetCustomReportItems(GetCustomReportItemRequest request)
        {
            SignRequest(request);
            return SendData(serializer => serializer.Serialize(request),
                                                       (s, serializer) =>
                                                       serializer.DeserializeGetCustomReportItemResponse(s));
        }
        
    }
}
