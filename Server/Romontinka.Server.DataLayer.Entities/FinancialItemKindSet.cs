using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Типы статей бюджета.
    /// </summary>
    public static class FinancialItemKindSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Romontinka.Server.DataLayer.Entities.FinancialItemKindSet"/> class.
        /// </summary>
        static FinancialItemKindSet()
        {
            Custom = new FinancialItemKind { Title = "Пользовательский",FinancialItemKindID = 1};
            OrderPaid = new FinancialItemKind { Title = "Приход от сервисной деятельности и ремонта", FinancialItemKindID = 2 };
            WarhouseItemsPaid = new FinancialItemKind { Title = "Расход от приобретения запчастей на склад", FinancialItemKindID = 3 };
            Kinds = new[] { Custom, OrderPaid, WarhouseItemsPaid };
        }

        /// <summary>
        ///   Возвращает тип по его коду.
        /// </summary>
        /// <returns> Тип или null. </returns>
        public static FinancialItemKind GetKindByID(byte? id)
        {
            return Kinds.FirstOrDefault(a => a.FinancialItemKindID == id);
        }

        /// <summary>
        ///   Список всех типов статей.
        /// </summary>
        public static ICollection<FinancialItemKind> Kinds { get; private set; }

        /// <summary>
        /// Пользовательский тип статьи.
        /// </summary>
        public static FinancialItemKind Custom { get; private set; }

        /// <summary>
        /// Статья доходов от оплаты заказов.
        /// </summary>
        public static FinancialItemKind OrderPaid { get; private set; }

        /// <summary>
        /// Статья расходов от оплаты запчастей для склада.
        /// </summary>
        public static FinancialItemKind WarhouseItemsPaid { get; private set; }
    }
}
