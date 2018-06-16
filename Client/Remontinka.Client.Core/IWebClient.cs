using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romontinka.Server.Protocol.AuthMessages;
using Romontinka.Server.Protocol.SynchronizationMessages;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Веб клиент для запросов в 
    /// </summary>
    public interface IWebClient
    {
        /// <summary>
        /// Отправляет запрос на регистрацию публичного ключа.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        RegisterPublicKeyResponse RegisterPublicKey(RegisterPublicKeyRequest request);

        /// <summary>
        /// Отправляет запрос на получение информации по ключам.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        ProbeKeyActivationResponse ProbeKeyActivation(ProbeKeyActivationRequest request);

        /// <summary>
        /// Отправляет запрос на получение информации по пользователям домена.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        GetDomainUsersResponse GetDomainUsers(GetDomainUsersRequest request);

        /// <summary>
        /// Отправляет запрос на получение информации по филиалам и их связей с пользователями.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        GetUserBranchesResponse GetUserBranches(GetUserBranchesRequest request);

        /// <summary>
        /// Отправляет запрос на получение информации по фингруппам и их связей с филиалами.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        GetFinancialGroupBranchesResponse GetFinancialGroups(GetFinancialGroupBranchesRequest request);

        /// <summary>
        /// Отправляет запрос на получение информации по складам и их связей с фингруппами.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        GetWarehousesResponse GetWarehouses(GetWarehousesRequest request);

        /// <summary>
        /// Отправляет запрос на получение информации по номенклатурам и категориям товаров.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        GetGoodsItemResponse GetGoodsItems(GetGoodsItemRequest request);

        /// <summary>
        /// Отправляет запрос на получение информации по остаткам на складе.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        GetWarehouseItemsResponse GetWarehouseItems(GetWarehouseItemsRequest request);

        /// <summary>
        /// Отправляет запрос на получение информации по остаткам на складе.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        GetOrderStatusesResponse GetOrderStatuses(GetOrderStatusesRequest request);

        /// <summary>
        /// Отправляет запрос на получение серверных хэшей заказов.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        GetServerRepairOrderHashesResponse GetServerRepairOrderHashes(GetServerRepairOrderHashesRequest request);

        /// <summary>
        /// Отправляет запрос на получение серверных заказов.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        GetRepairOrdersResponse GetRepairOrders(GetRepairOrdersRequest request);

        /// <summary>
        /// Отправляет запрос на сохранение заказа на сервере.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        SaveRepairOrderResponse SaveRepairOrder(SaveRepairOrderRequest request);

        /// <summary>
        /// Отправляет запрос на получение информации по пользовательским отчетам.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Ответ.</returns>
        GetCustomReportItemResponse GetCustomReportItems(GetCustomReportItemRequest request);
    }
}
