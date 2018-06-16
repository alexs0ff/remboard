using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Wpf.Controls;
using Remontinka.Client.Wpf.Model.Controls;

namespace Remontinka.Client.Wpf.Controllers.Controls
{
    /// <summary>
    /// Контроллер управления контролом денежных средств.
    /// </summary>
    public class MoneyBoxController: UserControlControllerBase<MoneyBoxControlModel, MoneyBoxControl>
    {
        /// <summary>
        /// Вызывается при установлении модели для определенного контрола.
        /// </summary>
        /// <param name="control">Контрол.</param>
        /// <param name="model">Созданная модель.</param>
        protected override void OnSetModel(MoneyBoxControl control, MoneyBoxControlModel model)
        {
            
        }
    }
}
