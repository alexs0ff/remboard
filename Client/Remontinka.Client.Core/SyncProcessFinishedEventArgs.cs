using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core.Models;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Аргументы события завершения обработки синхронизации.
    /// </summary>
    public class SyncProcessFinishedEventArgs:EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.EventArgs"/> class.
        /// </summary>
        public SyncProcessFinishedEventArgs(SyncModelDescriptor modelDescriptor)
        {
            ModelDescriptor = modelDescriptor;
        }

        /// <summary>
        /// Получает описатель модели синхронизации.
        /// </summary>
        public SyncModelDescriptor ModelDescriptor { get; private set; }
    }
}
