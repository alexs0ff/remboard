using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Remontinka.Client.Core;
using Remontinka.Client.Core.Interception;

namespace Remontinka.Client.Wpf.Model
{
    /// <summary>
    /// Модель для приложения.
    /// </summary>
    public class ArmAppModel : BindableModelObject
    {
        public ArmAppModel()
        {
        }

        /// <summary>
        /// Текущая операция.
        /// </summary>
        [Dependency]
        public LongOperation CurrentOperation { get; set; }

        /// <summary>
        /// Задает или получает результат по показу главного меню.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual bool ShowMainMenu { get; set; }
    }
}
