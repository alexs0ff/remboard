using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.RepairOrderForm
{
    /// <summary>
    /// Набор пунктов фильтрации поиска.
    /// </summary>
    public static class OrderSearchSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        static OrderSearchSet()
        {
            All = new OrderSearchItem { Key = 1,Title = "Все"};
            CurrentUser = new OrderSearchItem { Key = 2, Title = "Текущий пользователь" };
            SpecificUser = new OrderSearchItem { Key = 3, Title = "Определенный пользователь" };
            OnlyUrgents = new OrderSearchItem { Key = 4, Title = "Срочные" };
        }

        /// <summary>
        /// Все заказы.
        /// </summary>
        public static OrderSearchItem All { get;private set; }

        /// <summary>
        /// Только текущего пользователя.
        /// </summary>
        public static OrderSearchItem CurrentUser { get; private set; }

        /// <summary>
        /// Определенного пользователя.
        /// </summary>
        public static OrderSearchItem SpecificUser { get; private set; }

        /// <summary>
        /// Только срочные.
        /// </summary>
        public static OrderSearchItem OnlyUrgents { get; private set; }
    }
}