using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Тип операции по доходам и расходам.
    /// </summary>
    public class TransactionKind:EntityBase<byte>
    {
        /// <summary>
        /// Задает или получает код типа операции.
        /// </summary>
        public byte? TransactionKindID { get; set; }

        /// <summary>
        /// Задает или получает название типа операции.
        /// </summary>
        public string Title { get; set; }
    }
}
