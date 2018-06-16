using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Model.Controls
{
    /// <summary>
    /// Атрибут обозначающий комбобокс.
    /// </summary>
    public class ComboBoxControlAttribute:Attribute
    {
        /// <summary>
        /// Задает или получает тип контроллера.
        /// </summary>
        public Type ControllerType { get; set; }

        /// <summary>
        /// Задает или получает допустимость Null.
        /// </summary>
        public bool AllowNull { get; set; }

        /// <summary>
        /// Задает или получает признак null атрибута.
        /// </summary>
        public bool ShowNullValue { get; set; }
    }
}
