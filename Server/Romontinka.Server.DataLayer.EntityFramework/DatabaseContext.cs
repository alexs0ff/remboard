using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Romontinka.Server.DataLayer.Entities;

namespace Romontinka.Server.DataLayer.EntityFramework
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
            Branches = CreateObjectSet<Branch>();
            Users = CreateObjectSet<User>();
            UserBranchMapItems = CreateObjectSet<UserBranchMapItem>();
            OrderKinds = CreateObjectSet<OrderKind>();
            OrderStatuses = CreateObjectSet<OrderStatus>();
            RepairOrders = CreateObjectSet<RepairOrder>();
            WorkItems = CreateObjectSet<WorkItem>();
            DeviceItems = CreateObjectSet<DeviceItem>();
            OrderTimelines = CreateObjectSet<OrderTimeline>();
            OrderCapacityItems = CreateObjectSet<OrderCapacityItem>();
            CustomReportItems = CreateObjectSet<CustomReportItem>();
            UserDomains = CreateObjectSet<UserDomain>();
            FinancialGroupItems = CreateObjectSet<FinancialGroupItem>();
            FinancialItems = CreateObjectSet<FinancialItem>();
            FinancialItemValues = CreateObjectSet<FinancialItemValue>();
            FinancialGroupBranchMapItems = CreateObjectSet<FinancialGroupBranchMapItem>();
            ItemCategories = CreateObjectSet<ItemCategory>();
            Warehouses = CreateObjectSet<Warehouse>();
            GoodsItems = CreateObjectSet<GoodsItem>();
            WarehouseItems = CreateObjectSet<WarehouseItem>();
            Contractors = CreateObjectSet<Contractor>();
            IncomingDocs = CreateObjectSet<IncomingDoc>();
            IncomingDocItems = CreateObjectSet<IncomingDocItem>();
            CancellationDocs = CreateObjectSet<CancellationDoc>();
            CancellationDocItems = CreateObjectSet<CancellationDocItem>();
            TransferDocs = CreateObjectSet<TransferDoc>();
            TransferDocItems = CreateObjectSet<TransferDocItem>();
            ProcessedWarehouseDocs = CreateObjectSet<ProcessedWarehouseDoc>();
            DimensionKinds = CreateObjectSet<DimensionKind>();
            FinancialGroupWarehouseMapItems = CreateObjectSet<FinancialGroupWarehouseMapItem>();
            RecoveryLoginItems = CreateObjectSet<RecoveryLoginItem>();
            UserPublicKeys = CreateObjectSet<UserPublicKey>();
            UserPublicKeyRequests = CreateObjectSet<UserPublicKeyRequest>();
            AutocompleteItems = CreateObjectSet<AutocompleteItem>();
            UserInterests = CreateObjectSet<UserInterest>();
            UserGridStates = CreateObjectSet<UserGridState>();
            UserGridFilters = CreateObjectSet<UserGridFilter>();
            UserSettingsItems = CreateObjectSet<UserSettingsItem>();
            UserDomainSettingsItems = CreateObjectSet<UserDomainSettingsItem>();
        }

        /// <summary>
        ///   Получает список всех пользователей.
        /// </summary>
        public ObjectSet<User> Users { get; private set; }

        /// <summary>
        /// Получает список всех филиалов. 
        /// </summary>
        public ObjectSet<Branch> Branches { get; private set; }

        public ObjectSet<CustomReportItem> CustomReportItems { get; private set; }

        public ObjectSet<DeviceItem> DeviceItems { get; private set; }

        public ObjectSet<WorkItem> WorkItems { get; private set; }

        public ObjectSet<OrderKind> OrderKinds { get; private set; }

        public ObjectSet<OrderStatus> OrderStatuses { get; private set; }

        public ObjectSet<OrderTimeline> OrderTimelines { get; private set; }

        public ObjectSet<RepairOrder> RepairOrders { get; private set; }

        public ObjectSet<UserBranchMapItem> UserBranchMapItems { get; private set; }

        public ObjectSet<OrderCapacityItem> OrderCapacityItems { get; private set; }

        public ObjectSet<UserDomain> UserDomains { get; private set; }

        public ObjectSet<FinancialGroupItem> FinancialGroupItems { get; private set; }

        public ObjectSet<FinancialItem> FinancialItems { get; private set; }

        public ObjectSet<FinancialItemValue> FinancialItemValues { get; private set; }

        public ObjectSet<FinancialGroupBranchMapItem> FinancialGroupBranchMapItems { get; private set; }

        public ObjectSet<ItemCategory> ItemCategories { get; private set; }

        public ObjectSet<Warehouse> Warehouses { get; private set; }

        public ObjectSet<GoodsItem> GoodsItems { get; private set; }

        public ObjectSet<WarehouseItem> WarehouseItems { get; private set; }

        public ObjectSet<Contractor> Contractors { get; private set; }

        public ObjectSet<IncomingDoc> IncomingDocs { get; private set; }

        public ObjectSet<IncomingDocItem> IncomingDocItems { get; private set; }

        public ObjectSet<CancellationDoc> CancellationDocs { get; private set; }

        public ObjectSet<CancellationDocItem> CancellationDocItems { get; private set; }

        public ObjectSet<TransferDoc> TransferDocs { get; private set; }

        public ObjectSet<TransferDocItem> TransferDocItems { get; private set; }

        public ObjectSet<ProcessedWarehouseDoc> ProcessedWarehouseDocs { get; private set; }

        public ObjectSet<DimensionKind> DimensionKinds { get; private set; }

        public ObjectSet<FinancialGroupWarehouseMapItem> FinancialGroupWarehouseMapItems { get; private set; }

        public ObjectSet<RecoveryLoginItem> RecoveryLoginItems { get; private set; }

        public ObjectSet<UserPublicKey> UserPublicKeys { get; private set; }

        public ObjectSet<UserPublicKeyRequest> UserPublicKeyRequests { get; private set; }

        public ObjectSet<AutocompleteItem> AutocompleteItems { get; private set; }

        public ObjectSet<UserInterest> UserInterests { get; private set; }

        public ObjectSet<UserGridState> UserGridStates { get; private set; }

        public ObjectSet<UserGridFilter> UserGridFilters { get; private set; }

        public ObjectSet<UserSettingsItem> UserSettingsItems { get; private set; }

        public ObjectSet<UserDomainSettingsItem> UserDomainSettingsItems { get; private set; }
    }
}
