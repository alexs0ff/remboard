using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Wpf.Controls;
using Remontinka.Client.Wpf.Model.Controls;

namespace Remontinka.Client.Wpf.Controllers.Controls
{
    /// <summary>
    /// Контроллер управления комбобоксом.
    /// </summary>
    public class ComboBoxController : UserControlControllerBase<ComboBoxControlModel, ComboBoxControl>
    {
        /// <summary>
        /// Вызывается при установлении модели для определенного контрола.
        /// </summary>
        /// <param name="control">Контрол.</param>
        /// <param name="model">Созданная модель.</param>
        protected override void OnSetModel(ComboBoxControl control, ComboBoxControlModel model)
        {
            
        }
    }
}
