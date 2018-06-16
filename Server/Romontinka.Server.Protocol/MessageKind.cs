using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol
{
    /// <summary>
    /// Тип сообщений.
    /// </summary>
    public enum MessageKind
    {
        /// <summary>
        /// Ответ с ошибкой авторизации.
        /// </summary>
        AuthErrorResponse = 10,

        /// <summary>
        /// Ответ с ошибкой.
        /// </summary>
        ErrorResponse = 12,

        /// <summary>
        /// Запрос на регистрацию публичного ключа.
        /// </summary>
        RegisterPublicKeyRequest = 104,

        /// <summary>
        /// Ответ н регистрацию публичного ключа.
        /// </summary>
        RegisterPublicKeyResponse = 105,

        /// <summary>
        /// Запрос на проверку активации ключей.
        /// </summary>
        ProbeKeyActivationRequest = 106,

        /// <summary>
        /// Ответ проверки активации ключей.
        /// </summary>
        ProbeKeyActivationResponse = 107,

        /// <summary>
        /// Запрос на получение пользователей.
        /// </summary>
        GetDomainUsersRequest = 108,

        /// <summary>
        /// Ответ на запрос получения пользователей домена.
        /// </summary>
        GetDomainUsersResponse = 109,

        /// <summary>
        /// Запрос на получение филиалов и их связей с пользователями.
        /// </summary>
        GetUserBranchesRequest = 110,

        /// <summary>
        /// Ответ на запрос о получении филиалов и их связей с пользователями
        /// </summary>
        GetUserBranchesResponse = 111,

        /// <summary>
        /// Запрос на получение финансовых групп и их связей с филиалами.
        /// </summary>
        GetFinancialGroupBranchesRequest = 112,

        /// <summary>
        /// Ответ на запрос о получении финансовых групп и их связей с филиалами.
        /// </summary>
        GetFinancialGroupBranchesResponse = 113,
        
        /// <summary>
        /// Запрос на получение информации по складам и их связям с фингруппами.
        /// </summary>
        GetWarehousesRequest = 114,

        /// <summary>
        /// Ответ на запроса на получение информации по складам и их связям с фингруппами.
        /// </summary>
        GetWarehousesResponse = 115,

        /// <summary>
        /// Запрос на получение информации о номенклатуре и категориях товаров.
        /// </summary>
        GetGoodsItemRequest = 116,

        /// <summary>
        /// Ответ на запрос на получение информации о номенклатуре и категориях товаров.
        /// </summary>
        GetGoodsItemResponse = 117,
        
        /// <summary>
        /// Запрос на получение информации об остатках на складах.
        /// </summary>
        GetWarehouseItemsRequest = 118,

        /// <summary>
        /// Ответ на запрос на получение информации об остатках на складах.
        /// </summary>
        GetWarehouseItemsResponse = 119,

        /// <summary>
        /// Запрос на получение информации статусам заказов.
        /// </summary>
        GetOrderStatusesRequest = 120,

        /// <summary>
        /// Ответ на запрос о получении статусов заказов.
        /// </summary>
        GetOrderStatusesResponse = 121,
        
        /// <summary>
        /// Запрос на получение хэшей заказов с сервера.
        /// </summary>
        GetServerRepairOrderHashesRequest = 122,

        /// <summary>
        /// Ответ на запрос о получении хэшей с заказов.
        /// </summary>
        GetServerRepairOrderHashesResponse = 123,

        /// <summary>
        /// Запрос на получение заказов.
        /// </summary>
        GetRepairOrdersRequest = 125,


        /// <summary>
        /// Ответ на запрос на получение заказов.
        /// </summary>
        GetRepairOrdersResponse = 126,
        
        /// <summary>
        /// Запрос на сохранение заказа.
        /// </summary>
        SaveRepairOrderRequest = 127,

        /// <summary>
        /// Ответ на запрос о сохранении заказа.
        /// </summary>
        SaveRepairOrderResponse = 128,

        /// <summary>
        /// Запрос на получение отчетов.
        /// </summary>
        GetCustomReportItemRequest = 129,

        /// <summary>
        /// Ответ на запрос о получении отчетов.
        /// </summary>
        GetCustomReportItemResponse = 130
    }
}
