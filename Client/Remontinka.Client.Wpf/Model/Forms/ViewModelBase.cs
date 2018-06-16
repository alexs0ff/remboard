using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Model.Forms
{
    /// <summary>
    /// Базовый класс для модельей привзяки к системе.
    /// </summary>
    /// <typeparam name="T">Тип ключа.</typeparam>
    public abstract class ViewModelBase<T>
        where T:struct
    {
        /// <summary>
        /// Задает или получает идентификатор объекта.
        /// </summary>
        public T? Id { get; set; }
    }
}
