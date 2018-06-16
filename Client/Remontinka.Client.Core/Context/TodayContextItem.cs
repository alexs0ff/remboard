using System;

namespace Remontinka.Client.Core.Context
{
    /// <summary>
    /// Контекст значения сегодняшнего дня.
    /// </summary>
    public class TodayContextItem : ContextItemBase
    {
        public TodayContextItem()
        {
            _values[ContextConstants.Today] = DateTime.Today;
        }
    }
}
