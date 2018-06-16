using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Model
{
    /// <summary>
    /// Модель пункта комбобокса.
    /// </summary>
    public class ComboBoxItemModel
    {
        /// <summary>
        /// Задает или получает название пункта комбобокса.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает значение  
        /// </summary>
        public object Value { get; set; }
    }
}
