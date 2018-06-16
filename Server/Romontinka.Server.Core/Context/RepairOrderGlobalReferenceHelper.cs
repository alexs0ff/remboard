using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Core.Context
{
    /// <summary>
    /// Хелпер для обработки глобальных номеров заказа.
    /// </summary>
    public static class RepairOrderGlobalReferenceHelper
    {
        /// <summary>
        /// Переводит номер клиента в старую ссылку.
        /// </summary>
        /// <param name="orderNumber">Номер клиента.</param>
        /// <param name="userDomainName">Номер домена.</param>
        /// <returns>Глобальная ссылка.</returns>
        public static string MakeGlobalReference(string orderNumber, int userDomainName)
        {
            if (!string.IsNullOrWhiteSpace(orderNumber))
            {
                return string.Format("{0}-{1}", userDomainName.ToString(CultureInfo.InvariantCulture), orderNumber);
            }

            return string.Empty;
        }

        /// <summary>
        /// Разбирает глобальный код заказа.
        /// </summary>
        /// <param name="reference">Глобальный код заказа.</param>
        /// <returns>Заказ.</returns>
        public static RepairOrderGlobalReference ParseGlobalReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                return null;
            }

            var parts = reference.Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                return null;
            }

            return new RepairOrderGlobalReference {RepairOrderNumber = parts[1],UserDomainNumber = int.Parse(parts[0],CultureInfo.InvariantCulture)};
        }
    }
}
