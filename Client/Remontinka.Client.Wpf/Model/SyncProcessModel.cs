using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Remontinka.Client.Core.Interception;
using Remontinka.Client.Core.Models;

namespace Remontinka.Client.Wpf.Model
{
    /// <summary>
    /// Модель синхронизации.
    /// </summary>
    public class SyncProcessModel : BindableModelObject
    {
        public SyncProcessModel()
        {
            Items = new Collection<SyncItem>();
        }

        #region Items

        /// <summary>
        /// Задает или получает пункт синхронизации пользователей.
        /// </summary>
        [Dependency]
        public SyncItem GetUsers { get; set; }

        /// <summary>
        /// Задает или получает пункт синхронизации филиалов и связей с пользователями.
        /// </summary>
        [Dependency]
        public SyncItem GetUserBranches { get; set; }

        /// <summary>
        /// Задает или получает пункт синхронизации фингрупп с филиалами.
        /// </summary>
        [Dependency]
        public SyncItem GetFinancialGroups { get; set; }

        /// <summary>
        /// Задает или получает пункт синхронизации складов с фингруппами.
        /// </summary>
        [Dependency]
        public SyncItem GetWarehouses { get; set; }

        /// <summary>
        /// Задает или получает пункт синхронизации номенклатуры с категориями товаров.
        /// </summary>
        [Dependency]
        public SyncItem GetGoodsItems { get; set; }

        /// <summary>
        /// Задает или получает пункт синхронизации остатков на складе.
        /// </summary>
        [Dependency]
        public SyncItem GetWarehouseItems { get; set; }

        /// <summary>
        /// Задает или получает пункт синхронизации статусов и типов заказов.
        /// </summary>
        [Dependency]
        public SyncItem GetOrderStatuses { get; set; }

        /// <summary>
        /// Задает или получает пункт синхронизации заказов.
        /// </summary>
        [Dependency]
        public SyncItem UpdateRepairOrders { get; set; }

        /// <summary>
        /// Задает или получает пункт синхронизации отчетов пользователя.
        /// </summary>
        [Dependency]
        public SyncItem GetCustomReportItems { get; set; }

        #endregion Items

        /// <summary>
        /// Получает коллекцию пунктов синхронизации.
        /// </summary>
        public ICollection<SyncItem> Items { get; private set; }

        /// <summary>
        /// Задает или получает текст с информацией.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string ErrorText { get; set; }

        /// <summary>
        /// Задает или получает строку информации.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string InfoText { get; set; }

        /// <summary>
        /// Задает или получает признак блокировки кнопок.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual bool IsEnabaled { get; set; }
    }
}
