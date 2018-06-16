using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.CancellationDocItemGridForm
{
    /// <summary>
    /// Модель данных для грида пунктов документа списания.
    /// </summary>
    public class CancellationDocItemGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "CancellationDocItemID"; } }

        /// <summary>
        /// ЗАдает или получает номенклатуру.
        /// </summary>
        public IQueryable<GoodsItem> GoodsItems { get; set; }
    }
}