using System.Collections;

namespace Romontinka.Server.Core.Context
{
    /// <summary>
    /// Базовый класс для записи значений контекста для создания строк.
    /// </summary>
    public abstract class ContextItemBase
    {
        /// <summary>
        /// Значения параметров.
        /// </summary>
        protected readonly Hashtable _values = new Hashtable();

        /// <summary>
        /// Возвращает список значений для записи в Context.
        /// </summary>
        /// <returns></returns>
        public Hashtable GetValues()
        {
            return _values;
        }
    }
}