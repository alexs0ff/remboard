using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Информация по пунктам.
    /// </summary>
    public class ItemsInfo
    {
        /// <summary>
        /// Задает или получает количество пунктов.
        /// </summary>
        public decimal? Count { get; set; }
        
        /// <summary>
        /// Задает или получает сумму количества пунктов.
        /// </summary>
        public decimal? SumCount { get; set; }

        /// <summary>
        /// Задает или получает сумму пунктов.
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Получает сумму всех пунктов умноженных на количество.
        /// </summary>
        public decimal? TotalAmount { get; set; }
    }
}
