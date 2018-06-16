using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Model.Forms
{
    /// <summary>
    /// Аргументы события изменения валидности формы.
    /// </summary>
    public class ModelIsValidStateChangedEventArgs:EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.EventArgs"/> class.
        /// </summary>
        public ModelIsValidStateChangedEventArgs(bool isValid)
        {
            IsValid = isValid;
        }

        /// <summary>
        /// Получает признак валидности формы.
        /// </summary>
        public bool IsValid { get; private set; }
    }
}
