using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.Common
{
    /// <summary>
    /// Класс с двумя свойствами.
    /// </summary>
    /// <typeparam name="T1">Первый тип свойства.</typeparam>
    /// <typeparam name="T2">Второй тип свойства.</typeparam>
    public class PairItem<T1, T2>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Romontinka.Server.WebSite.Common.PairItem"/> class.
        /// </summary>
        public PairItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PairItem"/> class.
        /// </summary>
        public PairItem(T1 firstItem, T2 secondItem)
        {
            FirstItem = firstItem;
            SecondItem = secondItem;
        }

        /// <summary>
        /// Задает или получает первое свойство.
        /// </summary>
        public T1 FirstItem { get; set; }

        /// <summary>
        /// Задает или получает Второе свойство.
        /// </summary>
        public T2 SecondItem { get; set; }
    }
}