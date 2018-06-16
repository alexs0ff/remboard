using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Remontinka.Client.Core.Interception;

namespace Remontinka.Client.Wpf.Model
{
    /// <summary>
    /// Модель формы авторизации.
    /// </summary>
    public class AuthControlModel : BindableModelObject
    {
        public AuthControlModel()
        {
            Users = new ObservableCollection<ComboBoxItemModel>();
        }

        /// <summary>
        /// Получает список пользователей.
        /// </summary>
        public ICollection<ComboBoxItemModel> Users { get; private set; }

        /// <summary>
        /// Задает текст ошибки для окна.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string ErrorText { get; set; }

        /// <summary>
        /// Задает или получает признак доступности контролов.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual bool IsEnabaled { get; set; }

        /// <summary>
        /// Задает или получает признак видимости области регистрации.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual bool RegisterAreaVisible { get; set; }
    }
}
