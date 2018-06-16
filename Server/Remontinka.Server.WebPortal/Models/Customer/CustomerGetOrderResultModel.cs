using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.Customer
{
    /// <summary>
    /// Результат поиска номера клиента.
    /// </summary>
    public class CustomerGetOrderResultModel
    {
        /// <summary>
        /// Результат удачного поиска.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Текстовый результат поиска.
        /// </summary>
        public string Description { get; set; }
    }
}