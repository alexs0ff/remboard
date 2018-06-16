using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Remontinka.Client.DataLayer.Entities;

namespace Remontinka.Client.DataLayer.EntityFramework
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    public class DatabaseContext : ObjectContext
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Data.Objects.ObjectContext"/> с заданной строкой подключения и именем контейнера сущностей по умолчанию.
        /// </summary>
        /// <param name="connectionString">Строка подключения, которая, кроме того, обеспечивает доступ к метаданным.</param><exception cref="T:System.ArgumentNullException">Параметр <paramref name="connectionString"/> имеет значение null.</exception><exception cref="T:System.ArgumentException">Значение параметра <paramref name="connectionString"/> недопустимо.— или —Недопустимая рабочая область метаданных. </exception>
        public DatabaseContext(string connectionString)
            : base(connectionString)
        {
            ContextOptions.LazyLoadingEnabled = true;
            ContextOptions.ProxyCreationEnabled = false;

            Users = CreateObjectSet<User>();
            UserKeys = CreateObjectSet<UserKey>();
            SyncOperations = CreateObjectSet<SyncOperation>();
            Branches = CreateObjectSet<Branch>();
            UserBranchMapItems = CreateObjectSet<UserBranchMapItem>();
            FinancialGroupItems = CreateObjectSet<FinancialGroupItem>();
            FinancialGroupBranchMapItems = CreateObjectSet<FinancialGroupBranchMapItem>();
            FinancialGroupWarehouseMapItems = CreateObjectSet<FinancialGroupWarehouseMapItem>();
            Warehouses = CreateObjectSet<Warehouse>();
            ItemCategories = CreateObjectSet<ItemCategory>();
            GoodsItems = CreateObjectSet<GoodsItem>();
            WarehouseItems = CreateObjectSet<WarehouseItem>();
            OrderStatuses = CreateObjectSet<OrderStatus>();
            OrderKinds = CreateObjectSet<OrderKind>();
            RepairOrders = CreateObjectSet<RepairOrder>();
            DeviceItems = CreateObjectSet<DeviceItem>();
            WorkItems = CreateObjectSet<WorkItem>();
            OrderTimelines = CreateObjectSet<OrderTimeline>();
            RepairOrderServerHashItems = CreateObjectSet<RepairOrderServerHashItem>();
            WorkItemServerHashItems = CreateObjectSet<WorkItemServerHashItem>();
            DeviceItemServerHashItems = CreateObjectSet<DeviceItemServerHashItem>();
            CustomReportItems = CreateObjectSet<CustomReportItem>();
        }

        /// <summary>
        ///   Получает список всех пользователей.
        /// </summary>
        public ObjectSet<User> Users { get; private set; }

        /// <summary>
        /// Получает список всех пользовательских ключей.
        /// </summary>
        public ObjectSet<UserKey> UserKeys { get; private set; }

        /// <summary>
        /// Получает список всех операций синхронизации.
        /// </summary>
        public ObjectSet<SyncOperation> SyncOperations { get; private set; }

        /// <summary>
        /// Получает список всех филиалов.
        /// </summary>
        public ObjectSet<Branch> Branches { get; private set; }

        /// <summary>
        /// Получает список всех соответсвий пользователей.
        /// </summary>
        public ObjectSet<UserBranchMapItem> UserBranchMapItems { get; private set; }

        /// <summary>
        /// Получает список всех финансовых групп филиалов.
        /// </summary>
        public ObjectSet<FinancialGroupItem> FinancialGroupItems { get; private set; }

        /// <summary>
        /// Получает список соответствий филилов и финансовых групп.
        /// </summary>
        public ObjectSet<FinancialGroupBranchMapItem> FinancialGroupBranchMapItems { get; private set; }

        /// <summary>
        /// Получает список соответствий фингрупп и складов.
        /// </summary>
        public ObjectSet<FinancialGroupWarehouseMapItem> FinancialGroupWarehouseMapItems { get; private set; }

        /// <summary>
        /// Получает список складов.
        /// </summary>
        public ObjectSet<Warehouse> Warehouses { get; private set; }

        /// <summary>
        /// Получает список категорий товаров.
        /// </summary>
        public ObjectSet<ItemCategory> ItemCategories { get; private set; }

        /// <summary>
        /// Получает список номенклатур товаров.
        /// </summary>
        public ObjectSet<GoodsItem> GoodsItems { get; private set; }

        /// <summary>
        /// Получает список остатков на складе.
        /// </summary>
        public ObjectSet<WarehouseItem> WarehouseItems { get; private set; }

        /// <summary>
        /// Получает список статусов заказа.
        /// </summary>
        public ObjectSet<OrderStatus> OrderStatuses { get; private set; }

        /// <summary>
        /// Получает список типов заказа.
        /// </summary>
        public ObjectSet<OrderKind> OrderKinds { get; private set; }

        /// <summary>
        /// Получает список заказов.
        /// </summary>
        public ObjectSet<RepairOrder> RepairOrders { get; private set; }

        /// <summary>
        /// Получает список запчастей.
        /// </summary>
        public ObjectSet<DeviceItem> DeviceItems { get; private set; }

        /// <summary>
        /// Получает список проделанных работ.
        /// </summary>
        public ObjectSet<WorkItem> WorkItems { get; private set; }

        /// <summary>
        /// Получает список пунктов графика заказов.
        /// </summary>
        public ObjectSet<OrderTimeline> OrderTimelines { get; private set; }

        /// <summary>
        /// Получает список серверных хэшей заказов.
        /// </summary>
        public ObjectSet<RepairOrderServerHashItem> RepairOrderServerHashItems { get; private set; }

        /// <summary>
        /// Получает список серверных хэшей проделанных работ.
        /// </summary>
        public ObjectSet<WorkItemServerHashItem> WorkItemServerHashItems { get; private set; }

        /// <summary>
        /// Получает список серверных хэшей установленных запчастей.
        /// </summary>
        public ObjectSet<DeviceItemServerHashItem> DeviceItemServerHashItems { get; private set; }

        /// <summary>
        /// Получает список документов пользователей.
        /// </summary>
        public ObjectSet<CustomReportItem> CustomReportItems { get; private set; }
    }
}
