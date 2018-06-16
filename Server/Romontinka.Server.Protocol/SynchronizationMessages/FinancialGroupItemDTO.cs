using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объект для финансовой группы.
    /// </summary>
    public class FinancialGroupItemDTO
    {
        /// <summary>
        /// Задает или получает код финансовой группы.
        /// </summary>
        public Guid? FinancialGroupID { get; set; }

        /// <summary>
        /// Задает или получает название финансовой группы.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает юр название фирмы.
        /// </summary>
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает торговую марку фирмы.
        /// </summary>
        public string Trademark { get; set; }
    }
}
