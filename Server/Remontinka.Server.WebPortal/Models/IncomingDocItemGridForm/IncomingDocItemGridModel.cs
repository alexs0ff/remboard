using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.IncomingDocItemGridForm
{
    /// <summary>
    /// Модель для грида пунктов приходных накладных.
    /// </summary>
    public class IncomingDocItemGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "IncomingDocItemID"; } }

        /// <summary>
        /// ЗАдает или получает номенклатуру.
        /// </summary>
        public IQueryable<GoodsItem> GoodsItems { get; set; }
    }
}