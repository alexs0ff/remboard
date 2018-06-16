using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Интерфейс системы оповещения пользователя.
    /// </summary>
    public interface IUserNotifier
    {
        /// <summary>
        /// Отображает в программе сообщение пользователю.
        /// </summary>
        /// <param name="title">Заголовок сообщения.</param>
        /// <param name="message">Текст сообщения.</param>
        void ShowMessage(string title, string message);
        
        /// <summary>
        /// Производит опрос пользователя.
        /// </summary>
        /// <param name="title">Заголовок окна.</param>
        /// <param name="message">Текст вопроса.</param>
        /// <returns>Результат, True - пользователь согласился.</returns>
        bool Confirm(string title, string message);
    }
}
