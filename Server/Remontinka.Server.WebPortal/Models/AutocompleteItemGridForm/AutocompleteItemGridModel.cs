using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.AutocompleteItemGridForm
{
    /// <summary>
    /// Модель для грида автодополнения.
    /// </summary>
    public class AutocompleteItemGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "AutocompleteItemID"; } }

        /// <summary>
        /// Получает типы автодополнений.
        /// </summary>
        public IEnumerable<AutocompleteKind> AutocompleteKinds { get { return AutocompleteKindSet.Kinds; } }
    }
}