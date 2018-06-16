using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Wpf.Controls;
using Remontinka.Client.Wpf.Model.Controls;

namespace Remontinka.Client.Wpf.Controllers.Controls
{
    /// <summary>
    /// Контроллер управления текстовым полем.
    /// </summary>
    public class TextBoxController : UserControlControllerBase<TextBoxControlModel,TextBoxControl>
    {
        /// <summary>
        /// Вызывается при установлении модели для определенного контрола.
        /// </summary>
        /// <param name="control">Контрол.</param>
        /// <param name="model">Созданная модель.</param>
        protected override void OnSetModel(TextBoxControl control, TextBoxControlModel model)
        {
            
        }
    }
}
