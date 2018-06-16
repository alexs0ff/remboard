using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities.ReportItems
{
    /// <summary>
    /// Пункт отчета по движению документов на складе.
    /// </summary>
    public class WarehouseFlowReportItem
    {
        /// <summary>
        /// Задает или получает тип документа.
        /// </summary>
        public int DocKind { get; set; }

        /// <summary>
        /// Типы документов.
        /// </summary>
        private static readonly IDictionary<int, string> _docKinds = new Dictionary<int, string>
                                                     {
                                                         {1,"Приходная накладная"},
                                                         {2,"Документ списания"},
                                                         {3,"Перемещение из склада"},
                                                         {4,"Перемещение на склад"},
                                                         {5,"Заказ"},
                                                     };

        /// <summary>
        /// Задает или получает название типа документа
        /// </summary>
        public string DocKindTitle
        {
            get
            {
                if (!_docKinds.ContainsKey(DocKind))
                {
                    return string.Empty;
                } //if
                return _docKinds[DocKind];
            }
        }

        /// <summary>
        /// Задает или получает дату обработки документа.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает дату документа.
        /// </summary>
        public DateTime DocDate { get; set; }

        /// <summary>
        /// Задает или получает номер документа.
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// Задает или получает номенклатуру товара.
        /// </summary>
        public string GoodsItemTitle { get; set; }

        /// <summary>
        /// Задает или получает приходное количество.
        /// </summary>
        public decimal InCount { get; set; }

        /// <summary>
        /// Задает или получает расоходное количество.
        /// </summary>
        public decimal OutCount { get; set; }
    }
}
