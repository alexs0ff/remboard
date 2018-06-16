using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Wpf.Model.Controls;

namespace Remontinka.Client.Wpf.Controllers.Controls
{
    /// <summary>
    /// Интерфейс контроллера контролов.
    /// </summary>
    public interface IUserControlController
    {
        /// <summary>
        /// Возвращает модель.
        /// </summary>
        ControlModelBase GetModel();
    }
}
