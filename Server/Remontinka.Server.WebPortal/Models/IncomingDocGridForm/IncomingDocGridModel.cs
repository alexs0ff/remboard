using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.IncomingDocGridForm
{
    /// <summary>
    /// Модель грида приходных накладных.
    /// </summary>
    public class IncomingDocGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "IncomingDocID"; } }

        /// <summary>
        /// Задает или получает склады.
        /// </summary>
        public IQueryable<Warehouse> Warehouses { get; set; }

        /// <summary>
        /// Задает или получает контрагентов.
        /// </summary>
        public IQueryable<Contractor> Contractors { get; set; }
    }
}