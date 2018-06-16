using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.Wpf.Model.Forms;

namespace Remontinka.Client.Wpf.Model
{
    /// <summary>
    /// Аргументы события сохранения модели.
    /// </summary>
    public class ModelSavedEventArgs<TModel, TKey> : EventArgs
        where TKey : struct
        where TModel: ViewModelBase<TKey>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.EventArgs"/> class.
        /// </summary>
        public ModelSavedEventArgs(TModel savedModel)
        {
            SavedModel = savedModel;
        }

        /// <summary>
        /// Получает сохраненную модель.
        /// </summary>
        public TModel SavedModel { get; private set; }
    }
}
