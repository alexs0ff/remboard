using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.ItemCategoryGridForm
{
    /// <summary>
    /// Модель грида категорий товаров.
    /// </summary>
    public class ItemCategoryGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "ItemCategoryID"; } }
    }
}