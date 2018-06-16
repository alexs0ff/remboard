using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Model.Controls
{
    /// <summary>
    /// Описатель контрола.
    /// </summary>
    public interface IControlDescriptor
    {
        /// <summary>
        /// Получает заголовок контрола.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Получает имя контрола.
        /// </summary>
        string ControlName { get; }

        /// <summary>
        /// Получает тип контрола.
        /// </summary>
        ControlType ControlType { get; }
    }
}
