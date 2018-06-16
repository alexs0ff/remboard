using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Тип автодополнения.
    /// </summary>
    public class AutocompleteKind : EntityBase<byte>
    {
        /// <summary>
        /// Задает или получает код типа автодополнения.
        /// </summary>
        public byte? AutocompleteKindID { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }
    }
}
