using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Updater
{
    /// <summary>
    /// EventArgs для события смены прогресса.
    /// </summary>
    public class ProgressChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.EventArgs"/>.
        /// </summary>
        public ProgressChangedEventArgs(int totalBytes, double percentCompleted, double transferRate)
        {
            TotalBytes = totalBytes;
            PercentCompleted = percentCompleted;
            TransferRate = transferRate;
        }

        /// <summary>
        /// Получает количество скаченных байт.
        /// </summary>
        public int TotalBytes { get; private set; }

        /// <summary>
        /// Получает процент выполненной загрузки.
        /// </summary>
        public double PercentCompleted { get; set; }

        /// <summary>
        /// Получает текущую скорость загрузки.
        /// </summary>
        public double TransferRate { get; set; }
    }
}
