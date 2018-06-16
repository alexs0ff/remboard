using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.TransferDocItemGridForm
{
    /// <summary>
    /// МОдель грида пунктов документа описания.
    /// </summary>
    public class TransferDocItemGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "TransferDocItemID"; } }

        /// <summary>
        /// Задает или получает номенклатуру.
        /// </summary>
        public IQueryable<GoodsItem> GoodsItems { get; set; }
    }
}