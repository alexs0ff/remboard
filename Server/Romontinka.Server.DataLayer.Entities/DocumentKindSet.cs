using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Набор типов документов.
    /// </summary>
    public static class DocumentKindSet
    {
        static DocumentKindSet()
        {
            OrderReportDocument = new DocumentKind {DocumentKindID = 1, Title = "Документы заказа"};
            Kinds = new Collection<DocumentKind> {OrderReportDocument};
        }

        /// <summary>
        ///   Список всех типов документов.
        /// </summary>
        public static ICollection<DocumentKind> Kinds { get; private set; }

        /// <summary>
        /// Документ заказа.
        /// </summary>
        public static DocumentKind OrderReportDocument { get; private set; }

        /// <summary>
        ///   Возвращает тип документа по его коду.
        /// </summary>
        /// <returns> Тип документа или null. </returns>
        public static DocumentKind GetKindByID(byte? id)
        {
            return Kinds.FirstOrDefault(a => a.DocumentKindID == id);
        }
    }
}