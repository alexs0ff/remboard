using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Core.Context
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
