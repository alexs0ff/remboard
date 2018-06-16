using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Remontinka.Client.Wpf.Model.Controls
{
    /// <summary>
    /// Event args для события по нажатию клавиш.
    /// </summary>
    public class PressKeyEventArgs : EventArgs
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.EventArgs"/>.
        /// </summary>
        public PressKeyEventArgs(object sender, Key key)
        {
            Key = key;
            Sender = sender;
        }

        /// <summary>
        /// Получает нажатую клавишу.
        /// </summary>
        public Key Key { get; private set; }

        /// <summary>
        /// Получает элемент отправивший событие.
        /// </summary>
        public object Sender { get; private set; }
    }
}
