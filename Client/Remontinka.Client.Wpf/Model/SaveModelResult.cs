using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remontinka.Client.Wpf.Model
{
    /// <summary>
    /// Модель результата сохранения сущности.
    /// </summary>
    public sealed class SaveModelResult
    {
        /// <summary>
        /// Задает или получает признак наличия ошибок в модели.
        /// </summary>
        public bool HasModelError
        {
            get { return _modelErrors != null && _modelErrors.Any(); }
        }

        /// <summary>
        /// Содержит ошибки модели.
        /// </summary>
        private List<PairItem<string, string>> _modelErrors;

        /// <summary>
        /// Получает ошибки модели.
        /// </summary>
        public List<PairItem<string, string>> ModelErrors
        {
            get
            {
                if (_modelErrors == null)
                {
                    _modelErrors = new List<PairItem<string, string>>();
                } //if
                return _modelErrors;
            }
        }
    }
}
