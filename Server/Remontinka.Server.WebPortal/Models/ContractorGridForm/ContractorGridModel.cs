using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.ContractorGridForm
{
    /// <summary>
    /// Модель данных грида контрагентов.
    /// </summary>
    public class ContractorGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "ContractorID"; } }
    }
}