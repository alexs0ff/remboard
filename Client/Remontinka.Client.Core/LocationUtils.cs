using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Утилитные методы для нахождения расположения файлов.
    /// </summary>
    public static class LocationUtils
    {
        /// <summary>
        /// Возвращает полный путь к папке со сборкой..
        /// </summary>
        /// <returns>Папка с терминалом.</returns>
        public static string GetFullPath()
        {
            var directory = Path.GetDirectoryName(typeof(LocationUtils).Assembly.Location);

            return directory;
        }
    }
}
