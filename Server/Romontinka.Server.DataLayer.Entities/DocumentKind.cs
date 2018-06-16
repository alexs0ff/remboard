using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Тип документа.
    /// </summary>
    public class DocumentKind:EntityBase<byte>
    {
        /// <summary>
        /// Задает или получает код типа документа.
        /// </summary>
        public byte? DocumentKindID { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }
    }
}
