using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Core.Context
{
    /// <summary>
    /// Простой пункт для записи в контекст.
    /// </summary>
    public class SimpleContextItem : ContextItemBase
    {
        /// <summary>
        /// Создает экземпляр и инициализирует значения.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="value">Значение.</param>
        public SimpleContextItem(string key, object value)
        {
            _values[key] = value;
        }
    }
}
