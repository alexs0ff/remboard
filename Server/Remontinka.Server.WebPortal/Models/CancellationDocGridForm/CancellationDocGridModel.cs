using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.CancellationDocGridForm
{
    /// <summary>
    /// Модель грида данных для документов списания.
    /// </summary>
    public class CancellationDocGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "CancellationDocID"; } }

        /// <summary>
        /// Задает или получает склады.
        /// </summary>
        public IQueryable<Warehouse> Warehouses { get; set; }
    }
}