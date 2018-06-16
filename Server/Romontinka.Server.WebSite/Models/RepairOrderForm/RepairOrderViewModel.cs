using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.RepairOrderForm
{
    /// <summary>
    /// Модель представления для заказов.
    /// </summary>
    public class RepairOrderViewModel
    {
        /// <summary>
        /// Задает или получает описатель грида с задачами.
        /// </summary>
        public DataGridDescriptor OrderGrid { get; set; }

        /// <summary>
        /// Задает или получает описатель грида с выполненными работами.
        /// </summary>
        public DataGridDescriptor WorkItemsGrid { get; set; }

        /// <summary>
        /// Задает или получает описатель грида с установленными запчастями.
        /// </summary>
        public DataGridDescriptor DeviceItemsGrid { get; set; }

        /// <summary>
        /// Задает или получает модели документов.
        /// </summary>
        public IEnumerable<RepairOrderDocumentModel> Documents { get; set; }

        /// <summary>
        /// Задает или получает пункты автодополнения у брендов устройств.
        /// </summary>
        public IEnumerable<string> DeviceTrademarkAutocompleteItems { get; set; }

        /// <summary>
        /// Задает или получает пункты автодополнения у комплектации устройств.
        /// </summary>
        public IEnumerable<string> DeviceOptionsAutocompleteItems { get; set; }

        /// <summary>
        /// Задает или получает пункты автодополнения у внешнего вида устройств.
        /// </summary>
        public IEnumerable<string> DeviceAppearanceAutocompleteItems { get; set; }
    }
}