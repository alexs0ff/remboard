using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Core.Models
{
    /// <summary>
    /// Тип пункта синхронизации.
    /// </summary>
    public enum SyncItemModelKind
    {
        /// <summary>
        /// Обновление пользователей.
        /// </summary>
        GetUsers,

        /// <summary>
        /// Обновление филиалов и связей.
        /// </summary>
        GetUserBranches,

        /// <summary>
        /// Обновление фингрупп и связей.
        /// </summary>
        GetFinancialGroups,

        /// <summary>
        /// Обновление складов.
        /// </summary>
        GetWarehouses,

        /// <summary>
        /// Обновление номенклатуры.
        /// </summary>
        GetGoodsItems,

        /// <summary>
        /// Обновление остатков на складе.
        /// </summary>
        GetWarehouseItems,

        /// <summary>
        /// Обновление статусов заказа.
        /// </summary>
        GetOrderStatuses,

        /// <summary>
        /// Обновление заказов.
        /// </summary>
        UpdateRepairOrders,

        /// <summary>
        /// Обновление отчетов.
        /// </summary>
        GetCustomReports

    }
}
