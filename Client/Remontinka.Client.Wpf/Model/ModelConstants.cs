using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Model
{
    /// <summary>
    /// Константы для моделей.
    /// </summary>
    public static class ModelConstants
    {
        /// <summary>
        /// Регулярное выражение для признака содержания хотябы одного символа.
        /// </summary>
        public const string RequiredRegex = "^.{1,}$";

        /// <summary>
        /// Регулярное выражение для имен.
        /// </summary>
        public const string NamesRegex = "^.{2,}$";

        /// <summary>
        /// Номер для исключения контрола при переходах по табуляции.
        /// </summary>
        public const int NotTabNumber = -1;
    }
}
