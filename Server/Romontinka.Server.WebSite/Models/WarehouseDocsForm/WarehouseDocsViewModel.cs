using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.WarehouseDocsForm
{
    /// <summary>
    /// Модель представления по управлению складскими документами.
    /// </summary>
    public class WarehouseDocsViewModel
    {
        /// <summary>
        /// Задает или получает описатель грида с приходными накладными.
        /// </summary>
        public DataGridDescriptor IncomingDocGrid { get; set; }

        /// <summary>
        /// Задает или получает описатель грида с элементами приходной накладной.
        /// </summary>
        public DataGridDescriptor IncomingDocItemsGrid { get; set; }

        /// <summary>
        /// Задает или получает описатель грида с документами о списании.
        /// </summary>
        public DataGridDescriptor CancellationDocGrid { get; set; }

        /// <summary>
        /// Задает или получает описатель грида с элементами документов о списании со склада.
        /// </summary>
        public DataGridDescriptor CancellationDocItemsGrid { get; set; }

        /// <summary>
        /// Задает или получает описатель грида с документами о перемещении.
        /// </summary>
        public DataGridDescriptor TransferDocGrid { get; set; }

        /// <summary>
        /// Задает или получает описатель грида с элементами документов о перемещении между складами.
        /// </summary>
        public DataGridDescriptor TransferDocItemsGrid { get; set; }
    }
}