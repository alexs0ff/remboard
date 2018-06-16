using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Тип вознаграждения.
    /// </summary>
    public class InterestKind : EntityBase<byte>
    {
        /// <summary>
        /// Задает или получает тип вознаграждения.
        /// </summary>
        public byte? InterestKindID { get; set; }

        /// <summary>
        /// Задает или получает название вознаграждения.
        /// </summary>
        public string Title { get; set; }
    }
}
