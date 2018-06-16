using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объекты для финансовых групп.
    /// </summary>
    public class FinancialGroupBranchMapItemDTO
    {
        /// <summary>
        /// Задает или получает код пункта соответствия финансовой группы и филиала.
        /// </summary>
        public Guid? FinancialGroupBranchMapID { get; set; }

        /// <summary>
        /// Задает или получает код филиала.
        /// </summary>
        public Guid? BranchID { get; set; }

        /// <summary>
        /// Задает или получает код финансовой группы.
        /// </summary>
        public Guid? FinancialGroupID { get; set; }
    }
}
