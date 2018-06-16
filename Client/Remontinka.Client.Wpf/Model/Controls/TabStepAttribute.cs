using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Model.Controls
{
    /// <summary>
    /// Аттрибут шага табуляции.
    /// </summary>
    public class TabStepAttribute:Attribute
    {
        /// <summary>
        /// Задает или получает номер шага табуляции.
        /// </summary>
        public int Step { get; set; }
    }
}
