using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.DataLayer.Entities;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Интерфейс доступа к данным.
    /// </summary>
    public interface IDataStore
    {
        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        /// <param name="projectRoleId">Код роли в проекте. </param>
        /// <returns>Список пользователей.</returns>
        IEnumerable<User> GetUsers(long? projectRoleId);

        /// <summary>
        /// Получает список пользователей с фильтром.
        /// </summary>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="userDomainID">Код домена пользователя.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список пользователей.</returns>
        IEnumerable<User> GetUsers(Guid? userDomainID, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Получение пользователя по логину и хэшу пароля.
        /// </summary>
        /// <param name="loginName">Логин пользователя.</param>
        /// <param name="passwordHash">Хэш пароля.</param>
        /// <returns>Пользователь, если найден.</returns>
        User GetUser(string loginName, string passwordHash);

        /// <summary>
        /// Обновляет хеш пароля на указанном пользователе.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <param name="newPasswordHash">Новый хэш пароля.</param>
        /// <returns>Признак успешности смены</returns>
        bool UpdatePasswordHash(Guid? userID, string newPasswordHash);

        /// <summary>
        /// Создает в системе нового пользователя.
        /// </summary>
        void CreateUser(User user);

        /// <summary>
        /// Обновление информации по существующему пользователю.
        /// Сохраняет все поля кроме хэша пароля.
        /// </summary>
        /// <param name="user">Пользователь для обновления информации.</param>
        void UpdateUser(User user);

        /// <summary>
        ///   Сохраняет информацию пользователе.
        /// </summary>
        /// <param name="user"> Сохраняемый пользователь. </param>
        void SaveUser(User user);

        /// <summary>
        /// Получает пользователя по его ID.
        /// </summary>
        /// <param name="id">Код пользователя.</param>
        /// <returns>Пользователь, если существует.</returns>
        User GetUser(Guid? id);

        /// <summary>
        /// Удаляет из хранилища пользователя по его ID.
        /// </summary>
        /// <param name="id">Код пользователя.</param>
        void DeleteUser(Guid? id);

        /// <summary>
        /// Проверяет наличие логина в системе.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <returns>Наличие логина.</returns>
        bool UserLoginExists(string login);

        /// <summary>
        ///   Сохраняет информацию ключе пользователя.
        /// </summary>
        /// <param name="userKey"> Сохраняемый ключ пользователя. </param>
        void SaveUserKey(UserKey userKey);

        /// <summary>
        /// Получает ключ пользователя по его ID.
        /// </summary>
        /// <param name="id">Код ключа пользователя.</param>
        /// <returns>Пользователь, если существует.</returns>
        UserKey GetUserKey(Guid? id);

        /// <summary>
        /// Удаляет из хранилища ключ пользователя по его ID.
        /// </summary>
        /// <param name="id">Код пользователя.</param>
        void DeleteUserKey(Guid? id);

        /// <summary>
        /// Получает первый попавшийся код домена пользователя или null.
        /// </summary>
        /// <returns>Код домена пользователя или null.</returns>
        Guid? GetFirstUserDomainID();

        /// <summary>
        /// Получение пользователя по логину.
        /// </summary>
        /// <param name="loginName">Логин пользователя.</param>
        /// <returns>Пользователь, если найден.</returns>
        User GetUser(string loginName);

        /// <summary>
        /// Получает текущий пользовательский ключ.
        /// </summary>
        /// <param name="userID">Код пользователя.</param>
        /// <returns>Ключи или null.</returns>
        UserKey GetCurrentUserKey(Guid? userID);

        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        /// <returns>Список пользователей.</returns>
        IEnumerable<User> GetUsers();

        /// <summary>
        /// Обновляет код пользователя, а также связанные с ним ключи.
        /// </summary>
        /// <param name="currentUserID">Текущие код пользователя.</param>
        /// <param name="newUserID">Новый код пользователя.</param>
        void UpdateUserAndKeyID(Guid? currentUserID, Guid? newUserID);

        /// <summary>
        ///   Сохраняет информацию об операции синхронизации.
        /// </summary>
        /// <param name="syncOperation"> Сохраняемая операция. </param>
        void SaveSyncOperation(SyncOperation syncOperation);

        /// <summary>
        /// Получает операцию синхронизации по ее ID.
        /// </summary>
        /// <param name="id">Код ключа пользователя.</param>
        /// <returns>Пользователь, если существует.</returns>
        SyncOperation GetSyncOperation(Guid? id);

        /// <summary>
        /// Удаляет из хранилища операцию синхронизации по ее ID.
        /// </summary>
        /// <param name="id">Код операции синзронизации.</param>
        void DeleteSyncOperation(Guid? id);

        /// <summary>
        /// Проверяет наличие успешных операций синхронизации в базе.
        /// </summary>
        /// <returns>Признак наличяия операций.</returns>
        bool SyncOperationExists();

        /// <summary>
        /// Удаляет все привязки филиалов и пользователей.
        /// </summary>
        void DeleteAllBranches();

        /// <summary>
        /// Удаляет все привязки филиалов и пользователей.
        /// </summary>
        void DeleteAllUserBranchMapItems();

        /// <summary>
        /// Получает список филиалов без фильтра.
        /// </summary>
        /// <returns>Список филиалов.</returns>
        IEnumerable<Branch> GetBranches();

        /// <summary>
        ///   Сохраняет информацию филиале.
        /// </summary>
        /// <param name="branch"> Сохраняемый филиал. </param>
        void SaveBranch(Branch branch);

        /// <summary>
        /// Получает филиал по его ID.
        /// </summary>
        /// <param name="id">Код описания филиала.</param>
        /// <returns>Филиал, если существует.</returns>
        Branch GetBranch(Guid? id);

        /// <summary>
        /// Удаляет из хранилища филиал по его ID.
        /// </summary>
        /// <param name="id">Код филиала.</param>
        void DeleteBranch(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о соответствии пользователя и филиала.
        /// </summary>
        /// <param name="userBranchMapItem"> Сохраняемое соответствие. </param>
        void SaveUserBranchMapItem(UserBranchMapItem userBranchMapItem);

        /// <summary>
        /// Получает соответствие пользователя и филиала по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        /// <returns>Соостветствие, если существует.</returns>
        UserBranchMapItem GetUserBranchMapItem(Guid? id);

        /// <summary>
        /// Удаляет из хранилища соответствие между пользователем и филиалом по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        void DeleteUserBranchMapItem(Guid? id);

        /// <summary>
        /// Получает список финансовая групп без фильтра.
        /// </summary>
        /// <returns>Список финансовых групп.</returns>
        IEnumerable<FinancialGroupItem> GetFinancialGroupItems();

        /// <summary>
        ///   Сохраняет информацию финансовой группе.
        /// </summary>
        /// <param name="financialGroupItem"> Сохраняемая финансовая группа. </param>
        void SaveFinancialGroupItem(FinancialGroupItem financialGroupItem);

        /// <summary>
        /// Получает финансовую группу по его ID.
        /// </summary>
        /// <param name="id">Код описания финансовой группы.</param>
        /// <returns>Финансовая группа, если существует.</returns>
        FinancialGroupItem GetFinancialGroupItem(Guid? id);

        /// <summary>
        /// Удаляет из хранилища финансовую группу по его ID.
        /// </summary>
        /// <param name="id">Код финансовой группы.</param>
        void DeleteFinancialGroupItem(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о соответствии финансовой группы и филиала.
        /// </summary>
        /// <param name="financialGroupBranchMapItem"> Сохраняемое соответствие. </param>
        void SaveFinancialGroupMapBranchItem(FinancialGroupBranchMapItem financialGroupBranchMapItem);

        /// <summary>
        /// Удаляет из хранилища соответствие между финансовой группой и филиалом по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        void DeleteFinancialGroupBranchMapItem(Guid? id);

        /// <summary>
        /// Удаляет все привязки филиалов и пользователей.
        /// </summary>
        void DeleteAllFinancialGroupBranchMapItems();

        /// <summary>
        /// Удаляет всех филиалов.
        /// </summary>
        void DeleteAllFinancialGroupItems();

        /// <summary>
        /// Получает соответствия финансовой группы и филиала по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        /// <returns>Соостветствие, если существует.</returns>
        FinancialGroupBranchMapItem GetFinancialGroupMapBranchItem(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о складе.
        /// </summary>
        /// <param name="warehouse"> Сохраняемый склад. </param>
        void SaveWarehouse(Warehouse warehouse);

        /// <summary>
        /// Получает склад по его ID.
        /// </summary>
        /// <param name="id">Код склада.</param>
        /// <returns>Склад товара, если существует.</returns>
        Warehouse GetWarehouse(Guid? id);

        /// <summary>
        /// Удаляет из хранилища склад по его ID.
        /// </summary>
        /// <param name="id">Код склада товара.</param>
        void DeleteWarehouse(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о соответствии финансовой группы и склада.
        /// </summary>
        /// <param name="financialGroupWarehouseMapItem"> Сохраняемое соответствие. </param>
        void SaveFinancialGroupWarehouseItem(FinancialGroupWarehouseMapItem financialGroupWarehouseMapItem);

        /// <summary>
        /// Получает соответствия финансовой группы и склада по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        /// <returns>Соостветствие, если существует.</returns>
        FinancialGroupWarehouseMapItem GetFinancialGroupMapWarehouseItem(Guid? id);

        /// <summary>
        /// Удаляет из хранилища соответствие между финансовой группой и складом по его ID.
        /// </summary>
        /// <param name="id">Код соответствия.</param>
        void DeleteFinancialGroupWarehouseMapItem(Guid? id);

        /// <summary>
        /// Удаляет все привязки фингрупп и складов.
        /// </summary>
        void DeleteAllFinancialGroupMapWarehouseItems();

        /// <summary>
        /// Удаляет все привязки фингрупп и складов.
        /// </summary>
        void DeleteAllWarehouses();

        /// <summary>
        ///   Сохраняет информацию о кактегории товара.
        /// </summary>
        /// <param name="itemCategory"> Сохраняемая категория товара. </param>
        void SaveItemCategory(ItemCategory itemCategory);

        /// <summary>
        /// Удаляет из хранилища категорию товара по его ID.
        /// </summary>
        /// <param name="id">Код категории товара.</param>
        void DeleteItemCategory(Guid? id);

        /// <summary>
        /// Получает категорию товара по его ID.
        /// </summary>
        /// <param name="id">Код категории товара.</param>
        /// <returns>Категория товара, если существует.</returns>
        ItemCategory GetItemCategory(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о номенклатуре.
        /// </summary>
        /// <param name="goodsItem"> Сохраняемая номенклатура. </param>
        void SaveGoodsItem(GoodsItem goodsItem);

        /// <summary>
        /// Получает номенклатуру по его ID.
        /// </summary>
        /// <param name="id">Код номенклатуры.</param>
        /// <returns>Номенклатура товара, если существует.</returns>
        GoodsItemDTO GetGoodsItem(Guid? id);

        /// <summary>
        /// Удаляет из хранилища номенклатуры по его ID.
        /// </summary>
        /// <param name="id">Код номенклатуры товара.</param>
        void DeleteGoodsItem(Guid? id);

        /// <summary>
        /// Удаляет все категории товаров.
        /// </summary>
        void DeleteAllItemCategories();

        /// <summary>
        /// Удаляет все номенклотуры товаров.
        /// </summary>
        void DeleteAllGoodsItems();

        /// <summary>
        /// Удаляет все номенклотуры товаров.
        /// </summary>
        void DeleteAllWarehouseItems();

        /// <summary>
        ///   Сохраняет информацию о остатках на складе.
        /// </summary>
        ///<remarks>Проверять домен на наличие склада перед вызовом.</remarks>
        /// <param name="warehouseItem"> Сохраняемые остатки на складе. </param>
        void SaveWarehouseItem(WarehouseItem warehouseItem);

        /// <summary>
        /// Получает остаток на складе по его ID.
        /// </summary>
        /// <param name="id">Код номенклатуры.</param>
        /// <returns>Остатки на складе товара, если существует.</returns>
        WarehouseItemDTO GetWarehouseItem(Guid? id);

        /// <summary>
        /// Удаляет из хранилища остаток на складе по его ID.
        /// </summary>
        /// <param name="id">Код номенклатуры товара.</param>
        void DeleteWarehouseItem(Guid? id);

        /// <summary>
        /// Получает список статусов заказов.
        /// </summary>
        /// <returns>Список статусов заказа.</returns>
        IEnumerable<OrderStatus> GetOrderStatuses();

        /// <summary>
        ///   Сохраняет информацию о статусе заказа.
        /// </summary>
        /// <param name="orderStatus"> Сохраняемый статус заказа. </param>
        void SaveOrderStatus(OrderStatus orderStatus);

        /// <summary>
        /// Получение статусов заказа по его типам.
        /// </summary>
        /// <param name="kindId">Тип статуса.</param>
        /// <returns>Если не находит пытается найти ближайший по смыслу.</returns>
        OrderStatus GetOrderStatusByKind(long? kindId);

        /// <summary>
        /// Получает статус заказа по его ID.
        /// </summary>
        /// <param name="id">Код статуса заказа.</param>
        /// <returns>Статус заказа, если существует.</returns>
        OrderStatus GetOrderStatus(Guid? id);

        /// <summary>
        /// Удаляет из хранилища статус заказа по его ID.
        /// </summary>
        /// <param name="id">Код статуса заказа.</param>
        void DeleteOrderStatus(Guid? id);

        /// <summary>
        /// Удаляет все номенклотуры товаров.
        /// </summary>
        void DeleteAllOrderStatuses();

        /// <summary>
        /// Удаляет все номенклотуры товаров.
        /// </summary>
        void DeleteAllOrderKinds();

        /// <summary>
        /// Получает полный список типов заказа.
        /// </summary>
        /// <returns>Список типов заказа.</returns>
        IEnumerable<OrderKind> GetOrderKinds();

        /// <summary>
        ///   Сохраняет информацию о типе заказа.
        /// </summary>
        /// <param name="orderKind"> Сохраняемый тип заказа. </param>
        void SaveOrderKind(OrderKind orderKind);

        /// <summary>
        /// Получает тип заказа по его ID.
        /// </summary>
        /// <param name="id">Код типа заказа.</param>
        /// <returns>Тип заказа, если существует.</returns>
        OrderKind GetOrderKind(Guid? id);

        /// <summary>
        /// Удаляет из хранилища тип заказа по его ID.
        /// </summary>
        /// <param name="id">Код типа заказа.</param>
        void DeleteOrderKind(Guid? id);

        /// <summary>
        /// Получает заказа по его ID.
        /// </summary>
        /// <param name="id">Код заказа.</param>
        /// <returns>Заказ, если существует.</returns>
        RepairOrder GetRepairOrder(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о заказе.
        /// </summary>
        /// <param name="repairOrder"> Сохраняемый заказ. </param>
        void SaveRepairOrder(RepairOrder repairOrder);

        /// <summary>
        /// Удаляет из хранилища заказ по его ID.
        /// </summary>
        /// <param name="id">Код заказа.</param>
        void DeleteRepairOrder(Guid? id);

        /// <summary>
        /// Получает запчасти по ее ID.
        /// </summary>
        /// <param name="id">Код запчасти.</param>
        /// <returns>Заказ, если существует.</returns>
        DeviceItem GetDeviceItem(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о запчати.
        /// </summary>
        /// <param name="deviceItem"> Сохраняемая запчасть. </param>
        void SaveDeviceItem(DeviceItem deviceItem);

        /// <summary>
        /// Удаляет из хранилища запчасти по ее ID.
        /// </summary>
        /// <param name="id">Код запчасти.</param>
        void DeleteDeviceItem(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о проделанной работе.
        /// </summary>
        /// <param name="workItem"> Сохраняемая проделанная работа. </param>
        void SaveWorkItem(WorkItem workItem);

        /// <summary>
        /// Получает проделанную работу по его ID.
        /// </summary>
        /// <param name="id">Код проделанной работы.</param>
        /// <returns>Проделанная работа, если существует.</returns>
        WorkItemDTO GetWorkItem(Guid? id);

        /// <summary>
        /// Удаляет из хранилища проделанную работу по его ID.
        /// </summary>
        /// <param name="id">Код проделанной работы.</param>
        void DeleteWorkItem(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о графике заказа.
        /// </summary>
        /// <param name="orderTimeline"> Сохраняемый пункт о графике работы над заказом. </param>
        void SaveOrderTimeline(OrderTimeline orderTimeline);

        /// <summary>
        /// Получает график проделанной работы над заказом по его ID.
        /// </summary>
        /// <param name="id">Код графика заказа.</param>
        /// <returns>График заказа, если существует.</returns>
        OrderTimeline GetOrderTimeline(Guid? id);

        /// <summary>
        /// Удаляет из хранилища график заказа по его ID.
        /// </summary>
        /// <param name="id">Код графика заказа.</param>
        void DeleteOrderTimeline(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о серверном хэше заказа.
        /// </summary>
        /// <param name="repairOrderServerHashItem"> Сохраняемый пункт о серверном хэше заказа. </param>
        void SaveRepairOrderServerHashItem(RepairOrderServerHashItem repairOrderServerHashItem);

        /// <summary>
        /// Получает серверных хэ заказа по его ID.
        /// </summary>
        /// <param name="id">Код серверного хэша заказа.</param>
        /// <returns>Серверный хэш заказа, если существует.</returns>
        RepairOrderServerHashItem GetRepairOrderServerHashItem(Guid? id);

        /// <summary>
        /// Удаляет из хранилища серверный хэш заказа по его ID.
        /// </summary>
        /// <param name="id">Код серверного хэша заказа.</param>
        void DeleteRepairOrderServerHashItem(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о серверном хэше проделанной работы.
        /// </summary>
        /// <param name="workItemServerHashItem"> Сохраняемый пункт о серверном хэше проделанной работы. </param>
        void SaveWorkItemServerHashItem(WorkItemServerHashItem workItemServerHashItem);

        /// <summary>
        /// Получает серверный хэш проделенной работы по его ID.
        /// </summary>
        /// <param name="id">Код серверного хэша проделанной работы.</param>
        /// <returns>Серверный хэш проделенной работы, если существует.</returns>
        WorkItemServerHashItem GetWorkItemServerHashItem(Guid? id);

        /// <summary>
        /// Удаляет из хранилища серверный хэш проделанной работы по его ID.
        /// </summary>
        /// <param name="id">Код серверного хэша проделанной работы.</param>
        void DeleteWorkItemServerHashItem(Guid? id);

        /// <summary>
        ///   Сохраняет информацию о серверном хэше установленной запчасти.
        /// </summary>
        /// <param name="deviceItemServerHashItem"> Сохраняемый пункт о серверном хэше установленной запчасти. </param>
        void SaveDeviceItemServerHashItem(DeviceItemServerHashItem deviceItemServerHashItem);

        /// <summary>
        /// Получает серверный хэш установленной запчасти по его ID.
        /// </summary>
        /// <param name="id">Код серверного хэша установленной запчасти.</param>
        /// <returns>Серверный хэш установленной запчасти, если существует.</returns>
        DeviceItemServerHashItem GetDeviceItemServerHashItem(Guid? id);

        /// <summary>
        /// Удаляет из хранилища серверный хэш установленной запчасти по его ID.
        /// </summary>
        /// <param name="id">Код серверного хэша установленной запчасти.</param>
        void DeleteDeviceItemServerHashItem(Guid? id);

        /// <summary>
        /// Получает графики проделанной работы у заказов.
        /// </summary>
        /// <param name="repairOrderId">Код заказа.</param>
        /// <returns>График заказа, если существует.</returns>
        IEnumerable<OrderTimeline> GetOrderTimelines(Guid? repairOrderId);

        /// <summary>
        /// Получение количества грфиков закзов.
        /// </summary>
        /// <param name="repairOrderId">Код заказа.</param>
        /// <returns>Количество.</returns>
        int GetOrderTimelineCount(Guid? repairOrderId);

        /// <summary>
        /// Получает список работ ID заказа.
        /// </summary>
        /// <param name="repairOrderId">Код заказа.</param>
        /// <returns>Список проделанных работ.</returns>
        IEnumerable<WorkItem> GetWorkItems(Guid? repairOrderId);

        /// <summary>
        /// Получает список устройств по ID заказа.
        /// </summary>
        /// <param name="repairOrderId">Код заказа.</param>
        /// <returns>Список установленных устройств.</returns>
        IEnumerable<DeviceItem> GetDeviceItems(Guid? repairOrderId);

        /// <summary>
        /// Получает серверные хэши установленных запчастей по заказам.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список серверных хэшей установленных запчастей.</returns>
        IEnumerable<DeviceItemServerHashItem> GetDeviceItemServerHashItems(Guid? repairOrderID);

        /// <summary>
        /// Удаляет все номенклотуры товаров.
        /// </summary>
        void DeleteAllDeviceItemServerHashItems(Guid? repairOrderID);

        /// <summary>
        /// Удаляет все номенклотуры товаров.
        /// </summary>
        void DeleteAllWorkItemServerHashItems(Guid? repairOrderID);

        /// <summary>
        /// Получает серверные хэши выполненных работ заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Список серверных хэшей выполненных работ.</returns>
        IEnumerable<WorkItemServerHashItem> GetWorkItemServerHashItems(Guid? repairOrderID);

        /// <summary>
        /// Удаляет все графики заказа определенного заказа.
        /// </summary>
        void DeleteAllOrderTimelines(Guid? repairOrderID);

        /// <summary>
        /// Удаляет все проделанные работы определенного заказа.
        /// </summary>
        void DeleteAllWorkItems(Guid? repairOrderID);

        /// <summary>
        /// Удаляет все установленных запчастей определенного заказа.
        /// </summary>
        void DeleteAllDeviceItems(Guid? repairOrderID);

        /// <summary>
        /// Возваращает все идентификаторы новых заказов.
        /// </summary>
        /// <returns>Заказ.</returns>
        IEnumerable<Guid?> GetNewRepairOrderIds();

        /// <summary>
        /// Возвращает информацию о связах конкретного пользователя с филиалами.
        /// </summary>
        /// <param name="userId">Код пользователя.</param>
        /// <param name="userProjectRoleId">Код проектной роли пользователя.</param>
        /// <returns>Филиалы.</returns>
        IEnumerable<UserBranchMapItemDTO> GetUserBranchMapByItemsByUser(Guid? userId, byte? userProjectRoleId);

        /// <summary>
        /// Возвращает информацию о связах конкретного пользователя с филиалами.
        /// </summary>
        /// <param name="userId">Код пользователя.</param>
        /// <returns>Филиалы.</returns>
        IEnumerable<UserBranchMapItemDTO> GetUserBranchMapByItemsByUser(Guid? userId);

        /// <summary>
        /// Возвращает информацию о связах конкретного филиала с пользователями определенной роли.
        /// </summary>
        /// <param name="branchId">Код пользователя.</param>
        /// <param name="userProjectRoleId">Код роли.</param>
        /// <returns>Филиалы.</returns>
        IEnumerable<UserBranchMapItemDTO> GetUserBranchMapByItemsByBranch(Guid? branchId, long? userProjectRoleId);

        /// <summary>
        /// Возвращает список заказов по фильтром.
        /// </summary>
        /// <param name="orderStatusId">Код статуса задачи.</param>
        /// <param name="isUrgent">Признак срочности.</param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        IEnumerable<RepairOrderDTO> GetRepairOrders( Guid? orderStatusId, bool? isUrgent, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает список заказов по фильтром по филиалам которые доступны пользователям.
        /// </summary>
        /// <param name="orderStatusId">Код статуса  задачи.</param>
        /// <param name="isUrgent">Признак срочности.</param>
        /// <param name="userId">Код пользователя по которому производится поиск филиалов. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        IEnumerable<RepairOrderDTO> GetRepairOrdersUserBranch(Guid? orderStatusId, bool? isUrgent, Guid? userId, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Возвращает список заказов по фильтром по конкретным исполнителям.
        /// </summary>
        /// <param name="orderStatusId">Код статуса  задачи.</param>
        /// <param name="isUrgent">Признак срочности.</param>
        /// <param name="userId">Код пользователя по которому производится поиск задач. </param>
        /// <param name="name">Строка поиска.</param>
        /// <param name="page">Текущая страница.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="count">Общее количество элементов.</param>
        /// <returns>Список заказов.</returns>
        IEnumerable<RepairOrderDTO> GetRepairOrdersUser( Guid? orderStatusId, bool? isUrgent, Guid? userId, string name, int page, int pageSize, out int count);

        /// <summary>
        /// Вычисление суммы по установленным запчастям конеретного заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Значение суммы.</returns>
        decimal? GetDeviceItemsSum(Guid? repairOrderID);

        /// <summary>
        /// Вычисление суммы по выполненным работам конеретного заказа.
        /// </summary>
        /// <param name="repairOrderID">Код заказа.</param>
        /// <returns>Значение суммы.</returns>
        decimal? GetWorkItemsSum(Guid? repairOrderID);

        /// <summary>
        /// Получает список работ ID заказа.
        /// </summary>
        /// <param name="repairOrderId">Код заказа.</param>
        /// <returns>Список проделанных работ.</returns>
        IEnumerable<WorkItemDTO> GetWorkItemDtos(Guid? repairOrderId);

        /// <summary>
        /// Инкрементирует и возвращает следующий номер документа для АРМа.
        /// </summary>
        /// <returns>Инкрементнутый номер документа.</returns>
        int GetNextDocNumber();

        /// <summary>
        /// Получает заказа по его ID.
        /// </summary>
        /// <param name="id">Код заказа.</param>
        /// <returns>Заказ, если существует.</returns>
        RepairOrderDTO GetRepairOrderDTO(Guid? id);

        /// <summary>
        /// Проверяет наличие серверного хэша заказа.
        /// </summary>
        /// <param name="repairOrderID">Код Заказа.</param>
        /// <returns>Признак существования.</returns>
        bool RepairOrderServerHashItemExists(Guid? repairOrderID);

        /// <summary>
        /// Получает остатоков на складе по его ID.
        /// </summary>
        /// <returns>Остатки на складе товара, если существует.</returns>
        IEnumerable<WarehouseItemDTO> GetWarehouseItems();

        /// <summary>
        /// Разворачивает файл базы, если его несуществует по определенному пути.
        /// </summary>
        void DeployIfNeeded();

        /// <summary>
        /// Получает список документов по определенному типу.
        /// </summary>
        /// <param name="documentKindID">Код типа документа.</param>
        /// <returns>Список документов.</returns>
        IEnumerable<CustomReportItem> GetCustomReportItems(long? documentKindID);

        /// <summary>
        /// Получает список документов без фильтра.
        /// </summary>
        /// <returns>Список документов.</returns>
        IEnumerable<CustomReportItem> GetCustomReportItems();

        /// <summary>
        ///   Сохраняет информацию документе.
        /// </summary>
        /// <param name="customReportItem"> Сохраняемый документ. </param>
        void SaveCustomReportItem(CustomReportItem customReportItem);

        /// <summary>
        /// Получает документ по его ID.
        /// </summary>
        /// <param name="id">Код описания документа.</param>
        /// <returns>Документ, если существует.</returns>
        CustomReportItem GetCustomReportItem(Guid? id);

        /// <summary>
        /// Удаляет из хранилища документ по его ID.
        /// </summary>
        /// <param name="id">Код документа.</param>
        void DeleteCustomReportItem(Guid? id);

        /// <summary>
        /// Удаляет все привязки филиалов и пользователей.
        /// </summary>
        void DeleteAllCustomReportItems();

        /// <summary>
        /// Получает статистическую информацию по установленным запчастям определенного заказа.
        /// </summary>
        /// <param name="repairOrderID">Заказ.</param>
        /// <returns>Статистика.</returns>
        ItemsInfo GetDeviceItemsTotal(Guid? repairOrderID);

        /// <summary>
        /// Получает статистическую информацию по выполненным работам определенного заказа.
        /// </summary>
        /// <param name="repairOrderID">Заказ.</param>
        /// <returns>Статистика.</returns>
        ItemsInfo GetWorkItemsTotal(Guid? repairOrderID);
    }
}
