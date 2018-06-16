using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Model.Controls
{
    /// <summary>
    /// Аттрибут маски.
    /// </summary>
    public class TextBoxMaskAttrubute:Attribute
    {
        /// <summary>
        /// Задает или получает маску.
        /// </summary>
        public string Mask { get; set; }
    }
}
