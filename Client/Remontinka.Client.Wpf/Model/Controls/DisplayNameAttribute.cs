using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Model.Controls
{
    /// <summary>
    /// Атрибут имени контрола.
    /// </summary>
    public class DisplayNameAttribute:Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Attribute"/> class.
        /// </summary>
        public DisplayNameAttribute(string title)
        {
            Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Attribute"/> class.
        /// </summary>
        public DisplayNameAttribute()
        {
        }

        /// <summary>
        /// Задает или получает название контроллера.
        /// </summary>
        public string Title { get; set; }
    }
}
