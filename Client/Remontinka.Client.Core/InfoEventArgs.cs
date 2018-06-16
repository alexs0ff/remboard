using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Аргументы события о смене информации.
    /// </summary>
    public class InfoEventArgs:EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.EventArgs"/> class.
        /// </summary>
        public InfoEventArgs(string infoText)
        {
            InfoText = infoText;
        }

        /// <summary>
        /// Получает текст информации.
        /// </summary>
        public string InfoText { get; private set; }
    }
}
