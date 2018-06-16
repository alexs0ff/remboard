using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Core.Interception;

namespace Remontinka.Client.Wpf.Model
{
    /// <summary>
    /// Модель представления для контейнера редактирования сущностей.
    /// </summary>
    public class EditModelMasterModel : BindableModelObject
    {
        /// <summary>
        /// Задает или получает название окна.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string WindowText { get; set; }

        /// <summary>
        /// Задает или получает текст информации.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string InfoText { get; set; }

        /// <summary>
        /// Задает или получает цвет текста информации.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual bool InfoTextIsRed { get; set; }

        /// <summary>
        /// Текст для кнопки "сохранить".
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string SaveButtonText { get; set; }

        /// <summary>
        /// Задает или получает признак доступности "сохранить".
        /// </summary>
        [NotifyPropertyChanged]
        public virtual bool SaveButtonEnabled { get; set; }

        /// <summary>
        /// Текст для кнопки "отменить".
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string CancelButtonText { get; set; }

        /// <summary>
        /// Задает или получает признак доступности "отменить".
        /// </summary>
        [NotifyPropertyChanged]
        public virtual bool CancelButtonEnabled { get; set; }

    }
}
