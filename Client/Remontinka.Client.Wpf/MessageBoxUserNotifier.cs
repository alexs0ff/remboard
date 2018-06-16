using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Remontinka.Client.Core;

namespace Remontinka.Client.Wpf
{
    public class MessageBoxUserNotifier : IUserNotifier
    {
        /// <summary>
        /// Отображает в программе сообщение пользователю.
        /// </summary>
        /// <param name="title">Заголовок сообщения.</param>
        /// <param name="message">Текст сообщения.</param>
        public void ShowMessage(string title, string message)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(new Action<string, string>(ShowMessage), title,
                                                      message);
                return;
            } //if

            MessageBox.Show(message, title);
        }

        /// <summary>
        /// Производит опрос пользователя.
        /// </summary>
        /// <param name="title">Заголовок окна.</param>
        /// <param name="message">Текст вопроса.</param>
        /// <returns>Результат, True - пользователь согласился.</returns>
        public bool Confirm(string title, string message)
        {
            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            return result == MessageBoxResult.Yes;
        }

        
    }
}
