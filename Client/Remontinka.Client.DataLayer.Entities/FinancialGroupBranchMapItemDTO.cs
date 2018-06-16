namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// DTO Объект для соответствия финансовой группы и филиалов.
    /// </summary>
    public class FinancialGroupBranchMapItemDTO : FinancialGroupBranchMapItem
    {
        /// <summary>
        /// Задает или получает название филиалов.
        /// </summary>
        public string BranchTitle { get; set; }

        /// <summary>
        /// Задает или получает название финансовой группы.
        /// </summary>
        public string FinancialGroupTitle { get; set; }

    }
}
