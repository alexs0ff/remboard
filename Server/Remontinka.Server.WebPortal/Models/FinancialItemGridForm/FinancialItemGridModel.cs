using System.Collections.Generic;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.FinancialItemGridForm
{
    /// <summary>
    /// Модель для грида финансовых статей.
    /// </summary>
    public class FinancialItemGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "FinancialItemID"; } }

        /// <summary>
        /// Получает тип статей бюджета.
        /// </summary>
        public IEnumerable<FinancialItemKind> FinancialItemKinds { get { return FinancialItemKindSet.Kinds; } }

        /// <summary>
        /// Задает или получает типы транзакций.
        /// </summary>
        public IEnumerable<TransactionKind> TransactionKinds { get { return TransactionKindSet.Kinds; } }
    }
}