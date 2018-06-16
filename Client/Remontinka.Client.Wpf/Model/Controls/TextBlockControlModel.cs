using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core.Interception;

namespace Remontinka.Client.Wpf.Model.Controls
{
    /// <summary>
    /// Информационная модель.
    /// </summary>
    public class TextBlockControlModel : ControlModelBase
    {
        /// <summary>
        /// Задает или получает текстовое представление значение контрола.
        /// </summary>        
        public override string Value
        {
            get { return base.Value; }
            set
            {
                if (base.Value != value)
                {
                    base.Value = value;
                    RisePropertyChanged("Value");
                } //if
            }
        }

        /// <summary>
        /// Задает или получает высоту контрола.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual int Height { get; set; }

        /// <summary>
        /// Задает или получает флаг показывающий, что текст контрола показывается красным.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual bool ShowRedText { get; set; }

        /// <summary>
        /// Получает признак валидности значения. 
        /// </summary>
        public override bool IsAcceptable
        {
            get { return true; }
        }
    }
}
