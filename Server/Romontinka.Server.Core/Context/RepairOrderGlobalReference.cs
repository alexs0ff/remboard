using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Core.Context
{
    /// <summary>
    /// Глобальный код для заказа.
    /// </summary>
    public class RepairOrderGlobalReference
    {
        /// <summary>
        /// Задает или получает код домена пользователя.
        /// </summary>
        public int UserDomainNumber { get; set; }

        /// <summary>
        /// Задает или получает код заказа.
        /// </summary>
        public string RepairOrderNumber{ get; set; }
    }
}
