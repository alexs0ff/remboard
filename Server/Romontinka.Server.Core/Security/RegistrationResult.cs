using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Core.Security
{
    /// <summary>
    /// Результат регистрации.
    /// </summary>
    public class RegistrationResult
    {
        /// <summary>
        /// Задает или получает флаг успешности регистрации.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Задает или получает описание результата регистрации.
        /// </summary>
        public string Description { get; set; }
    }
}
