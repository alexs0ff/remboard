using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Remontinka.Client.Core;
using Remontinka.Client.Wpf.Model.Controls;

namespace Remontinka.Client.Wpf.Controllers.Controls
{
    /// <summary>
    /// Базовый класс для контроллеров пользовательских элементов.
    /// </summary>
    public abstract class UserControlControllerBase<TModel, TView> : IUserControlController
        where TModel : ControlModelBase
        where TView: UserControl
    {
        /// <summary>
        /// Получает пользовательский контрол.
        /// </summary>
        public TView View { get; private set; }

        /// <summary>
        /// Получает модель контрола.
        /// </summary>
        public TModel Model { get; private set; }

        /// <summary>
        /// Установка отображения контроллеру.
        /// </summary>
        /// <param name="control">Контрол для установки.</param>
        public void SetView(TView control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            } //if

            View = control;

            Model = ClientCore.Instance.CreateInstance<TModel>();
            View.DataContext = Model;
            OnSetModel(View, Model);
            Model.ValidateValue();
        }

        /// <summary>
        /// Вызывается при установлении модели для определенного контрола.
        /// </summary>
        /// <param name="control">Контрол.</param>
        /// <param name="model">Созданная модель.</param>
        protected abstract void OnSetModel(TView control, TModel model);

        /// <summary>
        /// Возвращает модель.
        /// </summary>
        public ControlModelBase GetModel()
        {
            return Model;
        }
    }
}
