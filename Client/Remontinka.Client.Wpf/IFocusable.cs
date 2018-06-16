using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf
{
    /// <summary>
    /// Управляющий интерфейс контролами.
    /// </summary>
    public interface IFocusable
    {
        /// <summary>
        /// Установка фокуса на управляющий контрол.
        /// </summary>
        void SetFocus();

        /// <summary>
        /// Возвращает признак наличия фокуса в контроле.
        /// </summary>
        /// <returns>True - если есть фокус.</returns>
        bool HasFocus();
    }
}
