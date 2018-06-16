using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.TransferDocGridForm
{
    /// <summary>
    /// Модель грида документов перемещения.
    /// </summary>
    public class TransferDocGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "TransferDocID"; } }

        /// <summary>
        /// Задает или получает склады.
        /// </summary>
        public IQueryable<Warehouse> Warehouses { get; set; }
    }
}