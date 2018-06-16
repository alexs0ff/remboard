using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using Remontinka.Server.WebPortal.Services;
using Romontinka.Server.Core;
using Romontinka.Server.DataLayer.EntityFramework;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Models.AutocompleteItemForm;
using Romontinka.Server.WebSite.Models.Branch;
using Romontinka.Server.WebSite.Models.CancellationDocForm;
using Romontinka.Server.WebSite.Models.CancellationDocItemForm;
using Romontinka.Server.WebSite.Models.ContractorForm;
using Romontinka.Server.WebSite.Models.DeviceItemForm;
using Romontinka.Server.WebSite.Models.FinancialGroupForm;
using Romontinka.Server.WebSite.Models.FinancialItemForm;
using Romontinka.Server.WebSite.Models.FinancialItemValueForm;
using Romontinka.Server.WebSite.Models.GoodsItemForm;
using Romontinka.Server.WebSite.Models.IncomingDocForm;
using Romontinka.Server.WebSite.Models.IncomingDocItemForm;
using Romontinka.Server.WebSite.Models.ItemCategoryForm;
using Romontinka.Server.WebSite.Models.MyTasksForm;
using Romontinka.Server.WebSite.Models.OrderKind;
using Romontinka.Server.WebSite.Models.OrderStatus;
using Romontinka.Server.WebSite.Models.RepairOrderForm;
using Romontinka.Server.WebSite.Models.TransferDocForm;
using Romontinka.Server.WebSite.Models.TransferDocItemForm;
using Romontinka.Server.WebSite.Models.User;
using Romontinka.Server.WebSite.Models.UserInterestForm;
using Romontinka.Server.WebSite.Models.UserPublicKeyForm;
using Romontinka.Server.WebSite.Models.UserPublicKeyRequestForm;
using Romontinka.Server.WebSite.Models.WarehouseForm;
using Romontinka.Server.WebSite.Models.WarehouseItemForm;
using Romontinka.Server.WebSite.Models.WorkItemForm;
using Romontinka.Server.WebSite.Services;

namespace Romontinka.Server.WebSite
{
    /// <summary>
    /// Конфигурация контейнера типов.
    /// </summary>
    public class SiteConfiguration : ContainerConfiguration
    {
        /// <summary>
        /// Создает конфигурацию для сайта.
        /// </summary>
        /// <param name="container">Контейнер.</param>
        public SiteConfiguration(UnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Хранит unity container.
        /// </summary>
        private readonly UnityContainer _container;

        /// <summary>
        ///   Получает конфигурацию IoC контейрена.
        /// </summary>
        /// <returns> Контейнер Unity. </returns>
        public override UnityContainer GetConfiguration()
        {
            _container.RegisterType<IDataStore, RemontinkaStore>();
            _container.RegisterType<ISecurityService, SecurityService>();
            _container.RegisterType<IEntitiesFacade, EntitiesFacade>();
            _container.RegisterType<ITokenManager, TokenManager>();
            _container.RegisterType<IOrderTimelineManager, OrderTimelineManager>();
            _container.RegisterType<IHTMLReportService, HTMLReportService>();
            _container.RegisterType<IMailingService, MailingService>();
            _container.RegisterType<ISystemService, SystemService>();
            _container.RegisterType<ICryptoService, CryptoService>();

            _container.RegisterType<JGridDataAdapterBase<Guid, BranchGridItemModel, BranchCreateModel, BranchCreateModel, BranchSearchModel>, BranchDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, OrderStatusGridItemModel, OrderStatusCreateModel, OrderStatusCreateModel, OrderStatusSearchModel>, OrderStatusDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, OrderKindGridItemModel, OrderKindCreateModel, OrderKindCreateModel, OrderKindSearchModel>, OrderKindDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, UserGridItemModel, UserCreateModel, UserEditModel, UserSearchModel>, UserDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, RepairOrderGridItemModel, RepairOrderCreateModel, RepairOrderEditModel, RepairOrderSearchModel>, RepairOrderDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, WorkItemGridItemModel, WorkItemCreateModel, WorkItemCreateModel, WorkItemSearchModel>, WorkItemDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, DeviceItemGridItemModel, DeviceItemCreateModel, DeviceItemCreateModel, DeviceItemSearchModel>, DeviceItemDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, RepairOrderGridItemModel, RepairOrderCreateModel, RepairOrderEditModel, MyTasksSearchModel>, MyTasksDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, FinancialGroupGridItemModel, FinancialGroupCreateModel, FinancialGroupCreateModel, FinancialGroupSearchModel>, FinancialGroupDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, FinancialItemGridItemModel, FinancialItemCreateModel, FinancialItemCreateModel, FinancialItemSearchModel>, FinancialItemDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, FinancialItemValueGridItemModel, FinancialItemValueCreateModel, FinancialItemValueCreateModel, FinancialItemValueSearchModel>, FinancialItemValueDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, ItemCategoryGridItemModel, ItemCategoryCreateModel, ItemCategoryCreateModel, ItemCategorySearchModel>, ItemCategoryDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, GoodsItemGridItemModel, GoodsItemCreateModel, GoodsItemCreateModel, GoodsItemSearchModel>, GoodsItemDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, ContractorGridItemModel, ContractorCreateModel, ContractorCreateModel, ContractorSearchModel>, ContractorDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, WarehouseGridItemModel, WarehouseCreateModel, WarehouseCreateModel, WarehouseSearchModel>, WarehouseDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, WarehouseItemGridItemModel, WarehouseItemCreateModel, WarehouseItemEditModel, WarehouseItemSearchModel>, WarehouseItemDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, IncomingDocGridItemModel, IncomingDocCreateModel, IncomingDocCreateModel, IncomingDocSearchModel>, IncomingDocDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, IncomingDocItemGridItemModel, IncomingDocItemCreateModel, IncomingDocItemCreateModel, IncomingDocItemSearchModel>, IncomingDocItemDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, CancellationDocGridItemModel, CancellationDocCreateModel, CancellationDocCreateModel, CancellationDocSearchModel>, CancellationDocDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, CancellationDocItemGridItemModel, CancellationDocItemCreateModel, CancellationDocItemCreateModel, CancellationDocItemSearchModel>, CancellationDocItemDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, TransferDocGridItemModel, TransferDocCreateModel, TransferDocCreateModel, TransferDocSearchModel>, TransferDocDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, TransferDocItemGridItemModel, TransferDocItemCreateModel, TransferDocItemCreateModel, TransferDocItemSearchModel>, TransferDocItemDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, UserPublicKeyRequestGridItemModel, UserPublicKeyRequestCreateModel, UserPublicKeyRequestCreateModel, UserPublicKeyRequestSearchModel>, UserPublicKeyRequestDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, UserPublicKeyGridItemModel, UserPublicKeyCreateModel, UserPublicKeyCreateModel, UserPublicKeySearchModel>, UserPublicKeyDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, AutocompleteItemGridItemModel, AutocompleteItemCreateModel, AutocompleteItemCreateModel, AutocompleteItemSearchModel>, AutocompleteItemDataAdapter>();
            _container.RegisterType<JGridDataAdapterBase<Guid, UserInterestGridItemModel, UserInterestCreateModel, UserInterestCreateModel, UserInterestSearchModel>, UserInterestDataAdapter>();

            return _container;
        }
    }
}