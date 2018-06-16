using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.DataLayer.Entities.ReportItems;

namespace Remontinka.Server.WebPortal.Models.RepairOrderGridForm
{
    /// <summary>
    /// Модель для представления грида.
    /// </summary>
    public class RepairOrderGridModel: GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName {
            get
            {
                return "RepairOrderID";
            } }

        /// <summary>
        /// Задает или получает доступные филиалы.
        /// </summary>
        public IEnumerable<Branch> Branches { get; set; }

        /// <summary>
        /// Типы заказов.
        /// </summary>
        public IEnumerable<OrderKind> OrderKinds { get; set; }

        /// <summary>
        /// Статусы заказа.
        /// </summary>
        public IEnumerable<OrderStatus> OrderStatuses { get; set; }

        /// <summary>
        /// Типы заказов.
        /// </summary>
        public IEnumerable<StatusKind> StatusKinds { get; set; }

        /// <summary>
        /// Задает или получает инжинеров.
        /// </summary>
        public List<SelectListItem<Guid>> Engineers { get; set; }

        /// <summary>
        /// Задает или получает менеджеров.
        /// </summary>
        public List<SelectListItem<Guid>> Managers { get; set; }
        
    }
}