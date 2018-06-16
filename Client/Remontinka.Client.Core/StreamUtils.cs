using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Хэлперы для утилит.
    /// </summary>
    public static class StreamUtils
    {
        /// <summary>
        /// Копирует один поток в другой.
        /// </summary>
        /// <param name="input">Входной поток.</param>
        /// <param name="output">Выходной поток.</param>
        public static void CopyStream(Stream input, Stream output)
        {
            // Insert null checking here for production
            var buffer = new byte[8192];
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }
    }
}
