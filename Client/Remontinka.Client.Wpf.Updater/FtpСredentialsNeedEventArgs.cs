using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Remontinka.Client.Wpf.Updater
{
    /// <summary>
    /// EventArgs для события спрашивающего логин и пароль для FTP.
    /// </summary>
    public class FtpСredentialsNeedEventArgs : EventArgs
    {
        /// <summary>
        /// Задает или получает логин.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Задает или получает пароль.
        /// </summary>
        public string Password { get; set; }
    }
}
