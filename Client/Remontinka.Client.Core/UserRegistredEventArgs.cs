using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Аргументы события регистрации пользователя.
    /// </summary>
    public class UserRegistredEventArgs:EventArgs
    {
        public UserRegistredEventArgs(string loginName)
        {
            LoginName = loginName;
        }

        /// <summary>
        /// Получает логин зарегистрированного пользователя.
        /// </summary>
        public string LoginName { get; private set; }
    }
}
