using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Тип измерения.
    /// </summary>
    public class DimensionKind:EntityBase<byte>
    {
        /// <summary>
        /// Задает или получает код типа измерения.
        /// </summary>
        public byte? DimensionKindID { get; set; }

        /// <summary>
        /// Задает или получает название типа измерения.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает короткое название.
        /// </summary>
        public string ShortTitle { get; set; }
    }
}
