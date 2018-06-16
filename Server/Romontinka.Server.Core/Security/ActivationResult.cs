using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Core.Security
{
    /// <summary>
    /// Результат активации.
    /// </summary>
    public class ActivationResult
    {
        /// <summary>
        /// Задает или получает флаг успешности регистрации.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Задает или получает описание результата регистрации.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Задает или получает логин активации.
        /// </summary>
        public string Login { get; set; }
    }
}
