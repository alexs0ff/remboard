using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.DataLayer.Entities
{
    /// <summary>
    /// Тип документа.
    /// </summary>
    public class DocumentKind : EntityBase<long>
    {
        /// <summary>
        /// Задает или получает код типа документа.
        /// </summary>
        public long? DocumentKindID { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }
    }
}
