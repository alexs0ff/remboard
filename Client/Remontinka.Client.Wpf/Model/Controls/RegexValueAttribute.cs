using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Model.Controls
{
    /// <summary>
    /// Атрибут проверки значения контрола по регулярному выражению.
    /// </summary>
    public class RegexValueAttribute : Attribute
    {
        /// <summary>
        /// Задает или получает значение проверки по регулярному выражению.
        /// </summary>
        public string Regex { get; set; }
    }
}
