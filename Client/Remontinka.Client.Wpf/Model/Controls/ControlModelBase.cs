using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core.Interception;

namespace Remontinka.Client.Wpf.Model.Controls
{
    /// <summary>
    /// Базовый класс для моделей контролов управления данными.
    /// </summary>
    public class ControlModelBase : BindableModelObject
    {
        public ControlModelBase()
        {
            IsVisible = true;
        }

        /// <summary>
        /// Задает или получает строку название контрола.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string Title { get; set; }

        /// <summary>
        /// Задает или получает признак видимости контрола.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual bool IsVisible { get; set; }

        /// <summary>
        /// Задает или получает идентификатор поля ввода.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Задает или получает строку описания контрола.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string Description { get; set; }

        /// <summary>
        /// Задает или получает текстовое представление значение контрола.
        /// </summary>        
        public virtual string Value { get; set; }

        /// <summary>
        /// Задает или получает исходное представление типа.
        /// </summary>
        public virtual object RawValue { get; set; }

        /// <summary>
        /// Если поле только для чтения
        /// </summary>
        [NotifyPropertyChanged]
        public virtual bool ReadOnly { get; set; }

        /// <summary>
        /// Получает признак валидности значения. 
        /// </summary>
        [NotifyPropertyChanged]
        public virtual bool IsAcceptable { get; protected set; }

        /// <summary>
        /// Получает текущий номер в перемещении курсора.
        /// </summary>
        public int TabStep { get; internal set; }

        /// <summary>
        /// Вызывается для проверки значения.
        /// </summary>
        public virtual void ValidateValue()
        {
        }

        /// <summary>
        /// Возникает когда пользователь нажимает на клавишу.
        /// </summary>
        public event EventHandler<PressKeyEventArgs> PressKey;

        /// <summary>
        /// Вызывает событие PressKey.
        /// </summary>
        /// <param name="e">The event args.</param>
        public void RisePressKey(PressKeyEventArgs e)
        {
            EventHandler<PressKeyEventArgs> handler = PressKey;
            if (handler != null)
            {
                handler(this, e);
            }
            ValidateValue();
        }

        /// <summary>
        /// Пробует установить значение для параметра.
        /// </summary>
        /// <param name="value">Текстовое значение для установки.</param>
        /// <returns>При успехе возвращает true.</returns>
        public virtual bool TrySetValue(string value)
        {
            return false;
        }
    }
}
