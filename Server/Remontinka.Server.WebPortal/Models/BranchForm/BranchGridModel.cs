using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.BranchForm
{
    /// <summary>
    /// Модель грида для создания филиала.
    /// </summary>
    public class BranchGridModel: GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName {
            get { return "BranchID"; }
        }

    }
}